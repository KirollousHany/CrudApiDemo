using CrudApiDemo.Interfaces;
using CrudApiDemo.Models;

namespace CrudApiDemo.Services
{
    public class OrderService : IOrderService, ICrudService<Order>
    {
        private readonly ICrudService<Client> _clientService;
        private readonly ICrudService<OrderItem> _orderItemService;
        private readonly ICrudService<Product> _productService;
        

        public OrderService(ICrudService<Client> clientService, ICrudService<OrderItem> orderItemService , ICrudService<Product> productService)
        {
            _clientService = clientService;
            _orderItemService = orderItemService;
            _productService = productService;
        }
        private static List<Order> _orders = new List<Order> {};

        public bool Add(Order item)
        {
            if (GetById(item.Id) != null || !CheckIfUserExist(item.ClientId)) return false;
            if (!ValidateProductsInOrder(item)) return false;
            _orders.Add(item);
            AddOrderItems(item.Items);
            return true;
        }

        public bool CheckIfUserExist(int clientId)
        {
            return _clientService.GetById(clientId) != null;
        }

        public bool ValidateProductsInOrder(Order order)
        {
            if (order.Items == null || order.Items.Count == 0)
                return false;

            foreach (var item in order.Items)
            {
                if (_productService.GetById(item.ProductId) == null)
                    return false;

                if (item.OrderId != order.Id)
                    return false;
            }
            return true;
        }
        public void AddOrderItems(List<OrderItem> items)
        {
            foreach (var item in items)
            {
                _orderItemService.Add(item);
            }
        }
        public bool AddItemToOrder(int orderId, OrderItem item)
        {
            var existing = GetById(orderId);
            if (existing == null) return false;
            if(_productService.GetById(item.ProductId) == null) return false;

            existing.Items.Add(item);
            AddOrderItems([item]);
            return true;
        }


        public bool Delete(int id)
        {
            return DoIfExists(id , o => _orders.Remove(o));
        }

        public bool DoIfExists(int id, Action<Order> updateAction)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            updateAction(existing);
            return true;
        }

        public List<Order> GetAll()
        {
            return _orders;
        }

        public Order? GetById(int id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public bool RemoveItemFromOrder(int orderId, int orderItemId)
        {
            var order = GetById(orderId);
            if (order == null) return false;
            var item = order.Items.FirstOrDefault(i => i.Id == orderItemId);
            if (item == null) return false;

            order.Items.Remove(item);
            _orderItemService.Delete(item.Id);
            return true;
        }
    }
}
