using System.Text.Json.Serialization.Metadata;
using TestAppBack.Models;
using TestAppBack.Repositories;

namespace TestAppBack.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepository;
        public InvoiceService(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public InvoiceModel addInvoice(InvoiceModel invoice)
        {
            invoice.i_code = Guid.NewGuid().ToString("N");
            return _invoiceRepository.addInvoice(invoice);
        }
        public InvoiceModel saveInvoice(InvoiceModel invoice)
        {
            if (!string.IsNullOrEmpty(invoice.i_code) && invoice.i_id.HasValue)
            {
                return updateInvoice(invoice);
            }
            else
            {
                return addInvoice(invoice);
            }
        }
        public InvoiceModel updateInvoice(InvoiceModel invoice)
        {
            return _invoiceRepository.updateInvoice(invoice);
        }
        public InvoiceModel changeStatusInvoice(InvoiceModel invoice)
        {
            return _invoiceRepository.changeStatusInvoice(invoice);
        }

        public List<InvoiceModel> getAllInvoices(string? search = null)
        {
            return _invoiceRepository.getAllInvoices(search);
        }
        public InvoiceModel getInvoiceById(int idInvoice)
        {
            return _invoiceRepository.getInvoiceById(idInvoice);
        }
    }
}
