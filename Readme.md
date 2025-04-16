# 📝 TechOdyssey - Blog Platform

**TechOdyssey**, kullanıcıların teknoloji odaklı blog gönderileri okuyabileceği, yorum yapabileceği ve kendi içeriklerini oluşturabileceği modern bir MVC tabanlı blog platformudur.

---
## 🎥 Uygulama Tanıtım Videoları

<a href="https://drive.google.com/file/d/19KTJl0I1-y-paOR5M8GBglRr425RxJGB/view?usp=sharing" target="_blank">
  <img src="https://img.shields.io/badge/🎬%20Postman%20API%20Test%20Videosu-blue?style=for-the-badge" alt="Postman API Test Videosu">
</a>

<a href="https://drive.google.com/file/d/1t-hTMDQxl8p5GvNxmW92wE_HzgPeUUwQ/view?usp=sharing" target="_blank">
  <img src="https://img.shields.io/badge/🎬%20Blog%20Platformu%20Kullanım%20Videosu-green?style=for-the-badge" alt="TechOdyssey-Blog Platform Kullanım Videosu">
</a>

## 🚀 Özellikler

### 👤 Misafir Kullanıcı
- Ana sayfadan tüm blog gönderilerini görebilir.
- Blog detaylarını inceleyebilir, yorumları okuyabilir.
- Kategori filtreleme yaparak içerikleri listeleyebilir.
- Kayıt olma ve giriş yapma ekranlarına erişebilir.

### ✅ Kayıtlı Kullanıcı
- Giriş yaptıktan sonra:
  - Yeni blog gönderisi oluşturabilir.
  - Kendi gönderilerini güncelleyebilir veya silebilir.
  - Gönderilere yorum ekleyebilir.
  - Gönderileri kategoriye göre filtreleyebilir.

---

## 🛠️ Kullanılan Teknolojiler

### ⚙️ Backend
- **ASP.NET Core Web API** – RESTful API servislerini sunar. Controller tabanlı yapı ile dış dünyaya veri sağlar.
- **Entity Framework Core** – Veritabanı işlemleri için ORM olarak kullanıldı.  
  - `Code First` yaklaşımıyla modellemeler yapıldı.  
  - `EF Core Migrations` ile veritabanı şeması otomatik oluşturuldu ve güncellendi.
- **MS SQL Server** – Uygulamanın ilişkisel veritabanı motorudur.
- **JWT (JSON Web Token)** – Kullanıcı kimlik doğrulama ve yetkilendirme işlemleri için kullanıldı. MVC tarafında token bazlı erişimle korunan API uç noktalarına erişim sağlandı.

### 🖥️ Frontend (MVC Katmanı)
- **ASP.NET Core MVC** – Uygulamanın frontend tarafında kullanıldı. Razor View ile dinamik HTML sayfaları oluşturuldu.
- **Bootstrap 5** – Responsive ve modern arayüz tasarımı için kullanıldı.
  - MVC projesine **LibMan (Library Manager)** üzerinden yüklendi (`libman.json` ile).  
  - Bootstrap CSS ve JS kütüphaneleri proje klasörlerine çekildi, bu sayede CDN bağımlılığı olmadan offline çalışabilir hale geldi.
- **Model Doğrulama (Validation)**
  - `ViewModel` sınıfları üzerinde `[Required]`, `[StringLength]` gibi anotasyonlarla **server-side validation** uygulandı.  
  - Controller tarafında `ModelState.IsValid` ile validasyon kontrolü yapıldı.
  - Razor View’larda `jquery.validate.min.js` ve `jquery.validate.unobtrusive.min.js` kullanılarak **client-side validation** da sağlandı.
- **jQuery** – Doğrulama işlemlerinde ve kullanıcı arayüzü etkileşimlerinde kullanıldı.
- **Bootstrap Icons** – Görsel zenginlik ve ikon kullanımı.

---

## 📸 Uygulama Görselleri

### 🎯 Misafir Kullanıcı Arayüzü

**Ana Sayfa**  
![Image](https://github.com/user-attachments/assets/a3ee9caa-f10f-4bb8-8ced-32510fbb7416)

**Blog Detay ve Yorumlar**  
![Image](https://github.com/user-attachments/assets/1742bcef-d1cc-4f7d-a557-127a80b1c888)

**Kategoriye Göre Filtreleme**  
![Image](https://github.com/user-attachments/assets/950bd45c-c731-4980-892a-23bac0e97f87)

**Kayıt Olma Ekranı**  
![Image](https://github.com/user-attachments/assets/caaf5695-d071-4253-b7e3-f696957a15cd)

**Giriş Yapma Ekranı**  
![Image](https://github.com/user-attachments/assets/b75a9681-85d6-4797-95e3-685e528f333e)

---

### 🔐 Kayıtlı Kullanıcı Arayüzü

**Ana Sayfa (Login sonrası)**  
![Image](https://github.com/user-attachments/assets/b1d08e20-8e41-4c55-9095-5e95bc6405e3)

**Yorum Ekleme ve Detay Görüntüleme**  
![Image](https://github.com/user-attachments/assets/573a366c-27db-41f5-9e21-5fb7043d3442)

**Yeni Blog Gönderisi Oluşturma**  
![Image](https://github.com/user-attachments/assets/050c988e-7206-483a-8e90-7b4eb473e278)

**Blog Gönderisi Güncelleme**  
![Image](https://github.com/user-attachments/assets/7fd40793-ce4e-46e6-92c8-8264d1eff86d)

**Blog Gönderisi Silme**  
![Image](https://github.com/user-attachments/assets/ad77b54d-0e22-4e81-b4c1-38dad11163d0)

**Kategori Filtreleme**  
![Image](https://github.com/user-attachments/assets/d5bdadc0-35f4-4681-aeff-ca468c7b512e)

---

