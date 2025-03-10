using CurbeCorporativo.DAL.DBContext;

namespace TestAppBack.Repositories
{
    public class ProductRepository
    {
        private readonly IDbContext _dbContext;

        public ProductRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ProductModel addProduct(ProductModel product)
        {
            return _dbContext.getObject<ProductModel>(@$"INSERT INTO public.product
            (p_id, p_description, p_bar_code, p_price, p_stock, p_state)
            VALUES(nextval('product_p_id_seq'::regclass), @p_description, @p_bar_code, @p_price, @p_stock, @p_state) returning *;", product);
        }
        public ProductModel updateProduct(ProductModel product)
        {
            return _dbContext.getObject<ProductModel>(@$"UPDATE public.product
            SET p_description= @p_description, p_bar_code= @p_bar_code, p_price= @p_price, p_stock= @p_stock
            WHERE p_id= @p_id returning *;", product);
        }

        public ProductModel updateStock(ProductModel product)
        {
            return _dbContext.getObject<ProductModel>(@$"UPDATE public.product
            SET p_stock= @p_stock
            WHERE p_id= @p_id returning *;", product);
        }
        public ProductModel changeStatusProduct(ProductModel product)
        {
            return _dbContext.getObject<ProductModel>(@$"UPDATE public.product
            SET p_state= @p_state
            WHERE p_id= @p_id returning *;", product);
        }
        public List<ProductModel> getAllProducts(string? description = null, int ? idProduct = null)
        {
            return _dbContext.getList<ProductModel>(@$"select * from product p 
            where p.p_state = 1
            {(!string.IsNullOrEmpty(description) ? $" and ((lower(p.p_description) like lower('%{description}%')) OR (lower(p.p_bar_code) like lower('%{description}%')) )" : "")}
            {(idProduct.HasValue ? $" and p.p_id = {idProduct}" : "")}
            ;");
        }
    }
}
