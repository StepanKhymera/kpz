using Api.Models;
using System.Collections.Generic;

namespace Api
{
    public interface IPolicyRepository
    {
        void Add(Policy policy);
        void Delete(int id);
        Policy Get(int id);
        Policy GetLastAdded();
        List<Policy> GetAll();
        Policy Update(Policy policy);
    }
}