# Lessons learned: Git và GitHub

Branch, commit, pull request, bảo vệ nhánh, đặt tên.

Mã `GIT-NN`. Cách viết và luật: xem [mục lục](../lessons-learned.md).

### GIT-01. Branch sống ngắn và bị xóa sau khi merge

- **Thói quen cũ:** branch để lại tùm lum sau khi xong việc.
- **Vì sao có hại:** không ai biết branch nào còn dùng, branch nào đã merge, branch nào bỏ dở; dễ làm tiếp trên branch cũ rồi xung đột.
- **Áp dụng:** branch đặt tên theo loại (`feature/`, `chore/`, `fix/`), sống vài ngày, merge bằng squash, rồi xóa. GitHub tự xóa branch trên server sau merge; `/sync` xóa branch trên máy. Không làm tiếp trên branch đã squash.

### GIT-02. `main` được bảo vệ, mọi thay đổi đi qua PR và CI

- **Áp dụng:** không ai push thẳng `main`, kể cả chủ repo. CI phải xanh, branch phải cập nhật theo `main` trước khi merge. "Chạy được trên máy tôi" không phải bằng chứng; CI mới là bằng chứng.

### GIT-03. Message commit có cấu trúc

- **Áp dụng:** Conventional Commits (`feat`, `fix`, `docs`, `chore`, `ci`, `refactor`, `test`). Với squash merge, tiêu đề PR trở thành commit trên `main`, nên tiêu đề PR phải theo quy ước. Lợi ích: đọc `git log` là biết loại thay đổi, sinh được changelog tự động.

### GIT-04. Đặt quy ước đặt tên từ đầu

- **Chuyện gì xảy ra:** feature đầu tiên tên `gioi-thieu`, sau phải đổi thành `about` ở branch, thư mục tài liệu, class test và namespace.
- **Áp dụng:** tên branch, thư mục và code dùng tiếng Anh, kebab-case cho branch và thư mục. Nội dung hiển thị cho người đọc vẫn là tiếng Việt.
