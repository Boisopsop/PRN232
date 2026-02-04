# FU News Management System - Complete Project Plan

## 📋 Project Overview

**Project Name**: FU News Management System  
**Technology Stack**: ASP.NET Core 8 Web API, Entity Framework Core, MS SQL Server  
**Architecture**: 3-Layer Architecture with Repository & Singleton Patterns  
**Authentication**: JWT (JSON Web Token)  
**API Standard**: RESTful API with OData Support

---

## 🏗️ Solution Structure

### Backend Solution: `NguyenTrongThu_SE1856_A01_BE.sln`

```
NguyenTrongThu_SE1856_A01_BE/
├── PRN232.FUNewsManagement.API/          # API Layer (Presentation)
├── PRN232.FUNewsManagement.Services/     # Service Layer (Business Logic)
├── PRN232.FUNewsManagement.Repo/         # Repository Layer (Data Access)
└── PRN232.FUNewsManagement.Models/       # Shared Models
```

---

## 📦 Project 1: PRN232.FUNewsManagement.Models

### Purpose
Chứa tất cả các model types được sử dụng xuyên suốt các layer

### Folder Structure
```
PRN232.FUNewsManagement.Models/
├── Entities/              # Entity Models (DB mapping)
│   ├── SystemAccount.cs
│   ├── Category.cs
│   ├── NewsArticle.cs
│   ├── Tag.cs
│   └── NewsTag.cs
├── Business/              # Business Models (Service layer)
│   ├── SystemAccountBM.cs
│   ├── CategoryBM.cs
│   ├── NewsArticleBM.cs
│   ├── TagBM.cs
│   └── NewsTagBM.cs
├── Request/               # Request Models (API input)
│   ├── Auth/
│   │   ├── LoginRequest.cs
│   │   └── RegisterRequest.cs
│   ├── Account/
│   │   ├── CreateAccountRequest.cs
│   │   └── UpdateAccountRequest.cs
│   ├── Category/
│   │   ├── CreateCategoryRequest.cs
│   │   └── UpdateCategoryRequest.cs
│   ├── NewsArticle/
│   │   ├── CreateNewsArticleRequest.cs
│   │   └── UpdateNewsArticleRequest.cs
│   └── Common/
│       ├── PaginationRequest.cs
│       ├── SearchRequest.cs
│       └── SortRequest.cs
├── Response/              # Response Models (API output)
│   ├── Auth/
│   │   └── LoginResponse.cs
│   ├── Account/
│   │   ├── AccountResponse.cs
│   │   └── AccountDetailResponse.cs
│   ├── Category/
│   │   ├── CategoryResponse.cs
│   │   └── CategoryDetailResponse.cs
│   ├── NewsArticle/
│   │   ├── NewsArticleResponse.cs
│   │   └── NewsArticleDetailResponse.cs
│   ├── Report/
│   │   └── NewsStatisticReportResponse.cs
│   └── Common/
│       ├── ApiResponse.cs
│       ├── PaginatedResponse.cs
│       └── ErrorResponse.cs
├── Enums/
│   ├── AccountRole.cs
│   ├── NewsStatus.cs
│   └── CategoryStatus.cs
└── Constants/
    ├── ErrorMessages.cs
    ├── SuccessMessages.cs
    └── ValidationMessages.cs
```

### Detailed Implementation

#### 1. Entity Models

**SystemAccount.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Entities
{
    public class SystemAccount
    {
        public short AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int AccountRole { get; set; }
        public string AccountPassword { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();
    }
}
```

**Category.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Entities
{
    public class Category
    {
        public short CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDesciption { get; set; } = string.Empty;
        public short? ParentCategoryID { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
```

**NewsArticle.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Entities
{
    public class NewsArticle
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string NewsContent { get; set; } = string.Empty;
        public string NewsSource { get; set; } = string.Empty;
        public short CategoryID { get; set; }
        public bool NewsStatus { get; set; }
        public short CreatedByID { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual SystemAccount CreatedBy { get; set; } = null!;
        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
```

**Tag.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Entities
{
    public class Tag
    {
        public int TagID { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
```

**NewsTag.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Entities
{
    public class NewsTag
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public int TagID { get; set; }
        
        // Navigation properties
        public virtual NewsArticle NewsArticle { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
```

#### 2. Request Models

**LoginRequest.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
```

**CreateAccountRequest.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.Account
{
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, ErrorMessage = "Account name must not exceed 100 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [Range(1, 2, ErrorMessage = "Role must be 1 (Staff) or 2 (Lecturer)")]
        public int AccountRole { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [StringLength(100, ErrorMessage = "Password must not exceed 100 characters")]
        public string AccountPassword { get; set; } = string.Empty;
    }
}
```

**CreateCategoryRequest.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.Category
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name must not exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description must not exceed 250 characters")]
        public string CategoryDesciption { get; set; } = string.Empty;

        public short? ParentCategoryID { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
```

**CreateNewsArticleRequest.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.NewsArticle
{
    public class CreateNewsArticleRequest
    {
        [Required(ErrorMessage = "News title is required")]
        [StringLength(400, ErrorMessage = "News title must not exceed 400 characters")]
        public string NewsTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(150, ErrorMessage = "Headline must not exceed 150 characters")]
        public string Headline { get; set; } = string.Empty;

        [Required(ErrorMessage = "News content is required")]
        public string NewsContent { get; set; } = string.Empty;

        [StringLength(400, ErrorMessage = "News source must not exceed 400 characters")]
        public string NewsSource { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category ID is required")]
        public short CategoryID { get; set; }

        public bool NewsStatus { get; set; } = true;

        public List<int> TagIDs { get; set; } = new List<int>();
    }
}
```

**PaginationRequest.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Request.Common
{
    public class PaginationRequest
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int Page { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
```

#### 3. Response Models

**ApiResponse.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Response.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResult(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> FailureResult(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
```

**PaginatedResponse.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Response.Common
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResponse()
        {
        }

        public PaginatedResponse(List<T> items, int count, int page, int pageSize)
        {
            Items = items;
            TotalItems = count;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
```

**AccountResponse.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Response.Account
{
    public class AccountResponse
    {
        public short AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int AccountRole { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
```

**NewsArticleDetailResponse.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Response.NewsArticle
{
    public class NewsArticleDetailResponse
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string NewsContent { get; set; } = string.Empty;
        public string NewsSource { get; set; } = string.Empty;
        public short CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool NewsStatus { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public short CreatedByID { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime? ModifiedDate { get; set; }
        public List<TagResponse> Tags { get; set; } = new List<TagResponse>();
    }

    public class TagResponse
    {
        public int TagID { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}
```

#### 4. Enums

**AccountRole.cs**
```csharp
namespace PRN232.FUNewsManagement.Models.Enums
{
    public enum AccountRole
    {
        Staff = 1,
        Lecturer = 2
    }
}
```

---

## 📦 Project 2: PRN232.FUNewsManagement.Repo

### Purpose
Quản lý truy xuất dữ liệu, ánh xạ với database

### Folder Structure
```
PRN232.FUNewsManagement.Repo/
├── Data/
│   ├── FUNewsManagementContext.cs
│   └── DbInitializer.cs
├── Interfaces/
│   ├── ISystemAccountRepository.cs
│   ├── ICategoryRepository.cs
│   ├── INewsArticleRepository.cs
│   ├── ITagRepository.cs
│   └── IUnitOfWork.cs
├── Repositories/
│   ├── BaseRepository.cs
│   ├── SystemAccountRepository.cs
│   ├── CategoryRepository.cs
│   ├── NewsArticleRepository.cs
│   └── TagRepository.cs
└── UnitOfWork.cs
```

### Detailed Implementation

#### 1. DbContext

**FUNewsManagementContext.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Data
{
    public class FUNewsManagementContext : DbContext
    {
        public FUNewsManagementContext(DbContextOptions<FUNewsManagementContext> options)
            : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SystemAccount configuration
            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.ToTable("SystemAccount");
                entity.HasKey(e => e.AccountID);
                entity.Property(e => e.AccountID).UseIdentityColumn();
                entity.Property(e => e.AccountName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.AccountEmail).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.AccountEmail).IsUnique();
                entity.Property(e => e.AccountPassword).HasMaxLength(100).IsRequired();
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryID).UseIdentityColumn();
                entity.Property(e => e.CategoryName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CategoryDesciption).HasMaxLength(250);
                
                entity.HasOne(e => e.ParentCategory)
                    .WithMany(e => e.ChildCategories)
                    .HasForeignKey(e => e.ParentCategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // NewsArticle configuration
            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.ToTable("NewsArticle");
                entity.HasKey(e => e.NewsArticleID);
                entity.Property(e => e.NewsArticleID).HasMaxLength(20);
                entity.Property(e => e.NewsTitle).HasMaxLength(400).IsRequired();
                entity.Property(e => e.Headline).HasMaxLength(150).IsRequired();
                entity.Property(e => e.NewsSource).HasMaxLength(400);
                
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.NewsArticles)
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.CreatedBy)
                    .WithMany(e => e.CreatedNewsArticles)
                    .HasForeignKey(e => e.CreatedByID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Tag configuration
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");
                entity.HasKey(e => e.TagID);
                entity.Property(e => e.TagID).UseIdentityColumn();
                entity.Property(e => e.TagName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Note).HasMaxLength(400);
            });

            // NewsTag configuration
            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.ToTable("NewsTag");
                entity.HasKey(e => new { e.NewsArticleID, e.TagID });
                
                entity.HasOne(e => e.NewsArticle)
                    .WithMany(e => e.NewsTags)
                    .HasForeignKey(e => e.NewsArticleID)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Tag)
                    .WithMany(e => e.NewsTags)
                    .HasForeignKey(e => e.TagID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
```

#### 2. Repository Interfaces

**IBaseRepository.cs**
```csharp
using System.Linq.Expressions;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
```

**ISystemAccountRepository.cs**
```csharp
using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface ISystemAccountRepository : IBaseRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
        Task<bool> HasCreatedNewsArticlesAsync(short accountId);
        Task<(IEnumerable<SystemAccount> items, int totalCount)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? searchTerm = null,
            int? role = null,
            string? sortBy = null,
            bool isDescending = false);
    }
}
```

**ICategoryRepository.cs**
```csharp
using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> HasNewsArticlesAsync(short categoryId);
        Task<bool> NameExistsAsync(string name, short? excludeCategoryId = null);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetChildCategoriesAsync(short parentId);
        Task<(IEnumerable<Category> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? isActive = null,
            short? parentCategoryId = null,
            string? sortBy = null,
            bool isDescending = false);
    }
}
```

**INewsArticleRepository.cs**
```csharp
using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface INewsArticleRepository : IBaseRepository<NewsArticle>
    {
        Task<NewsArticle?> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<IEnumerable<NewsArticle>> GetNewsByStaffAsync(short staffId);
        Task<string> GenerateNewsIdAsync();
        Task<(IEnumerable<NewsArticle> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? status = null,
            short? categoryId = null,
            short? createdById = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isDescending = false);
        Task<IEnumerable<NewsArticle>> GetNewsStatisticByPeriodAsync(
            DateTime startDate,
            DateTime endDate);
    }
}
```

**IUnitOfWork.cs**
```csharp
namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISystemAccountRepository SystemAccounts { get; }
        ICategoryRepository Categories { get; }
        INewsArticleRepository NewsArticles { get; }
        ITagRepository Tags { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
```

#### 3. Repository Implementations

**BaseRepository.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;
using System.Linq.Expressions;

namespace PRN232.FUNewsManagement.Repo.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly FUNewsManagementContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(FUNewsManagementContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null 
                ? await _dbSet.CountAsync() 
                : await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
```

**NewsArticleRepository.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;

namespace PRN232.FUNewsManagement.Repo.Repositories
{
    public class NewsArticleRepository : BaseRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public async Task<NewsArticle?> GetByIdWithDetailsAsync(string id)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleID == id);
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.NewsStatus == true)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByStaffAsync(short staffId)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Where(n => n.CreatedByID == staffId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<string> GenerateNewsIdAsync()
        {
            var lastNews = await _dbSet
                .OrderByDescending(n => n.NewsArticleID)
                .FirstOrDefaultAsync();

            if (lastNews == null)
            {
                return "NEWS0001";
            }

            var lastNumber = int.Parse(lastNews.NewsArticleID.Replace("NEWS", ""));
            var newNumber = lastNumber + 1;
            return $"NEWS{newNumber:D4}";
        }

        public async Task<(IEnumerable<NewsArticle> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? status = null,
            short? categoryId = null,
            short? createdById = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            var query = _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(n =>
                    n.NewsTitle.ToLower().Contains(searchTerm) ||
                    n.Headline.ToLower().Contains(searchTerm) ||
                    n.NewsContent.ToLower().Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(n => n.NewsStatus == status.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(n => n.CategoryID == categoryId.Value);
            }

            if (createdById.HasValue)
            {
                query = query.Where(n => n.CreatedByID == createdById.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate <= toDate.Value);
            }

            // Sorting
            query = ApplySorting(query, sortBy, isDescending);

            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsStatisticByPeriodAsync(
            DateTime startDate,
            DateTime endDate)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        private IQueryable<NewsArticle> ApplySorting(
            IQueryable<NewsArticle> query,
            string? sortBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return query.OrderByDescending(n => n.CreatedDate);
            }

            query = sortBy.ToLower() switch
            {
                "title" => isDescending
                    ? query.OrderByDescending(n => n.NewsTitle)
                    : query.OrderBy(n => n.NewsTitle),
                "createddate" => isDescending
                    ? query.OrderByDescending(n => n.CreatedDate)
                    : query.OrderBy(n => n.CreatedDate),
                "category" => isDescending
                    ? query.OrderByDescending(n => n.Category.CategoryName)
                    : query.OrderBy(n => n.Category.CategoryName),
                _ => query.OrderByDescending(n => n.CreatedDate)
            };

            return query;
        }
    }
}
```

#### 4. Unit of Work

**UnitOfWork.cs**
```csharp
using Microsoft.EntityFrameworkCore.Storage;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Repo.Repositories;

namespace PRN232.FUNewsManagement.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FUNewsManagementContext _context;
        private IDbContextTransaction? _transaction;

        private ISystemAccountRepository? _systemAccounts;
        private ICategoryRepository? _categories;
        private INewsArticleRepository? _newsArticles;
        private ITagRepository? _tags;

        public UnitOfWork(FUNewsManagementContext context)
        {
            _context = context;
        }

        public ISystemAccountRepository SystemAccounts
        {
            get
            {
                _systemAccounts ??= new SystemAccountRepository(_context);
                return _systemAccounts;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                _categories ??= new CategoryRepository(_context);
                return _categories;
            }
        }

        public INewsArticleRepository NewsArticles
        {
            get
            {
                _newsArticles ??= new NewsArticleRepository(_context);
                return _newsArticles;
            }
        }

        public ITagRepository Tags
        {
            get
            {
                _tags ??= new TagRepository(_context);
                return _tags;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
```

---

## 📦 Project 3: PRN232.FUNewsManagement.Services

### Purpose
Xử lý business logic, mapping giữa các model types

### Folder Structure
```
PRN232.FUNewsManagement.Services/
├── Interfaces/
│   ├── IAuthService.cs
│   ├── ISystemAccountService.cs
│   ├── ICategoryService.cs
│   ├── INewsArticleService.cs
│   ├── ITagService.cs
│   ├── ITokenService.cs
│   └── IReportService.cs
├── Services/
│   ├── AuthService.cs
│   ├── SystemAccountService.cs
│   ├── CategoryService.cs
│   ├── NewsArticleService.cs
│   ├── TagService.cs
│   ├── TokenService.cs
│   └── ReportService.cs
├── Mappers/
│   ├── AccountMapper.cs
│   ├── CategoryMapper.cs
│   ├── NewsArticleMapper.cs
│   └── TagMapper.cs
├── Validators/
│   ├── AccountValidator.cs
│   ├── CategoryValidator.cs
│   └── NewsArticleValidator.cs
└── Helpers/
    ├── PasswordHelper.cs
    └── JwtSettings.cs
```

### Detailed Implementation

#### 1. Service Interfaces

**IAuthService.cs**
```csharp
using PRN232.FUNewsManagement.Models.Request.Auth;
using PRN232.FUNewsManagement.Models.Response.Auth;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<bool> ValidateTokenAsync(string token);
    }
}
```

**ISystemAccountService.cs**
```csharp
using PRN232.FUNewsManagement.Models.Request.Account;
using PRN232.FUNewsManagement.Models.Response.Account;
using PRN232.FUNewsManagement.Models.Response.Common;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface ISystemAccountService
    {
        Task<AccountDetailResponse?> GetByIdAsync(short id);
        Task<PaginatedResponse<AccountResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            int? role = null,
            string? sortBy = null,
            bool isDescending = false);
        Task<AccountDetailResponse> CreateAsync(CreateAccountRequest request);
        Task<AccountDetailResponse> UpdateAsync(short id, UpdateAccountRequest request);
        Task<bool> DeleteAsync(short id);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
    }
}
```

**INewsArticleService.cs**
```csharp
using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.Common;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface INewsArticleService
    {
        Task<NewsArticleDetailResponse?> GetByIdAsync(string id);
        Task<PaginatedResponse<NewsArticleResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? status = null,
            short? categoryId = null,
            short? createdById = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isDescending = false);
        Task<List<NewsArticleResponse>> GetActiveNewsAsync();
        Task<List<NewsArticleResponse>> GetNewsByStaffAsync(short staffId);
        Task<NewsArticleDetailResponse> CreateAsync(CreateNewsArticleRequest request, short createdById);
        Task<NewsArticleDetailResponse> UpdateAsync(string id, UpdateNewsArticleRequest request, short updatedById);
        Task<bool> DeleteAsync(string id);
    }
}
```

#### 2. Service Implementations

**AuthService.cs**
```csharp
using Microsoft.Extensions.Configuration;
using PRN232.FUNewsManagement.Models.Enums;
using PRN232.FUNewsManagement.Models.Request.Auth;
using PRN232.FUNewsManagement.Models.Response.Auth;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Helpers;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            // Check admin account from appsettings
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (request.Email == adminEmail && request.Password == adminPassword)
            {
                var token = _tokenService.GenerateToken(0, adminEmail, 0); // Role 0 for Admin
                return new LoginResponse
                {
                    Token = token,
                    Email = adminEmail,
                    Role = 0,
                    RoleName = "Admin"
                };
            }

            // Check regular account
            var account = await _unitOfWork.SystemAccounts.GetByEmailAsync(request.Email);
            if (account == null)
            {
                return null;
            }

            if (!PasswordHelper.VerifyPassword(request.Password, account.AccountPassword))
            {
                return null;
            }

            var userToken = _tokenService.GenerateToken(
                account.AccountID,
                account.AccountEmail,
                account.AccountRole);

            return new LoginResponse
            {
                Token = userToken,
                AccountId = account.AccountID,
                Email = account.AccountEmail,
                Name = account.AccountName,
                Role = account.AccountRole,
                RoleName = ((AccountRole)account.AccountRole).ToString()
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await Task.FromResult(_tokenService.ValidateToken(token));
        }
    }
}
```

**NewsArticleService.cs**
```csharp
using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Interfaces;
using PRN232.FUNewsManagement.Services.Mappers;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NewsArticleDetailResponse?> GetByIdAsync(string id)
        {
            var newsArticle = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(id);
            if (newsArticle == null)
            {
                return null;
            }

            return NewsArticleMapper.ToDetailResponse(newsArticle);
        }

        public async Task<PaginatedResponse<NewsArticleResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? status = null,
            short? categoryId = null,
            short? createdById = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            var (items, totalCount) = await _unitOfWork.NewsArticles.GetPagedAsync(
                page,
                pageSize,
                searchTerm,
                status,
                categoryId,
                createdById,
                fromDate,
                toDate,
                sortBy,
                isDescending);

            var responseItems = items.Select(NewsArticleMapper.ToResponse).ToList();

            return new PaginatedResponse<NewsArticleResponse>(
                responseItems,
                totalCount,
                page,
                pageSize);
        }

        public async Task<List<NewsArticleResponse>> GetActiveNewsAsync()
        {
            var newsArticles = await _unitOfWork.NewsArticles.GetActiveNewsAsync();
            return newsArticles.Select(NewsArticleMapper.ToResponse).ToList();
        }

        public async Task<List<NewsArticleResponse>> GetNewsByStaffAsync(short staffId)
        {
            var newsArticles = await _unitOfWork.NewsArticles.GetNewsByStaffAsync(staffId);
            return newsArticles.Select(NewsArticleMapper.ToResponse).ToList();
        }

        public async Task<NewsArticleDetailResponse> CreateAsync(
            CreateNewsArticleRequest request,
            short createdById)
        {
            // Validate category exists and is active
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryID);
            if (category == null || !category.IsActive)
            {
                throw new InvalidOperationException("Category does not exist or is inactive");
            }

            // Generate new ID
            var newsId = await _unitOfWork.NewsArticles.GenerateNewsIdAsync();

            // Create entity
            var newsArticle = NewsArticleMapper.ToEntity(request, newsId, createdById);

            await _unitOfWork.NewsArticles.AddAsync(newsArticle);
            await _unitOfWork.SaveChangesAsync();

            // Load full details
            var createdNews = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(newsId);
            return NewsArticleMapper.ToDetailResponse(createdNews!);
        }

        public async Task<NewsArticleDetailResponse> UpdateAsync(
            string id,
            UpdateNewsArticleRequest request,
            short updatedById)
        {
            var newsArticle = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(id);
            if (newsArticle == null)
            {
                throw new InvalidOperationException("News article not found");
            }

            // Validate category if changed
            if (request.CategoryID != newsArticle.CategoryID)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryID);
                if (category == null || !category.IsActive)
                {
                    throw new InvalidOperationException("Category does not exist or is inactive");
                }
            }

            // Update entity
            NewsArticleMapper.UpdateEntity(newsArticle, request, updatedById);

            _unitOfWork.NewsArticles.Update(newsArticle);
            await _unitOfWork.SaveChangesAsync();

            // Reload with details
            var updatedNews = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(id);
            return NewsArticleMapper.ToDetailResponse(updatedNews!);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var newsArticle = await _unitOfWork.NewsArticles.GetByIdAsync(id);
            if (newsArticle == null)
            {
                return false;
            }

            _unitOfWork.NewsArticles.Delete(newsArticle);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
```

#### 3. Mappers

**NewsArticleMapper.cs**
```csharp
using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;

namespace PRN232.FUNewsManagement.Services.Mappers
{
    public static class NewsArticleMapper
    {
        public static NewsArticleResponse ToResponse(NewsArticle entity)
        {
            return new NewsArticleResponse
            {
                NewsArticleID = entity.NewsArticleID,
                NewsTitle = entity.NewsTitle,
                Headline = entity.Headline,
                CreatedDate = entity.CreatedDate,
                CategoryID = entity.CategoryID,
                CategoryName = entity.Category?.CategoryName ?? string.Empty,
                NewsStatus = entity.NewsStatus,
                StatusText = entity.NewsStatus ? "Active" : "Inactive",
                CreatedByID = entity.CreatedByID,
                CreatedByName = entity.CreatedBy?.AccountName ?? string.Empty,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public static NewsArticleDetailResponse ToDetailResponse(NewsArticle entity)
        {
            return new NewsArticleDetailResponse
            {
                NewsArticleID = entity.NewsArticleID,
                NewsTitle = entity.NewsTitle,
                Headline = entity.Headline,
                CreatedDate = entity.CreatedDate,
                NewsContent = entity.NewsContent,
                NewsSource = entity.NewsSource,
                CategoryID = entity.CategoryID,
                CategoryName = entity.Category?.CategoryName ?? string.Empty,
                NewsStatus = entity.NewsStatus,
                StatusText = entity.NewsStatus ? "Active" : "Inactive",
                CreatedByID = entity.CreatedByID,
                CreatedByName = entity.CreatedBy?.AccountName ?? string.Empty,
                ModifiedDate = entity.ModifiedDate,
                Tags = entity.NewsTags?.Select(nt => new TagResponse
                {
                    TagID = nt.Tag.TagID,
                    TagName = nt.Tag.TagName,
                    Note = nt.Tag.Note
                }).ToList() ?? new List<TagResponse>()
            };
        }

        public static NewsArticle ToEntity(
            CreateNewsArticleRequest request,
            string newsId,
            short createdById)
        {
            return new NewsArticle
            {
                NewsArticleID = newsId,
                NewsTitle = request.NewsTitle,
                Headline = request.Headline,
                NewsContent = request.NewsContent,
                NewsSource = request.NewsSource,
                CategoryID = request.CategoryID,
                NewsStatus = request.NewsStatus,
                CreatedByID = createdById,
                CreatedDate = DateTime.Now,
                NewsTags = request.TagIDs.Select(tagId => new NewsTag
                {
                    NewsArticleID = newsId,
                    TagID = tagId
                }).ToList()
            };
        }

        public static void UpdateEntity(
            NewsArticle entity,
            UpdateNewsArticleRequest request,
            short updatedById)
        {
            entity.NewsTitle = request.NewsTitle;
            entity.Headline = request.Headline;
            entity.NewsContent = request.NewsContent;
            entity.NewsSource = request.NewsSource;
            entity.CategoryID = request.CategoryID;
            entity.NewsStatus = request.NewsStatus;
            entity.UpdatedByID = updatedById;
            entity.ModifiedDate = DateTime.Now;

            // Update tags
            entity.NewsTags.Clear();
            foreach (var tagId in request.TagIDs)
            {
                entity.NewsTags.Add(new NewsTag
                {
                    NewsArticleID = entity.NewsArticleID,
                    TagID = tagId
                });
            }
        }
    }
}
```

#### 4. Helpers

**PasswordHelper.cs**
```csharp
using System.Security.Cryptography;
using System.Text;

namespace PRN232.FUNewsManagement.Services.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}
```

**TokenService.cs**
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232.FUNewsManagement.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(short accountId, string email, int role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, accountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey!);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
```

---

## 📦 Project 4: PRN232.FUNewsManagement.API

### Purpose
API Layer - Controllers, middleware, configuration

### Folder Structure
```
PRN232.FUNewsManagement.API/
├── Controllers/
│   ├── AuthController.cs
│   ├── AccountsController.cs
│   ├── CategoriesController.cs
│   ├── NewsArticlesController.cs
│   ├── TagsController.cs
│   └── ReportsController.cs
├── Middleware/
│   ├── GlobalExceptionMiddleware.cs
│   └── JwtMiddleware.cs
├── Filters/
│   └── ValidateModelFilter.cs
├── Extensions/
│   ├── ServiceExtensions.cs
│   └── SwaggerExtensions.cs
├── Program.cs
└── appsettings.json
```

### Detailed Implementation

#### 1. Controllers

**AuthController.cs**
```csharp
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Models.Request.Auth;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login to the system
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var result = await _authService.LoginAsync(request);
            if (result == null)
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Invalid email or password"));
            }

            return Ok(ApiResponse<object>.SuccessResult(result, "Login successful"));
        }
    }
}
```

**NewsArticlesController.cs**
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Models.Request.Common;
using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Services.Interfaces;
using System.Security.Claims;

namespace PRN232.FUNewsManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        /// <summary>
        /// Get paginated list of news articles with filtering, sorting, and searching
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] bool? status = null,
            [FromQuery] short? categoryId = null,
            [FromQuery] short? createdById = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            var result = await _newsArticleService.GetPagedAsync(
                page,
                pageSize,
                searchTerm,
                status,
                categoryId,
                createdById,
                fromDate,
                toDate,
                sortBy,
                isDescending);

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get news article by ID with full details
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _newsArticleService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.FailureResult("News article not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get active news articles (public access)
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveNews()
        {
            var result = await _newsArticleService.GetActiveNewsAsync();
            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get news articles created by current staff
        /// </summary>
        [HttpGet("my-news")]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyNews()
        {
            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _newsArticleService.GetNewsByStaffAsync(accountId);
            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Create new news article
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _newsArticleService.CreateAsync(request, accountId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.NewsArticleID },
                ApiResponse<object>.SuccessResult(result, "News article created successfully"));
        }

        /// <summary>
        /// Update news article
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            try
            {
                var result = await _newsArticleService.UpdateAsync(id, request, accountId);
                return Ok(ApiResponse<object>.SuccessResult(result, "News article updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Delete news article
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _newsArticleService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse<object>.FailureResult("News article not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult<object>(null, "News article deleted successfully"));
        }
    }
}
```

#### 2. Middleware

**GlobalExceptionMiddleware.cs**
```csharp
using PRN232.FUNewsManagement.Models.Response.Common;
using System.Net;
using System.Text.Json;

namespace PRN232.FUNewsManagement.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.FailureResult(
                "An error occurred while processing your request",
                new List<string> { exception.Message });

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(json);
        }
    }
}
```

#### 3. Program.cs

**Program.cs**
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PRN232.FUNewsManagement.API.Middleware;
using PRN232.FUNewsManagement.Repo;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Interfaces;
using PRN232.FUNewsManagement.Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<FUNewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add UnitOfWork and Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Add Controllers
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FU News Management API",
        Version = "v1",
        Description = "API for managing news articles in FU News Management System"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

#### 4. appsettings.json

**appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=FUNewsManagement;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyForJWTToken123456789",
    "Issuer": "FUNewsManagement",
    "Audience": "FUNewsManagementUsers",
    "ExpiryMinutes": 60
  },
  "AdminAccount": {
    "Email": "admin@FUNewsManagementSystem.org",
    "Password": "@@abc123@@"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 🔄 Implementation Steps

### Phase 1: Setup & Database (Day 1)

1. **Create Solution Structure**
   ```bash
   - Create new solution: NguyenTrongThu_SE1856_A01_BE.sln
   - Add 4 projects with correct naming convention
   - Set dependencies between projects
   ```

2. **Setup Database**
   ```bash
   - Run the provided SQL script
   - Verify all tables and relationships
   - Add seed data for testing
   ```

3. **Configure Models Project**
   ```bash
   - Create all folder structures
   - Implement Entity models
   - Add NuGet packages (if needed)
   ```

### Phase 2: Repository Layer (Day 2)

1. **Setup DbContext**
   ```bash
   - Install EF Core packages
   - Create FUNewsManagementContext
   - Configure entity relationships
   - Add connection string to appsettings.json
   ```

2. **Implement Repositories**
   ```bash
   - Create base repository interface and implementation
   - Implement specific repositories for each entity
   - Implement advanced query methods (paging, filtering, sorting)
   - Create Unit of Work pattern
   ```

3. **Test Repository Layer**
   ```bash
   - Create migration
   - Update database
   - Test CRUD operations
   ```

### Phase 3: Business Models & Service Layer (Day 3-4)

1. **Create Request/Response Models**
   ```bash
   - Implement all request models with validation attributes
   - Implement all response models
   - Create common models (ApiResponse, PaginatedResponse)
   ```

2. **Implement Service Layer**
   ```bash
   - Create service interfaces
   - Implement AuthService with JWT
   - Implement business services
   - Create mapper classes
   - Add validation logic
   ```

3. **Test Services**
   ```bash
   - Unit test critical business logic
   - Test mapping between models
   ```

### Phase 4: API Layer (Day 5-6)

1. **Setup API Project**
   ```bash
   - Configure Program.cs
   - Add JWT authentication
   - Setup Swagger
   - Configure CORS
   ```

2. **Implement Controllers**
   ```bash
   - Create AuthController
   - Create AccountsController (Admin only)
   - Create CategoriesController (Staff only)
   - Create NewsArticlesController (Staff only)
   - Create TagsController
   - Create ReportsController (Admin only)
   - Add proper authorization attributes
   ```

3. **Add Middleware**
   ```bash
   - Global exception handler
   - Request/Response logging (optional)
   ```

4. **Configure Swagger Documentation**
   ```bash
   - Add XML comments
   - Configure JWT in Swagger
   - Document all endpoints
   ```

### Phase 5: Testing & Refinement (Day 7)

1. **API Testing**
   ```bash
   - Test all endpoints using Postman/Swagger
   - Verify authentication & authorization
   - Test validation rules
   - Test error handling
   ```

2. **Performance Optimization**
   ```bash
   - Review and optimize queries
   - Add appropriate indexes
   - Test with larger datasets
   ```

3. **Code Quality**
   ```bash
   - Remove unused code and comments
   - Apply consistent naming conventions
   - Ensure clean code principles
   - Document complex logic
   ```

---

## 📝 API Endpoints Summary

### Authentication
- `POST /api/auth/login` - Login (Public)

### Accounts (Admin Only)
- `GET /api/accounts` - Get paginated accounts with search/filter/sort
- `GET /api/accounts/{id}` - Get account details
- `POST /api/accounts` - Create account
- `PUT /api/accounts/{id}` - Update account
- `DELETE /api/accounts/{id}` - Delete account (if no news created)

### Categories (Staff Only)
- `GET /api/categories` - Get paginated categories with search/filter/sort
- `GET /api/categories/{id}` - Get category details
- `GET /api/categories/active` - Get active categories
- `POST /api/categories` - Create category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category (if no news linked)

### News Articles
- `GET /api/newsarticles` - Get paginated news with search/filter/sort
- `GET /api/newsarticles/{id}` - Get news details with tags
- `GET /api/newsarticles/active` - Get active news (Public)
- `GET /api/newsarticles/my-news` - Get current staff's news (Staff Only)
- `POST /api/newsarticles` - Create news (Staff Only)
- `PUT /api/newsarticles/{id}` - Update news (Staff Only)
- `DELETE /api/newsarticles/{id}` - Delete news (Staff Only)

### Tags (Staff Only)
- `GET /api/tags` - Get all tags
- `GET /api/tags/{id}` - Get tag details
- `POST /api/tags` - Create tag
- `PUT /api/tags/{id}` - Update tag
- `DELETE /api/tags/{id}` - Delete tag

### Reports (Admin Only)
- `GET /api/reports/statistics` - Get news statistics by period

---

## 🎯 Clean Code Principles Applied

### 1. Naming Conventions
- PascalCase for classes, methods, properties
- camelCase for parameters, local variables
- Meaningful and descriptive names
- Avoid abbreviations

### 2. SOLID Principles
- **Single Responsibility**: Each class has one responsibility
- **Open/Closed**: Open for extension, closed for modification
- **Liskov Substitution**: Derived classes can substitute base classes
- **Interface Segregation**: Specific interfaces, not general ones
- **Dependency Inversion**: Depend on abstractions, not concretions

### 3. Code Organization
- Separation of concerns (3-layer architecture)
- Proper use of namespaces and folders
- Consistent file structure

### 4. Error Handling
- Global exception middleware
- Proper HTTP status codes
- Meaningful error messages
- No exposed stack traces

### 5. Performance
- Async/await for all I/O operations
- Efficient database queries
- Proper use of Include for eager loading
- Pagination for large datasets

### 6. Security
- Password hashing
- JWT authentication
- Role-based authorization
- Input validation
- SQL injection prevention (EF Core parameterized queries)

---

## 📋 Testing Checklist

### Authentication
- [ ] Login with admin credentials
- [ ] Login with staff credentials
- [ ] Login with invalid credentials
- [ ] Access protected endpoints without token
- [ ] Access protected endpoints with expired token

### Account Management (Admin)
- [ ] Create new staff account
- [ ] Update account information
- [ ] Delete account without news articles
- [ ] Try to delete account with news articles (should fail)
- [ ] Search and filter accounts
- [ ] Sort accounts
- [ ] Pagination works correctly

### Category Management (Staff)
- [ ] Create new category
- [ ] Create child category
- [ ] Update category
- [ ] Delete category without news
- [ ] Try to delete category with news (should fail)
- [ ] Filter by active/inactive status
- [ ] Search categories
- [ ] Pagination works

### News Article Management (Staff)
- [ ] Create news with tags
- [ ] Update news and tags
- [ ] Delete news
- [ ] Filter by status, category, date range
- [ ] Search in title, headline, content
- [ ] Sort by different fields
- [ ] Pagination works
- [ ] View only own news articles

### Reports (Admin)
- [ ] Generate statistics for date range
- [ ] Sort results in descending order
- [ ] Export functionality (if implemented)

### Validation
- [ ] All required fields validated
- [ ] Email format validation
- [ ] String length validation
- [ ] Date range validation
- [ ] Foreign key validation

### Error Handling
- [ ] 400 Bad Request for validation errors
- [ ] 401 Unauthorized for missing/invalid token
- [ ] 403 Forbidden for insufficient permissions
- [ ] 404 Not Found for non-existent resources
- [ ] 500 Internal Server Error handled gracefully

---

## 🚀 Deployment Considerations

1. **Environment Configuration**
   - Separate appsettings for Development/Production
   - Secure connection strings
   - Environment-specific JWT settings

2. **Database**
   - Migration scripts ready
   - Backup strategy
   - Index optimization

3. **Security**
   - HTTPS enforced
   - CORS properly configured
   - Secrets in environment variables

4. **Monitoring**
   - Logging configured
   - Error tracking
   - Performance monitoring

---

## 📚 NuGet Packages Required

### PRN232.FUNewsManagement.API
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
```

### PRN232.FUNewsManagement.Repo
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
```

### PRN232.FUNewsManagement.Services
```xml
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.0" />
```

### PRN232.FUNewsManagement.Models
```xml
<PackageReference Include="System.ComponentModel.Annotations" Version="5.0.0" />
```

---

## 🎓 Key Learning Outcomes

After completing this project, you will have demonstrated:

1. **Architecture Skills**
   - 3-layer architecture implementation
   - Repository pattern
   - Unit of Work pattern
   - Dependency Injection

2. **API Development**
   - RESTful API design
   - JWT authentication
   - Role-based authorization
   - Input validation
   - Error handling

3. **Database Skills**
   - EF Core usage
   - Complex relationships
   - Query optimization
   - Migrations

4. **Clean Code**
   - SOLID principles
   - Naming conventions
   - Code organization
   - Documentation

5. **Testing**
   - API testing
   - Validation testing
   - Security testing

---

## 📞 Support & Resources

- **Official Documentation**: https://docs.microsoft.com/aspnet/core
- **EF Core Documentation**: https://docs.microsoft.com/ef/core
- **JWT Documentation**: https://jwt.io
- **Clean Code Principles**: Robert C. Martin's "Clean Code"

---

**End of Project Plan**

This comprehensive plan provides everything needed for an AI agent to generate the complete backend solution following PRN232 standards and clean code principles. Each section is detailed enough to be implemented independently while maintaining consistency across the entire project.
