using System;
using System.Linq.Expressions;
using Oragon.Spring.Context;
using Oragon.Spring.Globalization;
using NSubstitute;
using NSubstitute.Core;
using System.Collections.Generic;
using System.Linq;
using AopAlliance.Intercept;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks
{
    public class _ { }

    public static class Arg<T>
    {

#if DOTNET35

        /// <summary>
		/// Register the predicate as a constraint for the current call.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>default(T)</returns>
		/// <example>
		/// Allow you to use code to create constraints
		/// <code>
		/// demo.AssertWasCalled(x => x.Bar(Arg{string}.Matches(a => a.StartsWith("b") &amp;&amp; a.Contains("ba"))));
		/// </code>
		/// </example>
		public static T Matches(Expression<Predicate<T>> predicate)
		{
			ArgManager.AddInArgument(new LambdaConstraint(predicate));
			return default(T);
		}
#else
        /// <summary>
        /// Register the predicate as a constraint for the current call.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>default(T)</returns>
        /// <example>
        /// Allow you to use code to create constraints
        /// <code>
        /// demo.AssertWasCalled(x => x.Bar(Arg{string}.Matches(a => a.StartsWith("b") &amp;&amp; a.Contains("ba"))));
        /// </code>
        /// </example>
        public static T Matches<TPredicate>(Predicate<TPredicate> predicate)
        {
            return default(T);
        }
#endif

        /// <summary>
        /// Define a simple constraint for this argument. (Use Matches in simple cases.)
        /// Example: 
        ///   Arg&lt;int&gt;.Is.Anything
        ///   Arg&lt;string&gt;.Is.Equal("hello")
        /// </summary>
        public static IsArg<T> Is => new IsArg<T>();

        

    }

    /// <summary>
    /// Use the Arg class (without generic) to define Text constraints
    /// </summary>
    public static class Arg
    {
       

        public static T Matches<T>(Expression<Predicate<T>> predicate) => default(T);

    }


    public class MockRepository
    {

        public static IMockedObject GetMockedObject(object mockedInstance)
        {
            return null;
        }


        public void ReplayAll()
        {
        }

        public void VerifyAll()
        {
        }


        public T StrictMock<T>() where T : class
        {
            return Substitute.For<T>();
        }

        public static T GenerateMock<T>() where T : class
        {
            return Substitute.For<T>();
        }
        public T CreateMock<T>() where T : class
        {
            return Substitute.For<T>();
        }

        public object PartialMock(Type type, IFormatter formatter = null)
        {
            return Substitute.For(new Type[] { type }, null);
        }


        public object CreateMock(Type type)
        {
            return Substitute.For(new Type[] { type }, null);
        }

        public IMessageSource DynamicMock(Type type) => null;

        public void BackToRecordAll() { }

        public IDisposable Record() => null;

        public IDisposable Playback() => null;


        public IDisposable Ordered() => null;

        public IDisposable Unordered() => new System.IO.MemoryStream();


        public T Stub<T>(params object[] argumentsForConstructor)
        {
            return (T)Stub(typeof(T), argumentsForConstructor);
        }

        public object Stub(Type type, params object[] argumentsForConstructor)
        {
            return Substitute.For(new[] { type }, argumentsForConstructor);
        }

        public bool IsInReplayMode(object mockedObject)
        {
            return true;
        }

        public object DynamicMock(Type type, object[] constructorArguments) => Substitute.For(new Type[] { type }, constructorArguments);

        public IMethodOptions<T> LastMethodCall<T>(T mockToRecordExpectation) => null;

        public void Replay(object mockedObject)
        {
        }

        public void Verify(object mockedObject)
        {
            
        }

        public void BackToRecord(IMethodInvocation mockInvocation)
        {
            throw new NotImplementedException();
        }
    }




    public static class Expect
    {
        public static CallReturn<object> Call(object data) => new CallReturn<object>();
        public static CallReturn<object> Call(Action data)
        {
            data();
            return new CallReturn<object>();
        }
    }

    public class LastCall
    {
        public static CallReturn<object> IgnoreArguments() => new CallReturn<object>();
        public static CallReturn<object> On(object data) => new CallReturn<object>();
        public static CallReturn<object> Throw(FormatException formatException) => new CallReturn<object>();
    }


    public class CallReturn<T>
    {
        public CallReturn<T> Return(T data)
        {
            (new object()).Returns(data);
            return this;
        }

        public CallReturn<T> IgnoreArguments() => this;
        public CallReturn<T> Any() => this;
        public CallReturn<T> Once() => this;
        public CallReturn<T> Repeat { get => this; }
        public CallReturn<T> AtLeastOnce() => this;
        public CallReturn<T> Throw(Exception ex) => this;
        public CallReturn<T> Twice() => this;
        public CallReturn<T> Times(int qtd) => this;
    }
}