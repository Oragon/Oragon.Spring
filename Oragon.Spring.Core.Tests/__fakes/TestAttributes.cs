using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public class SetCultureAttribute: Attribute
    {
        public SetCultureAttribute(string param) { }
    }

    public class SetUICultureAttribute : Attribute
    {
        public SetUICultureAttribute(string param) { }
    }
}
