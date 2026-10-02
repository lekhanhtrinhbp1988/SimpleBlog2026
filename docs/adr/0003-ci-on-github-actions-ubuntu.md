# 0003. CI trên GitHub Actions, runner Ubuntu

- Trạng thái: Accepted
- Ngày: 2026-10-01
- Người quyết định: Lê Khánh Trình

## Bối cảnh

Cần một nơi build và test độc lập với máy cá nhân, làm điều kiện bắt buộc để merge vào `main` (xem [ADR-0002](0002-protected-main-squash-only.md)). Repo đã ở GitHub. Hiện chưa có feature nào dùng database. Môi trường dev dùng SQL Server LocalDB, vốn chỉ chạy trên Windows.

## Quyết định

- CI chạy bằng GitHub Actions (`.github/workflows/ci.yml`), job `build-and-test`, runner `ubuntu-latest`.
- Các bước: `dotnet format --verify-no-changes`, `dotnet build -c Release`, `dotnet test -c Release`.
- SDK lấy từ `global.json` để CI và máy dev cùng phiên bản.
- Chạy cho mọi PR vào `main` và mọi push lên `main`.

## Phương án đã cân nhắc

- **Runner Windows**: có sẵn LocalDB nên test database chạy giống máy dev, nhưng chậm hơn và tốn phút CI hơn. Chưa cần khi chưa có database.
- **Dịch vụ CI khác** (Azure Pipelines, Jenkins): thêm hệ thống phải quản lý, không lợi gì hơn khi repo đã ở GitHub.

## Hệ quả

- CI nhanh (khoảng 40 giây) và miễn phí cho repo public.
- **Việc còn mở:** CI không có LocalDB. Trước feature đầu tiên dùng database phải chọn: SQL Server chạy trong container trên Ubuntu (đề xuất), hoặc chuyển job sang runner Windows. Quyết định đó sẽ là một ADR mới thay thế phần liên quan của ADR này.
