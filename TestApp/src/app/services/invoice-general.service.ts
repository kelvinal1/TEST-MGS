import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InvoiceGeneralDTO } from '../modules/invoice/models/invoice-general-dto';
import { InvoiceItemModel } from '../modules/invoice/models/invoice-item-model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceGeneralService {
  private baseUrl: string;
  private api = 'api/InvoiceGeneral';

  constructor(
    @Inject('BASE_APP') baseUrl: string,
    private http: HttpClient,
  ) {
    this.baseUrl = baseUrl;
  }


  getAllInvoices(): Observable<any> {
    let request = `${this.baseUrl}${this.api}/get-all-invoices`
    return this.http.get(request);
  }
  getAllInvoicesBySearch(search: string): Observable<any> {
    let request = `${this.baseUrl}${this.api}/get-all-invoices?search=${search}`
    return this.http.get(request);
  }
  saveInvoice(invoice: InvoiceGeneralDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}${this.api}/save-invoice`, invoice);
  }
  changeStatusInvoice(idInvoice: number): Observable<any> {
    return this.http.put(`${this.baseUrl}${this.api}/change-status-invoice?idInvoice=${idInvoice}`, null);
  }
  changeStatusItemInvoice(item: InvoiceItemModel): Observable<any> {
    return this.http.put(`${this.baseUrl}${this.api}/change-status-item-invoice`, item);
  }
}
