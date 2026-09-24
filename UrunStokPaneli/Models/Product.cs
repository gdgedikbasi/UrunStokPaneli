using System.ComponentModel.DataAnnotations;

namespace UrunStokPaneli.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün kodu boş bırakılamaz.")]
        public string ProductCode { get; set; } //excel tablosundaki ürün kodu


        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        public string ProductName { get; set; } //excel tablosundaki ürün adı


        [Required(ErrorMessage = "Stok miktarı boş bırakılamaz.")]

        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır.")]
        public int? StockQuantity { get; set; } //excel tablosundaki stok miktarı

        public int InitialStockQuantity { get; set; } //ürünün sisteme ilk girilen stok miktarı


        [Required(ErrorMessage = "Birim boş bırakılamaz.")]
        public string Unit { get; set; } //excel tablosundaki birim

        
        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int? CategoryId { get; set; } //excel tablosundaki kategori


        public Category? Category { get; set; } // Ürünün bağlı olduğu kategori
                                               // Mesela bir ürünün CategoryId değeri 1" demek yerine, veritabanındaki 1 numaralı kategorinin adını da kullanabileceğiz.
    
        public string StockStatus
        {
            get
            {
                if(StockQuantity==0)
                {
                    return "Stok Yok";
                }
                if(StockQuantity < InitialStockQuantity /2.0)
                {
                    return "Düşük Stok";
                }

                return "Yeterli Stok";
            }
        }
    }
}
