using jenjennewborncare.Models;
using jenjennewborncare.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Linq;

namespace jenjennewborncare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : Controller
    {
        private readonly StripeSettings _stripeSettings;
        public PaymentsController(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public IActionResult Index()
        {
            var viewModel = new PaymentViewModel
            {
                AvailableOptions = new List<int> { 500, 1500, 2000, 2500 }
            };
            return View(viewModel);
        }

        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {

            string selectedOptionStr = HttpContext.Request.Form["SelectedOption"];
            long? selectedOption;

            if (long.TryParse(selectedOptionStr, out long parsedValue))
            {
                selectedOption = parsedValue;
                // Your logic here, using the 'selectedOption' variable of type 'long?'
            }
            else
            {
                selectedOption = null;
                // Handle the case when the selectedOptionStr is not a valid long
                // You can return an error message or set a default value for the 'selectedOption' variable
            }




            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {

              UnitAmount = selectedOption*100,
              Currency = "usd",
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = "Jenjennewborncare Services",
              },
            },
            Quantity = 1,
          },
        },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Payments", null, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Payments", null, Request.Scheme),
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet("success")]
        public ActionResult Sucess()
        {
            return View();
        }
        [HttpGet("cancel")]
        public ActionResult Cancel()
        {
            return View();
        }
    }
}