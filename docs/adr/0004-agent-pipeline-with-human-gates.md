# 0004. Pipeline agent theo feature, người giữ điểm duyệt yêu cầu và merge

- Trạng thái: Accepted
- Ngày: 2026-10-01
- Người quyết định: Lê Khánh Trình

## Bối cảnh

Code được viết với sự hỗ trợ của agent Claude Code. Cần cách để agent làm được nhiều việc mà vẫn kiểm soát được chất lượng và phạm vi, và để người mới hiểu ai chịu trách nhiệm ở bước nào.

## Quyết định

- Mỗi feature chạy qua skill `/feature` với năm agent: `ba` → `architect` → `developer` → `reviewer` → `tester`. Mỗi bước để lại một tài liệu trong `docs/features/<feature>/` (01 đến 05).
- **Người giữ hai điểm quyết định:**
  1. Duyệt `01-requirements.md` (đúng yêu cầu chưa) trước khi thiết kế.
  2. Đọc PR và bấm merge (đưa vào sản phẩm).
- **Agent tự làm những việc hoàn tác được hoặc đã có lớp bảo vệ khác:** viết code, chạy test, commit, push branch `feature/*` và `chore/*`, mở PR, đồng bộ máy sau merge (`/sync`).
- **Agent bị chặn** (trong `.claude/settings.json`): force push, xóa branch trên server, push `main`, `gh pr merge`.
- Thay đổi trong `.claude/` (agent, skill, quyền) đi qua PR và được người duyệt từng lần sửa; agent không tự nới quyền cho mình.

## Phương án đã cân nhắc

- **Agent làm hết, kể cả merge**: nhanh nhất, nhưng không còn ai kiểm soát việc agent tự viết, tự review, tự đưa vào sản phẩm.
- **Người duyệt mọi bước** (cả thiết kế, task, review): an toàn nhưng chậm, và phần lớn các bước đó đã có agent khác kiểm.
- **Chỉ dựa vào luật phía client**: luật trong `settings.json` chỉ áp cho ai dùng file đó. Vì vậy luật quan trọng nhất (không push `main`, CI bắt buộc) đặt ở GitHub (xem [ADR-0002](0002-protected-main-squash-only.md)).

## Hệ quả

- Việc của người cho mỗi feature: duyệt `01`, đọc PR, bấm merge, chạy `/sync`.
- Pipeline hiện chỉ có tầng feature, chưa có tầng dự án (tầm nhìn, yêu cầu phi chức năng, hướng dẫn giao diện, kiến trúc, backlog), và chưa có quy trình cho việc không phải feature. Xem [lessons learned](../lessons-learned.md) A1, A10. Cả hai sẽ được bổ sung bằng ADR mới.
- Test do tester viết chưa có bước review riêng (lessons learned A4).
