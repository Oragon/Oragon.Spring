using AopAlliance.Intercept;
using Oragon.Spring.Aop.Framework;

namespace Rhino.Mocks
{
    public class IsArg<T>
    {
        public IsArg()
        {
        }

        public T NotNull { get; set; }
        public T Anything { get; set; }
    }
}