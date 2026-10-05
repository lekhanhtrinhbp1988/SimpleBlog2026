# Lessons learned: Yêu cầu và quyết định

Acceptance criteria, quyết định nền, ADR.

Mã `REQ-NN`. Cách viết và luật: xem [mục lục](../lessons-learned.md).

### REQ-01. Yêu cầu phải kiểm chứng được

- **Áp dụng:** mỗi yêu cầu có acceptance criteria đánh số (`AC-1`, `AC-2`...). Mỗi AC có ít nhất một test tự động mang đúng mã AC. "Xong" nghĩa là mọi AC có test xanh.

### REQ-02. Quyết định nền trước, nhưng không quyết định mọi thứ ngay

- **Áp dụng:**
  - Chốt trước feature đầu tiên những thứ đổi về sau rất tốn: độc giả và mục tiêu, layout và thang chữ, cấu trúc URL, đa ngôn ngữ, cách lưu dữ liệu, đăng nhập, nơi deploy.
  - Hoãn những thứ rẻ để đổi, nhưng ghi lại thành câu hỏi mở có hạn chót (nguyên tắc Last Responsible Moment).
  - Feature đầu tiên là walking skeleton: một lát cắt mỏng chạy xuyên mọi tầng.

### REQ-03. Ghi lại "tại sao", không chỉ "cái gì"

- **Áp dụng:** quyết định kỹ thuật lớn ghi thành ADR (Architecture Decision Record) trong `docs/adr/`: bối cảnh, các phương án, lựa chọn, hệ quả. Code cho biết hệ thống làm gì; ADR cho biết vì sao nó được làm như vậy. Người mới đọc ADR sẽ không đề xuất lại phương án đã bị loại mà không biết lý do.

### REQ-04. Ghi ADR lúc quyết định, không phải sau

- **Chuyện gì xảy ra:** ADR-0002 đến 0005 được viết bù cho các quyết định đã làm từ vài ngày trước. Lý do còn nhớ được vì mới xảy ra; sau vài tháng sẽ mất.
- **Áp dụng:** quyết định kỹ thuật lớn có ADR trong cùng PR với thay đổi đó (đã có trong Definition of Done). ADR đã chốt không sửa; đổi ý thì viết ADR mới thay thế.

### REQ-05. ADR dựa trên số liệu dịch vụ bên ngoài phải ghi rõ số liệu chưa kiểm và có bước kiểm trước khi dùng

- **Chuyện gì xảy ra:** khi soạn ADR-0008 (database) và ADR-0009 (nơi chạy), agent `architect` không có tool đọc web. Mọi số liệu về gói miễn phí của Azure SQL, Neon, Render (thời gian thức dậy, hạn mức, tên miền riêng) là theo trí nhớ. Chính các số đó dẫn tới quyết định bỏ SQL Server, kéo theo khoảng 15 file phải sửa. Agent đã tự nói rõ điều này, và `architecture.md` có câu hỏi mở "kiểm lại điều khoản gói miễn phí trước lần deploy đầu".
- **Bài học:** điều khoản gói miễn phí đổi thường xuyên; trí nhớ của model có độ trễ (AGT-16). Một quyết định khó đổi mà dựa trên số liệu chưa kiểm thì người duyệt phải được biết, và phải có một thời điểm kiểm cụ thể.
- **Áp dụng:** người điều phối nêu cảnh báo này ở trạm duyệt; câu hỏi mở kiểm điều khoản có hạn chót "trước lần deploy đầu". Việc tiếp theo, qua `/design`: cân nhắc cho `architect` tool `WebFetch` ở chế độ dự án, hoặc thêm bước người điều phối kiểm nguồn của từng ADR trước trạm duyệt.
