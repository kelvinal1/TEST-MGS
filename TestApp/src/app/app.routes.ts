import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'invoice', pathMatch:'full' },
    { path: 'invoice', loadChildren: () => import('./modules/invoice/invoice.module').then(m => m.InvoiceModule) },
    { path: 'product', loadChildren: () => import('./modules/product/product.module').then(m => m.ProductModule) },
    { path: 'customer', loadChildren: () => import('./modules/customer/customer.module').then(m => m.CustomerModule) },
];
