# FU News Management System - Docker Deployment Guide

## 🚀 Deployment Thành Công!

Project FU News Management System của bạn đã được deploy thành công bằng Docker!

## 📦 Containers đang chạy:

### 1. **funews-api** - ASP.NET Core 8.0 Web API
- **Port**: 8080
- **URL**: http://localhost:8080
- **Swagger UI**: http://localhost:8080/swagger
- **Features**:
  - JWT Authentication
  - OData Support  
  - RESTful API cho News Management

### 2. **funews-sqlserver** - SQL Server 2022 Express
- **Port**: 1433
- **Server**: localhost,1433
- **Username**: sa
- **Password**: YourStrong@Passw0rd
- **Database**: FUNewsManagement

## 🔗 API Endpoints

### Authentication
- `POST /api/Auth/login` - Đăng nhập
- `POST /api/Auth/register` - Đăng ký

### System Accounts
- `GET /api/SystemAccounts` - Lấy danh sách accounts
- `GET /api/SystemAccounts/{id}` - Lấy account theo ID
- `POST /api/SystemAccounts` - Tạo account mới
- `PUT /api/SystemAccounts/{id}` - Cập nhật account
- `DELETE /api/SystemAccounts/{id}` - Xóa account

### Categories
- `GET /api/Categories` - Lấy danh sách categories
- `GET /api/Categories/{id}` - Lấy category theo ID
- `POST /api/Categories` - Tạo category mới
- `PUT /api/Categories/{id}` - Cập nhật category
- `DELETE /api/Categories/{id}` - Xóa category

### News Articles
- `GET /api/NewsArticles` - Lấy danh sách news articles
- `GET /api/NewsArticles/{id}` - Lấy news article theo ID
- `POST /api/NewsArticles` - Tạo news article mới
- `PUT /api/NewsArticles/{id}` - Cập nhật news article
- `DELETE /api/NewsArticles/{id}` - Xóa news article

### OData Queries
API hỗ trợ OData queries tại `/odata` endpoint:
- `GET /odata/SystemAccounts?$filter=AccountRole eq 1`
- `GET /odata/NewsArticles?$orderby=CreatedDate desc&$top=10`
- `GET /odata/Categories?$expand=NewsArticles`

## 🛠 Quản lý Docker

### Khởi động containers
```powershell
docker-compose up -d
```

### Dừng containers
```powershell
docker-compose down
```

### Xem logs
```powershell
# Xem logs API
docker logs funews-api

# Xem logs SQL Server
docker logs funews-sqlserver

# Xem logs realtime
docker logs -f funews-api
```

### Rebuild sau khi thay đổi code
```powershell
docker-compose up -d --build
```

### Xem trạng thái containers
```powershell
docker ps
```

### Kết nối vào container
```powershell
# Vào container API
docker exec -it funews-api bash

# Vào container SQL Server
docker exec -it funews-sqlserver bash
```

## 🗄 Kết nối Database

### Từ localhost (máy host)
```
Server=localhost,1433
Database=FUNewsManagement
User ID=sa
Password=YourStrong@Passw0rd
TrustServerCertificate=True
```

### Từ bên trong Docker network
```
Server=sqlserver
Database=FUNewsManagement
User ID=sa
Password=YourStrong@Passw0rd
TrustServerCertificate=True
```

### Sử dụng SQL Server Management Studio (SSMS)
1. Mở SSMS
2. Server name: `localhost,1433`
3. Authentication: SQL Server Authentication
4. Login: `sa`
5. Password: `YourStrong@Passw0rd`

## 🔐 Default Admin Account

Khi database được khởi tạo lần đầu, một admin account sẽ được tạo tự động:
- **Email**: admin@FUNewsManagementSystem.org
- **Password**: @@abc123@@

## 📊 Database Schema

Database gồm các bảng:
- **SystemAccount** - Quản lý user accounts
- **Category** - Quản lý categories
- **NewsArticle** - Quản lý news articles
- **Tag** - Quản lý tags
- **NewsTag** - Liên kết giữa news và tags

## 🔧 Troubleshooting

### API không khởi động?
```powershell
docker logs funews-api
```

### SQL Server không kết nối được?
```powershell
# Kiểm tra SQL Server đã sẵn sàng
docker logs funews-sqlserver

# Restart SQL Server
docker restart funews-sqlserver
```

### Xóa và tạo lại database
```powershell
# Dừng và xóa containers + volumes
docker-compose down -v

# Khởi động lại (database sẽ được tạo mới)
docker-compose up -d
```

## 📝 Test API với curl

### Login
```powershell
curl -X POST http://localhost:8080/api/Auth/login `
  -H "Content-Type: application/json" `
  -d '{\"email\":\"admin@FUNewsManagementSystem.org\",\"password\":\"@@abc123@@\"}'
```

### Get Categories (cần JWT token)
```powershell
$token = "your-jwt-token-here"
curl -X GET http://localhost:8080/api/Categories `
  -H "Authorization: Bearer $token"
```

## 🎯 Next Steps

1. ✅ Truy cập Swagger UI: http://localhost:8080/swagger
2. ✅ Test các API endpoints
3. ✅ Tạo dữ liệu mẫu
4. ✅ Tích hợp với frontend application
5. ✅ Deploy lên production server (nếu cần)

## 📦 Files quan trọng

- `docker-compose.yml` - Docker compose configuration
- `FUNewsManagementSystem/Dockerfile` - API Dockerfile
- `FUNewsManagementSystem/appsettings.json` - API configuration
- `FUNewsManagementSystem/Program.cs` - Application startup

## 🔒 Security Notes (Production)

Khi deploy production, nhớ thay đổi:
- ✅ SQL Server password
- ✅ JWT secret key
- ✅ Admin password
- ✅ Thêm HTTPS
- ✅ Configure firewall rules
- ✅ Sử dụng environment variables cho sensitive data

---

**Chúc mừng! 🎉** Project của bạn đã sẵn sàng sử dụng!
