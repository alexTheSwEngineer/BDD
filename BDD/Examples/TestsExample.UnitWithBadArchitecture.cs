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
        [Theory(DisplayName = "SomeDescriptiveName.SomeMethodUnderUnitTest")]
        [MemberData(nameof(Cases))]
        public void BadUnitTest(ExampleTestCase testcase)
        {
            //SETUP
            var mockSystemUser = systemUserBuilder.Build();
            var mockPaymentRepo = new Mock<IRepository<PaymentItem>>();
            mockPaymentRepo.Setup(x => x.Add(It.IsAny<PaymentItem>()));
            var mockDbContext = dbContextBuilder
                                .IncludeRepoFor<Payment>()
                                .WithRepo<IRepository<PaymentItem>,PaymentItem>(mockPaymentRepo.Object)
                                .BuildIDataContext();
            var sut = new ClientPaymentService(mockDbContext, mockSystemUser);

            //ACT
            sut.MakePayment(testcase.TestInput);

            //ASSERT
            mockPaymentRepo.Verify(x => x.Add(new PaymentItem
                                        {
                                            Amount = testcase.TestInput
                                        }),
                                        Times.Once(), 
                                        "Add items not called");

        }
               
    }
}
