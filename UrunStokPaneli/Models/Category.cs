namespace UrunStokPaneli.Models
{
    public class Category
    {
        public int Id { get; set; } // Kategorinin benzersiz kimliği

        public string Name { get; set; } // Kategorinin adı

        public List<Product> Products { get; set; } // Bu kategoriye ait ürünlerin listesi
    }
}
