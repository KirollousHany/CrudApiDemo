namespace CrudApiDemo.Models
{
    public class OrderItem
    {
        public int Id { get; }
        public int OrderId { get; }
        public int ProductId { get; }
        public int Quantity { get; set; }

        public OrderItem(int id, int orderId, int productId, int quantity)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
