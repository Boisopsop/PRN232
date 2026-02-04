# GitHub Copilot Instructions for PRN232 Project

## 📋 Project Rules

### ❌ DO NOT
- **NEVER create separate markdown files** (.md) to document changes, summaries, or reports
- **NEVER create files like:**
  - `CHANGES.md`
  - `SUMMARY.md`
  - `COMPLIANCE_REPORT.md`
  - `QUICK_START.md`
  - `IMPLEMENTATION_NOTES.md`
  - Any other documentation files

### ✅ DO
- **ALL documentation goes into `README.md`** only
- Make code changes directly
- Update README.md if needed
- Provide concise verbal responses
- Focus on code quality

## 🎯 Project Info

**Course:** PRN232 - LAB 2  
**Architecture:** 3-Layer (API → Service → Repository)  
**Framework:** ASP.NET Core 8.0  
**Database:** SQL Server  
**Auth:** JWT Bearer Token  

## 📦 Project Structure

```
PRN232/
├── FUNewsManagementSystem/     # API Layer (PRN232.FUNewsManagement.API)
├── ServiceLayer/               # Service Layer (PRN232.FUNewsManagement.Services)
└── RepositoryLayer/            # Repository Layer (PRN232.FUNewsManagement.Repo)
```

## 🛠️ Common Commands

```powershell
# Build
dotnet build

# Run
dotnet run --project FUNewsManagementSystem

# Docker
docker-compose up -d
```

## 📝 Naming Conventions

- **Projects:** PRN232.[ProjectName].[Layer]
- **Endpoints:** Resource-based, plural nouns (`/api/news-articles`)
- **Models:** 4 types (Entity, Business, Request, Response)
- **Responses:** Always wrap in `ApiResponse<T>`

## ⚠️ Important Notes

- **NO markdown files** unless explicitly requested by user
- Keep documentation in README.md
- Follow LAB 2 requirements strictly
- Maintain 3-layer architecture
- Never expose Entity models in API responses

---

**This file ensures consistent AI agent behavior across different machines.**
