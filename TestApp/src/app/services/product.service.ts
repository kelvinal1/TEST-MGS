import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ProductModel } from '../modules/product/models/product-model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl: string;
  private api = 'api/Product';

  constructor(
    @Inject('BASE_APP') baseUrl: string,
    private http: HttpClient,
  ) {
    this.baseUrl = baseUrl;
  }

  saveProduct(product: ProductModel): Observable<any> {
    return this.http.post(`${this.baseUrl}${this.api}/save-product`, product);
  }

  changeStatus(customer: ProductModel): Observable<any> {
    return this.http.put(`${this.baseUrl}${this.api}/change-status-product`, customer);
  }

  getAllProducts(description?: string, idProduct?: number): Observable<any> {
    let request = `${this.baseUrl}${this.api}/get-products`
    if (description != null && description != '') request += `?description=${description}`
    return this.http.get(request);
  }
  getAllProductsById(idProduct?: number): Observable<any> {
    let request = `${this.baseUrl}${this.api}/get-products`
    if (idProduct != null && idProduct != 0) request += `?idProduct=${idProduct}`
    return this.http.get(request);
  }
}
