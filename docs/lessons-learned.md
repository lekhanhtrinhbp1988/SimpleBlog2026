# Lessons learned

Những bài học rút ra khi dựng SimpleBlog và pipeline agent của nó. Bài học nằm trong thư mục [lessons-learned/](lessons-learned/), mỗi file một chủ đề. File này là mục lục và luật viết.

## Chủ đề

| Mã | File | Nói về |
|---|---|---|
| `AGT` | [agent-workflow.md](lessons-learned/agent-workflow.md) | Pipeline agent: vai trò, đầu vào, quyền, điểm duyệt của người, cách agent hiểu sai |
| `GIT` | [git-and-github.md](lessons-learned/git-and-github.md) | Branch, commit, pull request, bảo vệ nhánh, đặt tên |
| `SEC` | [security.md](lessons-learned/security.md) | Secret, rà soát trước khi public, các lớp bảo vệ trên GitHub |
| `BLD` | [build-test-and-environment.md](lessons-learned/build-test-and-environment.md) | Build tái lập được, chênh lệch môi trường, encoding, analyzer, code dễ test |
| `REQ` | [requirements-and-decisions.md](lessons-learned/requirements-and-decisions.md) | Acceptance criteria, quyết định nền, ADR |
| `PRC` | [process-and-documentation.md](lessons-learned/process-and-documentation.md) | Retro, tài liệu cho người và cho agent, mức trưởng thành của quy trình |

## Khi nào thêm bài học

Khi một việc sinh ra điều mà lần sau nên làm khác: lỗi bất ngờ, cách lách, đề xuất bị người sửa lại, bước quy trình còn thiếu, thói quen cũ hóa ra có hại. Thêm trong cùng PR với việc đó (xem Definition of Done trong [CONTRIBUTING.md](../CONTRIBUTING.md)).

## Cách viết

1. Chọn file chủ đề phù hợp nhất. Bài học thuộc nhiều chủ đề thì chọn chủ đề của **hành động cần làm khác đi**, rồi nhắc mã của nó ở file kia nếu cần.
2. Thêm vào **cuối** file, lấy số tiếp theo của mã đó: `### AGT-15. <Bài học, viết thành một câu khẳng định>`.
3. Nội dung gồm các gạch đầu dòng:
   - **Chuyện gì xảy ra:** sự việc cụ thể trong dự án này (không bắt buộc với bài học chung).
   - **Bài học:** điều rút ra, áp dụng được ngoài tình huống đó.
   - **Áp dụng:** đã hoặc sẽ thay đổi gì trong repo (file, luật, bước quy trình).
4. Không có chủ đề nào hợp: tạo file mới với mã 3 chữ cái mới, rồi thêm một dòng vào bảng trên.

## Luật

- **Mã không bao giờ đổi hay dùng lại.** Tài liệu khác (ADR, PR, mục khác) trích dẫn bằng mã.
- **Không sửa mục cũ cho "đẹp".** Bài học cũ hóa ra sai thì thêm mục mới, ghi "thay cho `XXX-NN`", và thêm dòng `- **Đã thay bằng:** XXX-MM` vào cuối mục cũ.
- Một file vượt khoảng 20 mục thì tách thành chủ đề hẹp hơn. Mục chuyển sang file mới **giữ nguyên mã**.

## Mã cũ

Trước ngày 2026-10-02, bài học nằm chung trong file này, đánh mã A1–A18 (dựng agent workflow) và B1–B17 (SDLC). Tài liệu cũ, như [ADR-0004](adr/0004-agent-pipeline-with-human-gates.md), còn trích dẫn mã cũ.

| Cũ | Mới | | Cũ | Mới | | Cũ | Mới |
|---|---|---|---|---|---|---|---|
| A1 | AGT-01 | | A13 | BLD-04 | | B7 | REQ-03 |
| A2 | AGT-02 | | A14 | AGT-11 | | B8 | BLD-01 |
| A3 | AGT-03 | | A15 | AGT-12 | | B9 | BLD-02 |
| A4 | AGT-04 | | A16 | AGT-13 | | B10 | SEC-02 |
| A5 | AGT-05 | | A17 | BLD-07 | | B11 | BLD-05 |
| A6 | AGT-06 | | A18 | AGT-14 | | B12 | PRC-01 |
| A7 | AGT-07 | | B1 | GIT-01 | | B13 | PRC-02 |
| A8 | AGT-08 | | B2 | SEC-01 | | B14 | REQ-04 |
| A9 | AGT-09 | | B3 | GIT-02 | | B15 | BLD-06 |
| A10 | AGT-10 | | B4 | GIT-03 | | B16 | SEC-03 |
| A11 | GIT-04 | | B5 | REQ-01 | | B17 | PRC-03 |
| A12 | BLD-03 | | B6 | REQ-02 | | | |
