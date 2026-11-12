# لعبة بطاقات الذاكرة (Memory Card Game)
**ASP.NET Core Web API + Entity Framework Core (Code-First) + Angular**

---

## 1. قاعدة البيانات والـ Models (EF Core Code-First)
* **الـ Models (`MemoryGameApi/Models`):**
  * `Player` (اللاعب)
  * `PlayerProfile` (الملف الشخصي وإحصائيات اللاعب)
  * `CardItem` (بطاقات اللعبة)
  * `GameSession` (جلسات اللعب وسجل النتائج)
  * `Achievement` (الأوسمة والإنجازات)
  * `PlayerAchievement` (جدول الربط - Linking Table)
* **العلاقات (Relationships):**
  * **One-to-One (1:1):** بين `Player` و `PlayerProfile`.
  * **One-to-Many (1:N):** بين `Player` و `GameSession`.
  * **Many-to-Many (N:M):** بين `Player` و `Achievement` عبر جدول الربط `PlayerAchievement`.
* **Data Annotations & Fluent API:** مطبقة داخل الـ Models وملف `Data/MemoryGameDbContext.cs`.

---

## 2. الـ Backend (ASP.NET Core Web API)
* **الـ Controllers (`MemoryGameApi/Controllers`):**
  * `CardsController` (`GET`, `POST`, `PUT`, `DELETE`)
  * `GameSessionsController` (`GET`, `POST`, `PUT`, `DELETE`)
  * `PlayersController` (`GET`, `POST`, `PUT`, `DELETE` + تسجيل الدخول وإنشاء الحساب)
* **Routing & Route Constraints:** مثل `[HttpGet("level/{level:int:range(1,2)}")]` و `[HttpGet("{id:int:min(1)}")]` و `[HttpGet("search/{keyword:alpha}")]`.
* **Data Binding:** استخدام `[FromBody]` و `[FromRoute]` و `[FromQuery]`.
* **Services & Interfaces (`MemoryGameApi/Services`):**
  * `ICardService` / `CardService`
  * `IGameSessionService` / `GameSessionService`
  * `IPlayerService` / `PlayerService`

---

## 3. الـ Frontend (Angular)
* **Interfaces (`src/app/models/game.interfaces.ts`):** `ICardItem`, `IBoardCard`, `IPlayer`, `IGameSession`, `IGameEngineService`, `IGameApiClient`.
* **Services (`src/app/services`):**
  * `GameApiService` (الاتصال بالـ API عبر `HttpClient`).
  * `MemoryGameService` (خلط البطاقات، التحقق من تطابق الأزواج، وحساب النقاط).
* **Data Binding:** Interpolation `{{ }}`، Property Binding `[ ]`، Event Binding `( )`، Two-Way Binding `[(ngModel)]`.
* **Pipes:** `TimeFormatPipe` (`timeFormat`) بالإضافة إلى `number` و `uppercase` و `date`.
* **Directives:** `*ngFor` و `*ngIf` و `[ngClass]`.

---

## طريقة التشغيل
1. **تشغيل الـ Backend:**
   ```powershell
   cd MemoryGameApi
   dotnet run
   ```
2. **تشغيل الـ Frontend:**
   ```powershell
   cd memory-game-client
   npm install
   npm start
   ```
