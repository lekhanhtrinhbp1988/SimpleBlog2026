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
