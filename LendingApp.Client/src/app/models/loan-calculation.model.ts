export interface LoanCalculationRequest {
  productId: string;
  loanAmount: number;
  termMonths: number;
  establishmentFee?: number;
  paymentFrequency?: 'Weekly' | 'Monthly';
}

export interface LoanCalculationResponse {
  loanAmount: number;
  termMonths: number;
  establishmentFee: number;
  interestRate: number;
  interestFreeMonths: number | 'all';
  totalInterest: number;
  totalRepayment: number;
  paymentAmount: number;
  paymentFrequency: string;
  paymentsPerYear: number;
  totalPayments: number;
  productId: string;
  productName: string;
  calculatedAt: string;
}