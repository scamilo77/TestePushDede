using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class MyTestSubjectClass
    {
        public ITestObject<TestParameterClass> testObject;

        public MyTestSubjectClass(ITestObject<TestParameterClass> _testObject)
        {
            testObject = _testObject;
        }

        public string Test()
        {
            return testObject.GetMessageFromParameter();
        }
    }
}
