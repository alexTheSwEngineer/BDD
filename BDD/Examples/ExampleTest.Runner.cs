using ClinicHQ.Tests.Common;
using Domain.Accounting;
using Services.Accounting;
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
        DataContextBuilder dbContextBuilder = new DataContextBuilder();
        SystemUserBuilder systemUserBuilder = new SystemUserBuilder();

        [Theory(DisplayName = "SomeDescriptiveName.SomeMethodUnderTest")]
        [MemberData(nameof(TestCases))]
        public void Example(DerivedTestCase testcase)
        {
            //SETUP
            // setup preexisting state 
            var mockSystemUser = systemUserBuilder.Build();
            var mockDbContext = dbContextBuilder
                            .IncludeRepoFor<Payment>()
                            .With(new List<PaymentItem>
                            {
                                new PaymentItem {
                                    Amount =-1* testcase.Given
                                 }
                            })
                            .BuildIDataContext();

            //create sysyem under test
            var sut = new ClientPaymentService(mockDbContext,mockSystemUser)
        
        }

        public static IEnumerable<IEnumerable<object>> TestCases
        {
            get
            {
                return new[]
                {
                    TestCase1,
                    GivenExistingDebtOf40_WhenPaid40More_ExpectTotalOf0
                }.ToMemberData();
            }
        }
    }
}
