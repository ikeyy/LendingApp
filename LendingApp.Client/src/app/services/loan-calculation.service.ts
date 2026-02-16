// src/app/services/loan-calculation.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../environments/environment.development';
import { LoanCalculationRequest, LoanCalculationResponse } from '../models/loan-calculation.model';
import { QuoteRequest, QuoteResponse } from '../models/quote.model';

@Injectable({
  providedIn: 'root'
})
export class LoanCalculationService {
  private apiUrl: string;

  constructor(private http: HttpClient) {
    // Use environment configuration for API URL
    this.apiUrl = environment.apiUrl;
  }

  /**
   * Calculate loan repayment details
   * This calls the backend to perform all financial calculations
   */
  calculateLoan(request: LoanCalculationRequest): Observable<LoanCalculationResponse> {
    const url = `${this.apiUrl}/api/LoanCalculation/calculate`;
    
    return this.http.post<LoanCalculationResponse>(url, request, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Create a complete quote with customer details
   * This generates a quote that can be saved and retrieved later
   */
  createQuote(request: QuoteRequest): Observable<QuoteResponse> {
    const url = `${this.apiUrl}/api/quotes/create`;
    
    return this.http.post<QuoteResponse>(url, request, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get an existing quote by ID
   */
  getQuoteById(quoteId: string): Observable<QuoteResponse> {
    const url = `${this.apiUrl}/api/quotes/${quoteId}`;
    
    return this.http.get<QuoteResponse>(url).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Recalculate loan when user adjusts amount or term in quote page
   */
  recalculateLoan(
    productId: string,
    loanAmount: number,
    termMonths: number,
    paymentFrequency: 'Monthly'
  ): Observable<LoanCalculationResponse> {
    return this.calculateLoan({
      productId,
      loanAmount,
      termMonths,
      paymentFrequency
    });
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: any): Observable<never> {
    let errorMessage = 'An error occurred while processing your request.';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.status === 400) {
        errorMessage = error.error?.message || 'Invalid request parameters.';
      } else if (error.status === 404) {
        errorMessage = 'Resource not found.';
      } else if (error.status === 500) {
        errorMessage = 'Server error. Please try again later.';
      } else if (error.error?.message) {
        errorMessage = error.error.message;
      }
    }
    
    console.error('Loan calculation error:', error);
    return throwError(() => new Error(errorMessage));
  }
}