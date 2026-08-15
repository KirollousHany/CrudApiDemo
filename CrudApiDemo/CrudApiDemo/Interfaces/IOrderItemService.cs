namespace CrudApiDemo.Interfaces
{
    public interface IOrderItemService
    {
        bool UpdateQuantity(int id, int newQuantity);
    }
}
