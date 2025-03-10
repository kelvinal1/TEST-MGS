using CurbeCorporativo.DAL.DBContext;
using TestAppBack.Enums;
using TestAppBack.Models;

namespace TestAppBack.Repositories
{
    public class InvoiceRepository
    {
        private readonly IDbContext _dbContext;

        public InvoiceRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public InvoiceModel addInvoice(InvoiceModel header)
        {
            return _dbContext.getObject<InvoiceModel>(@$"INSERT INTO public.invoice
            (i_id, i_date, i_customer, i_state, i_code, i_total)
            VALUES(nextval('invoice_i_id_seq'::regclass), @i_date, @i_customer, @i_state, @i_code, @i_total) returning *;", header);
        }

        public InvoiceModel updateInvoice(InvoiceModel header)
        {
            return _dbContext.getObject<InvoiceModel>(@$"UPDATE public.invoice
            SET i_date= @i_date, i_customer= @i_customer, i_total= @i_total
            WHERE i_id=@i_id or i_code= @i_code returning *;", header);
        }

        public InvoiceModel changeStatusInvoice(InvoiceModel header)
        {
            return _dbContext.getObject<InvoiceModel>(@$"UPDATE public.invoice
            SET i_state= @i_state
            WHERE i_id=i_id or i_code= @i_code returning *;", header);
        }

        public List<InvoiceModel> getAllInvoices(string? search = null)
        {
            return _dbContext.getList<InvoiceModel>(@$"select i.*, c.c_name as customer_name, c.c_ruc as customer_ruc from invoice i 
            join customer c 
            on c.c_id = i.i_customer
            where i.i_state = {(int)StatusEnum.ACTIVE}
            {(!string.IsNullOrEmpty(search) ? $" and (LOWER(c.c_name) like LOWER('%{search}%') or LOWER(c.c_ruc) like LOWER('%{search}%') or LOWER(i.i_code) like LOWER('%{search}%'))" :"")}
            ;");
        }

        public InvoiceModel getInvoiceById(int idInvoice)
        {
            return _dbContext.getObject<InvoiceModel>(@$"select i.* from invoice i where i.i_id={idInvoice}; ");
        }
    }
}
