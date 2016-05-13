using Library.Domain.Shareds.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.UnitTest
{
    public class TestService
    {
        public static IPromise<TestService> Config(string title, string[] cases)
        {
            var defer = new Deferred<TestService>();
            return defer.Promise();
        }

        internal static void AssertFalse(bool p)
        {
            throw new NotImplementedException();
        }

        internal static void AssertTrue(bool p)
        {
            throw new NotImplementedException();
        }


        internal static void AssertDone(IPromise promise)
        {
            throw new NotImplementedException();
        }

        internal static void AssertFail(IPromise promise)
        {
            throw new NotImplementedException();
        }

        internal static void Finish()
        {
            throw new NotImplementedException();
        }

        internal static void AssertEquals(int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}
