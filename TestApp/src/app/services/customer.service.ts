import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { CustomerModel } from '../modules/customer/models/customer-model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private baseUrl: string;
  private api = 'api/Customer';

  constructor(
    @Inject('BASE_APP') baseUrl: string,
    private http: HttpClient,
  ) {
    this.baseUrl = baseUrl;
  }

  saveCustomer(customer: CustomerModel): Observable<any> {
    return this.http.post(`${this.baseUrl}${this.api}/save-customer`, customer);
  }

  changeStatus(customer: CustomerModel): Observable<any> {
    return this.http.put(`${this.baseUrl}${this.api}/change-status-customer`, customer);
  }

  getAllCustomers(ruc?: string): Observable<any> {
    let request = `${this.baseUrl}${this.api}/get-customers`
    if (ruc != null && ruc != '') request += `?ruc=${ruc}`
    return this.http.get(request);
  }
  getCustomerById(idCustomer?: number): Observable<any> {
    let request = `${this.baseUrl}${this.api}/get-customer-by-id?idCustomer=${idCustomer}`
    return this.http.get(request);
  }



}
