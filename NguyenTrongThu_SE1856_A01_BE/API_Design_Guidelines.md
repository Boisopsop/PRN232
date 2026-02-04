# FU News Management API - Design Guidelines

## 1. Tổng Quan (Overview)

### 1.1 API Response Format

Tất cả API responses đều tuân theo format chuẩn:

```json
{
  "success": true,
  "message": "Success message",
  "data": { ... },
  "errors": null
}
```

| Field | Type | Description |
|-------|------|-------------|
| `success` | boolean | Trạng thái xử lý: `true` = thành công, `false` = thất bại |
| `message` | string | Thông điệp mô tả kết quả |
| `data` | object/array/null | Dữ liệu trả về (null khi thất bại hoặc không có data) |
| `errors` | array/null | Danh sách lỗi validation (null khi thành công) |

---

## 2. Get Collection (Lấy Danh Sách)

### 2.1 Endpoint Pattern
```
GET /api/{resource}
```

### 2.2 Query Parameters

#### Phân Trang (Pagination)
| Parameter | Type | Default | Max | Description |
|-----------|------|---------|-----|-------------|
| `page` | int | 1 | - | Số trang hiện tại (1-indexed) |
| `pageSize` | int | 10 | **50** | Số items mỗi trang |

**Quy tắc:**
- `page` phải >= 1, nếu < 1 thì mặc định = 1
- `pageSize` phải từ 1-50, nếu > 50 thì tự động = 50
- **Max Page Size = 50** (để tránh quá tải server)

#### Tìm Kiếm (Searching)
| Parameter | Type | Description |
|-----------|------|-------------|
| `searchTerm` | string | Từ khóa tìm kiếm (tìm kiếm text trong nhiều fields) |

**Quy tắc:**
- Tìm kiếm **case-insensitive**
- Tìm kiếm **contains** (không phải exact match)
- Tìm trong các fields: `name`, `title`, `email`, `description`...

#### Lọc Dữ Liệu (Filtering)

**Khi nào dùng Enum (số) vs Text:**

| Trường hợp | Format | Ví dụ |
|------------|--------|-------|
| Role, Status có giá trị cố định | **Enum (int)** | `role=0`, `status=true` |
| ID reference | **ID trực tiếp** | `categoryId=3` |
| Ngày tháng | **ISO 8601** | `fromDate=2024-01-01` |
| Text search | **String** | `searchTerm=vietnam` |

**Filtering đơn vs đa giá trị:**

| Trường hợp | Format | Ví dụ |
|------------|--------|-------|
| Lọc đơn giá trị (khuyến nghị) | `param=value` | `categoryId=3` |
| Lọc đa giá trị (nếu cần) | `param=v1,v2,v3` hoặc `param=v1&param=v2` | `categoryId=1,2,3` |

> **Khuyến nghị:** Dùng lọc đơn giá trị (`categoryId=3`) để đơn giản hóa. Nếu cần lọc nhiều giá trị, client gọi nhiều request hoặc dùng endpoint riêng.

#### Sắp Xếp (Sorting)
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `sortBy` | string | varies | Field để sắp xếp |
| `isDescending` | bool | false | `true` = giảm dần, `false` = tăng dần |

**Các giá trị `sortBy` hợp lệ:**
- `name`, `email`, `role` (Accounts)
- `name`, `status` (Categories)
- `title`, `createdDate`, `category` (NewsArticles)

### 2.3 Response Format

```json
{
  "success": true,
  "message": "Retrieved successfully",
  "data": {
    "items": [...],
    "page": 1,
    "pageSize": 10,
    "totalItems": 100,
    "totalPages": 10
  }
}
```

### 2.4 Ví Dụ Requests

```http
# Lấy trang 2, mỗi trang 20 items
GET /api/newsarticles?page=2&pageSize=20

# Tìm kiếm và lọc theo category
GET /api/newsarticles?searchTerm=covid&categoryId=5

# Lọc theo ngày và trạng thái
GET /api/newsarticles?fromDate=2024-01-01&toDate=2024-12-31&status=true

# Sắp xếp theo ngày tạo giảm dần
GET /api/newsarticles?sortBy=createdDate&isDescending=true
```

---

## 3. Get By ID (Lấy Chi Tiết Một Record)

### 3.1 Endpoint Pattern
```
GET /api/{resource}/{id}
```

### 3.2 Quy Tắc ID

| Trường hợp | Cách dùng |
|------------|-----------|
| Lấy **một** record | `GET /api/accounts/5` |
| Lấy **nhiều** records | Dùng Get Collection với filter: `GET /api/accounts?ids=1,2,3` (nếu cần) |

> **Khuyến nghị:** Chỉ hỗ trợ lấy 1 ID mỗi request để đơn giản. Nếu cần lấy nhiều, dùng Get Collection.

### 3.3 Response Format

**Thành công (200 OK):**
```json
{
  "success": true,
  "message": "Retrieved successfully",
  "data": {
    "categoryID": 1,
    "categoryName": "Technology",
    "categoryDesciption": "Tech news",
    "parentCategoryID": null,
    "parentCategoryName": null,
    "isActive": true,
    "statusText": "Active",
    "totalNewsArticles": 25,
    "childCategories": [...]
  }
}
```

**Không tìm thấy (404 Not Found):**
```json
{
  "success": false,
  "message": "Category not found",
  "data": null,
  "errors": null
}
```

---

## 4. Create (Tạo Mới)

### 4.1 Endpoint Pattern
```
POST /api/{resource}
```

### 4.2 Request Body
- Content-Type: `application/json`
- Body chứa các fields bắt buộc và tùy chọn

### 4.3 Response

**Thành công (201 Created):**
```json
{
  "success": true,
  "message": "Category created successfully",
  "data": { ... created object ... }
}
```

**Validation thất bại (400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    "CategoryName is required",
    "CategoryName must be at most 100 characters"
  ]
}
```

---

## 5. Update (Cập Nhật)

### 5.1 Endpoint Pattern
```
PUT /api/{resource}/{id}
```

### 5.2 Quy Tắc
- Gửi **toàn bộ** object cần cập nhật (không phải partial update)
- Nếu field không thay đổi, vẫn gửi giá trị cũ

### 5.3 Response

**Thành công (200 OK):**
```json
{
  "success": true,
  "message": "Category updated successfully",
  "data": { ... updated object ... }
}
```

---

## 6. Delete (Xóa)

### 6.1 Endpoint Pattern
```
DELETE /api/{resource}/{id}
```

### 6.2 Response

**Thành công (200 OK):**
```json
{
  "success": true,
  "message": "Category deleted successfully",
  "data": null
}
```

**Có constraint (400 Bad Request):**
```json
{
  "success": false,
  "message": "Cannot delete category with linked news articles",
  "data": null,
  "errors": null
}
```

---

## 7. HTTP Status Codes

| Code | Meaning | Usage |
|------|---------|-------|
| **200** | OK | GET, PUT, DELETE thành công |
| **201** | Created | POST tạo mới thành công |
| **400** | Bad Request | Validation thất bại, business rule violation |
| **401** | Unauthorized | Chưa login hoặc token hết hạn |
| **403** | Forbidden | Không có quyền truy cập |
| **404** | Not Found | Resource không tồn tại |
| **500** | Internal Server Error | Lỗi server |

---

## 8. Authentication

### 8.1 Login
```
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@FUNewsManagementSystem.org",
  "password": "@@abc123@@"
}
```

### 8.2 Using Token
```
GET /api/accounts
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
```

### 8.3 Role Values
| Role | Value | Description |
|------|-------|-------------|
| Admin | 0 | Quản trị viên |
| Staff | 1 | Nhân viên |
| Lecturer | 2 | Giảng viên (chỉ đọc) |

---

## 9. Validation Rules

### 9.1 String Fields
| Rule | Attribute | Message |
|------|-----------|---------|
| Required | `[Required]` | "{Field} is required" |
| Max Length | `[StringLength(100)]` | "{Field} must be at most {max} characters" |
| Min Length | `[MinLength(6)]` | "{Field} must be at least {min} characters" |
| Email | `[EmailAddress]` | "Invalid email format" |

### 9.2 Numeric Fields
| Rule | Attribute | Message |
|------|-----------|---------|
| Range | `[Range(0, 2)]` | "{Field} must be between {min} and {max}" |

---

## 10. API Endpoints Summary

### Authentication
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/login` | No | Đăng nhập |

### Accounts
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/accounts` | Admin | List với pagination, filter, sort |
| GET | `/api/accounts/{id}` | Admin | Chi tiết account |
| POST | `/api/accounts` | Admin | Tạo account mới |
| PUT | `/api/accounts/{id}` | Admin | Cập nhật account |
| DELETE | `/api/accounts/{id}` | Admin | Xóa account |

### Categories
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/categories` | No | List với pagination, filter, sort |
| GET | `/api/categories/{id}` | No | Chi tiết category |
| GET | `/api/categories/active` | No | Danh sách active categories |
| POST | `/api/categories` | Admin/Staff | Tạo category |
| PUT | `/api/categories/{id}` | Admin/Staff | Cập nhật category |
| DELETE | `/api/categories/{id}` | Admin/Staff | Xóa category |

### News Articles
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/newsarticles` | No | List với pagination, filter, sort |
| GET | `/api/newsarticles/{id}` | No | Chi tiết news |
| GET | `/api/newsarticles/active` | No | Danh sách active news |
| GET | `/api/newsarticles/my-news` | Staff | News của staff hiện tại |
| POST | `/api/newsarticles` | Staff | Tạo news |
| PUT | `/api/newsarticles/{id}` | Staff | Cập nhật news |
| DELETE | `/api/newsarticles/{id}` | Staff | Xóa news |

### Tags
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/tags` | No | List tất cả tags |
| GET | `/api/tags/{id}` | No | Chi tiết tag |
| POST | `/api/tags` | Admin/Staff | Tạo tag |
| PUT | `/api/tags/{id}` | Admin/Staff | Cập nhật tag |
| DELETE | `/api/tags/{id}` | Admin/Staff | Xóa tag |

### Reports
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/reports/statistics?startDate=...&endDate=...` | Admin | Thống kê news theo khoảng thời gian |
