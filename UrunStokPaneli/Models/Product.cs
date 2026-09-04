namespace UrunStokPaneli.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductCode { get; set; } //excel tablosundaki ürün kodu

        public string ProductName { get; set; } //excel tablosundaki ürün adı

        public int StockQuantity { get; set; } //excel tablosundaki stok miktarı

        public string Unit { get; set; } //excel tablosundaki birim

        public int CategoryId { get; set; } //excel tablosundaki kategori

        public Category Category { get; set; } // Ürünün bağlı olduğu kategori
                                               // Mesela bir ürünün CategoryId değeri 1" demek yerine, veritabanındaki 1 numaralı kategorinin adını da kullanabileceğiz.
    }
}
