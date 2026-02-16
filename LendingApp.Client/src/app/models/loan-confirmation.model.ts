export interface LoanConfirmationRequest {
  quoteId?: string;
  productId: string;
  productName: string;
  
  // Borrower Information
  borrowerDetails: {
    title: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    fullName: string;
    mobileNumber: string;
    email: string;
  };
  
  // Loan Details
  loanAmount: number;
  termMonths: number;
  monthlyPayment: number;
  totalRepayment: number;
  establishmentFee: number;
  totalInterest: number;
  frequency: string;
  
  // Interest-Free Details (if applicable)
  interestFreeMonths?: number | 'all';
  
  // Metadata
  applicationTimestamp: string;
  ipAddress?:string;
  userAgent?: string;
}

export interface LoanConfirmationResponse {
  success: boolean;
  confirmationNumber: string;
  applicationId: string;
  message: string;
  timestamp: string;
  nextSteps?: string[];
  estimatedProcessingTime?: string;
}

export interface ConfirmationError {
  code: string;
  message: string;
  details?: string[];
  timestamp: string;
}