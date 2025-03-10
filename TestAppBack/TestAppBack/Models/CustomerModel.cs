using TestAppBack.Enums;

namespace TestAppBack.Models
{
    public class CustomerModel
    {
        public int? c_id { get; set; }
        public string? c_name{ get; set; }
        public string? c_ruc{ get; set; }
        public int? c_state { get; set; } = (int)StatusEnum.ACTIVE;
    }
}
