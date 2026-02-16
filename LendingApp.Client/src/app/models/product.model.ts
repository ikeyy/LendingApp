export interface Product {
  id: string;
  displayName: string;
  interestRate: number;
  interestFreeMonths: number | 'all';
  minTermMonths: number;
  maxTermMonths: number;
  minLoanAmount: number;
  maxLoanAmount: number;
  defaultTermMonths: number;
  description?: string;
}