using System;
using Should;

namespace Thing.Tests
{
    public class SmokeTests : IDisposable
    {
        public SmokeTests()
        {
            Console.WriteLine("Setup");
        }

        public void Dispose()
        {
            Console.WriteLine("Teardown");
        }

        public void should_pass()
        {
            true.ShouldBeTrue();
            false.ShouldBeFalse();
        }
    }
}