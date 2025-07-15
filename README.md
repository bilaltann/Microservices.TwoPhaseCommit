# 🧩 Microservices.TwoPhaseCommit

Bu proje, mikroservis mimarisi içerisinde **Two-Phase Commit (2PC)** protokolünü kullanarak **dağıtık işlem tutarlılığı (distributed transaction consistency)** sağlamayı amaçlamaktadır.

## 🧱 Servisler

- 🧭 **Coordinator** – İşlemi başlatır, kontrol eder, commit veya rollback yaptırır.
- 🛒 **OrderAPI** – Sipariş yönetimi
- 💳 **PaymentAPI** – Ödeme işlemleri
- 📦 **StockAPI** – Stok yönetimi
- 📩 **MailAPI** – E-posta bildirimi
- 📚 **Shared** – Ortak modeller ve yapıların bulunduğu katman


- ## 🧩 Two-Phase Commit (2PC) Nedir?

**Two-Phase Commit (2PC)**, dağıtık sistemlerde **veri tutarlılığını sağlamak** amacıyla kullanılan bir koordinasyon protokolüdür.  
Özellikle bir işlemin birden fazla servis veya veritabanı üzerinde **atomik** (bölünemez) ve **tutarlı** biçimde tamamlanmasını garanti eder.

### 🎯 Temel Amaç:
> Bir işlemin tüm kaynaklarda **başarıyla tamamlanması** veya **hiçbirinde uygulanmaması**.

### 🛠️ Aşamalar:

1. **Prepare Phase (Hazırlık Aşaması)**  
   - Koordinatör, ilgili tüm mikroservislere “hazır mısınız?” mesajı gönderir.  
   - Servisler kendi işlemlerini hazırlayıp, sadece onay (ack) verir. Kalıcı yazım henüz yapılmaz.

2. **Commit Phase (Taahhüt Aşaması)**  
   - Tüm servislerden olumlu yanıt geldiyse koordinatör `commit` sinyali gönderir.  
   - Eğer bir servis hata dönerse tüm servislere `rollback` komutu gönderilir.
