using TestAppBack.Models;
using TestAppBack.Repositories;

namespace TestAppBack.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public CustomerModel addCustomer(CustomerModel customer)
        {
            return _customerRepository.addCustomer(customer);
        }


        public CustomerModel saveCustomer(CustomerModel customerModel)
        {
            var verifyItem = getAllCustomers(customerModel.c_ruc);
            if (verifyItem != null && verifyItem.Count >= 1)
            {
                customerModel.c_id = verifyItem.FirstOrDefault().c_id;
                return updateCustomer(customerModel);
            }
            else
            {
                return addCustomer(customerModel);
            }
        }
        public CustomerModel updateCustomer(CustomerModel customer)
        {
            return _customerRepository.updateCustomer(customer);
        }

        public CustomerModel changeStatusCustomer(CustomerModel customer)
        {
            return _customerRepository.changeStatusCustomer(customer);
        }

        public List<CustomerModel> getAllCustomers(string? ruc = null)
        {
            return _customerRepository.getAllCustomers(ruc);
        }

        public CustomerModel getAllCustomerById(int idCustomer)
        {
            return _customerRepository.getAllCustomerById(idCustomer);
        }
    }
}
