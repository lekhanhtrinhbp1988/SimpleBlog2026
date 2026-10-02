# Architecture Decision Records

Mỗi file ghi **một** quyết định kỹ thuật quan trọng: bối cảnh lúc đó, các phương án, lựa chọn, và hệ quả. Mục đích là trả lời câu hỏi "tại sao lúc đó lại làm vậy" cho người đến sau, kể cả chính mình sau vài tháng.

## Danh sách

| Số | Quyết định | Trạng thái |
|---|---|---|
| [0001](0001-record-architecture-decisions.md) | Ghi quyết định kiến trúc bằng ADR | Accepted |
| [0002](0002-protected-main-squash-only.md) | `main` được bảo vệ, chỉ merge qua PR bằng squash | Accepted |
| [0003](0003-ci-on-github-actions-ubuntu.md) | CI trên GitHub Actions, runner Ubuntu | Accepted |
| [0004](0004-agent-pipeline-with-human-gates.md) | Pipeline agent theo feature, người giữ điểm duyệt yêu cầu và merge | Accepted |
| [0005](0005-line-endings-and-encoding.md) | Line ending LF trong repo, UTF-8, `.cshtml` có BOM | Accepted |
| [0006](0006-code-quality-gates.md) | Cổng chất lượng: analyzer, warning thành lỗi, CodeQL, Dependabot | Accepted |

## Luật

- Đánh số tăng dần, 4 chữ số, không dùng lại số. Tên file: `NNNN-tieu-de-kebab-case.md` (tiếng Anh).
- **Không sửa nội dung ADR đã `Accepted`**, trừ lỗi chính tả. Khi đổi ý, viết ADR mới, ghi `Supersedes ADR-NNNN`, và đổi trạng thái ADR cũ thành `Superseded by ADR-MMMM`. Như vậy lịch sử lý do còn nguyên.
- Trạng thái: `Proposed` (đang bàn) → `Accepted` (đã chốt) → có thể thành `Deprecated` (không còn áp dụng) hoặc `Superseded by ADR-MMMM`.
- ADR mới đi qua PR như code. Thêm một dòng vào bảng trên.
- Ngắn là tốt: một trang là đủ.

## Mẫu

```markdown
# NNNN. <Tiêu đề quyết định>

- Trạng thái: Proposed | Accepted | Deprecated | Superseded by ADR-MMMM
- Ngày: YYYY-MM-DD
- Người quyết định: <tên>

## Bối cảnh

Vấn đề cần quyết định, các ràng buộc lúc đó.

## Quyết định

Chọn gì, nói rõ ràng ở thể khẳng định.

## Phương án đã cân nhắc

- **Phương án A**: ưu, nhược, vì sao không chọn.
- **Phương án B**: ...

## Hệ quả

Điều gì dễ hơn, điều gì khó hơn, việc phải làm tiếp, khi nào nên xem lại.
```
