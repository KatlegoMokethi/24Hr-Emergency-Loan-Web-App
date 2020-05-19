using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BuhleProject.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace BuhleProject.Controllers
{
    public class PaymentController : Controller
    {
        IRepositoryWrapper RepositoryWrapper; 
        
        public PaymentController(IRepositoryWrapper repositoryWrapper)
        {
            RepositoryWrapper = repositoryWrapper;
        }

        private int amount = 100;
        public IActionResult Index(int id)
        {
            var v = RepositoryWrapper.LoanRepository.GetById(id);
              ViewBag.PaymentAmount = v.amount;
            return View("Payment");
        }

        [HttpPost]
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            StripeConfiguration.ApiKey = "sk_test_ZxgfYV4PJz2m3gWvKXWFHxmv00XtyoLItH";

            Dictionary<string, string> Metadata = new Dictionary<string, string>();
            Metadata.Add("Product", "RubberDuck");
            Metadata.Add("Quantity", "10");
            var options = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = "USD",
                Description = "Buying 10 rubber ducks",
                Customer = createCust("sk_test_ZxgfYV4PJz2m3gWvKXWFHxmv00XtyoLItH").Id,//i added
                //SourceId = stripeToken,
                ReceiptEmail = stripeEmail,
                Metadata = Metadata
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
            return View();
        }

        private readonly string WebhookSecret = "whsec_OurSigningSecret";

        //Previous actions

        [HttpPost]
        public IActionResult ChargeChange()
        {
            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], WebhookSecret, throwOnApiVersionMismatch: true);
                Charge charge = (Charge)stripeEvent.Data.Object;
                switch (charge.Status)
                {
                    case "succeeded":
                        //This is an example of what to do after a charge is successful
                        //charge.Metadata.TryGetValue("Product", out string Product);
                        //charge.Metadata.TryGetValue("Quantity", out string Quantity);
                        //Database.ReduceStock(Product, Quantity);
                        break;
                    case "failed":
                        //Code to execute on a failed charge
                        break;
                }
            }
            catch (Exception e)
            {
                //e.Ship(HttpContext);
                return BadRequest();
            }
            return Ok();
        }

        private Customer createCust(string key)
        {
            StripeConfiguration.ApiKey = key;

            var options = new CustomerCreateOptions
            {
                Description = "My First Test Customer (created for API docs)",
            };
            var service = new CustomerService();
            return service.Create(options);
        }
    }
}