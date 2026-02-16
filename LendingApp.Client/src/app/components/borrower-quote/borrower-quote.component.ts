import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoanCalculationService } from '../../services/loan-calculation.service';
import { QuoteResponse } from '../../models/quote.model';

@Component({
  selector: 'app-borrower-quote',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './borrower-quote.component.html',
  styleUrls: ['./borrower-quote.component.css']
})
export class BorrowerQuoteComponent implements OnInit {
  isEditingInfo = false;
  isEditingFinance = false;
  isRecalculating = false;

  // Slider limits
  minLoan: number = 500;
  maxLoan: number = 15000;

  // Title options
  titleOptions: string[] = ['Mr.', 'Mrs.', 'Ms.'];

  borrowerInfo = {
    name: '',
    title: '',
    firstName: '',
    lastName: '',
    dateOfBirth:'',
    mobile: '',
    email: ''
  };


  financeDetails = {
    amount: 1000,
    months: 24,
    repaymentAmount: 0,
    frequency: 'Monthly',
    totalRepayment: 0,
    establishmentFee: 0,
    interest: 0,
    minLoan: 506,
    maxLoan: 15000,
    minTerm: 2,
    maxTerm: 60
  };

  // Store the full quote data from backend
  quoteData: QuoteResponse | null = null;

  constructor(
    private router: Router,
    private loanCalculationService: LoanCalculationService
  ) {
    // Get the navigation state
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras?.state) {
      this.quoteData = navigation.extras.state['quoteData'];
    }
  }

  ngOnInit(): void {
    console.log('Received quote data:', this.quoteData);

    // If quote data exists, populate the form with it
    if (this.quoteData) {
      this.populateFromQuoteData(this.quoteData);
    } else {
      console.warn('No quote data found from navigation');
    }
  }

  populateFromQuoteData(data: QuoteResponse): void {
    this.borrowerInfo = {
      name: `${data.borrowerDetails.title} ${data.borrowerDetails.firstName} ${data.borrowerDetails.lastName}`.trim(),
      title: data.borrowerDetails.title,
      firstName: data.borrowerDetails.firstName,
      dateOfBirth: data.borrowerDetails.dateOfBirth,
      lastName: data.borrowerDetails.lastName,
      mobile: data.borrowerDetails.mobileNumber,
      email: data.borrowerDetails.email
    };

    this.financeDetails = {
      amount: data.loanAmount,
      months: data.term,
      repaymentAmount: data.monthlyPayment,
      frequency: 'Monthly',
      totalRepayment: data.totalAmount,
      establishmentFee: data.establishmentFee,
      interest: data.interest,
      minLoan: data.minLoan,
      maxLoan: data.maxLoan,
      minTerm: data.minTerm,
      maxTerm: data.maxTerm
    };

    console.log('Form populated with quote data:', {
      borrowerInfo: this.borrowerInfo,
      financeDetails: this.financeDetails
    });
  }

  /**
   * Get slider tooltip position for amount
   */
  getAmountSliderTooltipPosition(): string {
    const percentage = ((this.financeDetails.amount - this.minLoan) / (this.maxLoan - this.minLoan)) * 100;
    return `${percentage}%`;
  }

  /**
   * Get slider tooltip position for duration
   */
  getDurationSliderTooltipPosition(): string {
    const percentage = ((this.financeDetails.months - this.financeDetails.minTerm) / (this.financeDetails.maxTerm - this.financeDetails.minTerm)) * 100;
    return `${percentage}%`;
  }

  onEdit(section: string) {
    if (section === 'information') {
      this.isEditingInfo = true;
    } else if (section === 'finance') {
      this.isEditingFinance = true;
    }
  }

  onSave(section: string) {
    if (section === 'information') {
      // Update the full name when saving
      this.borrowerInfo.name = `${this.borrowerInfo.title} ${this.borrowerInfo.firstName} ${this.borrowerInfo.lastName}`.trim();
      this.isEditingInfo = false;
    } else if (section === 'finance') {
      // Call backend to recalculate with new values
      this.recalculateLoan();
    }
  }

  onCancel(section: string) {
    if (section === 'information') {
      this.isEditingInfo = false;
    } else if (section === 'finance') {
      this.isEditingFinance = false;
    }
  }

  /**
   * Recalculate loan using backend API when user changes amount or term
   */
  recalculateLoan(): void {
    if (!this.quoteData) {
      console.error('No quote data available for recalculation');
      return;
    }

    this.isRecalculating = true;

    this.loanCalculationService.recalculateLoan(
      this.quoteData.productId,
      this.financeDetails.amount,
      this.financeDetails.months,
      this.financeDetails.frequency as 'Monthly'
    ).subscribe({
      next: (calculation) => {
        console.log('Recalculation result:', calculation);
        
        // Update finance details with backend calculations
        this.financeDetails.repaymentAmount = calculation.paymentAmount;
        this.financeDetails.interest = calculation.totalInterest;
        this.financeDetails.totalRepayment = calculation.totalRepayment;
        this.financeDetails.establishmentFee = calculation.establishmentFee;
        
        // Update quote data
        if (this.quoteData) {
          this.quoteData.loanAmount = calculation.loanAmount;
          this.quoteData.term = calculation.termMonths;
          this.quoteData.interest = calculation.totalInterest;
          this.quoteData.totalAmount = calculation.totalRepayment;
          this.quoteData.monthlyPayment = calculation.paymentAmount;
          this.quoteData.establishmentFee = calculation.establishmentFee;
        }
        
        this.isEditingFinance = false;
        this.isRecalculating = false;
      },
      error: (error: Error) => {
        console.error('Error recalculating loan:', error);
        alert(`Failed to recalculate: ${error.message}`);
        this.isRecalculating = false;
      }
    });
  }

  onApplyNow() {
    console.log('Apply now clicked');
    console.log('Borrower Info:', this.borrowerInfo);
    console.log('Finance Details:', this.financeDetails);
    console.log('Original Quote Data:', this.quoteData);
    
    // Prepare application data
    const applicationData = {
      quoteData: this.quoteData,
      borrowerInfo: {
        title: this.borrowerInfo.title,
        firstName: this.borrowerInfo.firstName,
        lastName: this.borrowerInfo.lastName,
        fullName: this.borrowerInfo.name,
        dateOfBirth: this.borrowerInfo.dateOfBirth,
        mobile: this.borrowerInfo.mobile,
        email: this.borrowerInfo.email
      },
      financeDetails: this.financeDetails,
      timestamp: new Date().toISOString()
    };
    
    console.log('Navigating to loan confirmation page with application data:', applicationData);
    
    // Navigate to loan confirmation page with application data
    this.router.navigate(['/loan-confirmation'], {
      state: { applicationData: applicationData }
    });
  }
}