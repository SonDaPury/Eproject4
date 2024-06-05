using backend.Dtos;
using backend.Service;
using backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]
        public async Task<object> CreateOrder(CreateOrder order)
        {
            return await _orderService.CreateOrder(order);
        }
        [HttpGet("confirm-payment")]
        public async Task<object> ConfirmPayment(long vnp_TxnRef, string vnp_BankTranNo, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_TransactionStatus, string vnp_SecureHash, string vnp_TmnCode, long vnp_Amount, string vnp_BankCode, string vnp_ResponseCode, string vnp_TransactionNo)
        {
            var confirmPayment = new ConfirmPayment
            {
                vnp_Amount = vnp_Amount,
                vnp_BankCode = vnp_BankCode,
                vnp_BankTranNo = vnp_BankTranNo,
                vnp_CardType = vnp_CardType,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_PayDate = vnp_PayDate,
                vnp_ResponseCode = vnp_ResponseCode,
                vnp_TmnCode = vnp_TmnCode,
                vnp_TransactionNo = vnp_TransactionNo,
                vnp_TransactionStatus = vnp_TransactionStatus,
                vnp_TxnRef = vnp_TxnRef,
                vnp_SecureHash = vnp_SecureHash

            };
            return await _orderService.ConfirmPayment(confirmPayment);
        }
        [HttpGet("get-all-order")]
        public async Task<object> GetAllOrder()
        {
            return await _orderService.GetAllOrder();
        }
    }
}
