// src/app/components/quote-calculator/quote-calculator.component.ts
// UPDATED VERSION - Uses backend for calculations
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';
import { LoanRequestService } from '../../services/loan-request.service';
import { LoanCalculationService } from '../../services/loan-calculation.service';
import { BorrowerData } from '../../models/borrower-data.model';
import { QuoteRequest } from '../../models/quote.model';

@Component({
  selector: 'app-quote-calculator',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './quote-calculator.component.html',
  styleUrls: ['./quote-calculator.component.css']
})
export class QuoteCalculatorComponent implements OnInit {
  // Product configuration
  productData: Product[] = [];
  selectedProductId: string = '';
  isLoadingProducts: boolean = true;
  productLoadError: string | null = null;

  // Loan amount
  minLoan: number = 500;
  maxLoan: number = 15000;
  loanAmount: number = 500;

  // Loan term
  loanTerm: number = 2;

  // Form fields
  titles: string[] = ['Mr.', 'Mrs.', 'Ms.'];
  title: string = 'Mr.';
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  mobileNumber: string = '';
  dateOfBirth: string = '';

  // Loan request data
  loanRequestId: string | null = null;
  isLoadingLoanRequest: boolean = false;
  loanRequestError: string | null = null;

  // Quote calculation state
  isCalculatingQuote: boolean = false;

  constructor(
    private productService: ProductService,
    private loanRequestService: LoanRequestService,
    private loanCalculationService: LoanCalculationService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Check for loan request ID in query parameters
    this.route.queryParams.subscribe(params => {
      this.loanRequestId = params['Id'] || params['id'] || null;
      
      if (this.loanRequestId) {
        this.loadLoanRequestData(this.loanRequestId);
      }
    });

    // Load products
    this.loadProducts();
  }

  /**
   * Load loan request data from API
   */
  loadLoanRequestData(id: string): void {
    this.isLoadingLoanRequest = true;
    this.loanRequestError = null;

    this.loanRequestService.getLoanRequestById(id).subscribe({
      next: (data: BorrowerData) => {
        console.log('Loan request data received in component:', data);
        
        if (data && typeof data === 'object') {
          this.populateFormWithBorrowerData(data);
        } else {
          console.error('Invalid data structure:', data);
          this.loanRequestError = 'Invalid data structure received from API';
        }
        
        this.isLoadingLoanRequest = false;
      },
      error: (error: Error) => {
        console.error('Error loading loan request:', error);
        this.loanRequestError = error.message;
        this.isLoadingLoanRequest = false;
      }
    });
  }

  /**
   * Populate form fields with borrower data
   */
  populateFormWithBorrowerData(data: BorrowerData): void {
    console.log('Populating form with:', data);
    
    
    this.title = data?.title || 'Mr';
    this.firstName = data?.firstName || '';
    this.lastName = data?.lastName || '';
    this.email = data?.email || '';
    this.mobileNumber = data?.mobile || '';
    this.loanAmount = data?.loanAmount || this.minLoan;
    this.loanTerm = data?.term || 12;
    this.dateOfBirth = new Intl.DateTimeFormat('en-CA').format(new Date(data.dateOfBirth));

    if (this.loanAmount < this.minLoan) {
      this.loanAmount = this.minLoan;
    }
    if (this.loanAmount > this.maxLoan) {
      this.loanAmount = this.maxLoan;
    }

    const selectedProduct = this.getSelectedProduct();
    if (selectedProduct) {
      if (this.loanTerm < selectedProduct.minTermMonths) {
        this.loanTerm = selectedProduct.minTermMonths;
      }
      if (this.loanTerm > selectedProduct.maxTermMonths) {
        this.loanTerm = selectedProduct.maxTermMonths;
      }
    }
    
    console.log('Form populated successfully');
  }

 
  retryLoadLoanRequest(): void {
    if (this.loanRequestId) {
      this.loadLoanRequestData(this.loanRequestId);
    }
  }

  retryLoadProducts(): void {
     this.loadProducts();
  }

  loadProducts(): void {
    this.isLoadingProducts = true;
    this.productLoadError = null;

    this.productService.getProduct().subscribe({
      next: (products: Product[]) => {
        this.productData = products;
        
        if (this.productData.length > 0) {
          this.selectedProductId = this.productData[0].id;
          this.onProductChange();
        }
        
        this.isLoadingProducts = false;
        console.log('Products loaded:', this.productData);
      },
      error: (error: Error) => {
        console.error('Error loading products:', error);
        this.productLoadError = 'Failed to load products from the database.';    
        this.isLoadingProducts = false;
      }
    });
  }

  /**
   * Get the currently selected product configuration
   */
  getSelectedProduct(): Product | undefined {
    return this.productData.find(p => p.id === this.selectedProductId);
  }

  /**
   * Handle product selection change
   */
  onProductChange(): void {
    const selectedProduct = this.getSelectedProduct();
    if (selectedProduct) {
      if (this.loanTerm < selectedProduct.minTermMonths) {
        this.loanTerm = selectedProduct.minTermMonths;
      }
      if (this.loanTerm > selectedProduct.maxTermMonths) {
        this.loanTerm = selectedProduct.maxTermMonths;
      }
    }
  }

  /**
   * Get minimum term for selected product
   */
  getMinTerm(): number {
    return this.getSelectedProduct()?.minTermMonths ?? 2;
  }

  /**
   * Get maximum term for selected product
   */
  getMaxTerm(): number {
    return this.getSelectedProduct()?.maxTermMonths ?? 60;
  }

  /**
   * Get interest-free information text
   */
  getInterestFreeInfo(): string | null {
    const product = this.getSelectedProduct();
    if (!product) return null;

    if (product.interestFreeMonths === 'all') {
      return '🎉 This entire loan is interest-free!';
    } else if (product.interestFreeMonths > 0) {
      return `🎉 First ${product.interestFreeMonths} ${product.interestFreeMonths === 1 ? 'month' : 'months'} interest-free!`;
    }
    return null;
  }

  /**
   * Get slider tooltip position for amount
   */
  getSliderTooltipPosition(): string {
    const percentage = ((this.loanAmount - this.minLoan) / (this.maxLoan - this.minLoan)) * 100;
    return `${percentage}%`;
  }

  /**
   * Get slider tooltip position for term
   */
  getTermSliderTooltipPosition(): string {
    const minTerm = this.getMinTerm();
    const maxTerm = this.getMaxTerm();
    const percentage = ((this.loanTerm - minTerm) / (maxTerm - minTerm)) * 100;
    return `${percentage}%`;
  }

  /**
   * Handle amount slider change
   */
  onSliderChange(): void {
    // Slider value changed
  }

  /**
   * Handle term slider change
   */
  onTermSliderChange(): void {
    // Slider value changed
  }

  /**
   * Format currency
   */
  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount);
  }

  /**
   * Calculate age from date of birth
   */
  getAge(): number | null {
    if (!this.dateOfBirth) return null;
    return this.loanRequestService.calculateAge(this.dateOfBirth);
  }

  /**
   * Calculate quote and navigate to borrower-quote page
   * NOW USES BACKEND API FOR CALCULATION
   */
  calculateQuote(): void {
    this.isCalculatingQuote = true;

    const quoteRequest: QuoteRequest = {
      productId: this.selectedProductId,
      loanRequestId: this.loanRequestId,
      loanAmount: this.loanAmount,
      termMonths: this.loanTerm,
      borrowerDetails: {
        title: this.title,
        firstName: this.firstName,
        lastName: this.lastName,
        dateOfBirth: this.dateOfBirth,
        email: this.email,
        mobileNumber: this.mobileNumber
      }
    };

    // Call backend API to create quote with calculations
    this.loanCalculationService.createQuote(quoteRequest).subscribe({
      next: (quoteResponse) => {
        console.log('Quote created successfully:', quoteResponse);
        
        // Navigate to borrower-quote page with the quote data
        this.router.navigate(['/borrower-quote'], {
          state: { quoteData: quoteResponse }
        });
        
        this.isCalculatingQuote = false;
      },
      error: (error: Error) => {
        console.error('Error creating quote:', error);
        alert(`Failed to create quote: ${error.message}`);
        this.isCalculatingQuote = false;
      }
    });
  }
}