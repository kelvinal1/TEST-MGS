import { Component, OnInit } from '@angular/core';
import { NgZorroModule } from '../../../../ng-zorro.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InvoiceGeneralService } from '../../../../services/invoice-general.service';
import { InvoiceGeneralDTO } from '../../models/invoice-general-dto';
import { CommonModule } from '@angular/common';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import { Router } from '@angular/router';
import { InvoiceModel } from '../../models/invoice-model';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';


@Component({
  selector: 'app-list',
  imports: [NgZorroModule, ReactiveFormsModule, FormsModule, CommonModule],
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

  drawerDetails: boolean = false;
  listInvoices: InvoiceGeneralDTO[] = [];
  invoiceSelect?: InvoiceGeneralDTO;

  constructor(private _invoiceGeneralService: InvoiceGeneralService, private _routeService: Router,
    private _modalService: NzModalService, private _ntService: NzNotificationService
  ) { }

  async ngOnInit() {
    await this.getAllInvoices();
  }


  async getAllInvoices() {
    const response = await this._invoiceGeneralService.getAllInvoices().toPromise();
    if (response.data) this.listInvoices = response.data;
    console.log(response);
  }

  addInvoice() {
    this._routeService.navigateByUrl('invoice/form')
  }


  viewDetails(invoice: InvoiceGeneralDTO) {
    this.invoiceSelect = invoice;
    this.drawerDetails = true;
  }

  closeDetails() {
    this.drawerDetails = false;
    this.invoiceSelect = undefined;
  }

  editItem(invoice: InvoiceModel) {
    this._routeService.navigate(['invoice/form'], { queryParams: { codeInvoice: invoice.i_code } });
  }

  deleteInvoice(invoice: InvoiceModel) {
    this._modalService.confirm({
      nzTitle: '<i>¿Está seguro de este pedido?</i>',
      nzContent: '<b>La pedido se eliminará permanentemente</b>',
      nzOnOk: () => {
        invoice.i_state = 0;
        this._invoiceGeneralService.changeStatusInvoice(invoice.i_id!).subscribe(value => {
          this.getAllInvoices();
        }, (e: Error) => {
          this._ntService.error('ERROR AL ELIMINAR PEDIDO', 'error: ' + e.message)
        })
      }
    });
  }
}
