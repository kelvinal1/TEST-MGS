namespace TestAppBack.Models.DTO
{
    public class InvoiceGeneralDTO
    {
        public InvoiceModel invoice { get; set; } = new InvoiceModel();
        public List<InvoiceItemModel> items { get; set; } = new List<InvoiceItemModel>();
    }
}
