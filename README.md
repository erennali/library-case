# Library Management System API

Modern, ölçeklenebilir ve güvenli bir kütüphane yönetim sistemi API'si. ASP.NET Core 9, Clean Architecture ve SOLID prensipleri kullanılarak geliştirilmiştir.

## 🏗️ Mimari

Bu proje **Clean Architecture (Onion Architecture)** pattern'ini kullanır:

```
Library/
├── Library.Domain/          # Entities, Enums, Domain Logic
├── Library.Application/      # Use Cases, DTOs, Interfaces, Validation
├── Library.Infrastructure/  # External Services, Email, SMS
├── Library.Persistence/     # Database, Repositories, EF Core
└── Library.API/            # Controllers, Middleware, Configuration
```

## 🚀 Özellikler

### 📚 Kitap Yönetimi
- Kitap CRUD işlemleri
- Kategori bazlı sınıflandırma
- Stok takibi (toplam kopya, mevcut kopya)
- ISBN ve detaylı kitap bilgileri

### 👥 Üye Yönetimi
- Üye kayıt ve profil yönetimi
- Üyelik türleri (Regular, Premium, Student)
- Üyelik durumu takibi
- Maksimum kitap limiti

### 🔄 Ödünç Alma/İade
- Kitap ödünç alma
- İade işlemleri
- Yenileme (renewal)
- Otomatik ceza hesaplama
- Concurrency koruması

### 💰 Ceza Yönetimi
- Gecikme cezaları
- Ceza ödeme
- Ceza affetme
- Ceza raporları

### 📅 Rezervasyon Sistemi
- Kitap rezervasyonu
- Rezervasyon önceliği
- Otomatik bildirimler
- Rezervasyon süresi yönetimi

### ⭐ Değerlendirme Sistemi
- Kitap puanlama (1-5 yıldız)
- Yorum sistemi
- Onay süreci
- İstatistikler

### 🔔 Bildirim Sistemi
- Email bildirimleri
- Push notifications
- SMS bildirimleri
- Toplu bildirim gönderimi

### 📊 Raporlama
- Döngüsel raporlar
- İstatistikler
- Dashboard
- Export (PDF, Excel, CSV)

### 🔍 Gelişmiş Arama
- Global arama
- Filtreleme
- Sıralama
- Arama önerileri

### 📈 İstatistikler
- Döngüsel istatistikler
- Üye aktivite analizi
- Kitap popülerliği
- Trend analizi

### 🔐 Güvenlik
- JWT tabanlı kimlik doğrulama
- Rol tabanlı yetkilendirme
- Audit logging
- Input validation

### 🏥 Sistem Sağlığı
- Health checks
- Database connectivity
- Service monitoring
- Performance metrics

## 🛠️ Teknolojiler

- **.NET 9** - En son .NET sürümü
- **ASP.NET Core Web API** - Modern web API framework
- **Entity Framework Core 9** - ORM ve veritabanı erişimi
- **SQL Server** - Ana veritabanı
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **MediatR** - CQRS pattern (opsiyonel)
- **Health Checks** - Sistem sağlığı kontrolü

## 📋 API Endpoints

### Books
- `GET /api/books` - Kitap listesi
- `GET /api/books/{id}` - Kitap detayı
- `POST /api/books` - Yeni kitap ekleme
- `PUT /api/books/{id}` - Kitap güncelleme
- `DELETE /api/books/{id}` - Kitap silme
- `GET /api/books/available` - Mevcut kitaplar
- `GET /api/books/overdue` - Gecikmiş kitaplar
- `GET /api/books/popular` - Popüler kitaplar

### Transactions
- `POST /api/transactions/borrow` - Kitap ödünç alma
- `POST /api/transactions/return` - Kitap iade
- `POST /api/transactions/renew` - Kitap yenileme
- `GET /api/transactions/overdue` - Gecikmiş işlemler
- `GET /api/transactions/active` - Aktif işlemler

### Members
- `GET /api/members` - Üye listesi
- `GET /api/members/{id}` - Üye detayı
- `POST /api/members` - Yeni üye ekleme
- `PUT /api/members/{id}` - Üye güncelleme
- `DELETE /api/members/{id}` - Üye silme
- `GET /api/members/active` - Aktif üyeler
- `GET /api/members/overdue` - Gecikmiş üyeler

### Fines
- `GET /api/fines` - Ceza listesi
- `POST /api/fines/pay` - Ceza ödeme
- `POST /api/fines/waive` - Ceza affetme
- `GET /api/fines/pending` - Bekleyen cezalar
- `GET /api/fines/overdue` - Gecikmiş cezalar

### Reservations
- `POST /api/reservations` - Rezervasyon oluşturma
- `POST /api/reservations/cancel` - Rezervasyon iptali
- `GET /api/reservations/active` - Aktif rezervasyonlar
- `GET /api/reservations/expired` - Süresi dolmuş rezervasyonlar

### Reviews
- `POST /api/reviews` - Değerlendirme ekleme
- `PUT /api/reviews/{id}` - Değerlendirme güncelleme
- `DELETE /api/reviews/{id}` - Değerlendirme silme
- `GET /api/reviews/approved` - Onaylanmış değerlendirmeler
- `GET /api/reviews/pending` - Bekleyen değerlendirmeler

### Notifications
- `POST /api/notifications` - Bildirim oluşturma
- `POST /api/notifications/bulk` - Toplu bildirim
- `GET /api/notifications/unread` - Okunmamış bildirimler
- `POST /api/notifications/mark-read` - Bildirimi okundu olarak işaretleme

### Reports
- `POST /api/reports/generate` - Rapor oluşturma
- `GET /api/reports/download/{id}` - Rapor indirme
- `GET /api/reports/list` - Rapor listesi
- `GET /api/reports/overdue-summary` - Gecikmiş özeti

### Dashboard
- `GET /api/dashboard/overview` - Genel bakış
- `GET /api/dashboard/circulation-stats` - Döngüsel istatistikler
- `GET /api/dashboard/overdue-summary` - Gecikmiş özeti
- `GET /api/dashboard/book-popularity` - Kitap popülerliği

### Search
- `GET /api/search/global` - Global arama
- `GET /api/search/books` - Kitap arama
- `GET /api/search/members` - Üye arama
- `GET /api/search/advanced` - Gelişmiş arama
- `GET /api/search/suggestions` - Arama önerileri

### Statistics
- `GET /api/statistics/overview` - Genel istatistikler
- `GET /api/statistics/circulation` - Döngüsel istatistikler
- `GET /api/statistics/books` - Kitap istatistikleri
- `GET /api/statistics/members` - Üye istatistikleri
- `GET /api/statistics/trends` - Trend analizi

### Health
- `GET /api/health` - Sistem sağlığı
- `GET /api/health/detailed` - Detaylı sağlık kontrolü
- `GET /api/health/ready` - Hazır olma durumu
- `GET /api/health/live` - Canlılık durumu

## 🚀 Kurulum

### Gereksinimler
- .NET 9 SDK
- SQL Server 2022
- Visual Studio 2022 veya VS Code

### 1. Repository'yi klonlayın
```bash
git clone https://github.com/yourusername/library-management-api.git
cd library-management-api
```

### 2. Veritabanı bağlantı string'ini ayarlayın
`Library.API/appsettings.json` dosyasında connection string'i güncelleyin:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=LibraryDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 3. Migration'ları çalıştırın
```bash
cd Library.API
dotnet ef database update
```

### 4. Uygulamayı çalıştırın
```bash
dotnet run
```

API varsayılan olarak `https://localhost:7000` adresinde çalışacaktır.

## 📚 Veritabanı Şeması

### Ana Tablolar
- **Books** - Kitap bilgileri
- **Categories** - Kategori sınıflandırması
- **Members** - Üye bilgileri
- **Transactions** - Ödünç alma/iade kayıtları
- **Fines** - Ceza kayıtları
- **Reservations** - Rezervasyon kayıtları
- **Reviews** - Kitap değerlendirmeleri
- **Notifications** - Bildirim kayıtları
- **Librarians** - Kütüphaneci bilgileri
- **LibrarySettings** - Sistem ayarları

### İlişkiler
- Bir kitap bir kategoriye ait olabilir
- Bir üye birden fazla kitap ödünç alabilir
- Her işlem bir üye ve kitap ile ilişkilidir
- Ceza işlemlerle ilişkilidir
- Rezervasyonlar üye ve kitaplarla ilişkilidir

## 🔒 Güvenlik

### Kimlik Doğrulama
- JWT tabanlı token sistemi
- Refresh token desteği
- Token expiration yönetimi

### Yetkilendirme
- **Admin** - Tam sistem erişimi
- **Librarian** - Günlük işlemler
- **Member** - Kendi bilgileri ve işlemleri

### Validation
- FluentValidation ile input validation
- SQL injection koruması
- XSS koruması
- Rate limiting

## 📊 Monitoring ve Logging

### Health Checks
- Database connectivity
- Service availability
- Response time monitoring

### Logging
- Structured logging
- Correlation ID tracking
- Error logging
- Performance logging

## 🧪 Test

### Unit Tests
```bash
dotnet test Library.Tests.Unit
```

### Integration Tests
```bash
dotnet test Library.Tests.Integration
```

### API Tests
```bash
dotnet test Library.Tests.API
```

## 📈 Performance

### Optimizasyonlar
- Async/await pattern
- Connection pooling
- Query optimization
- Caching strategies

### Monitoring
- Response time tracking
- Memory usage monitoring
- Database query performance
- Error rate tracking

## 🚀 Deployment

### Docker
```bash
docker build -t library-api .
docker run -p 8080:80 library-api
```

### Azure
- Azure App Service
- Azure SQL Database
- Azure Key Vault
- Application Insights

### AWS
- AWS ECS/Fargate
- AWS RDS
- AWS Secrets Manager
- CloudWatch

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add amazing feature'`)
4. Push yapın (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

## 📞 İletişim

- **Proje Sahibi**: [Your Name]
- **Email**: [your.email@example.com]
- **GitHub**: [@yourusername]

## 🙏 Teşekkürler

Bu projeyi geliştirmemde yardımcı olan herkese teşekkürler:

- ASP.NET Core ekibi
- Entity Framework ekibi
- Clean Architecture topluluğu
- Open source katkıda bulunanlar

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!

