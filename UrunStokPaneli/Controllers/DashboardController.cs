using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrunStokPaneli.Data;
using UrunStokPaneli.Models;

namespace UrunStokPaneli.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Toplam ürün sayısını hesapla
            var totalProducts = _context.Products.Count();

            // Tüm ürünlerin toplam stok miktarını hesapla
            var totalStock = _context.Products.Sum(p => p.StockQuantity);

            // Kategorilere göre ürün sayılarını hesapla
            var categoryProductCounts = _context.Products
                .GroupBy(p => p.Category.Name)
                .Select(g => new CategoryProductCount
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count()
                })
                .ToList();

            // Düşük stoklu ürünleri bul
            var lowStockProducts = _context.Products
                .Include(p => p.Category)
                .Where(p => p.StockQuantity > 0 &&
                            p.StockQuantity * 2 < p.InitialStockQuantity)
                .ToList();

            // Dashboard verilerini ViewModel'e aktar
            var model = new DashboardViewModel
            {
                TotalProducts = totalProducts,
                TotalStock = totalStock,
                CategoryProductCounts = categoryProductCounts,
                LowStockProducts = lowStockProducts
            };

            return View(model);
        }
    }
}