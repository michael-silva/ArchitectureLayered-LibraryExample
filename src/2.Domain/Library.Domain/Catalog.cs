using Library.Domain.Shareds.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class Catalog
    {
        private User u;

        public Catalog(User u)
        {
            this.u = u;
        }

        public bool IsReadonly()
        {
            throw new NotImplementedException();
        }

        public IPromise AddMaterial(Material material)
        {
            throw new NotImplementedException();
        }

        public IPromise RemoveMaterial(Material material)
        {
            throw new NotImplementedException();
        }
    }
}
