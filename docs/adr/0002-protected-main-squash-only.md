# 0002. `main` được bảo vệ, chỉ merge qua PR bằng squash

- Trạng thái: Accepted
- Ngày: 2026-10-01
- Người quyết định: Lê Khánh Trình

## Bối cảnh

Repo bắt đầu trên máy cá nhân với nhánh `master`, merge thẳng trên máy. Kinh nghiệm từ dự án cũ: branch để lại tùm lum, không rõ branch nào đã merge. Dự án sẽ có thêm người và agent cùng đẩy code, nên cần một điểm kiểm soát chung mà không phụ thuộc vào việc ai đó nhớ luật.

Pipeline `/feature` cố ý tạo nhiều commit nhỏ (requirements, design, feat) để reviewer phát hiện việc sửa lén tài liệu đã duyệt. Các commit này hữu ích trong lúc làm, nhưng làm rối lịch sử `main`.

## Quyết định

- Nhánh chính tên `main`, host tại GitHub, repo public.
- Bảo vệ `main`: bắt buộc qua PR; CI `build-and-test` phải xanh; branch phải cập nhật theo `main` trước khi merge; cấm force push và xóa; áp dụng cả cho admin.
- Chỉ cho phép **Squash and merge**. Commit squash lấy tiêu đề và mô tả PR, nên tiêu đề PR theo Conventional Commits.
- GitHub tự xóa branch sau khi merge.
- Số người duyệt bắt buộc: 0 khi còn một người làm (không tự duyệt PR của mình được); nâng lên 1 khi có người thứ hai.

## Phương án đã cân nhắc

- **Merge commit**: giữ đủ lịch sử từng bước, nhưng `main` lẫn commit trung gian và đồ thị rẽ nhánh.
- **Rebase and merge**: lịch sử thẳng, nhưng vẫn mang theo commit trung gian; mã commit đổi so với branch.
- **Giữ `master`, merge trên máy**: không có điểm kiểm soát chung; không có CI bắt buộc.

## Hệ quả

- `main` mỗi dòng là một PR; gỡ một feature bằng một lệnh `git revert`.
- Chi tiết từng bước vẫn xem được trên trang PR.
- Luật "phải cập nhật theo `main`" làm PR merge sau phải bấm Update branch thêm một lần. Khi nhiều người làm song song mà thấy phiền, cân nhắc bật merge queue.
- Không làm tiếp trên branch đã squash.
