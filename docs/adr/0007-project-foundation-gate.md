# 0007. Giai đoạn khởi động dự án và cổng chặn trước khi làm feature

- Trạng thái: Accepted
- Ngày: 2026-10-02
- Người quyết định: Lê Khánh Trình

## Bối cảnh

Pipeline `/feature` ([ADR-0004](0004-agent-pipeline-with-human-gates.md)) chỉ có tầng feature. Feature đầu tiên (trang Giới thiệu) được làm khi chưa có quyết định nào về độc giả, giao diện, độ rộng trang, kiến trúc hay thứ tự ưu tiên. BA phải tự giả định ("cách trình bày do bước sau quyết định") và giả định đó đi qua trạm duyệt (lessons learned AGT-01, AGT-02).

Với một dự án mới (greenfield), các quyết định nền phải có trước feature, và phải có thứ gì đó **bắt buộc** chúng tồn tại, không dựa vào việc người hay agent tự nhớ (AGT-11).

## Quyết định

1. **Năm tài liệu nền** trong `docs/project/`: `vision.md`, `nfr.md`, `ui-guidelines.md`, `architecture.md`, `backlog.md`. Mỗi file có phần đầu `status: draft | approved`, `approved_by`, `approved_on`. Chỉ người đổi được thành `approved`.
2. **Skill `/init-project`** điều phối việc soạn năm file theo thứ tự phụ thuộc, có trạm duyệt sau mỗi file. Hai agent mới (`product`, `ux`) và agent `architect` mở rộng thêm chế độ dự án.
3. **Cổng chặn ba lớp:**
   - `/feature` dừng ở bước 0 nếu năm file chưa cùng `approved`.
   - Agent dừng (`BLOCKED`) khi feature không có trong backlog, hoặc cần quyết định chưa có.
   - CI đỏ khi PR thêm hoặc sửa `docs/features/**` mà năm file chưa cùng `approved`. Lớp này chạy trên server nên không ai bỏ qua được.
4. **Feature đầu tiên trong backlog là walking skeleton:** layout chung theo `ui-guidelines.md`, CSS nền, `public partial class Program`.

Chi tiết: [docs/design/project-foundation.md](../design/project-foundation.md).

## Phương án đã cân nhắc

- **Chỉ viết tài liệu, không có cổng chặn:** rẻ, nhưng chính dự án này đã cho thấy luật chỉ nằm trong tài liệu sẽ bị bỏ qua (AGT-11).
- **Cổng chặn chỉ trong `/feature`:** đủ cho agent, nhưng người hoặc công cụ khác có thể tạo `docs/features/**` mà không qua skill. CI chặn được cả hai.
- **Người tự viết năm file, không có agent:** chủ động hơn, nhưng chậm và dễ thiếu mục. Agent soạn nháp kèm câu hỏi, người trả lời và duyệt.
- **Quyết định mọi thứ ngay từ đầu:** an toàn nhưng chậm và nhiều quyết định sẽ sai vì thiếu thông tin. Thay vào đó: chốt những gì đổi về sau rất tốn, còn lại ghi thành câu hỏi mở có hạn chót (Last Responsible Moment).

## Hệ quả

- Không thể làm feature mới cho tới khi chạy xong `/init-project`. Việc `chore`, `fix` và tài liệu không bị chặn.
- Feature `about` đã có từ trước; sửa nó sau khi cổng chặn có hiệu lực cũng phải chờ năm file được duyệt.
- Thêm việc cho người ở giai đoạn đầu: trả lời phỏng vấn và duyệt năm file.
- Sửa một file nền đã `approved` sau này đi qua PR như mọi thay đổi khác, và phải cập nhật `approved_on`.
