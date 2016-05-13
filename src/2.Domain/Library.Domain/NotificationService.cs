using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class NotificationService
    {
        private User userValid;

        public NotificationService(User userValid)
        {
            this.userValid = userValid;
        }

        public int Count()
        {
            throw new NotImplementedException();
        }
    }
}
