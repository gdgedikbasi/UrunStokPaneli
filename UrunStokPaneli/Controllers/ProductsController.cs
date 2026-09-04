using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrunStokPaneli.Data;
using UrunStokPaneli.Models;

namespace UrunStokPaneli.Controllers
{
    public class ProductsController:Controller
    {
        private readonly ApplicationDbContext _context; //Controller'a veritabanına erişebilmesi için DbContext'i tanıtıyor.

        //ApplicationDbContext'i otomatik olarak oluşturup size veriyorum.
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products= _context.Products.ToList(); //mssql'deki Products tablosunu getirdim.
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList(); // MSSQL'deki Categories tablosundaki kategorileri liste olarak getirdim.
            ViewBag.Categories = categories; //ViewBag ile kategorileri view'e gönderdim.
            return View(); // Create View'ını döndürdüm.
        }
    }
}
