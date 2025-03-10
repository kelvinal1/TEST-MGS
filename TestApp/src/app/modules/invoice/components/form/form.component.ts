import { Component, OnInit } from '@angular/core';
import { NgZorroModule } from '../../../../ng-zorro.module';
import { FormGroup, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerModel } from '../../../customer/models/customer-model';
import { CustomerService } from '../../../../services/customer.service';
import { ProductModel } from '../../../product/models/product-model';
import { ProductService } from '../../../../services/product.service';
import { InvoiceItemModel } from '../../models/invoice-item-model';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { InvoiceGeneralDTO } from '../../models/invoice-general-dto';
import { InvoiceGeneralService } from '../../../../services/invoice-general.service';
import { InvoiceModel } from '../../models/invoice-model';

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

  codeInvoice: string = '';
  customerSearch: string = '';
  productSearch: string = '';

  drawerCustomer: boolean = false;
  drawerProduct: boolean = false;
  modalItem: boolean = false;

  currentDate = new Date();

  validateForm!: FormGroup;

  totalGeneral: number = 0;

  customerSelect?: CustomerModel;
  listCustomersFilter: CustomerModel[] = [];
  listProductsFilter: ProductModel[] = [];

  invoiceSearch?: InvoiceGeneralDTO;

  listItems: InvoiceItemModel[] = [];
  itemUpdate?: InvoiceItemModel;

  constructor(private _activatedRouteService: ActivatedRoute, private _fbService: NonNullableFormBuilder, private _routerService: Router, private _customerService: CustomerService,
    private _productService: ProductService, private _ntService: NzNotificationService, private _invoiceGeneralService: InvoiceGeneralService
  ) { }

  ngOnInit() {
    this._activatedRouteService.queryParams.subscribe(async params => {
      this.codeInvoice = params['codeInvoice'];
      await this.buildForm();
      await this.verificateInvoice()
    });
  }

  async save() {
    if (await this.validateItems()) {
      try {
        const response = await this._invoiceGeneralService.saveInvoice(this.buildItemToSave(this.customerSelect!, this.listItems)).toPromise();
        this._ntService.success('PEDIDO GENERADO EXITOSAMENTE', '')
        this._routerService.navigateByUrl('invoice');
      } catch (error) {
        this._ntService.error('ERROR AL GENERAR PEDIDO', '')
      }
    }
  }

  buildForm() {
    this.validateForm = this._fbService.group({
      i_id: this._fbService.control(null),
      i_customer: this._fbService.control(null, [Validators.required]),
      i_date: this._fbService.control(null, [Validators.required]),
      i_total: this._fbService.control(null),
      i_code: this._fbService.control(null),
    });
  }

  back() {
    this._routerService.navigateByUrl('invoice');
  }

  closeCustomers() {
    this.drawerCustomer = false;
    this.customerSearch = "";
    this.listCustomersFilter = [];
  }

  closeProducts() {
    this.drawerProduct = false;
    this.productSearch = "";
    this.listProductsFilter = [];
  }

  async searchCustomer() {
    if (this.customerSearch == '' || this.customerSearch == null) {
      this.listCustomersFilter = [];
      return;
    } else {
      const response = await this._customerService.getAllCustomers(this.customerSearch).toPromise();
      console.log(response.data)
      if (response.data) this.listCustomersFilter = response.data;
    }
  }

  addItemToList(product: ProductModel) {
    if (this.validateItem(product)) {
      let item = new InvoiceItemModel();
      item.ii_price = product.p_price;
      item.ii_subtotal = 0;
      item.ii_total = 0;
      item.ii_product = product.p_id;
      item.product_name = product.p_description;
      this.listItems.push(item);
      this.closeProducts();
    } else {
      this._ntService.error('Producto agregado previamente', '');
    }
  }


  validateItem(product: ProductModel): boolean {
    if (this.listItems.length == 0) return true;
    if (this.listItems.length >= 1) {
      let findItem = this.listItems.find(x => x.ii_product == product.p_id);
      if (findItem) return false;
      return true;
    } else {
      return true;
    }
  }

  async searchProduct() {
    if (this.productSearch == '' || this.productSearch == null) {
      this.listProductsFilter = [];
      return;
    } else {
      const response = await this._productService.getAllProducts(this.productSearch).toPromise();
      console.log(response)
      if (response.data) this.listProductsFilter = response.data;
    }
  }

  selectCustomer(customer: CustomerModel) {
    this.closeCustomers();
    this.customerSelect = customer;
  }

  editItem(item: InvoiceItemModel) {
    this.itemUpdate = item;
    this.modalItem = true;
  }

  cancelItem() {
    this.itemUpdate = undefined;
    this.modalItem = false;
  }

  async updateItem() {
    if (this.itemUpdate != null) {
      await this.calculateAndSave()
      this.cancelItem()
    }
  }

  async calculateAndSave() {
    if (this.itemUpdate!.ii_price === undefined || this.itemUpdate!.ii_quantity === undefined || this.itemUpdate!.ii_iva === undefined) {
      console.error("Faltan datos para calcular el subtotal y el total.");
      return;
    }
    this.itemUpdate!.ii_subtotal = this.itemUpdate!.ii_price * this.itemUpdate!.ii_quantity!;
    const ivaAmount = this.itemUpdate!.ii_subtotal * (this.itemUpdate!.ii_iva! / 100);
    this.itemUpdate!.ii_total = this.itemUpdate!.ii_subtotal + ivaAmount;
    this.listItems.forEach(x => {
      if (x.ii_product == this.itemUpdate?.ii_product) {
        x = this.itemUpdate!;
      }
    });
    this.updateTotalGeneral();
  }

  updateTotalGeneral() {
    this.totalGeneral = 0;
    this.listItems.forEach(x => {
      if (x.ii_total != null) this.totalGeneral += x.ii_total!;
    });
  }

  async deleteItem(item: InvoiceItemModel) {
    if (item.ii_id != null) {
      item.ii_state = 2;
      const response = await this._invoiceGeneralService.changeStatusItemInvoice(item).toPromise()
      if (response) {
        const index = this.listItems.findIndex(item => item.ii_product === item.ii_product);
        if (index !== -1) {
          this.listItems.splice(index, 1);
          this.updateTotalGeneral();
        }
      }
    } else {
      const index = this.listItems.findIndex(item => item.ii_product === item.ii_product);
      if (index !== -1) {
        this.listItems.splice(index, 1);
        this.updateTotalGeneral();
      }
    }
  }


  async validateItems() {
    if (this.customerSelect == null || this.customerSelect == undefined) {
      this._ntService.error('CLIENTE NO SELECCIONADO', 'Seleccione un cliente');
      return false;
    }

    if (this.listItems.length == 0) {
      this._ntService.error('SIN ITEMS PARA PEDIDO', 'Ingrese items para el pedido');
      return false;
    }
    for (let x of this.listItems) {
      if (x.ii_quantity != null) {
        const response = await this._productService.getAllProductsById(x.ii_product).toPromise();
        if (response.data) {
          if (x.ii_quantity > response.data[0].p_stock) {
            this._ntService.error('STOCK INSUFICIENTE', 'La cantidad del producto ' + x.product_name + " excede al stock disponible");
            return false;  // Retornamos inmediatamente en caso de error
          }
        } else {
          this._ntService.error('PRODUCTO NO ENCONTRADO', 'No se pudo encontrar el producto ' + x.product_name);
          return false;  // Retornamos inmediatamente si no se encuentra el producto
        }
      } else {
        this._ntService.error('CANTIDAD VACIA', 'La cantidad del producto ' + x.product_name + " no tiene valores");
        return false;  // Retornamos inmediatamente si no hay cantidad para el producto
      }
    }
    return true;
  }


  buildItemToSave(customer: CustomerModel, items: InvoiceItemModel[]): InvoiceGeneralDTO {
    let invoice = new InvoiceGeneralDTO();
    invoice.invoice = new InvoiceModel();
    console.log(customer.c_id)
    if (this.codeInvoice != '' && this.codeInvoice != null) {
      invoice.invoice.i_code = this.codeInvoice;
      invoice.invoice.i_id = this.invoiceSearch?.invoice?.i_id;
    }
    invoice.invoice!.i_customer = customer.c_id;
    invoice.invoice!.i_total = this.totalGeneral;
    invoice.invoice!.i_date = this.currentDate;
    invoice.items = items;
    return invoice;
  }

  async verificateInvoice() {
    if (this.codeInvoice == '' || this.codeInvoice == null) return;
    const response = await this._invoiceGeneralService.getAllInvoicesBySearch(this.codeInvoice).toPromise();
    if (response.data) {
      this.invoiceSearch = response.data[0];
      if (this.invoiceSearch != null) {
        if (this.invoiceSearch.items!.length >= 1) this.listItems = this.invoiceSearch?.items!;
        this.totalGeneral = this.invoiceSearch.invoice?.i_total!;
        const responseCustomer = await this._customerService.getCustomerById(this.invoiceSearch.invoice?.i_customer).toPromise();
        if (responseCustomer.data) this.customerSelect = responseCustomer.data;
      }
    }
    console.log(this.invoiceSearch);
  }
}
