namespace CrudApiDemo.Interfaces
{
    public interface IProductService
    {
        bool UpdateName(int id, string newName);
        bool UpdatePrice(int id, decimal newPrice);
    }
}
