#region

using Services.Accounting.ClientPayment;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Accounting;
using System.Collections.Generic;

#endregion

namespace Web.Api.Accounting.Payments
{
    [RoutePrefix("api/accounting/payments")]
    public partial class PaymentsController : ChqApiController
    {
        private readonly IClientPaymentService _clientPaymentService;

        public PaymentsController(IClientPaymentService clientPaymentService)
        {
            _clientPaymentService = clientPaymentService;
        }
        
        [Route("overpay")]
        [HttpPost]
        public HttpResponseMessage ApplyOverpaid(int  amount)
        {
            DataContext.Payments.Add(new Payment
            {
                PaymentItems = new List<PaymentItem>
                {
                    new PaymentItem
                    {
                        Amount = amount
                    }
                }
            });

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
