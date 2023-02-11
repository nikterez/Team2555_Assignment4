using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.PaymentModel;

namespace WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = WorkingWithSession.GetObjectFromJson<List<SelectedItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Certificate.Price * item.Quantity);
            return View();
        }

		public async Task<IActionResult> Buy(int Id)
		{
			//    ProductModelproductModel = new ProductModel();
			var certificate = _context.Certificates.FirstOrDefault(m => m.CertificateId == Id);


			if (WorkingWithSession.GetObjectFromJson<List<SelectedItem>>(HttpContext.Session, "cart") == null)
			{
				List<SelectedItem> cart = new List<SelectedItem>();
				cart.Add(new SelectedItem { Certificate = await _context.Certificates.FindAsync(Id), Quantity = 1 });
				WorkingWithSession.SetObjectasJson(HttpContext.Session, "cart", cart);
			}
			else
			{
				List<SelectedItem> cart = WorkingWithSession.GetObjectFromJson<List<SelectedItem>>(HttpContext.Session, "cart");
				int index = IsExist(Id);
				if (index != -1)
				{
					cart[index].Quantity++;
				}
				else
				{
					cart.Add(new SelectedItem { Certificate = await _context.Certificates.FindAsync(Id), Quantity = 1 });
				}
				WorkingWithSession.SetObjectasJson(HttpContext.Session, "cart", cart);

			}
			return RedirectToAction("Index");
		}

		private int IsExist(int Id)
		{
			List<SelectedItem> cart = WorkingWithSession.GetObjectFromJson<List<SelectedItem>>(HttpContext.Session, "cart");
			for (int i = 0; i < cart.Count; i++)
			{
				if (cart[i].Certificate.CertificateId.Equals(Id))
				{
					return i;
				}
			}
			return -1;
		}


	}
}
