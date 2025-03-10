import { Component, OnInit } from '@angular/core';
import { NgZorroModule } from '../../../../ng-zorro.module';
import { FormGroup, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CustomerModel } from '../../models/customer-model';
import { CustomerService } from '../../../../services/customer.service';
import { Router } from '@angular/router';
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
export class FormComponent implements OnInit {

  validateForm!: FormGroup;
  constructor(private _fbService: NonNullableFormBuilder, private _customerService: CustomerService, private _routerService: Router) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.validateForm = this._fbService.group({
      c_id: this._fbService.control(null),
      c_name: this._fbService.control(null, [Validators.required]),
      c_ruc: this._fbService.control(null, [Validators.required]),
      c_state: this._fbService.control(1),
    });
  }


  async save() {
    if (this.validateForm.valid) {
      let saveCustomer = new CustomerModel();
      saveCustomer = this.validateForm.value;
      const response = await this._customerService.saveCustomer(saveCustomer).toPromise();
      if (response.data) {
        this._routerService.navigateByUrl('customer')
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
