using NUnit.Framework;
using System;

namespace Redux.Tests
{
    [TestFixture]
    public class FunctionalHelpersTests
    {
        [Test]
        public void Should_compose_funcs_from_right_to_left()
        {
            Func<string,string> f = (string s) => s + "f";
            Func<string,string> g = (string s) => s + "g";
            Func<string,string> h = (string s) => s + "h";

            var composedFunc = FunctionalHelpers.Compose(f, g, h);

            var actual = composedFunc("value");

            Assert.AreEqual(f(g(h("value"))), actual);
        }
    }
}
