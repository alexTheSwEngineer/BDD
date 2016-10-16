
using Data;
using Data.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClinicHQ.Tests.Common
{
    public partial class DataContextBuilder
    {
        private static object InvokeGeneric(DataContextBuilder target, string MethodName, Type[] genericParams, object[] arguments = null)
        {
            var methodInfo = typeof(DataContextBuilder).GetMethod(MethodName);
            var method = methodInfo.MakeGenericMethod(genericParams);
            return method.Invoke(target, arguments);
        }
        
        private object InvokeGeneric(string methodName, Type[] genericParams, object[] arguments = null)
        {
            return InvokeGeneric(this, methodName, genericParams, arguments);
        }
    }
}
