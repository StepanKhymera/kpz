using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace codef
{
    class PolicyRepository : IPolicyRepository
    {
        private readonly Context _context;

        public PolicyRepository()
        {
            _context = new Context();
        }

        public Policy Get(int id)
        {
            var result = _context.Policies.FirstOrDefault(x => x.PolicyID == id);

            return result;
        }

        public List<Policy> GetAll()
        {
            var result = _context.Policies.ToList();

            return result;
        }


        public Policy GetLastAdded()
        {
            int maxId = _context.Policies.Max(x => x.PolicyID);
            var result = _context.Policies.FirstOrDefault(x => x.PolicyID == maxId);
            return result;
        }

        public Policy Add(Policy policy)
        {
            var result = _context.Policies.Add(new Policy() {Policy_Expiration_Date = DateTime.Now, Policy_Start_Date = DateTime.Now });
            _context.SaveChanges();

            return result;
        }

        public Policy Update(Policy policy)
        {
            var result = Get(policy.PolicyID);

            result = policy;
            _context.SaveChanges();

            return result;
        }
 
        public void Delete(int id)
        {
            var policy = Get(id);
            _context.Policies.Remove(policy);
            _context.SaveChanges();

        }
    }
}
