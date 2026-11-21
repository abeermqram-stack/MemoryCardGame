# لعبة بطاقات الذاكرة (Memory Card Game)
**ASP.NET Core Web API (.NET 8) + Entity Framework Core (Code-First) + Angular (SPA)**

## أعضاء الفريق (Team Members)
1. عبير محمد مقرم
2. آسيا سعيد بن عبيد
3. سارة عوض بن جوبح
4. فاطمة عصام باطويل
5. حنين عبدالعزيز باوزير
6. فاطمة بازياد

---

## الروابط المباشرة للمشروع (Live Links)
* **موقع Angular — صفحة اللعب الرئيسية:** https://abeermqram-stack.github.io/MemoryCardGame/game
* **صفحة اللاعبة (تسجيل الدخول / حساب جديد / الملف الشخصي / الأوسمة / جدول التقدم):** https://abeermqram-stack.github.io/MemoryCardGame/player
* **لوحة المشرف (Admin — إدارة البطاقات والنتائج):** https://abeermqram-stack.github.io/MemoryCardGame/admin
* **صفحة السواقر (Swagger API):** https://abeermqram-stack.github.io/MemoryCardGame/swagger/

### روابط الكود على GitHub
* **المستودع الكامل:** https://github.com/abeermqram-stack/MemoryCardGame
* **الـ Backend (ASP.NET Core Web API + EF Core):** https://github.com/abeermqram-stack/MemoryCardGame/tree/main/MemoryGameApi
* **الـ Frontend (Angular):** https://github.com/abeermqram-stack/MemoryCardGame/tree/main/memory-game-client

### بيانات تسجيل الدخول للتجربة
* **المشرف (Admin):** `abeer@gmail.com` — كلمة المرور: `1234`
* **حسابات اللاعبات (كلمة المرور للجميع: `123456`):**
  * `asia@memorygame.sa`
  * `fatima@memorygame.sa`
  * `haneen@memorygame.sa`
  * `sara@memorygame.sa`
  * `fatimab@memorygame.sa`

---

## 1. قاعدة البيانات والـ Models (EF Core Code-First)
* **الـ Models (`MemoryGameApi/Models`):**
  * `Player` (اللاعب)
  * `PlayerProfile` (الملف الشخصي وإحصائيات اللاعب)
  * `CardItem` (بطاقات اللعبة)
  * `GameSession` (جلسات اللعب وسجل النتائج)
  * `Achievement` (الأوسمة والإنجازات)
  * `PlayerAchievement` (جدول الربط - Linking Table)
  * `UpdateCardTitleDto` (DTO للتحديث المخصص)
* **العلاقات (Relationships):**
  * **One-to-One (1:1):** بين `Player` و `PlayerProfile`.
  * **One-to-Many (1:N):** بين `Player` و `GameSession`.
  * **Many-to-Many (N:M):** بين `Player` و `Achievement` عبر جدول الربط `PlayerAchievement` بمفتاح أساسي مركب (`PlayerId`, `AchievementId`).
* **Data Annotations & Fluent API:** مطبقة داخل جميع الـ Models وملف `Data/MemoryGameDbContext.cs` مع ملفات `Migrations/`.

---

## 2. الـ Backend (ASP.NET Core Web API)
* **الـ Controllers (`MemoryGameApi/Controllers`):**
  * `CardsController` (`GET`, `GET level/{level}`, `GET search`, `GET {id}`, `POST`, `PUT {id}`, `PUT {id}/update-title`, `DELETE {id}`)
  * `GameSessionsController` (`GET`, `GET paged`, `GET {id}`, `POST`, `PUT {id}`, `DELETE {id}`)
  * `PlayersController` (`GET` مع كامل العلاقات 1:1 و 1:N و N:M، `GET {id}`, `GET login`, `GET search/{keyword}`, `POST`, `PUT {id}`, `DELETE {id}`)
* **Routing & Route Constraints:** `[Route("api/[controller]")]` مع قيود مثل `{level:int:range(1,2)}` و `{id:int:min(1)}` و `search/{keyword:alpha}`.
* **Data Binding:** استخدام `[FromBody]` و `[FromRoute]` و `[FromQuery]`.
* **Services & Interfaces (`MemoryGameApi/Services`):**
  * `ICardService` / `CardService`
  * `IGameSessionService` / `GameSessionService`
  * `IPlayerService` / `PlayerService`

---

## 3. الـ Frontend (Angular SPA)
* **Components & Routing (`src/app/app.routes.ts` & `src/app/components`):**
  * `AppComponent` (`<router-outlet>`, `[routerLink]`, `routerLinkActive`)
  * `GameComponent` (`/game` — لوحة اللعب والمستويات والعدادات)
  * `PlayerComponent` (`/player` — تسجيل الدخول/حساب جديد، الملف الشخصي 1:1، الأوسمة N:M، جدول التقدم 1:N)
  * `AdminComponent` (`/admin` — إضافة/تعديل/حذف/بحث البطاقات وحذف الجلسات)
* **Interfaces (`src/app/models/game.interfaces.ts`):**
  * بنية البيانات: `ICardItem`, `IBoardCard`, `IPlayer`, `IPlayerProfile`, `IAchievement`, `IPlayerAchievement`, `IGameSession`.
  * تواقيع الدوال (Function Signatures): `IGameEngineService`, `IGameApiClient`, `IAuthStateService`.
* **Services (`src/app/services`):**
  * `GameApiService` (الاتصال بالـ API عبر `HttpClient` و `HttpParams`)
  * `MemoryGameService` (خلط البطاقات، التحقق من تطابق الأزواج، وحساب النقاط)
  * `AuthStateService` (إدارة حالة تسجيل دخول اللاعبة والمشرف عبر الصفحات)
* **Data Binding (الأنواع الأربعة):** Interpolation `{{ }}`، Property Binding (`[disabled]`, `[style.border-color]`, `[title]`)، Event Binding (`(click)`, `(ngSubmit)`)، Two-Way Binding (`[(ngModel)]`).
* **Pipes:** Custom Pipe (`timeFormat`) بالإضافة إلى `number` و `uppercase` و `date`.
* **Directives:** `*ngFor` و `*ngIf` و `[ngClass]`.

---

## طريقة التشغيل
1. **تشغيل الـ Backend (Swagger متاح على `http://localhost:5150/swagger`):**
   ```powershell
   cd MemoryGameApi
   dotnet run
   ```
2. **تشغيل الـ Frontend (على `http://localhost:4200`):**
   ```powershell
   cd memory-game-client
   npm install
   npm start
   ```
