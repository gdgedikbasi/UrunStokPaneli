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

        public IActionResult Index(string search, int? categoryId)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // Ürün adına veya ürün koduna göre arama yap
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => 
                    p.ProductName.Contains(search) ||
                    p.ProductCode.Contains(search));
            }

            //kategoriye göre filtreleme yap
            if(categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            // Kategorileri filtreleme kutusunda göstermek için getir
            ViewBag.Categories = _context.Categories.ToList();
            return View(products.ToList());
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
            //formdaki bilgilerin validation kurallarına uygun olup olmadığını kontrol ettim.
            if(!ModelState.IsValid)
            {
                //kategorileri tekrar forma gönder
                ViewBag.Categories = _context.Categories.ToList();
                //hatalar varsa ürünü kaydetmeden forma geri dön
                return View(product);
            }
            // Ürün ilk kez eklenirken mevcut stok miktarını ilk stok olarak kaydet.
            product.InitialStockQuantity = product.StockQuantity.Value;

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
            // Formdaki bilgilerin validation kurallarına uygun olup olmadığını kontrol et
            if (!ModelState.IsValid)
            {
                // Kategorileri tekrar forma gönder
                ViewBag.Categories = _context.Categories.ToList();

                // Hatalar varsa ürünü güncellemeden forma geri dön
                return View(product);
            }


            // Veritabanındaki mevcut ürünü bul
            var existingProduct = _context.Products.Find(product.Id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Sadece değiştirilebilir bilgileri güncelle
            existingProduct.ProductCode = product.ProductCode;
            existingProduct.ProductName = product.ProductName;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.Unit = product.Unit;
            existingProduct.CategoryId = product.CategoryId;

            // InitialStockQuantity değiştirilmez.
            // Çünkü bu değer ürünün sisteme ilk girdiği stok miktarıdır.

            _context.SaveChanges();

            return RedirectToAction("Index");
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

            // Sadece Excel dosyalarını kabul et
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (extension != ".xlsx")
            {
                return Content("Lütfen .xlsx uzantılı bir Excel dosyası seçin.");
            }

            ExcelPackage.License.SetNonCommercialPersonal("Duygu");

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);

                using (var package = new ExcelPackage(stream))
                {
                    // Excel dosyasında çalışma sayfası var mı kontrol et
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        return Content("Excel dosyasında çalışma sayfası bulunamadı.");
                    }

                    var worksheet = package.Workbook.Worksheets[0];

                    // Excel sayfası tamamen boş mu kontrol et
                    if (worksheet.Dimension == null)
                    {
                        return Content("Excel çalışma sayfası boş.");
                    }

                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Excel hücrelerindeki değerleri metin olarak al
                        var productCode = worksheet.Cells[row, 1].Text?.Trim();
                        var productName = worksheet.Cells[row, 2].Text?.Trim();
                        var stockQuantity = worksheet.Cells[row, 3].Text?.Trim();
                        var unit = worksheet.Cells[row, 4].Text?.Trim();
                        var category = worksheet.Cells[row, 5].Text?.Trim();

                        // Boş alanları kontrol et
                        var missingFields = new List<string>();

                        if (string.IsNullOrWhiteSpace(productCode))
                        {
                            missingFields.Add("Ürün Kodu");
                        }

                        if (string.IsNullOrWhiteSpace(productName))
                        {
                            missingFields.Add("Ürün Adı");
                        }

                        if (string.IsNullOrWhiteSpace(stockQuantity))
                        {
                            missingFields.Add("Stok Miktarı");
                        }

                        if (string.IsNullOrWhiteSpace(unit))
                        {
                            missingFields.Add("Birim");
                        }

                        if (string.IsNullOrWhiteSpace(category))
                        {
                            missingFields.Add("Kategori");
                        }

                        // Eksik alan varsa kullanıcıya bildir
                        if (missingFields.Any())
                        {
                            return Content(
                                $"Excel'deki {row}. satırda şu alanlar boş: {string.Join(", ", missingFields)}"
                            );
                        }

                        // Stok miktarını sayıya çevirmeyi dene
                        if (!int.TryParse(stockQuantity, out int stock))
                        {
                            return Content(
                                $"Excel'deki {row}. satırda stok miktarı geçersiz: {stockQuantity}"
                            );
                        }

                        // Stok miktarı negatif olamaz
                        if (stock < 0)
                        {
                            return Content(
                                $"Excel'deki {row}. satırda stok miktarı 0'dan küçük olamaz: {stock}"
                            );
                        }

                        // Yeni ürün oluştur
                        var product = new Product
                        {
                            ProductCode = productCode,
                            ProductName = productName,
                            StockQuantity = stock,
                            InitialStockQuantity = stock,
                            Unit = unit
                        };

                        // Kategoriyi veritabanından bul
                        var categoryEntity = _context.Categories
                            .FirstOrDefault(c => c.Name == category);

                        // Kategori bulunamadıysa hata ver
                        if (categoryEntity == null)
                        {
                            return Content(
                                $"Excel'deki {row}. satırda kategori bulunamadı: {category}"
                            );
                        }

                        product.CategoryId = categoryEntity.Id;

                        // Ürün daha önce eklenmiş mi kontrol et
                        var existingProduct = _context.Products
                            .FirstOrDefault(p => p.ProductCode == product.ProductCode);

                        // Daha önce yoksa ekle
                        if (existingProduct == null)
                        {
                            _context.Products.Add(product);
                        }
                    }

                    // Tüm satırlar kontrol edildikten sonra tek seferde kaydet
                    _context.SaveChanges();

                    return Content($"Excel'deki {rowCount - 1} ürün okundu.");
                }
            }
        }
    }
}
