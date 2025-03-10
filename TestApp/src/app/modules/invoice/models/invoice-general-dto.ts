import { InvoiceItemModel } from "./invoice-item-model";
import { InvoiceModel } from "./invoice-model";

export class InvoiceGeneralDTO {
    invoice?: InvoiceModel;
    items?: InvoiceItemModel[] = [];
}
