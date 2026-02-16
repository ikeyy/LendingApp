// src/app/services/loan-request.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { BorrowerData } from '../models/borrower-data.model';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class LoanRequestService {
  private apiUrl: string;
  private currentBorrowerSubject = new BehaviorSubject<BorrowerData | null>(null);
  public currentBorrower$ = this.currentBorrowerSubject.asObservable();

  constructor(private http: HttpClient) {
      // Use environment configuration for API URL
      this.apiUrl = environment.apiUrl;
  }

  /**
   * Get loan request data by ID
   */
  getLoanRequestById(id: string): Observable<BorrowerData> {
  const params = new HttpParams().set('Id', id);

  return this.http.get<any>(`${this.apiUrl}/api/LoanRequest`, { params }).pipe(
    tap(response => {
      console.log('Raw API Response:', response);
      console.log('Response type:', typeof response);
      console.log('Response keys:', Object.keys(response));
    }),
    map(response => {
      // Handle different response structures
      if (response.value) {
        return response.value;
      } else if (response.title) {
        return response;
      } else {
        throw new Error('Unexpected response structure');
      }
    }),
    tap(borrower => {
      console.log('Parsed Borrower data:', borrower);
      this.currentBorrowerSubject.next(borrower);
    }),
    catchError(this.handleError)
  );
}

  /**
   * Get loan request data by ID (alternative direct value response)
   * Use this if your API returns the value directly without wrapping
   */
  getLoanRequestByIdDirect(id: string): Observable<BorrowerData> {
    const params = new HttpParams().set('Id', id);

    return this.http.get<BorrowerData>(`${this.apiUrl}`, { params }).pipe(
      tap(borrower => {
        console.log('Borrower data fetched successfully:', borrower);
        this.currentBorrowerSubject.next(borrower);
      }),
      catchError(this.handleError)
    );
  }

  /**
   * Format date of birth to readable format
   */
  formatDateOfBirth(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'long', 
      day: 'numeric' 
    });
  }

  /**
   * Calculate age from date of birth
   */
  calculateAge(dateOfBirth: string): number {
    const dob = new Date(dateOfBirth);
    const today = new Date();
    let age = today.getFullYear() - dob.getFullYear();
    const monthDiff = today.getMonth() - dob.getMonth();
    
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
      age--;
    }
    
    return age;
  }

  /**
   * Format currency
   */
  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(amount);
  }

  /**
   * Clear current borrower data
   */
  clearCurrentBorrower(): void {
    this.currentBorrowerSubject.next(null);
  }

  /**
   * Get current borrower value (synchronous)
   */
  getCurrentBorrower(): BorrowerData | null {
    return this.currentBorrowerSubject.value;
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code
      switch (error.status) {
        case 404:
          errorMessage = 'Loan request not found';
          break;
        case 400:
          errorMessage = 'Invalid request parameters';
          break;
        case 500:
          errorMessage = 'Server error occurred';
          break;
        default:
          errorMessage = `Server Error: ${error.status}\nMessage: ${error.message}`;
      }
    }

    console.error('LoanRequestService Error:', errorMessage, error);
    return throwError(() => new Error(errorMessage));
  }
}