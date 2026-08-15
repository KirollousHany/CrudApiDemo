using CrudApiDemo.Interfaces;
using CrudApiDemo.Models;

namespace CrudApiDemo.Services
{
    public class OrderItemService: IOrderItemService , ICrudService<OrderItem>
    {
        private static List<OrderItem> _orderItems = new List<OrderItem> { };

        public bool Add(OrderItem item)
        {
            if (GetById(item.Id) != null) return false;
            _orderItems.Add(item);
            return true;
        }

        public bool Delete(int id)
        {
            return DoIfExists(id, i =>_orderItems.Remove(i));
        }

        public List<OrderItem> GetAll()
        {
            return _orderItems;
        }

        public OrderItem? GetById(int id)
        {
            return _orderItems.FirstOrDefault(i => i.Id == id);
        }

        public bool UpdateQuantity(int id, int newQuantity)
        {
            return DoIfExists(id, i => i.Quantity = newQuantity);
        } 

        public bool DoIfExists(int id, Action<OrderItem> updateAction)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            updateAction(existing);
            return true;
        }


    }
}
