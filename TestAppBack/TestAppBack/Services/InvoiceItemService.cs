using TestAppBack.Models;
using TestAppBack.Repositories;

namespace TestAppBack.Services
{
    public class InvoiceItemService
    {
        private readonly InvoiceItemRepository _invoiceItemRepository;
        private readonly ProductService _productService;

        public InvoiceItemService(InvoiceItemRepository invoiceItemRepository, ProductService productService)
        {
            _invoiceItemRepository = invoiceItemRepository;
            _productService = productService;
        }

        public InvoiceItemModel addItem(InvoiceItemModel item)
        {
            return _invoiceItemRepository.addItem(item);
        }
        public InvoiceItemModel updateItem(InvoiceItemModel item)
        {
            return _invoiceItemRepository.updateItem(item);
        }
        public InvoiceItemModel changeStateItem(InvoiceItemModel item)
        {
            var product = _productService.getAllProducts(null, item.ii_product.Value);
            if (product != null && product.Count >= 1)
            {
                var currentProduct = product.FirstOrDefault();
                if (currentProduct != null)
                {
                    double previousQuantity = getAllItemsById(item.ii_id.Value).ii_quantity.Value;
                    currentProduct.p_stock = currentProduct.p_stock.Value + previousQuantity;
                    if (currentProduct.p_stock < 0) throw new InvalidOperationException("Stock insuficiente para modificar el ítem.");
                    _productService.updateStock(currentProduct);
                }
            }
            return _invoiceItemRepository.changeStateItem(item);
        }
        public InvoiceItemModel saveItem(InvoiceItemModel item)
        {
            var verify = _invoiceItemRepository.verifyItem(item);
            if (verify != null && verify.ii_id.HasValue)
            {
                item.ii_id = verify.ii_id;
                var product = _productService.getAllProducts(null, item.ii_product.Value);
                if (product != null && product.Count >= 1)
                {
                    var currentProduct = product.FirstOrDefault();
                    if (currentProduct != null)
                    {
                        double previousQuantity = getAllItemsById(item.ii_id.Value).ii_quantity.Value;
                        currentProduct.p_stock = currentProduct.p_stock.Value + previousQuantity - item.ii_quantity.Value;
                        if (currentProduct.p_stock < 0) throw new InvalidOperationException("Stock insuficiente para modificar el ítem.");
                        _productService.updateStock(currentProduct);
                    }
                }
                return updateItem(item);
            }
            else
            {
                var product = _productService.getAllProducts(null, item.ii_product.Value);
                if (product != null && product.Count >= 1)
                {
                    var currentProduct = product.FirstOrDefault();
                    if (currentProduct != null)
                    {
                        if (currentProduct.p_stock < item.ii_quantity.Value) throw new InvalidOperationException("Stock insuficiente para agregar el ítem.");
                        currentProduct.p_stock = currentProduct.p_stock.Value - item.ii_quantity.Value;
                        _productService.updateStock(currentProduct);
                    }
                }
                return addItem(item);
            }
        }
        public List<InvoiceItemModel> saveItems(List<InvoiceItemModel> items)
        {
            List<InvoiceItemModel> listResult = new List<InvoiceItemModel>();
            foreach (var item in items)
            {
                listResult.Add(saveItem(item));
            }
            return listResult;
        }

        public List<InvoiceItemModel> getAllItemsByInvoice(int idInvoice)
        {
            return _invoiceItemRepository.getAllItemsByInvoice(idInvoice);
        }

        public InvoiceItemModel getAllItemsById(int idInvoiceItem)
        {
            return _invoiceItemRepository.getAllItemsById(idInvoiceItem);
        }

    }
}
