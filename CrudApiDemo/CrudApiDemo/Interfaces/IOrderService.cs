using CrudApiDemo.Models;

namespace CrudApiDemo.Interfaces
{
    public interface IOrderService
    {
        bool AddItemToOrder(int orderId, OrderItem item);
        bool RemoveItemFromOrder(int orderId, int orderItemId);
        bool CheckIfUserExist(int clientId);
        bool ValidateProductsInOrder(Order order);
        void AddOrderItems(List<OrderItem> items);
    }
}
