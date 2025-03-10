using TestAppBack.Repositories;
using TestAppBack.Services;

namespace TestAppBack.Config
{
    public class SevicesConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<CustomerRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<InvoiceRepository>();
            services.AddScoped<InvoiceItemRepository>();
            services.AddScoped<CustomerService>();
            services.AddScoped<ProductService>();
            services.AddScoped<InvoiceService>();
            services.AddScoped<InvoiceItemService>();
            //services generals
            services.AddScoped<InvoiceGeneralService>();
        }
    }
}
