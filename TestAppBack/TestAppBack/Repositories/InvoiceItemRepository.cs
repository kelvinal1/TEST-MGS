using CurbeCorporativo.DAL.DBContext;
using TestAppBack.Enums;
using TestAppBack.Models;

namespace TestAppBack.Repositories
{
    public class InvoiceItemRepository
    {
        private readonly IDbContext _dbContext;

        public InvoiceItemRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public InvoiceItemModel addItem(InvoiceItemModel item)
        {
            return _dbContext.getObject<InvoiceItemModel>(@$"INSERT INTO public.invoice_item
            (ii_id, ii_product, ii_invoice, ii_price, ii_quantity, ii_subtotal, ii_total, ii_state, ii_iva)
            VALUES(nextval('invoice_item_ii_id_seq'::regclass), @ii_product, @ii_invoice, @ii_price, @ii_quantity, @ii_subtotal, @ii_total, @ii_state, @ii_iva) returning *;", item);
        }
        public InvoiceItemModel updateItem(InvoiceItemModel item)
        {
            return _dbContext.getObject<InvoiceItemModel>(@$"UPDATE public.invoice_item
            SET ii_price=@ii_price, ii_quantity=@ii_quantity, ii_subtotal=@ii_subtotal, ii_total=@ii_total, ii_iva=@ii_iva
            WHERE ii_id=@ii_id and ii_product = @ii_product and ii_invoice = @ii_invoice returning *;", item);
        }
        public InvoiceItemModel changeStateItem(InvoiceItemModel item)
        {
            return _dbContext.getObject<InvoiceItemModel>(@$"UPDATE public.invoice_item
            SET ii_state=@ii_state
            WHERE ii_id=@ii_id returning *;", item);
        }

        public InvoiceItemModel verifyItem(InvoiceItemModel item)
        {
            return _dbContext.getObject<InvoiceItemModel>(@$"select * from invoice_item 
            where invoice_item.ii_invoice =@ii_invoice  and invoice_item.ii_product =@ii_product and invoice_item.ii_state= {(int)StatusEnum.ACTIVE};", item);
        }

        public List<InvoiceItemModel> getAllItemsByInvoice(int idInvoice)
        {
            return _dbContext.getList<InvoiceItemModel>(@$"select ii.*, p.p_description as product_name from invoice_item ii 
            join product p 
            on p.p_id = ii.ii_product 
            where ii.ii_state = {(int)StatusEnum.ACTIVE} 
            and ii.ii_invoice = {idInvoice};");
        }

        public InvoiceItemModel getAllItemsById(int idInvoiceItem)
        {
            return _dbContext.getObject<InvoiceItemModel>(@$"select ii.*, p.p_description as product_name from invoice_item ii 
            join product p 
            on p.p_id = ii.ii_product 
            where ii.ii_state = {(int)StatusEnum.ACTIVE} 
            and ii.ii_id= {idInvoiceItem};");
        }
    }
}
