export interface ApiProduct {
  id: string;
  name: string;
  displayName: string;
  interestRate: number;
  interestFreeMonths: number | string;
  minTermMonths: number;
  maxTermMonths: number;
  minLoanAmount: number;
  maxLoanAmount: number;
  defaultTermMonths: number;
  description?: string;
  isActive?: boolean;
}