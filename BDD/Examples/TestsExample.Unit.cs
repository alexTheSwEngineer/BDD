using ClinicHQ.Tests.Common;
using Data;
using Domain.Accounting;
using Moq;
using Services.Accounting;
using Services.Accounting.ClientPayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Examples
{
    public partial class TestsExample
    {
        DataContextBuilder dbContextBuilder = new DataContextBuilder();
        SystemUserBuilder systemUserBuilder = new SystemUserBuilder();

        [Theory(DisplayName = "SomeDescriptiveName.SomeMethodUnderTest")]
        [MemberData(nameof(Cases))]
        public void UnitTest(ExampleTestCase testcase)
        {
            //SETUP
            var dbContext = dbContextBuilder.BuildIDataContext();
            var systemUser = systemUserBuilder.Build();
            var sut = new ClientPaymentService(dbContext,systemUser);

            //ACT
            var newPayments = sut.MakePaymentBlackBox(testcase.TestInput);

            //ASSERT
            var actual = testcase.Given + newPayments.Amount;
            testcase.DoAsserts(actual);
        }

        public static IEnumerable<IEnumerable<object>> Cases
        {
            get
            {
                return new[]
                {
                    TestCases.TestCase1,
                    TestCases.GivenExistingDebtOf40_WhenPaid30More_ExpectTotalOf10
                }.ToMemberData();
            }
        }
    }
}
