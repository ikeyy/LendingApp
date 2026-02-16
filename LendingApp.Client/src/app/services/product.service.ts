import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Product } from '../models/product.model';
import { ApiProduct } from '../models/api-product.model';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl: string;
  
  constructor(private http: HttpClient) {
      // Use environment configuration for API URL
      this.apiUrl = environment.apiUrl;
  }

  getProduct(): Observable<Product[]> {
    return this.http.get<ApiProduct[]>(`${this.apiUrl}/api/Product`).pipe(
      map(products => products.map(p => this.transformToProductConfig(p))),
      catchError(this.handleError)
    );
  }

  private transformToProductConfig(apiProduct: ApiProduct): Product {
    return {
      id: apiProduct.id,
      displayName: apiProduct.displayName,
      interestRate: apiProduct.interestRate,
      interestFreeMonths: apiProduct.interestFreeMonths === 'all' 
        ? 'all' 
        : Number(apiProduct.interestFreeMonths),
      minTermMonths: apiProduct.minTermMonths,
      maxTermMonths: apiProduct.maxTermMonths,
      minLoanAmount: apiProduct.minLoanAmount,
      maxLoanAmount: apiProduct.maxLoanAmount,
      defaultTermMonths: apiProduct.defaultTermMonths,
      description: apiProduct.description
    };
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Server Error: ${error.status}\nMessage: ${error.message}`;
    }

    console.error('ProductService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}