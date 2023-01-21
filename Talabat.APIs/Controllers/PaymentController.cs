using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;
using Talabat.Core.Entities.Order_Aggregate;
using Microsoft.Extensions.Logging;

namespace Talabat.APIs.Controllers
{

    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private const string _whSecret = "whsec_78a55f06dca2b8f8b9afd46aff8c48ae141df3e72a74a7849861bb5a3f1846d7";

        public PaymentController(IPaymentService paymentService , ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]

        [HttpPost("{basketId}")] // Post  : api/Payment/order1

        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400));
            return Ok(basket);

        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _whSecret);

            PaymentIntent intent;
            Order order;
            // Handle the event
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    order = await _paymentService.UpdatePaymentIntentSucceededOrFailed(intent.Id, true);
                    _logger.LogInformation("Payment Succeeded");
                    break;

                case Events.PaymentIntentPaymentFailed:
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    order = await _paymentService.UpdatePaymentIntentSucceededOrFailed(intent.Id, false);
                    _logger.LogError("Payment Failed");

                    break;

            }

            return new EmptyResult();


        }

    }
}
