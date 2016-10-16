using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHQ.Tests.Common
{
    public static class MemberDataExstensions
    {
        public static IEnumerable<object> ToMemberData(this object obj)
        {
            return new[] { obj };
        }

        public static IEnumerable<IEnumerable<object>> ToMemberData(this IEnumerable<object> obj)
        {
            return obj.Select(x => x.ToMemberData()).AsEnumerable();
        }

        public static IEnumerable<T> ToArray<T>(this T obj) where T : class
        {
            return new T[] { obj };
        }
    }
}
