# 0008. Nơi lưu bài viết và dữ liệu: database nào

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (chọn phương án B ngày 2026-10-05, chờ duyệt để chuyển Accepted)

## Bối cảnh

- Vision cần lưu: bài, bản dịch, thẻ, bình luận, tài khoản độc giả. Tác giả soạn bài trên trang quản trị web, nên nội dung phải ghi được lúc chạy. Phương án "bài là file Markdown trong repo, đăng qua PR" bị loại: trái với vision (soạn trên web) và vẫn cần database cho bình luận, tài khoản.
- Ngân sách 0 đồng (vision). `nfr.md`: trang đầu tiên sau khi server ngủ trả về trong ≤ 15 s; sao lưu hằng ngày, bản sao lưu nằm ngoài nơi chạy database.
- Hiện trạng: project đã có package `Microsoft.EntityFrameworkCore.SqlServer`, dev dùng SQL Server LocalDB (chỉ Windows), CI chạy Ubuntu không có LocalDB (ADR-0003). Chưa có DbContext, entity hay migration nào, nên đổi loại database lúc này gần như không tốn gì.
- Dữ liệu nhỏ: blog một tác giả, chủ yếu chữ; vài chục MB trong nhiều năm.

## Quyết định

Chọn **Phương án B: PostgreSQL trên Neon, gói Free, vùng Singapore**, truy cập qua EF Core với provider Npgsql (`Npgsql.EntityFrameworkCore.PostgreSQL`).

- Production: database Neon; connection string chỉ nằm trong cấu hình nơi deploy (ADR-0009), không nằm trong repo.
- Máy dev: PostgreSQL cài cục bộ trên Windows, `localhost`, database `simpleblog`; acceptance test dùng database riêng `simpleblog_test`, không bao giờ dùng `simpleblog`.
- Bỏ SQL Server LocalDB và package `Microsoft.EntityFrameworkCore.SqlServer`.

Lý do: phương án A rẻ công nhất nhưng database miễn phí của Azure ngủ và thức dậy chậm, lại có hạn mức giờ chạy mỗi tháng; với lượt truy cập rải rác của blog, nó vừa có nguy cơ trượt mức ≤ 15 s của `nfr.md`, vừa có nguy cơ hết hạn mức giữa tháng (blog ngừng tới tháng sau). B thức dậy trong khoảng một giây và không có kiểu "hết hạn mức thì tắt cả tháng" với mức dùng của blog này. Cái giá là đổi provider EF và máy dev cài PostgreSQL thay LocalDB, làm ngay bây giờ khi chưa có dòng code database nào.

## Phương án đã cân nhắc

- **A. Azure SQL Database, gói miễn phí (free offer).** Giữ SQL Server: không đổi package, dev vẫn dùng LocalDB, luật database trong `CLAUDE.md` giữ nguyên. Nhược: database kiểu serverless tự ngủ khi vắng, thức dậy có thể mất cỡ một phút; hạn mức khoảng 100 000 vCore-giây mỗi tháng, mỗi lần thức chạy tối thiểu tới hết thời gian chờ ngủ, nên lượt đọc rải rác trong ngày tiêu hạn mức nhanh; hết hạn mức thì phải chọn tạm dừng tới tháng sau (blog chết) hoặc trả tiền (trái ngân sách). Cần tài khoản Azure có thẻ để xác minh.
- **B. PostgreSQL trên Neon, gói Free (đã chọn).** Thức dậy nhanh (cỡ dưới một giây đến vài giây), 0,5 GB dung lượng, không cần thẻ, có vùng Singapore gần Việt Nam. EF Core dùng provider Npgsql (package `Npgsql.EntityFrameworkCore.PostgreSQL`). Nhược: đổi provider; dev cài PostgreSQL trên Windows (miễn phí) thay LocalDB; phải sửa `README.md`, `CLAUDE.md` và luật database trong các agent (`.claude/`, qua PR người duyệt); hạn mức giờ chạy tính toán mỗi tháng, cần theo dõi.
- **C. SQLite, một file trên đĩa của server.** Đơn giản nhất, không cần dịch vụ database, CI không cần database server. Nhược: gói hosting miễn phí hoặc không có đĩa bền (dữ liệu mất khi khởi động lại), hoặc đĩa là ổ mạng mà tài liệu SQLite khuyên không dùng vì lỗi khóa file; cũng phải đổi provider.
- **Hosting .NET miễn phí kèm SQL Server (nhà cung cấp nhỏ dạng shared hosting):** giữ được SQL Server, nhưng điều khoản và độ bền của nhà cung cấp chưa kiểm chứng được, hỗ trợ .NET 10 không rõ. Không đưa vào lựa chọn.

## Đối chiếu hướng dẫn

Nguồn cần mở lại trước lần deploy đầu tiên, vì điều khoản gói miễn phí hay đổi (lần soạn này không mở trực tiếp được, số liệu trên là theo hiểu biết của agent và có thể đã cũ):

- Azure SQL free offer: https://learn.microsoft.com/azure/azure-sql/database/free-offer và serverless auto-pause: https://learn.microsoft.com/azure/azure-sql/database/serverless-tier-overview
- Neon Free plan: https://neon.com/pricing và scale to zero: https://neon.com/docs/introduction/scale-to-zero
- EF Core provider Npgsql: https://www.npgsql.org/efcore/
- SQLite trên ổ mạng: https://www.sqlite.org/useovernet.html

Quyết định theo đúng `nfr.md` (khởi động lại ≤ 15 s) và ràng buộc 0 đồng của vision.

## Hệ quả

- Một PR `chore` trước hoặc trong walking skeleton: thay package SqlServer bằng Npgsql; đổi `DefaultConnection` trong `appsettings.Development.json` sang PostgreSQL cục bộ (`localhost`, database `simpleblog`); sửa `README.md`, `CLAUDE.md`, `SECURITY.md`, `docs/glossary.md` và luật database trong các agent `.claude/agents/` (qua PR, người duyệt từng chỗ sửa) từ `(localdb)\MSSQLLocalDB` / `SimpleBlog_Test` sang PostgreSQL `localhost` / `simpleblog_test`.
- Máy dev phải cài PostgreSQL (miễn phí); connection string dev có mật khẩu nên đặt bằng user-secrets, không ghi vào `appsettings.Development.json` (`SECURITY.md`).
- ADR-0012 (database trên CI) dùng image `postgres`.
- Theo dõi hạn mức giờ tính toán và dung lượng của Neon từ lần deploy đầu.
- Sao lưu hằng ngày ra ngoài nơi chạy database (`nfr.md`) là việc riêng, xem mục "Chưa quyết" của `architecture.md`.
- Xem lại khi: gói miễn phí đổi điều khoản, dữ liệu vượt 50% hạn mức dung lượng, hoặc đo thời gian thức dậy vượt 15 s.
