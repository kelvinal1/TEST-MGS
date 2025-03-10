import { Component } from '@angular/core';
import { FormGroup, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../../../services/product.service';
import { Router } from '@angular/router';
import { ProductModel } from '../../models/product-model';
import { NgZorroModule } from '../../../../ng-zorro.module';
import { CommonModule } from '@angular/common';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-form',
  imports: [NgZorroModule, ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './form.component.html',
  styleUrl: './form.component.css',
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
export class FormComponent {

  validateForm!: FormGroup;
  constructor(private _fbService: NonNullableFormBuilder, private _productService: ProductService, private _routerService: Router) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.validateForm = this._fbService.group({
      p_id: this._fbService.control(null),
      p_description: this._fbService.control(null, [Validators.required]),
      p_bar_code: this._fbService.control(null,),
      p_price: this._fbService.control(null, [Validators.required]),
      p_stock: this._fbService.control(null, [Validators.required]),
      p_state: this._fbService.control(1),
    });
  }


  async save() {
    if (this.validateForm.valid) {
      let saveProduct = new ProductModel();
      saveProduct = this.validateForm.value;
      const response = await this._productService.saveProduct(saveProduct).toPromise();
      if (response.data) {
        this._routerService.navigateByUrl('product')
      }
    } else {
      Object.values(this.validateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
