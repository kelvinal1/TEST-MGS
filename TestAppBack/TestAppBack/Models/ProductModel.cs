using TestAppBack.Enums;

namespace TestAppBack.Repositories
{
    public class ProductModel
    {
        public int? p_id { get; set; }
        public string? p_description { get; set; }
        public string? p_bar_code { get; set; }
        public double? p_price { get; set; }
        public double? p_stock { get; set; }
        public int? p_state { get; set; } = (int)StatusEnum.ACTIVE;
    }
}
