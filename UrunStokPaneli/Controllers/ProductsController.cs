using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrunStokPaneli.Data;
using UrunStokPaneli.Models;
using OfficeOpenXml;

namespace UrunStokPaneli.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context; //Controller'a veritabanına erişebilmesi için DbContext'i tanıtıyor.

        //ApplicationDbContext'i otomatik olarak oluşturup size veriyorum.
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .ToList();

            return View(products);
        }

        //Oluşturma metodu
        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList(); // MSSQL'deki Categories tablosundaki kategorileri liste olarak getirdim.
            ViewBag.Categories = categories; //ViewBag ile kategorileri view'e gönderdim.
            return View(); // Create View'ını döndürdüm.
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product); //Product modelinden gelen veriyi Products tablosuna ekledim.
            _context.SaveChanges(); //Değişiklikleri kaydettim.
            return RedirectToAction("Index"); //Index action'ına yönlendirdim.

        }

        //Düzenleme metodu
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.Products.Update(product);//EF Core'a "Bu ürün mevcut bir ürün, bilgileri güncellendi." diyorum.
            _context.SaveChanges(); //değişikliği mssql'e kaydediyorum.
            return RedirectToAction("Index"); //ürün listesine dönüyorum.
        }

        //Silme Metodu

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //Excel dosyayı yükleme metodu
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("Dosya seçilmedi.");
            }

            ExcelPackage.License.SetNonCommercialPersonal("Duygu"); //EPPlus'a kişisel kullanım lisansını kimin adına ayarladığımızı belirtiyor.

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream); //yüklediğimiz Excel dosyasını bu bellekteki akışa kopyalıyor.

                //Excel dosyasını bilgisayarımıza ayrıca kaydetmeden, yüklenen dosyayı bellekte okumamızı sağlıyor.
                using (var package = new ExcelPackage(stream)) //"Bu Excel dosyasını aç ve okumama izin ver."
                {
                    var worksheet = package.Workbook.Worksheets[0]; //Excel'in ilk çalışma sayfasını (Sheet1) alıyoruz.

                    var rowCount = worksheet.Dimension.Rows;

                    for (int row=2;row<=rowCount;row++)
                    {
                        // Excel'den ürün bilgilerini oku
                        var productCode = worksheet.Cells[row, 1].Value;
                        var productName= worksheet.Cells[row, 2].Value;
                        var stockQuantity= worksheet.Cells[row, 3].Value;
                        var unit= worksheet.Cells[row, 4].Value;
                        var category= worksheet.Cells[row, 5].Value;

                        // Excel satırından Product nesnesi oluştur
                        var product = new Product
                        {
                            ProductCode = productCode.ToString(),
                            ProductName = productName.ToString(),
                            StockQuantity = Convert.ToInt32(stockQuantity),
                            Unit = unit.ToString()
                        };

                        // Kategoriyi veritabanından bul
                        var categoryEntity = _context.Categories
                            .FirstOrDefault(c => c.Name == category.ToString());

                        // Bulunan kategorinin Id'sini ürüne bağla
                        product.CategoryId = categoryEntity.Id;

                        //Ürün daha önce eklenmiş mi kontrol et
                        var existingProduct = _context.Products
                            .FirstOrDefault(p => p.ProductCode == product.ProductCode);

                        //Ürün daha önce yoksa ekle
                        if(existingProduct==null)
                        {
                            _context.Products.Add(product);
                        }
                        
                    }
                    _context.SaveChanges();
                    
                    return Content($"Excel'deki {rowCount - 1} ürün okundu.");
                }
            }
        }
    }
}
