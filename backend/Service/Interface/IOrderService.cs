using backend.Dtos;

namespace backend.Service.Interface
{
    public interface IOrderService
    {
        Task<object> CreateOrder(CreateOrder order);
        Task<object> ConfirmPayment(ConfirmPayment confirmPayment);
        Task<object> GetAllOrder();
        /*        Task<Order> GetOrder(int id);
                Task<Order> UpdateOrder(int id, Order order);
                Task<Order> DeleteOrder(int id);
                Task<Order> ConfirmPayment(ConfirmPayment payment);*/
    }
}
