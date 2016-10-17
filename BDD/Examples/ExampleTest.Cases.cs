using Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Examples
{
    public partial class ExampleTest
    {
        public static DerivedTestCase TestCase1 = 
                new DerivedTestCase()
                .DescriptionIs($"Given existing debth of 4, when paid 40 more, expect total of 35")
                .WhenGiven(30m)
                .WhenCalledWith(40m)
                .Expect(35m)
                .Asserts((expected, actuall) =>
                {
                    Assert.Equal(expected, actuall);
                });

        public static DerivedTestCase GivenExistingDebtOf40_WhenPaid50More_ExpectTotalOf0 =
                new DerivedTestCase()
                .DescriptionIs(nameof(GivenExistingDebtOf40_WhenPaid50More_ExpectTotalOf0))
                .WhenGiven(40m)
                .WhenCalledWith(50m)
                .Expect(0m)
                .Asserts((expected, actuall) =>
                {
                    Assert.Equal(expected, actuall);
                });
    }
}
