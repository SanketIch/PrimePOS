using System;

namespace PrimePOSTestCases
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CriticalTestAttribute : Attribute
    {
    }
}
