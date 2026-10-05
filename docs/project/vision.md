---
status: approved
approved_by: TRINH LE
approved_on: 2026-10-05
---

# Tầm nhìn

## Mục đích

SimpleBlog là blog cá nhân viết bằng ASP.NET Core MVC (.NET 10) của Lê Khánh Trình, phục vụ ba mục đích: chia sẻ kiến thức .NET và lập trình web, ghi chép quá trình học tập cá nhân, và làm hồ sơ nghề nghiệp công khai. Đồng thời, dự án là nơi thử nghiệm quy trình phát triển có agent AI hỗ trợ (BA → Architect → Developer → Reviewer → Tester, người duyệt và merge qua pull request).

**Thứ tự ưu tiên khi xung đột:** thử quy trình agent là chính, blog là phụ. Khi phải chọn giữa "ra bài nhanh hơn" và "làm đúng quy trình", chọn làm đúng quy trình; khi một feature chỉ có giá trị cho blog nhưng làm méo quy trình, hoãn feature đó.

## Độc giả

**Độc giả chính: lập trình viên Việt Nam đang đi làm.**

- Tuổi 20 đến 35, nghề lập trình viên (chủ yếu .NET và web), đọc tiếng Việt.
- Nhu cầu: tìm cách giải một vấn đề kỹ thuật cụ thể, đọc ghi chép ngắn để học thêm, đọc bài suy ngẫm về nghề.
- Hoàn cảnh đọc: trong giờ làm việc, xen giữa các việc khác, thường đến từ công cụ tìm kiếm hoặc liên kết được chia sẻ; hay sao chép đoạn code trong bài sang trình soạn thảo.
- Thiết bị: máy tính (màn hình rộng, bàn phím, chuột). Điện thoại không phải thiết bị chính (giả định: vẫn phải đọc được trên điện thoại, nhưng bố cục tối ưu cho máy tính; `ui-guidelines.md` chốt mức hỗ trợ).
- Có thể đăng nhập để bình luận (xem Phạm vi).

**Độc giả đọc tiếng Anh:** blog có giao diện và bài dịch tiếng Anh (xem Nội dung, Phạm vi). Giả định: đây là lập trình viên cùng độ tuổi, thiết bị và hoàn cảnh đọc như độc giả chính, chỉ khác ngôn ngữ; bản tiếng Anh cũng phục vụ mục đích hồ sơ nghề nghiệp. Tiếng Việt vẫn là ngôn ngữ chính: khi phải chọn, ưu tiên trải nghiệm tiếng Việt.

**Người dùng thứ hai: tác giả** (chủ blog), đăng nhập trang quản trị trên web để soạn, sửa, đăng bài và quản lý bình luận.

Giả định: mục đích "hồ sơ nghề nghiệp" được phục vụ qua chính nội dung bài và trang Giới thiệu; không có chân dung độc giả riêng cho nhà tuyển dụng.

## Thành công

Người dùng chọn ba chỉ số nhưng chưa nêu mức. Các mức dưới đây là **giả định** dựa trên ví dụ đã đưa ra, chờ xác nhận (câu hỏi mở 1). Mốc "6 tháng" tính từ ngày bài đầu tiên được đăng công khai.

| Chỉ số | Mức mong muốn | Khi nào đánh giá |
|---|---|---|
| Số feature trong backlog đi trọn pipeline agent (đủ `01` đến `05`, CI xanh) và được merge | Giả định: 100% feature đã làm đi trọn pipeline, không feature nào merge mà bỏ bước | Mỗi lần merge một feature; tổng kết khi hết backlog đã duyệt |
| Số bài đã đăng công khai | Giả định: 12 bài | 6 tháng sau bài đầu tiên |
| Số người đọc duy nhất mỗi tháng (đo bằng công cụ analytics) | Giả định: 500 người/tháng | Hằng tháng từ khi có analytics; đánh giá mức ở tháng thứ 6 sau bài đầu tiên |

Chỉ số đầu tiên đứng trên cùng vì thử quy trình là mục tiêu chính.

## Nội dung

- **Ai viết:** một tác giả là chủ blog (giả định: chỉ một tác giả, không có người viết khác; câu hỏi mở 2).
- **Ngôn ngữ:** tiếng Việt và tiếng Anh.
  - Giao diện (menu, nút, nhãn, thông báo) có đủ cả hai ngôn ngữ.
  - Mỗi bài viết bằng một ngôn ngữ và **có thể** có bản dịch sang ngôn ngữ kia, do chính tác giả viết (không dịch máy). Bài không bắt buộc có bản dịch.
  - Cách hiển thị khi bài chưa có bản dịch, ngôn ngữ mặc định, và cách tìm kiếm, RSS, thẻ xử lý hai ngôn ngữ là câu hỏi mở 3, 7, 8.
- **Loại bài:**
  - Hướng dẫn kỹ thuật: dài, có nhiều đoạn code.
  - Ghi chép ngắn: ghi lại điều vừa học.
  - Bài dài suy ngẫm: chủ yếu là chữ.
- **Tần suất đăng:** không cố định.
- **Công cụ soạn bài:** trang quản trị trên web, tác giả phải đăng nhập. Định dạng nội dung bài (Markdown, trình soạn thảo trực quan...) để `architecture.md` đề xuất.
- **Nguồn nội dung:** chỉ nội dung tự làm; không dùng ảnh hay nội dung của người khác.

## Phạm vi

Đã có:

- Trang Giới thiệu (feature `about`, PR #1).

Sẽ có:

- Danh sách bài và trang chi tiết bài.
- Thẻ và/hoặc chuyên mục để phân loại bài.
- Tìm kiếm bài.
- RSS.
- Trang quản trị cho tác giả: đăng nhập, soạn, sửa, đăng bài, quản lý bình luận.
- Bình luận dưới bài, **độc giả phải đăng nhập để bình luận**. Đọc bài không cần đăng nhập. Cách đăng nhập (tài khoản riêng của blog hay đăng nhập qua dịch vụ bên ngoài) để `architecture.md` đề xuất.
- Analytics để đo số người đọc duy nhất mỗi tháng.
- Đa ngôn ngữ tiếng Việt và tiếng Anh: giao diện chuyển được giữa hai ngôn ngữ; trang quản trị cho tác giả thêm và sửa bản dịch của từng bài. Cách tổ chức URL theo ngôn ngữ để `architecture.md` đề xuất.

## Ngoài phạm vi

Cố ý không có:

- Bản tin email.
- Quảng cáo.
- Ảnh và nội dung của người khác.
- Dịch máy tự động nội dung bài.
- Ngôn ngữ khác ngoài tiếng Việt và tiếng Anh.

Không có trong bản hiện tại (giả định, có thể mở lại qua PR sửa vision):

- Nhiều tác giả.

## Ràng buộc

- **Ngân sách:** 0 đồng. Chỉ dùng gói miễn phí cho hosting, database, tên miền (nếu có), analytics và mọi dịch vụ bên ngoài. Một phương án cần trả phí là không hợp lệ.
- **Thời gian:** không có hạn ra mắt bài đầu tiên.
- **Nơi chạy:** chưa quyết; `architecture.md` đề xuất trong giới hạn gói miễn phí.
- **Pháp lý:**
  - Bản quyền: chỉ đăng nội dung và ảnh tự làm.
  - Dữ liệu cá nhân: tài khoản độc giả (để bình luận) và analytics đều thu thập dữ liệu cá nhân của độc giả, nên phải tuân thủ quy định bảo vệ dữ liệu cá nhân của Việt Nam (Nghị định 13/2023/NĐ-CP) và nói rõ cho độc giả biết thu thập gì, để làm gì. Cách thể hiện cụ thể là câu hỏi mở 5.
- **Công nghệ đã có:** ASP.NET Core MVC, .NET SDK 10.0.201 (ghim trong `global.json`), SQL Server LocalDB khi phát triển (chỉ chạy trên Windows).
- **Quy trình:** mọi thay đổi đi qua PR vào `main` được bảo vệ, CI trên GitHub Actions Ubuntu, người merge (ADR-0002, ADR-0003, ADR-0004). Feature phải có trong backlog đã duyệt trước khi làm (ADR-0007).

## Rủi ro

| Rủi ro | Cách giảm |
|---|---|
| Quy trình nặng (năm tài liệu nền, pipeline năm bước) làm chậm bài đầu tiên, tác giả mất động lực | Chấp nhận được vì thử quy trình là mục tiêu chính; backlog vẫn ưu tiên đường ngắn nhất tới bài đầu tiên được đăng (walking skeleton trước) |
| Đăng nhập độc giả và bình luận làm phạm vi lớn lên nhiều: quản lý tài khoản, spam, kiểm duyệt, dữ liệu cá nhân | Đặt bình luận và đăng nhập độc giả sau các feature đọc bài trong backlog; chốt cách kiểm duyệt (câu hỏi mở 4) trước khi làm; ưu tiên phương án đăng nhập không phải tự lưu mật khẩu nếu `architecture.md` thấy hợp |
| Gói miễn phí có giới hạn (ngủ khi không có truy cập, dung lượng database nhỏ, không có SQL Server miễn phí lâu dài) làm blog chậm hoặc ngừng chạy | `architecture.md` chọn nơi chạy và database theo ràng buộc 0 đồng, nêu rõ giới hạn của gói đã chọn |
| CI không có LocalDB nên test cần database khó chạy trên CI (ADR-0003) | `architecture.md` quyết định cách lưu dữ liệu và cách test trên CI |
| Tần suất đăng không cố định nên không đạt chỉ số số bài và số người đọc | Theo dõi chỉ số số bài hằng tháng; nếu sau 3 tháng đi chậm hơn mức giả định thì xem lại mức (câu hỏi mở 1) |
| Viết bản dịch tốn gấp đôi công sức, làm chậm việc đăng bài và kéo chỉ số số bài xuống | Bản dịch không bắt buộc; một bài chỉ có một ngôn ngữ vẫn được đăng và được tính vào chỉ số số bài |
| Đa ngôn ngữ làm mọi feature hiển thị (danh sách, chi tiết, thẻ, tìm kiếm, RSS, quản trị) phức tạp hơn và dễ sót chuỗi giao diện chưa dịch | `architecture.md` chốt cách làm đa ngôn ngữ trước feature nội dung đầu tiên; backlog đặt feature hạ tầng ngôn ngữ giao diện sớm để các feature sau dùng chung; mỗi feature có AC cho cả hai ngôn ngữ |
| Người đọc gặp bài chưa có bản dịch, thấy trang trống hoặc lỗi | Chốt cách hiển thị (câu hỏi mở 3) trước feature `post-translations` |
| Analytics và tài khoản độc giả vi phạm quy định dữ liệu cá nhân | Chốt cách thông báo và xin đồng ý (câu hỏi mở 5) trước feature đầu tiên thu thập dữ liệu độc giả |

## Câu hỏi mở

Tên feature ở cột hạn chót là tên dự kiến; `backlog.md` sẽ dùng đúng các tên này.

| Câu hỏi | Hạn chót | Ai trả lời |
|---|---|---|
| 1. Xác nhận hoặc sửa mức của ba chỉ số thành công (giả định: 100% feature đi trọn pipeline; 12 bài và 500 người đọc/tháng sau 6 tháng kể từ bài đầu tiên) | Trước feature `analytics` | Lê Khánh Trình |
| 2. Chỉ một tác giả, hay cần nhiều tài khoản tác giả có quyền khác nhau? (Gợi ý: một tác giả / nhiều tác giả cùng quyền / có vai trò biên tập) | Trước feature `admin-auth` | Lê Khánh Trình |
| 3. Bài chưa có bản dịch thì người đọc ở ngôn ngữ kia thấy gì? (Gợi ý: ẩn bài khỏi danh sách ngôn ngữ kia / hiện bài bằng ngôn ngữ gốc kèm ghi chú "chưa có bản dịch" / hiện trong cả hai danh sách với nhãn ngôn ngữ) | Trước feature `post-translations` | Lê Khánh Trình |
| 4. Bình luận được kiểm duyệt thế nào? (Gợi ý: hiện ngay, tác giả xóa sau / chờ tác giả duyệt mới hiện / hiện ngay với người đã có bình luận được duyệt) | Trước feature `comments` | Lê Khánh Trình |
| 5. Thông báo và xin đồng ý về dữ liệu cá nhân thế nào? (Gợi ý: trang chính sách quyền riêng tư / thêm banner đồng ý cookie / chọn analytics không dùng cookie để khỏi cần banner) | Trước feature `reader-auth` hoặc `analytics`, feature nào làm trước | Lê Khánh Trình |
| 6. Bài có ảnh tự làm (ảnh chụp màn hình, sơ đồ) cần tải lên qua trang quản trị không? (Gợi ý: có / không, chỉ chữ và code / để sau) | Trước feature `post-editor` | Lê Khánh Trình |
| 7. Người đọc vào lần đầu thấy ngôn ngữ nào? (Gợi ý: luôn tiếng Việt, có nút chuyển / theo ngôn ngữ trình duyệt / theo ngôn ngữ trong URL, mặc định tiếng Việt) | Trước feature `ui-localization` | Lê Khánh Trình |
| 8. Tìm kiếm, RSS và thẻ tách theo ngôn ngữ hay dùng chung? (Gợi ý: tách riêng cho từng ngôn ngữ / dùng chung, có nhãn ngôn ngữ / tìm kiếm chung, RSS tách) | Trước feature đầu tiên trong `tags`, `search`, `rss` | Lê Khánh Trình |
