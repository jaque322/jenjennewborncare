using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace jenjennewborncare.Controllers
{
    public class PaymentsController : Controller
    {
        [Authorize]
        public IActionResult Charge()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = 500, // In cents, e.g. 500 = $5.00
                Description = "Test Charge",
                Currency = "usd",
                Customer = customer.Id
            });

            // Handle successful charge
            return View();
        }
    }
}
