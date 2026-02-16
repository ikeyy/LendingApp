// src/app/models/borrower-data.model.ts
export interface BorrowerData {
  title: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  mobile: string;
  email: string;
  loanAmount: number;
  term: number;
}

export interface BorrowerDataResponse {
  value: BorrowerData;
}