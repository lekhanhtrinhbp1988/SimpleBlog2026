# 0009. Nơi deploy web app

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (chọn phương án A ngày 2026-10-05, chờ duyệt để chuyển Accepted)

## Bối cảnh

- Ngân sách 0 đồng; chấp nhận server ngủ khi vắng, nhưng trang đầu tiên sau khi thức phải về trong ≤ 15 s (`nfr.md`). Chỉ HTTPS, HSTS ở production. Chuyển được sang nơi khác trong ≤ 7 ngày.
- App là ASP.NET Core MVC .NET 10, một process, không cần dịch vụ chạy nền.
- Độc giả chính ở Việt Nam: vùng Đông Nam Á (Singapore) cho độ trễ thấp nhất; database đề xuất ở ADR-0008 cũng ở Singapore.
- Deploy sẽ chạy từ GitHub Actions khi merge vào `main` (ADR-0003), không deploy tay từ máy dev.

## Quyết định

Chọn **Phương án A: Azure App Service gói Free F1, vùng Southeast Asia**, deploy bằng GitHub Actions khi push lên `main`, đăng nhập Azure bằng OIDC (không lưu mật khẩu Azure trong GitHub). Secret production (connection string Neon, client secret GitHub OAuth) chỉ nằm trong app settings của App Service.

Lý do: chạy thẳng app .NET không cần Docker, là nơi Microsoft hỗ trợ chính cho ASP.NET Core, có HTTPS sẵn trên tên miền `*.azurewebsites.net`. Hạn mức CPU mỗi ngày đủ cho blog ít người đọc. Phải đo thời gian thức dậy ngay lần deploy đầu; nếu vượt 15 s thì chuyển sang C (cùng nhà cung cấp, đổi ít).

## Phương án đã cân nhắc

- **A. Azure App Service, Free F1 (đã chọn).** Ưu: không cần container; deploy bằng action chính thức; HTTPS sẵn; cấu hình secret (connection string, khóa OAuth) bằng app settings. Nhược: hạn mức 60 phút CPU mỗi ngày; ngủ sau khoảng 20 phút vắng; không gắn được tên miền riêng ở gói F1; cần tài khoản Azure có thẻ để xác minh (đặt cảnh báo chi phí 0 để tránh bị tính tiền nhầm).
- **B. Render, Free web service (Docker).** Ưu: không cần thẻ; gắn tên miền riêng có HTTPS miễn phí. Nhược: phải viết Dockerfile; ngủ sau 15 phút vắng và tài liệu của Render nói thức dậy có thể mất tới khoảng một phút, trượt mức 15 s của `nfr.md`.
- **C. Azure Container Apps, gói tiêu dùng (consumption) có hạn mức miễn phí hằng tháng.** Ưu: co về 0 khi vắng, thức thường trong vài giây; gắn tên miền riêng với chứng chỉ miễn phí. Nhược: phải viết Dockerfile và đẩy image lên GitHub Container Registry; cấu hình nhiều hơn; phải tắt Log Analytics để không phát sinh phí; cũng cần tài khoản Azure có thẻ.

## Đối chiếu hướng dẫn

Nguồn cần mở lại trước lần deploy đầu tiên (hạn mức gói miễn phí hay đổi; số liệu trên theo hiểu biết của agent):

- App Service pricing và giới hạn F1: https://azure.microsoft.com/pricing/details/app-service/
- Deploy từ GitHub Actions bằng OIDC: https://learn.microsoft.com/azure/app-service/deploy-github-actions
- Container Apps billing (hạn mức miễn phí): https://learn.microsoft.com/azure/container-apps/billing
- Render free instances: https://render.com/docs/free

## Hệ quả

- Thêm workflow deploy (job riêng, chạy khi push lên `main`, sau khi CI xanh). Secret production chỉ nằm trong app settings của Azure, không nằm trong repo (`SECURITY.md`).
- Phải đo thời gian thức dậy theo `nfr.md` sau lần deploy đầu và ghi vào `architecture.md`.
- Tên miền riêng chưa có: blog chạy trên `*.azurewebsites.net`. Đổi tên miền về sau làm đổi URL công khai (cần 301), nên nếu muốn tên miền riêng thì nên quyết trước khi đăng bài đầu tiên (xem "Chưa quyết").
- Kế hoạch chuyển nơi chạy (≤ 7 ngày): app không phụ thuộc dịch vụ riêng của Azure ngoài nơi chạy; chuyển sang C hoặc B chỉ cần thêm Dockerfile và đổi workflow deploy.
