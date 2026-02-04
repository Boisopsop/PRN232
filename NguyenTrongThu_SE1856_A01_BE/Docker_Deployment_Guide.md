# FU News Management API - Docker Deployment Guide

## Yêu Cầu (Prerequisites)

- Docker Desktop đã cài đặt
- Docker Compose đã cài đặt
- Ít nhất 4GB RAM cho Docker

## Cấu Trúc Docker Files

```
NguyenTrongThu_SE1856_A01_BE/
├── Dockerfile              # Build image cho API
├── docker-compose.yml      # Orchestration cho API + SQL Server
├── .dockerignore           # Exclude files từ build context
└── Database/
    └── init.sql            # Script khởi tạo database
```

## Hướng Dẫn Deploy

### 1. Build và Run Containers

```bash
# Di chuyển đến thư mục project
cd d:\tutor\NguyenTrongThu_SE1856_A01_BE

# Build và start containers
docker-compose up -d --build

# Xem logs
docker-compose logs -f api
```

### 2. Kiểm Tra Containers

```bash
# Xem status containers
docker-compose ps

# Expected output:
# NAME               STATUS    PORTS
# funews-api         Up        0.0.0.0:5000->8080/tcp
# funews-sqlserver   Up        0.0.0.0:1433->1433/tcp
```

### 3. Truy Cập API

- **Swagger UI:** http://localhost:5000/swagger
- **API Base URL:** http://localhost:5000/api

### 4. Khởi Tạo Database

Sau khi containers chạy, chạy script init.sql:

```bash
# Copy script vào container
docker cp Database/init.sql funews-sqlserver:/tmp/

# Execute script
docker exec -it funews-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -i /tmp/init.sql
```

Hoặc dùng SSMS/Azure Data Studio kết nối:
- **Server:** localhost,1433
- **User:** sa
- **Password:** YourStrong@Passw0rd

### 5. Stop Containers

```bash
# Stop containers
docker-compose down

# Stop và xóa volumes (reset data)
docker-compose down -v
```

## Thông Tin Đăng Nhập

### Admin Account (Config)
- **Email:** admin@FUNewsManagementSystem.org
- **Password:** @@abc123@@

### Staff Account (Database)
- **Email:** staff@funews.com
- **Password:** Staff@123

### Lecturer Account (Database)
- **Email:** lecturer@funews.com
- **Password:** Lecturer@123

## Environment Variables

| Variable | Mô tả | Default |
|----------|-------|---------|
| `ConnectionStrings__DefaultConnection` | Connection string | SQL Server container |
| `JwtSettings__SecretKey` | JWT Secret Key | YourSuperSecretKeyForJWTToken123456789 |
| `JwtSettings__Issuer` | JWT Issuer | FUNewsManagement |
| `JwtSettings__Audience` | JWT Audience | FUNewsManagementUsers |
| `JwtSettings__ExpiryMinutes` | Token expiry | 60 |
| `AdminAccount__Email` | Admin email | admin@FUNewsManagementSystem.org |
| `AdminAccount__Password` | Admin password | @@abc123@@ |

## Troubleshooting

### API không kết nối được SQL Server

```bash
# Kiểm tra SQL Server health
docker-compose logs sqlserver

# Đợi SQL Server khởi động (30-60s)
docker-compose up -d
sleep 60
docker-compose logs api
```

### Port đã bị sử dụng

```bash
# Đổi port trong docker-compose.yml
ports:
  - "5001:8080"  # Đổi 5000 thành 5001
```

### Reset hoàn toàn

```bash
# Xóa tất cả containers và volumes
docker-compose down -v

# Xóa images
docker rmi funews-api

# Build lại
docker-compose up -d --build
```
