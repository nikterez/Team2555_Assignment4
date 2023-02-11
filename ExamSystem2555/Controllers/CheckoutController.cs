using Microsoft.AspNetCore.Mvc;
using Stripe;
using WebApp.PaymentModel;

namespace WebApp.Controllers
{
    public class CheckoutController : Controller
    {
		[TempData]
		public string TotalAmount { get; set; }


		public IActionResult Index()
		{
			var cart = WorkingWithSession.GetObjectFromJson<List<SelectedItem>>(HttpContext.Session, "cart");
			ViewBag.cart = cart;
			ViewBag.DollarAmount = cart.Sum(item => item.Certificate.Price * item.Quantity);
			ViewBag.total = ViewBag.DollarAmount;


			double total = ViewBag.total;


			TotalAmount = total.ToString();
			TempData["TotalAmount"] = TotalAmount;
			return View();
		}


		public IActionResult Processing(string stripeToken, string stripeEmail)
		{
			var optionCust = new CustomerCreateOptions
			{
				Email = stripeEmail,
				Name = "Gio",
				Phone = "2338595119"
			};
			var serviceCust = new CustomerService();
			Customer customer = serviceCust.Create(optionCust);
			var optionsCharge = new ChargeCreateOptions
			{
				Amount = Convert.ToInt64(TempData["TotalAmount"]),
				Currency = "EUR",
				Description = "Certificate Selling amount",
				Source = stripeToken,
				ReceiptEmail = stripeEmail


			};
			var serviceCharge = new ChargeService();
			Charge charge = serviceCharge.Create(optionsCharge);
			if (charge.Status == "successded")
			{
				ViewBag.AmountPaid = charge.Amount;
				ViewBag.Customer = customer.Name;
			}
			return View();




		}


	}
}
