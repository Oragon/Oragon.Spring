using System;
using System.Collections.Generic;

namespace Oragon.Spring.Util
{
    public interface IEventExceptionsCollector
    {
        bool HasExceptions { get; }
        IList<Delegate> Sources { get;}
        IList<Exception> Exceptions { get; }
        Exception this[Delegate source] { get; }
    }
}