# UrunStokPaneli

## Proje Hakkında

UrunStokPaneli, ürün ve stok yönetimini kolaylaştırmak amacıyla geliştirilmiş bir web uygulamasıdır. Projede ASP.NET Core MVC, Entity Framework Core, MSSQL, EPPlus, HTML ve CSS teknolojileri kullanılmıştır.

## Backend Geliştirmeleri

UrunStokPaneli projesinin backend tarafını ASP.NET Core MVC, Entity Framework Core ve MSSQL kullanarak geliştirdim. Öncelikle MSSQL veritabanı bağlantısını kurarak ürün ve kategori tablolarını oluşturdum ve aralarındaki ilişkileri Entity Framework üzerinden yapılandırdım.

Ürünlerin eklenmesi, listelenmesi, güncellenmesi ve silinmesi için CRUD işlemlerini geliştirdim. Ürün ve kategori bilgilerinin birbirleriyle ilişkili şekilde yönetilmesini sağladım.

Projenin temel özelliklerinden biri olan Excel aktarım sistemini EPPlus kullanarak geliştirdim. Excel dosyasındaki ürün bilgilerini okuyarak veritabanına aktardım. Aktarım sırasında aynı ürünün tekrar eklenmesini engelleyen duplicate kontrolü, boş alan kontrolü, stok değerinin geçerlilik kontrolü, negatif stok kontrolü ve Excel'deki kategorinin veritabanında bulunup bulunmadığını kontrol eden doğrulamalar ekledim. Ayrıca geçersiz dosya türü ve boş Excel sayfası gibi durumlar için hata kontrolleri oluşturdum. Veritabanına kayıt işlemini tüm kontroller tamamlandıktan sonra gerçekleştirecek şekilde düzenledim.

Ürün listesinin daha kolay yönetilebilmesi için ürün adına, ürün koduna ve kategoriye göre arama ve filtreleme özellikleri geliştirdim.

Stok takibini daha anlamlı hale getirmek için ürünlerin sisteme ilk giriş yaptığı stok miktarını ayrıca kaydettim. Mevcut stok miktarını ilk stok miktarıyla karşılaştırarak ürünlerin stok durumunu otomatik olarak belirleyen bir yapı geliştirdim. Böylece ürünlerin yeterli stokta, düşük stokta veya stokta yok durumunda olup olmadığı backend tarafından hesaplanabiliyor.

Dashboard için gerekli verileri hazırlayan ayrı bir ViewModel ve Controller yapısı oluşturdum. Dashboard'da kullanılmak üzere toplam ürün sayısı, toplam stok miktarı, kategori bazında ürün sayıları ve düşük stoklu ürünlerin listesini veritabanından sorgulayarak hazırladım.

Kullanıcı girişlerinin güvenilir olması için Data Annotation ve ModelState kullanarak validation sistemi geliştirdim. Ürün kodu, ürün adı, stok miktarı, birim ve kategori gibi zorunlu alanları kontrol ettim. Stok miktarının geçerli ve negatif olmayan bir değer olmasını sağladım. Hem ürün ekleme hem de ürün düzenleme işlemlerinde validation kontrollerini uyguladım.

Son olarak tüm backend fonksiyonlarını tek tek test ederek Excel aktarımı, CRUD işlemleri, arama ve filtreleme, stok takibi, dashboard verileri ve validation süreçlerinin birlikte düzgün çalıştığını kontrol ettim.

## Frontend Geliştirmeleri

Frontend tarafında, kullanıcıların ürün ve stok işlemlerini kolayca gerçekleştirebilmeleri için modern, sade ve kullanıcı dostu bir arayüz tasarladım. Sayfalar arasında görsel bütünlük sağlamak amacıyla ortak bir tasarım dili ve yerleşim yapısı oluşturdum.

Genel tasarımda koyu lacivert yan menü, beyaz üst menü, açık gri arka plan ve mavi vurgu renkleri kullandım. Sayfa başlıkları, kartlar, tablolar, formlar ve butonlar için ortak CSS sınıfları oluşturarak tutarlı bir görünüm sağladım.

Dashboard sayfasını toplam ürün sayısı, toplam stok miktarı, kategorilere göre ürün sayıları ve düşük stoklu ürünler gibi önemli bilgileri görüntüleyecek şekilde düzenledim.

Ürün yönetimi kapsamında ürünlerin listelenebildiği, aranabildiği ve filtrelenebildiği ürünler sayfasını tasarladım. Yeni ürün ekleme, mevcut ürünleri düzenleme ve ürün silme işlemleri için ayrı arayüzler hazırladım. Form alanlarını, işlem butonlarını ve silme onay ekranını ortak tasarıma uygun hale getirdim.

Kategori yönetimi için kategorilerin listelendiği ve yeni kategorilerin eklenebildiği sayfaları düzenledim.

Excel aktarımı için kullanıcıların `.xlsx` dosyası seçerek ürün verilerini sisteme aktarabilmesini sağlayan bir dosya yükleme arayüzü oluşturdum. Dosya seçme alanını, bilgilendirme mesajını ve aktarım butonlarını diğer sayfalarla uyumlu olacak şekilde tasarladım.

Son olarak ortak sayfa düzenini, yan menü bağlantılarını ve CSS stillerini düzenleyerek uygulamanın farklı sayfalarında tutarlı ve düzenli bir kullanıcı deneyimi oluşturdum.

## Test ve Sonuç

Backend ve frontend geliştirmelerini tamamladıktan sonra ürün ve kategori işlemlerini, arama ve filtreleme özelliklerini, stok durumu hesaplamalarını, Dashboard verilerini ve Excel aktarımını test ettim. Excel aktarımında başarılı veri yükleme, eksik alan, geçersiz stok değeri ve negatif stok miktarı senaryolarını kontrol ettim.

Yapılan geliştirmeler sonucunda ürün ve stok bilgilerinin yönetilebildiği, Excel üzerinden toplu ürün aktarımı yapılabilen ve stok durumlarının takip edilebildiği bir web uygulaması oluşturdum.
