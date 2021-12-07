using System.Collections.Generic;

namespace databaseF
{
    public interface IPolicyRepository
    {
        Policy Add(Policy policy);
        void Delete(int id);
        Policy Get(int id);
        Policy GetLastAdded();
        List<Policy> GetAll();
        Policy Update(Policy policy);
        void SaveChanges();
        List<Policy> GetAllChanged();

    }
}