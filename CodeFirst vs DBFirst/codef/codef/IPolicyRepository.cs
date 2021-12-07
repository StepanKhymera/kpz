using System.Collections.Generic;

namespace codef
{
    public interface IPolicyRepository
    {
        Policy Add(Policy policy);
        void Delete(int id);
        Policy Get(int id);
        Policy GetLastAdded();
        List<Policy> GetAll();
        Policy Update(Policy policy);
    }
}