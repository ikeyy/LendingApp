import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map, retry } from 'rxjs/operators';
import { 
  LoanConfirmationRequest, 
  LoanConfirmationResponse, 
  ConfirmationError 
} from '../models/loan-confirmation.model';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class LoanConfirmationService {
  private apiUrl: string;

  constructor(private http: HttpClient) {
    // Use environment configuration for API URL
    this.apiUrl = environment.apiUrl;
  }

  submitLoanConfirmation(
    confirmationData: LoanConfirmationRequest
  ): Observable<LoanConfirmationResponse> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    // Add user agent and timestamp if not provided
    const enrichedData: LoanConfirmationRequest = {
      ...confirmationData,
      applicationTimestamp: confirmationData.applicationTimestamp || new Date().toISOString(),
      userAgent: confirmationData.userAgent || navigator.userAgent
    };

    console.log('Submitting loan confirmation:', enrichedData);

    return this.http.post<LoanConfirmationResponse>(
      `${this.apiUrl}/api/LoanConfirmation`,
      enrichedData,
      { headers }
    ).pipe(
      retry(1), // Retry once on failure
      map(response => {
        console.log('Confirmation successful:', response);
        return response;
      }),
      catchError((error) => this.handleError(error))
    );
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage: ConfirmationError;

    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = {
        code: 'CLIENT_ERROR',
        message: 'A network error occurred. Please check your connection and try again.',
        details: [error.error.message],
        timestamp: new Date().toISOString()
      };
    } else {
      console.log(error.error)
      // Backend returned an unsuccessful response code
      errorMessage = {
        code: `HTTP_${error.status}`,
        message: this.getErrorMessage(error.status),
        details: this.extractErrorDetails(error),
        timestamp: new Date().toISOString()
      };
    }

    console.error('Loan confirmation error:', errorMessage);
    return throwError(() => errorMessage);
  }

  private getErrorMessage(status: number): string {
    switch (status) {
      case 400:
        return 'Invalid application data. Please check your information and try again.';
      case 500:
        return 'Server error. Please try again later.';
      default:
        return 'An unexpected error occurred. Please try again.';
    }
  }

  private extractErrorDetails(error: HttpErrorResponse): string[] {
  const errorBody = error.error;

  // Handle array of error objects
  if (errorBody?.errors && Array.isArray(errorBody.errors)) {
    return errorBody.errors.map((err: any) => {
      if (err.error && err.property) {
        return `${err.error}`;
      }
      return err.error || JSON.stringify(err);
    });
  }

  return [error.message || 'An unexpected error occurred'];
}
}