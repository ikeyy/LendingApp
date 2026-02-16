import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoanConfirmationService } from '../../services/loan-confirmation.service';
import { 
  LoanConfirmationRequest, 
  LoanConfirmationResponse,
  ConfirmationError 
} from '../../models/loan-confirmation.model';

@Component({
  selector: 'app-loan-confirmation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loan-confirmation.component.html',
  styleUrls: ['./loan-confirmation.component.css']
})
export class LoanConfirmationComponent implements OnInit {
  // State management
  isSubmitting = true;
  isSuccess = false;
  isError = false;
  
  // Response data
  confirmationResponse: LoanConfirmationResponse | null = null;
  errorDetails: ConfirmationError | null = null;
  
  // Application data from navigation
  applicationData: any = null;
  
  // Animation states
  showConfetti = false;
  animateIn = false;

  constructor(
    private router: Router,
    private loanConfirmationService: LoanConfirmationService
  ) {
    // Get the navigation state containing application data
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras?.state) {
      this.applicationData = navigation.extras.state['applicationData'];
    }
  }

  ngOnInit(): void {
    console.log('Application Success Page - Received data:', this.applicationData);

    // Trigger animation after short delay
    setTimeout(() => {
      this.animateIn = true;
    }, 100);

    if (!this.applicationData) {
      console.error('No application data found');
      this.handleError({
        code: 'NO_DATA',
        message: 'Application data not found. Please start a new application.',
        timestamp: new Date().toISOString()
      });
      return;
    }

    // Submit the application to the backend
    this.submitApplication();
  }

  /**
   * Submit application to the backend API
   */
  submitApplication(): void {
    const confirmationRequest = this.mapToConfirmationRequest(this.applicationData);
    
    console.log('Submitting confirmation request:', confirmationRequest);

    this.loanConfirmationService.submitLoanConfirmation(confirmationRequest)
      .subscribe({
        next: (response) => {
          console.log('Application submitted successfully:', response);
          this.handleSuccess(response);
        },
        error: (error: ConfirmationError) => {
          console.error('Application submission failed:', error);
          this.handleError(error);
        }
      });
  }

  /**
   * Map application data to confirmation request format
   */
  private mapToConfirmationRequest(data: any): LoanConfirmationRequest {
    return {
      quoteId: data.quoteData?.quoteId,
      productId: data.quoteData?.productId || '',
      productName: data.quoteData?.product || '',
      
      borrowerDetails: {
        title: data.borrowerInfo.title || '',
        firstName: data.borrowerInfo.firstName || '',
        lastName: data.borrowerInfo.lastName || '',
        fullName: `${data.borrowerInfo.lastName}, ${data.borrowerInfo.firstName}` || data.borrowerInfo.name || '',
        dateOfBirth: data.borrowerInfo.dateOfBirth,
        mobileNumber: data.borrowerInfo.mobile || '',
        email: data.borrowerInfo.email || ''
      },
      
      loanAmount: data.financeDetails.amount || data.quoteData?.loanAmount || 0,
      termMonths: data.financeDetails.months || data.quoteData?.term || 0,
      monthlyPayment: data.financeDetails.repaymentAmount || data.quoteData?.monthlyPayment || 0,
      totalRepayment: data.financeDetails.totalRepayment || data.quoteData?.totalAmount || 0,
      establishmentFee: data.financeDetails.establishmentFee || data.quoteData?.establishmentFee || 0,
      totalInterest: data.financeDetails.interest || data.quoteData?.interest || 0,
      frequency: data.financeDetails.frequency || 'Monthly',
      
      interestFreeMonths: data.quoteData?.interestFreeMonths,
      
      applicationTimestamp: data.timestamp || new Date().toISOString()
    };
  }

  /**
   * Handle successful submission
   */
  private handleSuccess(response: LoanConfirmationResponse): void {
    this.isSubmitting = false;
    this.isSuccess = true;
    this.confirmationResponse = response;
    
    // Trigger confetti animation
    setTimeout(() => {
      this.showConfetti = true;
    }, 300);
    
    // Stop confetti after 3 seconds
    setTimeout(() => {
      this.showConfetti = false;
    }, 3300);
  }

  /**
   * Handle submission error
   */
  private handleError(error: ConfirmationError): void {
    this.isSubmitting = false;
    this.isError = true;
    this.errorDetails = error;
  }

  /**
   * Try submission again
   */
  retrySubmission(): void {
    this.isSubmitting = true;
    this.isError = false;
    this.errorDetails = null;
    this.submitApplication();
  }

  /**
   * Copy confirmation number to clipboard
   */
  copyConfirmationNumber(): void {
    if (this.confirmationResponse?.confirmationNumber) {
      navigator.clipboard.writeText(this.confirmationResponse.confirmationNumber)
        .then(() => {
          // Show temporary success message
          const btn = document.querySelector('.copy-btn');
          if (btn) {
            btn.textContent = 'Copied!';
            setTimeout(() => {
              btn.textContent = 'Copy';
            }, 2000);
          }
        })
        .catch(err => {
          console.error('Failed to copy:', err);
        });
    }
  }

  /**
   * Format currency
   */
  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2
    }).format(amount);
  }

  /**
   * Format date
   */
  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}