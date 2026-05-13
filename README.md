# 🏦 ASP.NET Core MVC - NextGen Banking System (v10.0)

![C#](https://img.shields.io/badge/C%23-15.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET Core](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL](https://img.shields.io/badge/Database-SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-2026-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)

Bu layihə, müasir bankçılıq infrastrukturunu simulyasiya edən, **Entity Framework Core** üzərində qurulmuş və dinamik valyuta mühərrikinə sahib olan professional bank idarəetmə sistemidir.

## 🚀 Layihənin Öndə Çıxan Özəllikləri

### 💎 Ağıllı Valyuta Sistemi
* **Dinamik Konvertasiya:** Admin tərəfindən müəyyən edilən canlı məzənnələr əsasında AZN, USD, EUR və RUB arasında çarpaz köçürmələr.
* **Avtomatik Hesablama:** Köçürmə zamanı mənbə və hədəf valyutaları fərqli olduqda, sistem avtomatik olaraq məbləği əsas valyuta (AZN) üzərindən hesablayır.

### 🛡️ Admin və Nəzarət Paneli
* **İstifadəçi Menecmenti:** Yeni müştəri yaratma, məlumatların redaktəsi və tam silinməsi.
* **Təhlükəsizlik:** Role-based Authorization (Admin/Customer) və istifadəçilərin anlıq olaraq bloklanması sistemi.
* **Fərdi Tarixçə:** Hər bir istifadəçinin köçürmə tarixçəsinə (Mədaxil/Məxaric) fərdi nəzarət imkanı.

### 📊 Müştəri Portalı (UX Focus)
* **Bank Kartı Simulyasiyası:** Hesabların vizual olaraq bank kartı formasında göstərilməsi.
* **Əməliyyat Tarixçəsi:** Son 10 əməliyyatın (Mədaxil - Yaşıl / Məxaric - Qırmızı) rəngli indikatorlarla izlənilməsi.
* **Blok Sistemi:** Bloklanmış istifadəçilərin sistemə girişinin avtomatik məhdudlaşdırılması.

## 🛠 Texnoloji Stack

- **Backend:** C# 15, ASP.NET Core MVC 10.0 (Latest Release)
- **Database:** SQLite (Relational DB) via Entity Framework Core
- **Arxitektura:** Service-Oriented Architecture (BankDataService)
- **Data Protection:** Session-based authentication & context-aware data seeding
- **UI:** Bootstrap 5.3, Bootstrap Icons, Custom CSS3 Animations

## 🧠 Texniki Detallar (Müəllim üçün qeyd)

1.  **EF Core Migration:** Layihədə `InitialCreate` miqrasiyası ilə bazanın SQL-ə keçidi təmin edilib.
2.  **Data Seeding:** Proqram ilk dəfə işə düşəndə bazanı avtomatik olaraq 1 Admin və 3 Test İstifadəçisi ilə doldurur.
3.  **InvariantCulture:** Bütün riyazi hesablamalar `CultureInfo.InvariantCulture` ilə aparılır ki, bu da onluq kəsrlərdə (nöqtə/vergül) səhvlərin qarşısını alır.
