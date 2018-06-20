using System.Collections.Generic;
using Rhino.Mocks.Interfaces;

namespace Oragon.Spring.Aop.Support
{
    internal class ExpectationVerificationInformation
    {
        public IList<object[]> ArgumentsForAllCalls { get; internal set; }
        public IExpectation Expected { get; internal set; }
    }
}