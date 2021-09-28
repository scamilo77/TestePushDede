using System;
using System.Reflection;

namespace Tests
{
    public interface ITestObject<T> where T : TestParameterClass
    {
        string Id { get; set; }
        string GetMessageFromParameter();
    }

    public class TestObject<T> : ITestObject<T> where T: TestParameterClass
    {
        public string Id { get; set; }

        public string GetMessageFromParameter()
        {
            MethodInfo method = typeof(T).GetMethod("GetMessage");
            method = method.MakeGenericMethod(typeof(T));
            return method.Invoke(this, new object[0]).ToString();
        }
    }
}
