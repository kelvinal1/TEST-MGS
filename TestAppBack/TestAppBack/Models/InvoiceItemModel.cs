using TestAppBack.Enums;

namespace TestAppBack.Models
{
    public class InvoiceItemModel
    {
        public int? ii_id { get; set; }
        public int? ii_invoice { get; set; }
        public int? ii_product { get; set; }
        public float? ii_price { get; set; }
        public float? ii_quantity { get; set; }
        public float? ii_iva { get; set; }
        public float? ii_subtotal { get; set; }
        public float? ii_total { get; set; }
        public int? ii_state { get; set; } = (int)StatusEnum.ACTIVE;
        //variables of product
        public string ? product_name { get; set; }
    }
}
