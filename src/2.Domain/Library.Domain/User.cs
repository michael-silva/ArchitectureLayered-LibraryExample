using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class User
    {
        public bool TypeIs(UserType type)
        {
            throw new NotImplementedException();
        }
    }

    public enum UserType
    {
        Admin
    }
}
