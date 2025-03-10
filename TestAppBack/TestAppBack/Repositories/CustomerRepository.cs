using CurbeCorporativo.DAL.DBContext;
using TestAppBack.Models;

namespace TestAppBack.Repositories
{
    public class CustomerRepository
    {
        private readonly IDbContext _dbContext;

        public CustomerRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CustomerModel addCustomer(CustomerModel customer)
        {
            return _dbContext.getObject<CustomerModel>(@$"INSERT INTO public.customer
            (c_id, c_name, c_ruc, c_state)
            VALUES(nextval('customer_c_id_seq'::regclass), @c_name, @c_ruc, @c_state) returning *;", customer);
        }
        public CustomerModel updateCustomer(CustomerModel customer)
        {
            return _dbContext.getObject<CustomerModel>(@$"UPDATE public.customer
            SET c_name= @c_name, c_ruc= @c_ruc
            WHERE c_id=@c_id returning *;", customer);
        }
        public CustomerModel changeStatusCustomer(CustomerModel customer)
        {
            return _dbContext.getObject<CustomerModel>(@$"UPDATE public.customer
            SET c_state= @c_state
            WHERE c_id=@c_id returning *;", customer);
        }
        public List<CustomerModel> getAllCustomers(string? ruc = null)
        {
            return _dbContext.getList<CustomerModel>(@$"select * from customer c 
            where c.c_state =1
            {(!string.IsNullOrEmpty(ruc) ? $"and (lower(c.c_ruc) like lower('%{ruc}%') or lower(c.c_name) like lower('%{ruc}%'))" :"")}
            ;");
        }

        public CustomerModel getAllCustomerById(int idCustomer)
        {
            return _dbContext.getObject<CustomerModel>(@$"select * from customer c 
            where c.c_state =1
            and c.c_id = {idCustomer}
            ;");
        }
    }
}
