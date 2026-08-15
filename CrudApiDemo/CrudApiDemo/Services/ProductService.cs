using CrudApiDemo.Interfaces;
using CrudApiDemo.Models;

namespace CrudApiDemo.Services
{
    public class ProductService : ICrudService<Product> , IProductService
    {
        private static List<Product> _products = new List<Product>
        {
            new Product(1, "Laptop", 15000m),
            new Product(2, "Mouse", 250m),
            new Product(3, "Keyboard", 500m)
        };
        public bool Add(Product item)
        {
            if (GetById(item.Id) != null) return false;

            _products.Add(item);
            return true;
        }

        public bool Delete(int id)
        {
           return DoIfExists(id,p => _products.Remove(p));
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public bool UpdateName(int id, string newName)
        {
            return DoIfExists(id, p => p.Name = newName);
        }

        public bool UpdatePrice(int id, decimal newPrice)
        {
            return DoIfExists(id, p => p.Price = newPrice);
        }
        
        public bool DoIfExists(int id, Action<Product> updateAction)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            updateAction(existing);
            return true;
        }
    }
}
