import { Component } from '@angular/core';
import { NgZorroModule } from '../../ng-zorro.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [NgZorroModule, ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {


  constructor(private _routerService: Router) { }

  goOrders() {
    this._routerService.navigateByUrl('invoice');
  }

  goCustomers() {
    this._routerService.navigateByUrl('customer');
  }

  goProducts() {
    this._routerService.navigateByUrl('product');
  }

}
