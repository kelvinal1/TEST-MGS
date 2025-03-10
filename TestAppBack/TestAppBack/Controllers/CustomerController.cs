using CurbeCorporativo.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using TestAppBack.Models;
using TestAppBack.Services;

namespace TestAppBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost, Route("add-customer")]
        public IActionResult addAssing([FromBody] CustomerModel customer)
        {
            try
            {
                return Ok(new ResponseResult<CustomerModel>(_customerService.addCustomer(customer)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost, Route("save-customer")]
        public IActionResult saveCustomer([FromBody] CustomerModel customer)
        {
            try
            {
                return Ok(new ResponseResult<CustomerModel>(_customerService.saveCustomer(customer)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut, Route("update-customer")]
        public IActionResult updateAssing([FromBody] CustomerModel customer)
        {
            try
            {
                return Ok(new ResponseResult<CustomerModel>(_customerService.updateCustomer(customer)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut, Route("change-status-customer")]
        public IActionResult changeStatusCustomer([FromBody] CustomerModel customer)
        {
            try
            {
                return Ok(new ResponseResult<CustomerModel>(_customerService.changeStatusCustomer(customer)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet, Route("get-customers")]
        public IActionResult getAllCustomers([FromQuery] string? ruc = null)
        {
            try
            {
                return Ok(new ResponseResult<List<CustomerModel>>(_customerService.getAllCustomers(ruc)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet, Route("get-customer-by-id")]
        public IActionResult getAllCustomerById([FromQuery] int idCustomer)
        {
            try
            {
                return Ok(new ResponseResult<CustomerModel>(_customerService.getAllCustomerById(idCustomer)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}
