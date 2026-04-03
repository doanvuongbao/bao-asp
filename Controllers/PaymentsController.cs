using Microsoft.AspNetCore.Mvc;
using bao_asp.Models;

namespace bao_asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpPost("cod")]
        public IActionResult COD([FromBody] PaymentRequest request)
        {
            return Ok(new
            {
                message = "Thanh toán COD thành công",
                orderId = request.OrderId
            });
        }

        [HttpPost("vnpay")]
        public IActionResult VNPay([FromBody] PaymentRequest request)
        {
            return Ok(new
            {
                message = "Tạo thanh toán VNPay thành công",
                orderId = request.OrderId,
                amount = request.Amount
            });
        }

        [HttpPost("momo")]
        public IActionResult Momo([FromBody] PaymentRequest request)
        {
            return Ok(new
            {
                message = "Tạo thanh toán Momo thành công",
                orderId = request.OrderId,
                amount = request.Amount
            });
        }
    }
}