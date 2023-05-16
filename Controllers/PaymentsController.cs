using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using jenjennewborncare.Areas.Identity.Data;
using jenjennewborncare.Data;
using jenjennewborncare.Migrations;
using jenjennewborncare.Models;
using jenjennewborncare.Services;
using jenjennewborncare.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg;
using Stripe;
using Stripe.Checkout;
using System.Data.Entity;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace jenjennewborncare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : Controller
    {
        private readonly string _host;
        private readonly UserManager<User> _userManager;
        private readonly jenjennewborncareContext _dbcontext;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly StripeSettings _stripeSettings;
        public PaymentsController(ILogger<PaymentsController> logger, UserManager<User> userManager, IOptions<StripeSettings> stripeSettings, IEmailSender? emailSender, jenjennewborncareContext dbcontext)
        {
            _userManager = userManager;
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _emailSender = emailSender;
            _dbcontext = dbcontext;
            _logger = logger;
        }


        [Authorize]
        public IActionResult Index()
        {
          
            return View();
        }

        [HttpPost("create-checkout-session")]
        [Authorize]
        public ActionResult CreateCheckoutSession()
        {
            //declaring domain
            var domain = "https://localhost:44311";

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
                CustomerEmail = User.Identity.Name,
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
                SuccessUrl = domain+ "/Payments/success?session_id={CHECKOUT_SESSION_ID}",


                CancelUrl = Url.Action("Cancel", "Payments", null, Request.Scheme),
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet("success")]
        [Authorize]
        //[GoogleScopedAuthorize(PeopleServiceService.ScopeConstants.UserinfoProfile)]
        public async Task<ActionResult> Sucess()
        {
            //Get the   id
            string sessionId = HttpContext.Request.Query["session_id"].ToString();
             var result= await GetOrderInfo(sessionId);

            if (result is OkObjectResult okResult) 
            {
                var stripeSession = okResult.Value as Session;
                var name=stripeSession.CustomerDetails.Name;


                var user = await _userManager.GetUserAsync(User);
         

            var userId = user.Id;


            Models.Invoice invoice = new Models.Invoice { };
            invoice.PaymentId = stripeSession.PaymentIntentId;
            invoice.Name = name;
            invoice.Date = DateTime.Now;
            invoice.TotalAmount = 100;
            invoice.User = user;
            _dbcontext.Add(invoice);
            _dbcontext.SaveChanges();
            }
            
        

            //if (User.Identity.IsAuthenticated)
            //{
            //    var nameClaim = User.FindFirst(ClaimTypes.Name);
            //    var emailClaim = User.FindFirst(ClaimTypes.Email);

            //    if (nameClaim != null && emailClaim != null)
            //    {
            //        var userName = nameClaim.Value;
            //        var userEmail = emailClaim.Value;

            //        // Use the user's name and email here
            //    }
            //}



            //_emailSender.SendEmailAsync("janselsobrino@gmail.com", "test from sucess", "all working properly");


            return View();
        }
        [HttpGet("cancel")]
        [Authorize]
        public ActionResult Cancel()
        {
            return View();
        }


        //public static long GenerateInvoiceNumber()
        //{
        //    string date = DateTime.Now.ToString("yyyyMMdd");
        //    string randomDigits = GenerateRandomDigits(6);
        //    string invoiceNumberString = date + randomDigits;
        //    long invoiceNumber = long.Parse(invoiceNumberString);
        //    return invoiceNumber;
        //}

        //public static string GenerateRandomDigits(int length)
        //{
        //    Random random = new Random();
        //    string randomDigits = "";

        //    for (int i = 0; i < length; i++)
        //    {
        //        randomDigits += random.Next(0, 10);
        //    }

        //    return randomDigits;
        //}

        [HttpGet("order-info/{session_id}")]
        public async Task<ActionResult> GetOrderInfo(string session_id)
        {
            var options = new SessionGetOptions
            {
                Expand = new List<string> { "customer", "invoice" }
            };

            var service = new SessionService();
            var session = await service.GetAsync(session_id, options);
            _logger.LogInformation($" sessions Customer Id=>{session.CustomerId}");
            return Ok(session);
        }


    }
}