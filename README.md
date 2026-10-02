# SimpleBlog

Blog cá nhân viết bằng ASP.NET Core MVC (.NET 10). Dự án cũng là nơi thử nghiệm quy trình phát triển có agent AI hỗ trợ: mỗi feature đi qua các bước BA → Architect → Developer → Reviewer → Tester, rồi được người duyệt và merge qua pull request.

## Yêu cầu

- **.NET SDK 10.0.201** đúng bản này. `global.json` ghim SDK, không cho dùng bản khác.
- **SQL Server LocalDB** (đi kèm Visual Studio, hoặc cài riêng). Chỉ chạy trên Windows. Hiện chưa có feature nào dùng database.
- **Git**. Tùy chọn: **GitHub CLI** (`gh`) nếu dùng các skill `/feature`, `/sync` của Claude Code.

## Chạy và test

Chạy từ thư mục gốc repo; `dotnet` tự tìm `SimpleBlog.slnx`.

```powershell
dotnet build
dotnet test
dotnet test --filter "FullyQualifiedName~AboutAcceptanceTests"            # một class
dotnet run --project src/SimpleBlog.Web                                    # http://localhost:5227
dotnet run --project src/SimpleBlog.Web --launch-profile https             # https://localhost:7276
dotnet format                                                              # sửa định dạng theo .editorconfig
```

Khi app đang chạy, bản build Debug bị khóa file. Muốn chạy test song song thì dùng `dotnet test -c Release`.

EF Core CLI là local tool, ghim trong `dotnet-tools.json` ở thư mục gốc:

```powershell
dotnet tool restore
dotnet ef migrations add <Name> --project src/SimpleBlog.Web
dotnet ef database update --project src/SimpleBlog.Web
```

Các lệnh `ef` sẽ lỗi cho tới khi có DbContext được đăng ký trong `Program.cs`.

## Cấu trúc

```
src/SimpleBlog.Web/        ASP.NET Core MVC, minimal hosting, route mặc định {controller=Home}/{action=Index}/{id?}
tests/SimpleBlog.Tests/    xUnit v2; Unit/ cho unit test, Acceptance/<Feature>/ cho acceptance test theo AC
docs/project/              Tài liệu nền đã duyệt: vision, nfr, ui-guidelines, architecture, backlog (khung trong _template/)
docs/design/               Thiết kế chi tiết của các thay đổi lớn về quy trình hoặc hệ thống
docs/features/<feature>/   01-requirements → 05-test-report của từng feature
docs/adr/                  Architecture Decision Records: vì sao hệ thống được làm như vậy
docs/lessons-learned/      Bài học rút ra khi làm dự án, mỗi file một chủ đề (mục lục: docs/lessons-learned.md)
.claude/                   Agent, skill và quyền của Claude Code
.github/                   CI và mẫu pull request
```

## Kỹ thuật

- `net10.0`, bật nullable reference types và implicit usings.
- Solution dùng định dạng `.slnx` (mặc định của SDK 10), không phải `.sln`.
- Test dùng xUnit v2 (`xunit` 2.9.3), có `Microsoft.AspNetCore.Mvc.Testing` để viết integration test bằng `WebApplicationFactory<Program>`.
- Connection string `DefaultConnection` chỉ có trong `appsettings.Development.json`, trỏ tới `(localdb)\MSSQLLocalDB`, database `SimpleBlog`. `appsettings.json` không có connection string. Xem [SECURITY.md](SECURITY.md) về cách giữ secret.

## Tài liệu

| Muốn biết | Đọc |
|---|---|
| Quy trình đóng góp: branch, commit, PR, Definition of Done | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Vì sao một quyết định kỹ thuật được chọn | [docs/adr/](docs/adr/) |
| Secret để ở đâu, báo lỗ hổng thế nào | [SECURITY.md](SECURITY.md) |
| Bài học đã rút ra | [docs/lessons-learned.md](docs/lessons-learned.md) |
| Hướng dẫn riêng cho agent Claude Code | [CLAUDE.md](CLAUDE.md) |
