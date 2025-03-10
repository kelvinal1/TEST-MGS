using System.Collections.Generic;
using System.Net.WebSockets;
using TestAppBack.Enums;
using TestAppBack.Models;
using TestAppBack.Models.DTO;

namespace TestAppBack.Services
{
    public class InvoiceGeneralService
    {
        private readonly InvoiceService _invoiceService;
        private readonly InvoiceItemService _invoiceItemService;
        public InvoiceGeneralService(InvoiceService invoiceService, InvoiceItemService invoiceItemService)
        {
            _invoiceService = invoiceService;
            _invoiceItemService = invoiceItemService;
        }
        public void saveInvoice(InvoiceGeneralDTO invoice)
        {
            var resultInvoice = _invoiceService.saveInvoice(invoice.invoice);
            invoice.items.ForEach(x => x.ii_invoice = resultInvoice.i_id);
            _invoiceItemService.saveItems(invoice.items);
        }
        public List<InvoiceGeneralDTO> getAllInvoices(string? search = null)
        {
            var listResult = new List<InvoiceGeneralDTO>();
            var invoices = _invoiceService.getAllInvoices(search);
            foreach (var item in invoices)
            {
                if (item.i_id.HasValue)
                {
                    InvoiceGeneralDTO i = new InvoiceGeneralDTO();
                    i.invoice = item;
                    i.items = _invoiceItemService.getAllItemsByInvoice(item.i_id.Value);
                    listResult.Add(i);
                }
            }
            return listResult;
        }
        public void changeStatusInvoice(int idInvoice)
        {
            var invoice = _invoiceService.getInvoiceById(idInvoice);
            if (invoice == null) return;
            var listItems = _invoiceItemService.getAllItemsByInvoice(idInvoice);
            listItems.ForEach(x =>
            {
                x.ii_state = (int)StatusEnum.INACTIVE;
                changeStatusItemsInvoice(x);
            });
            invoice.i_state = (int)StatusEnum.INACTIVE;
            _invoiceService.changeStatusInvoice(invoice);
        }
        public void changeStatusItemsInvoice(InvoiceItemModel item)
        {
            _invoiceItemService.changeStateItem(item);
        }
    }
}
