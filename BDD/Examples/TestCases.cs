using Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Examples
{
    public  class TestCases
    {
        public static ExampleTestCase TestCase1 = 
                new ExampleTestCase()
                .DescriptionIs($"Given existing debth of 30, when paid 40 more, expect total of 10")
                .WhenGiven(-30m)
                .WhenCalledWith(40m)
                .Expect(10m)
                .Asserts((expected, actuall) =>
                {
                    Assert.Equal(expected, actuall);
                });

        public static ExampleTestCase GivenExistingDebtOf40_WhenPaid30More_ExpectTotalOf10 =
                new ExampleTestCase()
                .DescriptionIs(nameof(GivenExistingDebtOf40_WhenPaid30More_ExpectTotalOf10))
                .WhenGiven(-40m)
                .WhenCalledWith(30m)
                .Expect(-10m)
                .Asserts((expected, actuall) =>
                {
                    Assert.Equal(expected, actuall);
                });
    }
}
