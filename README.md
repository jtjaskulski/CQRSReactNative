# CQRSReactNative
# Instrukcja: React + ASP.NET Core + Docker + CQRS + MediatR
## Projekt: SolutionOrdersReact (Struktura Server/Client)

---

## CZĘŚĆ 1: Utworzenie projektu SolutionOrdersReact

### 1.1. Tworzenie projektu w Visual Studio

1. Otwórz **Microsoft Visual Studio 2022**.
2. Wybierz **File → New → Project**.
3. W oknie wyszukiwania wpisz: **React**.
4. Wybierz szablon: **ASP.NET Core with React.js** lub **React and ASP.NET Core**.
5. Kliknij **Next**.
6. W polu **Project name** wpisz: `SolutionOrdersReact`.
7. Wybierz lokalizację projektu i kliknij **Next**.
8. W kolejnym oknie:
   - **Framework**: .NET 8.0
   - **Authentication type**: None
   - Zaznacz **Configure for HTTPS**
   - **NIE ZAZNACZAJ** "Enable Docker" (Docker skonfigurujemy ręcznie)
   - Kliknij **Create**.

### 1.2. Struktura projektu

Po utworzeniu projektu zobaczysz następującą strukturę w Solution Explorer:

```
Solution 'SolutionOrdersReact'
├── SolutionOrdersReact.Server (ASP.NET Core Web API)
│   ├── Controllers/
│   │   └── WeatherForecastController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── SolutionOrdersReact.Server.csproj
└── solutionordersreact.client (React App)
    ├── src/
    │   ├── App.jsx
    │   └── main.jsx
    ├── public/
    ├── package.json
    └── solutionordersreact.client.esproj
```

### 1.3. Weryfikacja działania projektu

1. Naciśnij **F5** lub kliknij **Start** aby uruchomić projekt.
2. Aplikacja powinna uruchomić się w przeglądarce.
3. Sprawdź czy działa przykładowa strona React i endpoint API.

**UWAGA:** Jeśli projekt nie uruchamia się poprawnie:
- Sprawdź czy Node.js jest zainstalowany: `node --version`
- W terminalu przejdź do folderu `solutionordersreact.client` i wykonaj: `npm install`

---

## CZĘŚĆ 2: Instalacja pakietów NuGet

### 2.1. Entity Framework Core

Kliknij prawym na projekt **SolutionOrdersReact.Server** → **Manage NuGet Packages**.

Lub użyj **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console):

**WAŻNE:** Ustaw **Default project** na `SolutionOrdersReact.Server`!

```powershell
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.Design
```

### 2.2. MediatR

```powershell
Install-Package MediatR
```

---

## CZĘŚĆ 3: Konfiguracja bazy danych TestReactDb

### 3.1. Utworzenie modeli encji

W projekcie **SolutionOrdersReact.Server** utwórz folder `Models` i dodaj następujące pliki:

**Models/UnitOfMeasurement.cs:**
```csharp
namespace SolutionOrdersReact.Server.Models
{
    public class UnitOfMeasurement
    {
        public int IdUnitOfMeasurement { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
```

**Models/Category.cs:**
```csharp
namespace SolutionOrdersReact.Server.Models
{
    public class Category
    {
        public int IdCategory { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
```

**Models/Client.cs:**
```csharp
namespace SolutionOrdersReact.Server.Models
{
    public class Client
    {
        public int IdClient { get; set; }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
```

**Models/Worker.cs:**
```csharp
namespace SolutionOrdersReact.Server.Models
{
    public class Worker
    {
        public int IdWorker { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public string Login { get; set; } = string.Empty;
        public string? Password { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
```

**Models/Item.cs:**
```csharp
namespace SolutionOrdersReact.Server.Models
{
    public class Item
    {
        public int IdItem { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int IdCategory { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string? FotoUrl { get; set; }
        public int? IdUnitOfMeasurement { get; set; }
        public string? Code { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual UnitOfMeasurement? UnitOfMeasurement { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
```

**Models/Order.cs:**
```csharp
namespace SolutionOrdersReact.Server.Models
{
    public class Order
    {
        public int IdOrder { get; set; }
        public DateTime? DataOrder { get; set; }
        public int? IdClient { get; set; }
        public int? IdWorker { get; set; }
        public string? Notes { get; set; }
        public DateTime? DeliveryDate { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual Worker? Worker { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
```

**Models/OrderItem.cs:**
```csharp
namespace SolutionOrdersReact.Server.Models
{
    public class OrderItem
    {
        public int IdOrderItem { get; set; }
        public int IdOrder { get; set; }
        public int IdItem { get; set; }
        public decimal? Quantity { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
}
```

### 3.2. Utworzenie DbContext

W projekcie **SolutionOrdersReact.Server** utwórz folder `Data` i plik `ApplicationDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UnitOfMeasurement
            modelBuilder.Entity<UnitOfMeasurement>(entity =>
            {
                entity.HasKey(e => e.IdUnitOfMeasurement);
                entity.Property(e => e.IsActive).IsRequired();
            });

            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory);
                entity.Property(e => e.IsActive).IsRequired();
            });

            // Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient);
                entity.Property(e => e.IsActive).IsRequired();
            });

            // Worker
            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(e => e.IdWorker);
                entity.Property(e => e.Login).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
            });

            // Item
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.IdItem);
                entity.Property(e => e.IdCategory).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Items)
                    .HasForeignKey(e => e.IdCategory)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UnitOfMeasurement)
                    .WithMany(u => u.Items)
                    .HasForeignKey(e => e.IdUnitOfMeasurement)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder);

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(e => e.IdClient)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Worker)
                    .WithMany(w => w.Orders)
                    .HasForeignKey(e => e.IdWorker)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.IdOrderItem);
                entity.Property(e => e.IdOrder).IsRequired();
                entity.Property(e => e.IdItem).IsRequired();
                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.IdOrder)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Item)
                    .WithMany(i => i.OrderItems)
                    .HasForeignKey(e => e.IdItem)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data - przykładowe dane
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // UnitOfMeasurement
            modelBuilder.Entity<UnitOfMeasurement>().HasData(
                new UnitOfMeasurement { IdUnitOfMeasurement = 1, Name = "szt", Description = "Sztuki", IsActive = true },
                new UnitOfMeasurement { IdUnitOfMeasurement = 2, Name = "kg", Description = "Kilogramy", IsActive = true },
                new UnitOfMeasurement { IdUnitOfMeasurement = 3, Name = "l", Description = "Litry", IsActive = true }
            );

            // Category
            modelBuilder.Entity<Category>().HasData(
                new Category { IdCategory = 1, Name = "Elektronika", Description = "Urządzenia elektroniczne", IsActive = true },
                new Category { IdCategory = 2, Name = "Żywność", Description = "Produkty spożywcze", IsActive = true }
            );

            // Client
            modelBuilder.Entity<Client>().HasData(
                new Client { IdClient = 1, Name = "Jan Kowalski", Adress = "ul. Główna 1, Warszawa", PhoneNumber = "500-100-200", IsActive = true },
                new Client { IdClient = 2, Name = "Anna Nowak", Adress = "ul. Kwiatowa 5, Kraków", PhoneNumber = "600-200-300", IsActive = true }
            );

            // Worker
            modelBuilder.Entity<Worker>().HasData(
                new Worker { IdWorker = 1, FirstName = "Piotr", LastName = "Kowalczyk", Login = "pkowalczyk", Password = "haslo123", IsActive = true },
                new Worker { IdWorker = 2, FirstName = "Maria", LastName = "Wiśniewska", Login = "mwisnieska", Password = "haslo456", IsActive = true }
            );

            // Item
            modelBuilder.Entity<Item>().HasData(
                new Item { IdItem = 1, Name = "Laptop Dell", Description = "Laptop Dell Inspiron 15", IdCategory = 1, Price = 3500, Quantity = 10, IdUnitOfMeasurement = 1, Code = "LAP001", IsActive = true },
                new Item { IdItem = 2, Name = "Monitor Samsung", Description = "Monitor 24 cale", IdCategory = 1, Price = 800, Quantity = 15, IdUnitOfMeasurement = 1, Code = "MON001", IsActive = true }
            );
        }
    }
}
```

### 3.3. Konfiguracja w appsettings.json

W projekcie **SolutionOrdersReact.Server** edytuj plik `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TestReactDb;User=sa;Password=YourStrong@Password123;TrustServerCertificate=True"
  }
}
```

### 3.4. Rejestracja w Program.cs

W projekcie **SolutionOrdersReact.Server** edytuj plik `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("https://localhost:5173") // Port domyślny Vite dla React
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Automatyczne zastosowanie migracji przy starcie
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Błąd podczas migracji bazy danych");
    }
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
```

### 3.5. Utworzenie migracji

W **Package Manager Console** upewnij się że **Default project** jest ustawiony na `SolutionOrdersReact.Server`, następnie wykonaj:

```powershell
Add-Migration InitialCreate
Update-Database
```

---

## CZĘŚĆ 4: Implementacja CQRS z MediatR

### 4.1. Struktura folderów

W projekcie **SolutionOrdersReact.Server** utwórz następującą strukturę:

```
SolutionOrdersReact.Server/
├── Features/
│   ├── Items/
│   │   ├── Commands/
│   │   │   ├── CreateItem/
│   │   │   └── DeleteItem/
│   │   └── Queries/
│   │       ├── GetAllItems/
│   │       └── GetItemById/
│   └── Orders/
│       ├── Commands/
│       │   └── CreateOrder/
│       └── Queries/
│           ├── GetAllOrders/
│           └── GetOrderById/
```

### 4.2. Przykład Query - GetAllItems

**Features/Items/Queries/GetAllItems/GetAllItemsQuery.cs:**

```csharp
using MediatR;

namespace SolutionOrdersReact.Server.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsQuery : IRequest<List<ItemDto>>
    {
    }

    public class ItemDto
    {
        public int IdItem { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string? UnitName { get; set; }
        public string? Code { get; set; }
        public bool IsActive { get; set; }
    }
}
```

**GetAllItemsHandler.cs:**

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Data;

namespace SolutionOrdersReact.Server.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsHandler : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllItemsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.UnitOfMeasurement)
                .Where(i => i.IsActive)
                .Select(i => new ItemDto
                {
                    IdItem = i.IdItem,
                    Name = i.Name,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    UnitName = i.UnitOfMeasurement != null ? i.UnitOfMeasurement.Name : null,
                    Code = i.Code,
                    IsActive = i.IsActive
                })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
```

### 4.3. Przykład Command - CreateItem

**Features/Items/Commands/CreateItem/CreateItemCommand.cs:**

```csharp
using MediatR;

namespace SolutionOrdersReact.Server.Features.Items.Commands.CreateItem
{
    public class CreateItemCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int IdCategory { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string? FotoUrl { get; set; }
        public int? IdUnitOfMeasurement { get; set; }
        public string? Code { get; set; }
    }
}
```

**CreateItemHandler.cs:**

```csharp
using MediatR;
using SolutionOrdersReact.Server.Data;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Features.Items.Commands.CreateItem
{
    public class CreateItemHandler : IRequestHandler<CreateItemCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateItemHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                IdCategory = request.IdCategory,
                Price = request.Price,
                Quantity = request.Quantity,
                FotoUrl = request.FotoUrl,
                IdUnitOfMeasurement = request.IdUnitOfMeasurement,
                Code = request.Code,
                IsActive = true
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync(cancellationToken);

            return item.IdItem;
        }
    }
}
```

### 4.4. Controller dla Items

**Controllers/ItemsController.cs:**

```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionOrdersReact.Server.Features.Items.Commands.CreateItem;
using SolutionOrdersReact.Server.Features.Items.Queries.GetAllItems;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/items
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllItemsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST: api/items
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
        {
            var itemId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = itemId }, itemId);
        }
    }
}
```

### 4.5. Przykład Query - GetAllOrders

**Features/Orders/Queries/GetAllOrders/GetAllOrdersQuery.cs:**

```csharp
using MediatR;

namespace SolutionOrdersReact.Server.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
    }

    public class OrderDto
    {
        public int IdOrder { get; set; }
        public DateTime? DataOrder { get; set; }
        public string? ClientName { get; set; }
        public string? WorkerName { get; set; }
        public string? Notes { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int IdOrderItem { get; set; }
        public string? ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
```

**GetAllOrdersHandler.cs:**

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Data;

namespace SolutionOrdersReact.Server.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllOrdersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Worker)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .Select(o => new OrderDto
                {
                    IdOrder = o.IdOrder,
                    DataOrder = o.DataOrder,
                    ClientName = o.Client != null ? o.Client.Name : null,
                    WorkerName = o.Worker != null ? $"{o.Worker.FirstName} {o.Worker.LastName}" : null,
                    Notes = o.Notes,
                    DeliveryDate = o.DeliveryDate,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        IdOrderItem = oi.IdOrderItem,
                        ItemName = oi.Item.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Item.Price
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return orders;
        }
    }
}
```

### 4.6. Controller dla Orders

**Controllers/OrdersController.cs:**

```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionOrdersReact.Server.Features.Orders.Queries.GetAllOrders;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllOrdersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
```

---

## CZĘŚĆ 5: Docker - Baza danych

### 5.1. Utworzenie pliku docker-compose-db.yml

W głównym katalogu rozwiązania (obok pliku `.sln`) utwórz plik `docker-compose-db.yml`:

```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: testReactDb-sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password123
      - MSSQL_PID=Developer
    ports:
      - "1433:1433"
    volumes:
      - testReactDb-data:/var/opt/mssql
    networks:
      - dev-network

networks:
  dev-network:
    driver: bridge

volumes:
  testReactDb-data:
```

### 5.2. Uruchomienie bazy danych

W terminalu (w katalogu głównym rozwiązania) wykonaj:

```bash
docker-compose -f docker-compose-db.yml up -d
```

### 5.3. Weryfikacja

```bash
docker ps
```

Powinieneś zobaczyć:
```
CONTAINER ID   IMAGE                                        PORTS                    NAMES
abc123def456   mcr.microsoft.com/mssql/server:2022-latest   0.0.0.0:1433->1433/tcp   testReactDb-sqlserver
```

### 5.4. Zatrzymanie bazy danych

```bash
docker-compose -f docker-compose-db.yml down
```

---

## CZĘŚĆ 6: Docker - Cała aplikacja (opcjonalnie)

### 6.1. Dockerfile

W głównym katalogu rozwiązania (obok `.sln`) utwórz plik `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Instalacja Node.js dla projektu React
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash -
RUN apt-get install -y nodejs

# Kopiowanie plików projektu Server
COPY ["SolutionOrdersReact.Server/SolutionOrdersReact.Server.csproj", "SolutionOrdersReact.Server/"]
# Kopiowanie plików projektu Client (esproj)
COPY ["solutionordersreact.client/solutionordersreact.client.esproj", "solutionordersreact.client/"]
COPY ["solutionordersreact.client/package.json", "solutionordersreact.client/"]
COPY ["solutionordersreact.client/package-lock.json", "solutionordersreact.client/"]

# Restore projektu Server
RUN dotnet restore "SolutionOrdersReact.Server/SolutionOrdersReact.Server.csproj"

# Kopiowanie całego kodu
COPY . .

# Instalacja zależności npm dla frontendu
WORKDIR "/src/solutionordersreact.client"
RUN npm install

# Build projektu Server
WORKDIR "/src/SolutionOrdersReact.Server"
RUN dotnet build "SolutionOrdersReact.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/SolutionOrdersReact.Server"
RUN dotnet publish "SolutionOrdersReact.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolutionOrdersReact.Server.dll"]
```

### 6.2. docker-compose.yml

W głównym katalogu rozwiązania utwórz plik `docker-compose.yml`:

```yaml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: orders-react-api
    ports:
      - "5000:8080"
      - "5001:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:8080;https://+:8081
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=TestReactDb;User=sa;Password=YourStrong@Password123;TrustServerCertificate=True
    depends_on:
      - sqlserver
    networks:
      - orders-network

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: orders-sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password123
      - MSSQL_PID=Developer
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql
    networks:
      - orders-network

networks:
  orders-network:
    driver: bridge

volumes:
  sqlserver-data:
```

### 6.3. .dockerignore

W głównym katalogu rozwiązania utwórz plik `.dockerignore`:

```
**/bin
**/obj
**/node_modules
**/.vs
**/.vscode
**/TestResults
**/*.user
**/*.suo
**/logs
**/npm-debug.log
**/yarn-error.log
**/build
**/dist
**/.DS_Store
**/Thumbs.db
**/Dockerfile
**/docker-compose*.yml
**/.dockerignore
```

### 6.4. Uruchomienie całej aplikacji w Docker

```bash
docker-compose up --build
```

Aplikacja będzie dostępna pod: `http://localhost:5000`

---

## CZĘŚĆ 7: Testowanie aplikacji

### 7.1. Uruchomienie lokalne (POLECANE dla developmentu)

**Krok 1: Uruchom bazę danych**
```bash
docker-compose -f docker-compose-db.yml up -d
```

**Krok 2: Uruchom aplikację w Visual Studio**
- Naciśnij `F5` lub kliknij **Start**
- Aplikacja uruchomi się (backend + frontend)

**Krok 3: Testowanie API**

Otwórz przeglądarkę:
- Swagger UI: `https://localhost:7xxx/swagger` (sprawdź port w Visual Studio)
- API Items: `https://localhost:7xxx/api/items`
- API Orders: `https://localhost:7xxx/api/orders`

### 7.2. Testowanie z Postman

**GET - Pobierz wszystkie produkty:**
```
GET https://localhost:7xxx/api/items
```

**POST - Dodaj nowy produkt:**
```
POST https://localhost:7xxx/api/items
Content-Type: application/json

{
  "name": "Mysz Logitech",
  "description": "Mysz bezprzewodowa",
  "idCategory": 1,
  "price": 99.99,
  "quantity": 50,
  "idUnitOfMeasurement": 1,
  "code": "MYS001"
}
```

### 7.3. Zatrzymanie bazy danych

Po zakończeniu pracy:

```bash
docker-compose -f docker-compose-db.yml down
```

---

## CZĘŚĆ 8: Teoria - CQRS, MediatR, Docker

### 8.1. CQRS (Command Query Responsibility Segregation)

**Definicja:**
CQRS to wzorzec architektoniczny, który rozdziela operacje odczytu (Queries) od operacji zapisu (Commands).

**Główne założenia:**
- **Commands** - operacje zmieniające stan systemu (CREATE, UPDATE, DELETE)
- **Queries** - operacje tylko do odczytu (SELECT)
- Każda operacja ma dedykowany handler
- Brak bezpośrednich zależności między warstwami

**Zalety:**
- Lepsze skalowanie (odczyt i zapis można skalować osobno)
- Łatwiejsze testowanie (każdy handler testujemy osobno)
- Wyraźny podział odpowiedzialności
- Możliwość optymalizacji zapytań osobno dla odczytu i zapisu
- Łatwiejsze utrzymanie kodu

**Przykład przepływu:**
```
1. Controller otrzymuje request
2. Tworzy Command/Query
3. Wysyła do MediatR (Send)
4. MediatR znajduje odpowiedni Handler
5. Handler wykonuje logikę (dostęp do bazy)
6. Zwraca wynik do Controllera
```

### 8.2. MediatR

**Definicja:**
MediatR to biblioteka implementująca wzorzec Mediatora w .NET, która ułatwia wdrożenie CQRS.

**Jak działa:**
1. Definiujemy Request (Command lub Query) implementujący `IRequest<TResponse>`
2. Tworzymy Handler implementujący `IRequestHandler<TRequest, TResponse>`
3. W kontrolerze używamy `IMediator.Send(request)`
4. MediatR automatycznie znajduje odpowiedni handler i wykonuje operację

**Zalety:**
- Eliminuje bezpośrednie zależności między kontrolerami a logiką biznesową
- Łatwe dodawanie Behaviors (logowanie, walidacja, cache)
- Przejrzysty kod w kontrolerach
- Łatwe testowanie (mockujemy IMediator)

### 8.3. Docker

**Docker:**
- Platforma do konteneryzacji aplikacji
- Kontener = lekka "wirtualna maszyna" z aplikacją i jej zależnościami
- Gwarantuje, że aplikacja będzie działać tak samo na każdym środowisku

**docker-compose:**
- Narzędzie do zarządzania wieloma kontenerami
- Definiujemy wszystkie serwisy w jednym pliku YAML
- Możemy uruchomić cały stack (API + baza) jedną komendą

**Zalety:**
- Spójne środowisko na dev, test i produkcji
- Łatwe udostępnianie projektu innym deweloperom
- Izolacja aplikacji i bazy danych
- Łatwe skalowanie

---

## CZĘŚĆ 9: Przydatne komendy

### 9.1. Docker

```bash
# Uruchomienie bazy danych
docker-compose -f docker-compose-db.yml up -d

# Zatrzymanie bazy danych
docker-compose -f docker-compose-db.yml down

# Zatrzymanie i usunięcie danych
docker-compose -f docker-compose-db.yml down -v

# Sprawdzenie działających kontenerów
docker ps

# Logi kontenera
docker logs testReactDb-sqlserver

# Uruchomienie całej aplikacji
docker-compose up --build

# Zatrzymanie całej aplikacji
docker-compose down
```

### 9.2. Entity Framework

```bash
# Dodanie nowej migracji
Add-Migration NazwaMigracji

# Zastosowanie migracji
Update-Database

# Cofnięcie ostatniej migracji
Update-Database -Migration NazwaPoprzednejMigracji

# Usunięcie ostatniej migracji (jeśli nie została zastosowana)
Remove-Migration
```

### 9.3. .NET CLI

```bash
# Uruchomienie projektu
dotnet run --project SolutionOrdersReact.Server

# Build projektu
dotnet build

# Restore pakietów
dotnet restore
```

---

## PODSUMOWANIE

W tym materiale nauczyłeś się:

✅ Utworzyć projekt React + ASP.NET Core (struktura Server/Client)  
✅ Skonfigurować bazę danych TestReactDb z pełną strukturą relacyjną  
✅ Podłączyć Entity Framework Core do SQL Server  
✅ Zaimplementować wzorzec CQRS z MediatR  
✅ Utworzyć API z podziałem na Commands i Queries  
✅ Uruchomić bazę danych w Docker  
✅ Skonfigurować całą aplikację do uruchomienia w Docker  
✅ Testować API przez Swagger i Postman  

### Zalecany workflow dla studentów:

**Development (codzienne):**
```bash
# 1. Uruchom bazę danych
docker-compose -f docker-compose-db.yml up -d

# 2. Uruchom aplikację w Visual Studio (F5)

# 3. Testuj API i rozwijaj aplikację

# 4. Zatrzymaj bazę po zakończeniu
docker-compose -f docker-compose-db.yml down
```

**Testing/Production:**
```bash
# Uruchom wszystko w Docker
docker-compose up --build
```

### Kolejne kroki:

1. Dodać pozostałe operacje CRUD (Update, Delete) dla wszystkich encji
2. Zaimplementować walidację (FluentValidation)
3. Dodać obsługę błędów i wyjątków
4. Rozbudować komponenty React do wyświetlania i zarządzania danymi
5. Dodać autoryzację i uwierzytelnianie (JWT)
6. Zaimplementować paginację dla list
7. Dodać filtry i wyszukiwanie

---

**Powodzenia w nauce!** 🚀

Rozdział projektu SolutionOrdersReact – React Native Mobile (pnpm, CLI, native API)
SolutionOrdersMobile
Mobilny klient (React Native CLI, TypeScript, pnpm) do rozwiązania SolutionOrdersReact (.NET API)

1. Inicjalizacja projektu
bash
# [1] Wejście do katalogu repo
cd CQRSReactNativetest/SolutionOrdersReact

# [2] Nowy projekt:
npx @react-native-community/cli init SolutionOrdersMobile

# [3] Przejdź do katalogu i zainstaluj zależności przez pnpm
cd SolutionOrdersMobile
pnpm install

# [4] (Monorepo, opcjonalnie) Zmień .npmrc
echo "node-linker=hoisted" > .npmrc

# [5] Dodaj SolutionOrdersMobile do pnpm-workspace.yaml w głównym repo
# packages:
#   - 'SolutionOrdersMobile'
#   - 'other-projects...'
2. Uruchamianie projektu na emulatorze (Android/iOS)
Emulator ANDROID (REKOMENDOWANY dla Windows/Linux/Mac)
Krok 1:

Zainstaluj najnowszy Android Studio. Otwórz “Device Manager” → ustaw typ Pixel 7 / Pixel 6 z systemem Android 13+.

Krok 2:

Otwórz emulator (“play”), nie zamykaj programu Android Studio.

Krok 3:

W konsoli uruchom:

bash
pnpm react-native run-android
Pierwszy build może potrwać do kilku minut!

Jeśli wszystko się powiodło, a aplikacja nie pojawia się automatycznie – znajdź “SolutionOrdersMobile” na liście aplikacji w urządzeniu lub wykonaj adb reverse tcp:5000 tcp:5000 aby API .NET backend działało z mobilki.

Emulator iOS (Tylko Mac):
Krok 1:

Xcode → “Devices and Simulators” → dodaj iPhone 15 (lub nowszy).

Krok 2:
bash pnpm react-native run-ios

Domyślnie uruchomi się symulator iPhone oraz otworzy aplikację.

Najczęstsze problemy:
Jeśli Port 8081 error/Metro: pnpm start --reset-cache

Jeżeli API nie odpowiada: sprawdź, czy backend .NET działa na tym samym porcie i spróbuj IP 10.0.2.2:PORT na Androidzie.

3. Struktura projektu
text
SolutionOrdersMobile/
├── android/
├── ios/
├── app.json
├── src/
│   ├── api/
│   │   └── ordersApi.ts
│   ├── components/
│   │   └── ItemCard.tsx
│   ├── context/
│   │   └── ItemsContext.tsx
│   ├── screens/
│   │   ├── ListScreen.tsx
│   │   ├── DetailsScreen.tsx
│   │   └── PermissionsExample.tsx
│   └── App.tsx
├── package.json
└── .npmrc
4. Integracja z API .NET
src/api/ordersApi.ts:

ts
import { Platform } from 'react-native';
const host = Platform.OS === 'android' ? '10.0.2.2' : 'localhost';
const BASE_URL = `http://${host}:5000/api`;

export async function fetchItems() {
  const res = await fetch(`${BASE_URL}/items`);
  return res.json();
}

export async function addItem(item, token) {
  return fetch(`${BASE_URL}/items`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(item)
  }).then(r => r.json());
}
5. Przykład komponentów i zaawansowany state
src/components/ItemCard.tsx:

tsx
import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

export const ItemCard = ({ name, price, categoryName }) => (
  <View style={styles.wrap}>
    <Text style={styles.name}>{name}</Text>
    <Text style={styles.price}>{price} zł</Text>
    <Text>{categoryName}</Text>
  </View>
);
const styles = StyleSheet.create({
  wrap: { borderWidth: 1, margin: 8, padding: 16, borderRadius: 8 },
  name: { fontWeight: 'bold', fontSize: 16 },
  price: { color: 'green' }
});
Context API — nowoczesny state globalny
src/context/ItemsContext.tsx:

tsx
import React, { createContext, useContext, useState } from 'react';

type ItemsContextProps = {
  items: any[];
  setItems: React.Dispatch<React.SetStateAction<any[]>>;
};
export const ItemsContext = createContext<ItemsContextProps>({ items: [], setItems: () => {} });

export const ItemsProvider: React.FC = ({ children }) => {
  const [items, setItems] = useState<any[]>([]);
  return (
    <ItemsContext.Provider value={{ items, setItems }}>
      {children}
    </ItemsContext.Provider>
  );
};
export const useItems = () => useContext(ItemsContext);
Użycie Contextu w komponencie:

tsx
import { useItems } from '../context/ItemsContext';
const { items, setItems } = useItems();
6. Lista produktów z API
src/screens/ListScreen.tsx:

tsx
import React, { useEffect } from 'react';
import { View, FlatList, ActivityIndicator } from 'react-native';
import { fetchItems } from '../api/ordersApi';
import { ItemCard } from '../components/ItemCard';
import { useItems } from '../context/ItemsContext';

export default function ListScreen() {
  const { items, setItems } = useItems();
  const [loading, setLoading] = React.useState(true);
  useEffect(() => {
    fetchItems().then(setItems).finally(() => setLoading(false));
  }, []);
  if (loading) return <ActivityIndicator />;
  return (
    <FlatList
      data={items}
      renderItem={({ item }) => <ItemCard {...item} />}
      keyExtractor={item => item.idItem?.toString() ?? Math.random().toString()}
    />
  );
}
7. Nawigacja
Instalacja:

bash
pnpm add @react-navigation/native @react-navigation/native-stack
pnpm add react-native-screens react-native-safe-area-context
src/App.tsx:

tsx
import * as React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import ListScreen from './screens/ListScreen';
import DetailsScreen from './screens/DetailsScreen';
import { ItemsProvider } from './context/ItemsContext';

const Stack = createNativeStackNavigator();

export default function App() {
  return (
    <ItemsProvider>
      <NavigationContainer>
        <Stack.Navigator>
          <Stack.Screen name="Lista" component={ListScreen} />
          <Stack.Screen name="Szczegóły" component={DetailsScreen} />
        </Stack.Navigator>
      </NavigationContainer>
    </ItemsProvider>
  );
}
8. Permissions (Przykład)
src/screens/PermissionsExample.tsx:

tsx
import React from 'react';
import { Button, PermissionsAndroid } from 'react-native';

export default function PermissionsExample() {
  const askCameraPermission = async () => {
    const granted = await PermissionsAndroid.request(
      PermissionsAndroid.PERMISSIONS.CAMERA
    );
    alert(granted === PermissionsAndroid.RESULTS.GRANTED ? "Masz dostęp" : "Brak uprawnień");
  };
  return <Button title="Poproś o kamerę" onPress={askCameraPermission} />;
}
9. Troubleshooting i najczęstsze błędy
Nie podawaj --template przy CLI ≥0.71+ — TypeScript jest domyślny!

Nazwa projektu tylko alfanumeryczna, bez kropek, myślników, spacji

Na Androidzie do komunikacji z lokalnym API .NET używaj 10.0.2.2 zamiast localhost

Jeśli porty nie działają – sprawdź firewall/emulator

pnpm, CLI i workspace zawsze odpalaj z katalogu projektu mobile
10. Autoryzacja JWT oraz obsługa relacji 1:N (Order + OrderItems) w React Native
10.1. Uzyskiwanie i przechowywanie JWT (logowanie użytkownika)
Rejestracja/logowanie – front React Native (przykład)
tsx
const API_URL = `http://${host}:5000/api`;

export async function loginUser(email: string, password: string) {
  const resp = await fetch(`${API_URL}/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  if(!resp.ok) throw new Error('Invalid credentials');
  const data = await resp.json();
  // { token: "JWT..." }
  return data.token;
}
Zapis tokena np. w state lub secure storage (AsyncStorage):

tsx
import AsyncStorage from '@react-native-async-storage/async-storage';
AsyncStorage.setItem('jwt', token);
// użycie: const token = await AsyncStorage.getItem('jwt');
10.2. Pobieranie zamówień użytkownika z relacją Order → OrderItem (One-To-Many)
Endpoint backend (.NET CQRS)
C# przykładowy handler:

csharp
public class GetUserOrdersQuery : IRequest<List<OrderDto>> 
{
    public string UserId { get; set; }
}
public class OrderDto
{
    public int IdOrder { get; set; }
    public DateTime? DataOrder { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
public class OrderItemDto
{
    public int IdOrderItem { get; set; }
    public string? ItemName { get; set; }
    public decimal? Quantity { get; set; }
}

// Handler
public class GetUserOrdersHandler : IRequestHandler<GetUserOrdersQuery, List<OrderDto>>
{
    private readonly ApplicationDbContext _ctx;
    public GetUserOrdersHandler(ApplicationDbContext ctx) => _ctx = ctx;
    public async Task<List<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken ct)
    {
        return await _ctx.Orders
            .Where(x => x.UserId == request.UserId)
            .Include(x => x.OrderItems)
                .ThenInclude(oi => oi.Item)
            .Select(o => new OrderDto
            {
                IdOrder = o.IdOrder,
                DataOrder = o.DataOrder,
                Items = o.OrderItems.Select(oi => new OrderItemDto {
                    IdOrderItem = oi.IdOrderItem,
                    ItemName = oi.Item.Name,
                    Quantity = oi.Quantity
                }).ToList()
            })
            .ToListAsync(ct);
    }
}
Kontroler API (.NET)
csharp
[Authorize]
[HttpGet("api/orders/my")]
public async Task<IActionResult> GetMyOrders()
{
    var userId = User.FindFirst("sub")?.Value;
    var query = new GetUserOrdersQuery { UserId = userId };
    var result = await _mediator.Send(query);
    return Ok(result);
}
10.3. Pobieranie relacji po stronie mobilnej React Native (fetch z tokenem JWT)
src/api/ordersApi.ts:

tsx
import AsyncStorage from '@react-native-async-storage/async-storage';

export async function fetchUserOrders() {
  const token = await AsyncStorage.getItem('jwt');
  const resp = await fetch(`${BASE_URL}/orders/my`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  if(!resp.ok) throw new Error(await resp.text());
  return resp.json();
}
src/screens/OrdersWithItemsScreen.tsx:

tsx
import React, { useEffect, useState } from 'react';
import { View, Text, FlatList } from 'react-native';
import { fetchUserOrders } from '../api/ordersApi';

export default function OrdersWithItemsScreen() {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState('');
  useEffect(() => {
    fetchUserOrders()
      .then(setOrders)
      .catch(e => setError(e.message));
  }, []);
  if (error) return <Text>Błąd: {error}</Text>;
  if (orders.length === 0) return <Text>Brak zamówień</Text>;
  return (
    <FlatList
      data={orders}
      keyExtractor={order => order.idOrder.toString()}
      renderItem={({ item: order }) => (
        <View style={{ margin: 8, borderWidth: 1, borderRadius: 8, padding: 8 }}>
          <Text>Data: {order.dataOrder}</Text>
          <Text>Pozycje:</Text>
          {order.items.map(oi => (
            <Text key={oi.idOrderItem}>• {oi.itemName} x {oi.quantity}</Text>
          ))}
        </View>
      )}
    />
  );
}
10.4. Bonus: wysyłanie zamówienia (POST z listą produktów)
src/api/ordersApi.ts

tsx
export async function placeOrder(items, token) {
  return fetch(`${BASE_URL}/orders`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({
      // np. tablica produktów:
      items: items.map(i => ({
        idItem: i.idItem,
        quantity: i.qty
      }))
    })
  }).then(r => r.json());
}
10.5. Podsumowanie: Key takeaways
JWT obsługiwany przez Authorization header w każdym fetch do API (AsyncStorage/Context do przechowywania).

Relacje 1:N (Order z OrderItem) pobierane przez CQRS i mapowane do typów DTO.

Komponenty React Native pokazują całą kolekcję (zamówienia i ich elementy) z API.

Kod gotowy do rozbudowy o modyfikację zamówień, rejestrację, obsługę wylogowania, dodatkowe filtry i obsługę błędów.

11. Najczęstsze problemy przy stawianiu React Native + rozwiązania
Problem 1: Brak adb/nie wykrywa emulatora
Objaw:
"adb" is not recognized as a command

Rozwiązanie:
Dodaj do systemowego PATH ścieżkę do platform-tools (np.
C:\Users\TwojUser\AppData\Local\Android\Sdk\platform-tools)

Problem 2: Brak ustawionego JAVA_HOME
Objaw:
ERROR: JAVA_HOME is not set and no 'java' command could be found

Rozwiązanie:
Ustaw zmienną środowiskową JAVA_HOME na katalog instalacji JDK 11/17 i dodaj do PATH:
%JAVA_HOME%\bin

Problem 3: Brak/emulator nie uruchamia się, błąd No emulators found
Objaw:
No emulators found as output of emulator -list-avds

Rozwiązanie:
Utwórz nowy emulator w Android Studio Device Manager i uruchom ręcznie przed buildem.

Problem 4: Błąd SDK location not found
Objaw:
Define a valid SDK location with ANDROID_HOME or local.properties

Rozwiązanie:
Dodaj plik android/local.properties o treści:
sdk.dir=C:/Users/TwojUser/AppData/Local/Android/Sdk
(lub ustaw ANDROID_HOME w systemie)

Problem 5: Build/Metro, błąd No apps connected
Objaw:
No apps connected. Sending reload to all React Native apps failed

Rozwiązanie:
Uruchom aplikację na emulatorze:
pnpm react-native run-android
(Metro samo wykryje aplikację, gdy emulator wystartuje apkę.)

Problem 6: Zbyt długa ścieżka/cmake/ninja na Windows
Objaw:
Filename longer than 260 characters

Rozwiązanie:
Przenieś projekt do katalogu o bardzo krótkiej ścieżce (np. C:\Projekty\Nazwaprojektu), ew. włącz obsługę długich ścieżek w rejestrze Windows.

Problem 7: Brak folderu @react-native/gradle-plugin w node_modules
Objaw:
Included build ...gradle-plugin does not exist.

Rozwiązanie:
Najlepiej przejdź na npm install (zamiast pnpm) – pnpm bywa niekompatybilny z layoutem node_modules React Native CLI. Po npm/yarn problem znika.

Problem 8: Błędy wersji paczek, duża liczba ostrzeżeń, deprecated
Objaw:
Duże ilości warningów .deprecated w npm/pnpm install

Rozwiązanie:
Sprawdź, czy wersje paczek nie są hardkodowane w package.json, zaktualizuj (o ile to nie psuje builda), ignoruj warningi dot. ESLint/rimraf/inflight – problem nie wpływa na uruchamianie RN.

Problem 9: Komunikaty Metro WARN the transform cache was reset
Rozwiązanie:
To nie błąd. Jeśli build się powiesi:
pnpm start --reset-cache lub npx react-native start --reset-cache

Problem 10: Błąd połączenia z backendem .NET (API)
Objaw:
Network error, brak połączenia z API z aplikacji mobilnej.

Rozwiązanie:
Na emulatorze Android używaj IP 10.0.2.2 zamiast localhost. Upewnij się, że backend .NET uruchomiony jest na tym samym porcie, co adres API w kodzie.

Problem 11: Różnica npm/yarn/pnpm – kiedy zmieniać?
Rozwiązanie:
Do buildów na Windows z React Native CLI i Android zawsze polecany jest npm lub yarn. pnpm używaj tylko przy stabilnych monorepo lub na Mac/Linux.

Podsumowanie:
Większość tych problemów to konfiguracja środowiska Windows/Java/Android SDK, layout node_modules i ograniczenie długich ścieżek. Wystarczy pilnować krótkiej ścieżki do projektu, właściwego menedżera pakietów oraz logicznie sprawdzać logi błędów — a React Native CLI działa stabilnie!
