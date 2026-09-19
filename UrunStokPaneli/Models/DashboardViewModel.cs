namespace UrunStokPaneli.Models
{
    public class DashboardViewModel
    {
        // Sistemdeki toplam ürün sayısı
        public int TotalProducts { get; set; }

        // Tüm ürünlerin toplam stok miktarı
        public int TotalStock { get; set; }

        // Kategorilere göre ürün sayıları
        public List<CategoryProductCount> CategoryProductCounts { get; set; }

        // Düşük stoklu ürünler
        public List<Product> LowStockProducts { get; set; }
    }
}