import { Routes } from '@angular/router';

export const routes: Routes = [
    // Set the landing page component for the root path
  { 
    path: '', 
    loadComponent: () => import('./components/quote-calculator/quote-calculator.component')
      .then(m => m.QuoteCalculatorComponent),
    pathMatch: 'full' 
  },
  // Borrower quote component route
  { 
    path: 'borrower-quote', 
    loadComponent: () => import('./components/borrower-quote/borrower-quote.component')
      .then(m => m.BorrowerQuoteComponent)
  },
  { 
    path: 'loan-confirmation', 
    loadComponent: () => import('./components/loan-confirmation/loan-confirmation.component')
      .then(m => m.LoanConfirmationComponent)
  },
];
