namespace CrudApiDemo.Models
{
    public class Order
    {
        public int Id { get; }
        public int ClientId { get; }
        public DateTime Date { get; }
        public List<OrderItem> Items { get; set; }

        public Order(int id, int clientId)
        {
            Id = id;
            ClientId = clientId;
            Date = DateTime.Now;
            Items = new List<OrderItem>();
        }
    }
}
