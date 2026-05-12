# 🏦 ASP.NET Core MVC - Professional Bank Management System

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)
![JSON Storage](https://img.shields.io/badge/Database-JSON-orange?style=for-the-badge&logo=json&logoColor=white)

Bu tətbiq, müasir bankçılıq infrastrukturunu simulyasiya edən, dinamik valyuta konvertasiyası mühərrikinə və ətraflı admin idarəetmə panelinə sahib olan **ASP.NET Core MVC** layihəsidir. 

## 🌟 Əsas Özəlliklər

### 👨‍💼 Admin Portalı
- **İstifadəçi İdarəetməsi:** Yeni istifadəçilərin yaradılması (Email unikal yoxlaması ilə), redaktə edilməsi və silinməsi.
- **Təhlükəsizlik:** İstifadəçilərin anlıq olaraq bloklanması və girişinin məhdudlaşdırılması.
- **Hesab Menecmenti:** Müştərilər üçün müxtəlif valyutalarda (AZN, USD, EUR, RUB) yeni bank hesablarının (IBAN simulyasiyası) açılması.
- **Canlı Məzənnə Tənzimlənməsi:** Valyuta məzənnələrinin admin tərəfindən real vaxtda yenilənməsi.

### 👤 Müştəri Paneli
- **Balans İzləmə:** Bütün bank hesablarının və qalıqların tək bir ekranda izlənilməsi.
- **Ağıllı Transfer:** Çarpaz valyuta dəstəyi ilə daxili köçürmələr (Məsələn: USD hesabından AZN hesabına avtomatik konvertasiya ilə pul göndərmə).
- **Profil İdarəetməsi:** Şəxsi məlumatların və sessiyanın idarə olunması.

## 🧠 Texniki Memarlıq

Layihədə **Dependency Injection** və **Service-Oriented Architecture** prinsipləri tətbiq olunmuşdur.

- **Dinamik Konvertasiya:** Transferlər zamanı bütün hesablamalar bankın əsas valyutası (AZN) üzərindən `InvariantCulture` dəstəyi ilə aparılır. Bu, nöqtə/vergül xətalarının qarşısını alır.
- **Məlumat Saxlanılması:** Verilənlər bazası olaraq JSON formatlı fayl sistemindən istifadə olunub. Bu, layihənin portativliyini və sürətini təmin edir.
- **Sessiya İdarəetməsi:** `HttpContext.Session` vasitəsilə rol-əsaslı (Admin/User) avtorizasiya sistemi qurulmuşdur.

## 🛠 Texnologiyalar

- **Backend:** C#, ASP.NET Core MVC
- **Frontend:** Bootstrap 5, Bootstrap Icons, HTML5, CSS3
- **Data:** JSON Serialization (System.Text.Json)
- **Tooling:** Visual Studio 2022, .NET SDK 6.0+

## 🚀 Quraşdırma və İstifadə

1. **Repository-ni kopyalayın:**
   ```bash
   git clone [https://github.com/username/layihe-adi.git](https://github.com/username/layihe-adi.git)
