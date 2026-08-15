using CrudApiDemo.Interfaces;
using CrudApiDemo.Models;

namespace CrudApiDemo.Services
{
    public class ClientService : IClientService, ICrudService<Client>
    {
        private static List<Client> _clients = new List<Client>
        {
            new Client(1, "Ahmed Youssef", "ahmed@email.com","password123"),
            new Client(2, "Sara Ali", "sara@email.com","password456")
        };
        
        public bool Add(Client item)
        {
            if (GetByEmail(item.Email) != null || GetById(item.Id) != null) return false;

            _clients.Add(item);
            return true;
        }

        public bool Delete(int id)
        {
            return DoIfExists(id, c => _clients.Remove(c));
        }

        public List<Client> GetAll()
        {
            return _clients;
        }

        public Client? GetById(int id)
        {
            return _clients.FirstOrDefault(c => c.Id == id);
        }
        
        public Client? GetByEmail(string email)
        {
            return _clients.FirstOrDefault(c => c.Email == email);
        }
        
        public bool UpdateName(int id, string newName)
        {
            return DoIfExists(id, c => c.Name = newName);
        }

        public bool UpdateEmail(int id, string newEmail)
        {
            return DoIfExists(id, c => c.Email = newEmail);
        }

        public bool UpdatePassword(int id, string newPassword)
        {
            return DoIfExists(id, c => c.Password = newPassword);
        }

        public bool DoIfExists(int id, Action<Client> updateAction)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            updateAction(existing);
            return true;
        }

    }
}
