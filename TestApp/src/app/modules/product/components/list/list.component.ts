import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../services/product.service';
import { Router } from '@angular/router';
import { ProductModel } from '../../models/product-model';
import { NgZorroModule } from '../../../../ng-zorro.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';


@Component({
  selector: 'app-list',
  imports: [NgZorroModule, FormsModule, ReactiveFormsModule, CommonModule],
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

  listProducts: ProductModel[] = [];

  constructor(private _productService: ProductService, private _routerService: Router) { }


  async ngOnInit() {
    await this.getAllProducts()
  }


  async getAllProducts() {
    this.listProducts = [];
    const response = await this._productService.getAllProducts().toPromise();
    if (response.data) this.listProducts = response.data;
  }

  addProduct() {
    this._routerService.navigateByUrl('product/form');
  }



}
