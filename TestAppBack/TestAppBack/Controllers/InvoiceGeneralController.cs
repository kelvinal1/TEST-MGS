using CurbeCorporativo.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using TestAppBack.Models;
using TestAppBack.Models.DTO;
using TestAppBack.Services;

namespace TestAppBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceGeneralController : Controller
    {
        private readonly InvoiceGeneralService _invoiceGeneralService;

        public InvoiceGeneralController(InvoiceGeneralService invoiceGeneralService)
        {
            _invoiceGeneralService = invoiceGeneralService;
        }

        [HttpPost, Route("save-invoice")]
        public IActionResult addAssing([FromBody] InvoiceGeneralDTO invoice)
        {
            try
            {
                _invoiceGeneralService.saveInvoice(invoice);
                return Ok(new ResponseResult<string>("FACTURA GUARDADA"));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet, Route("get-all-invoices")]
        public IActionResult getAllInvoices([FromQuery] string? search = null)
        {
            try
            {
                return Ok(new ResponseResult<List<InvoiceGeneralDTO>>(_invoiceGeneralService.getAllInvoices(search)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut, Route("change-status-invoice")]
        public IActionResult changeStatusInvoice([FromQuery] int idInvoice)
        {
            try
            {
                _invoiceGeneralService.changeStatusInvoice(idInvoice);
                return Ok(new ResponseResult<string>("PEDIDO ELIMINADO"));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut, Route("change-status-item-invoice")]
        public IActionResult changeStatusItemInvoice([FromBody] InvoiceItemModel item)
        {
            try
            {
                _invoiceGeneralService.changeStatusItemsInvoice(item);
                return Ok(new ResponseResult<string>("ITEM ELIMINADO"));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
