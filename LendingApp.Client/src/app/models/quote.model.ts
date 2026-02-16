export interface QuoteRequest {
  productId: string;
  loanRequestId: string | null;
  loanAmount: number;
  termMonths: number;
  borrowerDetails: {
    title: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    email: string;
    mobileNumber: string;
  };
}

export interface QuoteResponse {
  quoteId: string;
  loanRequestId: string | null;
  product: string;
  productId: string;
  loanAmount: number;
  term: number;
  interest: number;
  totalAmount: number;
  monthlyPayment: number;
  establishmentFee: number;
  interestRate: number;
  interestFreeMonths: number | 'all';
  minLoan: number;
  maxLoan: number;
  minTerm: number;
  maxTerm: number;
  borrowerDetails: {
    title: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    age: number | null;
    email: string;
    mobileNumber: string;
  };
  createdAt: string;
}