using ClinicHQ.Tests.Common;
using Domain.Accounting;
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
        [Theory(DisplayName = "SomeDescriptiveName.SomeMethodUnderIntegration-ishTest")]
        [MemberData(nameof(Cases))]
        public void IntegrationExample(ExampleTestCase testcase)
        {
            //SETUP
            var mockSystemUser = systemUserBuilder.Build();
            var mockDbContext = dbContextBuilder
                                .IncludeRepoFor<Payment>()
                                .With(new List<PaymentItem>
                                {
                                    new PaymentItem {
                                        Amount =testcase.Given
                                     }
                                })
                                .BuildIDataContext();
            var sut = new ClientPaymentService(mockDbContext, mockSystemUser);

            //ACT
            sut.MakePayment(testcase.TestInput);

            //ASSERT
            decimal actual = mockDbContext.Payments.Total();
            testcase.DoAsserts(actual);

        }
    }
}
