using MVC;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var a = "123";
            a = a;

            var service = new TestService();
            var b = service.GetName();
            Assert.NotEmpty(b);

        }
    }
}
