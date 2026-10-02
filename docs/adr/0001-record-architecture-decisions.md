# 0001. Ghi quyết định kiến trúc bằng ADR

- Trạng thái: Accepted
- Ngày: 2026-10-02
- Người quyết định: Lê Khánh Trình

## Bối cảnh

Dự án dự kiến mở rộng lên 3 đến dưới 10 người, có agent AI tham gia viết code. Lý do của các quyết định kỹ thuật đang nằm rải rác trong các cuộc trao đổi, không ai tra lại được. Người mới, hoặc agent, dễ đề xuất lại một phương án đã bị loại mà không biết vì sao nó bị loại.

## Quyết định

Mỗi quyết định kỹ thuật quan trọng được ghi thành một Architecture Decision Record trong `docs/adr/`, theo mẫu và luật trong [README.md](README.md). ADR đã chốt không sửa; đổi ý thì viết ADR mới thay thế.

## Phương án đã cân nhắc

- **Wiki hoặc tài liệu ngoài repo** (Confluence, Google Docs): dễ viết, nhưng tách khỏi code, không đi qua review, dễ cũ, agent không đọc được.
- **Chỉ ghi trong mô tả PR**: có lý do cho từng thay đổi, nhưng khó tìm lại quyết định cấp hệ thống nằm ở PR nào.
- **Một file `architecture.md` duy nhất, sửa liên tục**: thấy được hiện trạng, nhưng mất lịch sử "lúc đó vì sao chọn vậy" mỗi khi sửa.

## Hệ quả

- Lý do nằm cạnh code, đi qua PR, agent đọc được.
- Tốn thêm vài phút cho mỗi quyết định lớn.
- Mô tả PR vẫn giữ lý do của từng thay đổi nhỏ; ADR chỉ dành cho quyết định cấp hệ thống.
