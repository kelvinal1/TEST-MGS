using TestAppBack.Repositories;

namespace TestAppBack.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ProductModel addProduct(ProductModel product)
        {
            product.p_bar_code = Guid.NewGuid().ToString("N");
            return _productRepository.addProduct(product);
        }

        public ProductModel updateProduct(ProductModel product)
        {
            return _productRepository.updateProduct(product);
        }
        public ProductModel updateStock(ProductModel product)
        {
            return _productRepository.updateStock(product);
        }

        public ProductModel saveProduct(ProductModel product)
        {
            var verification = verifyProduct(product);
            if (verification != null && verification.Count >= 1)
            {
                product.p_id = verification.FirstOrDefault().p_id;
                return updateProduct(product);
            }
            else
            {
                return addProduct(product);
            }
        }
        public List<ProductModel> verifyProduct(ProductModel product)
        {
            if (string.IsNullOrEmpty(product.p_bar_code)) return [];
            return _productRepository.getAllProducts(product.p_bar_code);
        }

        public ProductModel changeStatusProduct(ProductModel product)
        {
            return _productRepository.changeStatusProduct(product);
        }

        public List<ProductModel> getAllProducts(string? description = null, int? idProduct = null)
        {
            return _productRepository.getAllProducts(description, idProduct);
        }
    }
}
