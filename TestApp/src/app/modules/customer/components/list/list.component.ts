import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../../../../services/customer.service';
import { CustomerModel } from '../../models/customer-model';
import { NgZorroModule } from '../../../../ng-zorro.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';


@Component({
  selector: 'app-list',
  imports: [NgZorroModule, ReactiveFormsModule, FormsModule,],
  templateUrl: './list.component.html',
  styleUrl: './list.component.css',
  animations: [
    trigger('blurFocusAnimation', [
      transition(':enter', [
        animate('800ms ease-in-out', keyframes([
          style({ filter: 'blur(10px)', opacity: 0, offset: 0 }),
          style({ filter: 'blur(5px)', opacity: 0.5, offset: 0.5 }),
          style({ filter: 'blur(0px)', opacity: 1, offset: 1 }),
        ]))
      ])
    ])
  ]
})
export class ListComponent implements OnInit {

  loadingList: boolean = false;

  listCustomers: CustomerModel[] = [];

  textSearch: string = "";

  constructor(private _customerService: CustomerService, private _routerService: Router) { }
  async ngOnInit() {
    await this.getAllCustomers()
  }

  addCustomer() {
    this._routerService.navigateByUrl('customer/form');
  }

  async getAllCustomers() {
    this.listCustomers = [];
    const response = await this._customerService.getAllCustomers().toPromise();
    if (response.data) this.listCustomers = response.data;
  }


  async getCustomersByRuc() {
    if (this.textSearch == '') return;
    const response = await this._customerService.getAllCustomers(this.textSearch).toPromise();
    if (response.data) this.listCustomers = response.data;
  }

}
