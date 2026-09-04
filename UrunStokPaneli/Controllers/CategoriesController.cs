using Microsoft.AspNetCore.Mvc;
using UrunStokPaneli.Data;
using UrunStokPaneli.Models;

namespace UrunStokPaneli.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList(); //mssql'deki Categories tablosunu getirir

            return View(categories);
        }

        //Kullanıcı kategori ekleme sayfasını istediğinde bu metot çalışacak.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category) //Kullanıcı forma: Elektronik yazıp kaydet butonuna basınca () içerisindeki
                                                       //formdan gelen bilgiyi alıyor.
        {
            _context.Categories.Add(category); //girilen kategoriyi ekler.
            _context.SaveChanges(); //mssql veritabanına kaydeder.
            return RedirectToAction("Index"); //kullanıcıyı kategori listesi sayfasına yönlendirir.
        }
    }
}
