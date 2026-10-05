# 0012. Database cho test trên CI

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (chọn phương án A ngày 2026-10-05, chờ duyệt để chuyển Accepted)
- Supersedes: phần "Việc còn mở" trong ADR-0003 (phần còn lại của ADR-0003 giữ nguyên)

## Bối cảnh

- CI chạy trên runner Ubuntu (ADR-0003), không có SQL Server LocalDB. ADR-0003 để mở việc chọn cách chạy test cần database trước feature đầu tiên dùng database.
- Acceptance test chạy app thật qua `WebApplicationFactory`, đổi connection string sang database test riêng (`simpleblog_test` theo ADR-0008), không bao giờ dùng database dev.
- Loại database theo ADR-0008: PostgreSQL.
- Tài liệu EF Core khuyên test trên cùng loại database với production; provider in-memory không phải database quan hệ và cho kết quả khác.

## Quyết định

Chọn **Phương án A: PostgreSQL chạy trong container cạnh job CI** (GitHub Actions service container, image `postgres` cùng phiên bản chính với Neon), trên runner Ubuntu. Connection string của database test trên CI (database `simpleblog_test` trong container) đặt bằng biến môi trường trong file workflow; trên máy dev giữ giá trị mặc định trỏ tới `simpleblog_test` trên PostgreSQL cục bộ.

Lý do: giữ runner Ubuntu nhanh và miễn phí, test chạy trên đúng loại database của production, không thêm package.

## Phương án đã cân nhắc

- **A. Service container trên runner Ubuntu (đã chọn).** Ưu: như trên; cấu hình vài dòng YAML; container PostgreSQL khởi động trong vài giây. Nhược: máy dev và CI khác cách chạy database (cài sẵn và container).
- **B. Chuyển job test sang runner Windows có LocalDB.** Không còn áp dụng vì ADR-0008 chọn PostgreSQL. Ưu: giống hệt máy dev. Nhược: chậm hơn rõ (khởi động runner, cài LocalDB); Playwright và Lighthouse trên Windows chậm hơn.
- **C. Testcontainers: test tự bật container database.** Ưu: dev và CI chạy giống nhau. Nhược: thêm package; máy dev phải có Docker đang chạy; test chậm hơn mỗi lần chạy.
- **Provider in-memory hoặc SQLite thay database thật khi test:** nhanh nhưng khác hành vi database production (tài liệu EF Core khuyên tránh). Loại.

## Đối chiếu hướng dẫn

- EF Core, chọn chiến lược test: https://learn.microsoft.com/ef/core/testing/choosing-a-testing-strategy (khuyên test trên database thật cùng loại production; theo đúng)
- GitHub Actions service containers: https://docs.github.com/actions/use-cases-and-examples/using-containerized-services/about-service-containers
- PostgreSQL service container: https://docs.github.com/actions/use-cases-and-examples/using-containerized-services/creating-postgresql-service-containers

## Hệ quả

- Fixture acceptance test đọc connection string test từ biến môi trường nếu có, mặc định trỏ database test cục bộ. Chỉ workflow CI đặt biến này. Luật cho agent giữ nguyên tinh thần (chỉ dùng database cục bộ, test chỉ dùng database test, không truyền connection string trỏ nơi khác) nhưng đổi tên: PostgreSQL `localhost`, database `simpleblog_test`, thay cho `(localdb)\MSSQLLocalDB` và `SimpleBlog_Test` (sửa `.claude/agents/` theo ADR-0008).
- Thêm khối `services: postgres` và biến môi trường connection string vào job test của `.github/workflows/ci.yml`.
- Đăng ký DbContext phải đọc connection string lúc tạo DbContext (overload `AddDbContext<T>((sp, options) => ...)`) để fixture ghi đè được.
- Khi ADR này được chấp nhận, ADR-0003 thêm dòng "Phần việc còn mở được trả lời bởi ADR-0012" (chỉ đổi trạng thái, không sửa nội dung).
- Xem lại khi: thời gian CI vượt 5 phút vì database, hoặc ADR-0008 đổi loại database.
