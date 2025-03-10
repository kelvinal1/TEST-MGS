using TestAppBack.Enums;

namespace TestAppBack.Models
{
    public class InvoiceModel
    {
        public int? i_id { get; set; }
        public DateTime? i_date { get; set; } = DateTime.Now;
        public int? i_customer { get; set; }
        public int? i_state { get; set; } = (int)StatusEnum.ACTIVE;
        public string? i_code { get; set; }
        public float? i_total { get; set; }
        //customer name NOT MAPPED
        public string ? customer_name { get; set; }
        public string ? customer_ruc { get; set; }
    }
}
