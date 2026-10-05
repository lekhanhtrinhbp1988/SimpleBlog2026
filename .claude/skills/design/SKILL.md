---
name: design
description: Thiết kế hoặc đánh giá thiết kế dựa trên hướng dẫn chính thức của Anthropic, có dẫn nguồn. Dùng khi người dùng hỏi cách thiết kế hay sửa agent workflow, orchestrator, pipeline, skill, agent, hook, CLAUDE.md, CI hoặc quy trình phát triển (SDLC), hỏi một luật nên đặt ở đâu, hoặc hỏi một thiết kế có đúng best practice không. Use for any agent, workflow, Claude Code setup or SDLC design question.
argument-hint: "<câu hỏi thiết kế>"
---

# /design

Câu hỏi: `$ARGUMENTS` (không có thì lấy câu hỏi thiết kế gần nhất của người dùng).

Mọi đề xuất phải dựa trên hướng dẫn chính thức của Anthropic, đọc trực tiếp lúc này, không dựa vào trí nhớ.

## Các bước

1. **Nêu lại vấn đề** trong 2–4 câu: cần quyết điều gì, ràng buộc nào. Đọc các ADR liên quan trong `docs/adr/` và `docs/process-roadmap.md` để không đề xuất lại điều đã quyết hoặc đã loại.
2. **Chọn nguồn.** Đọc `docs/references/anthropic-guidance.md`, chọn các nguồn liên quan. Mở trực tiếp ít nhất hai nguồn liên quan nhất bằng WebFetch. Câu hỏi về tính năng Claude Code: dùng agent `claude-code-guide` hoặc mục lục https://code.claude.com/docs/llms.txt.
3. **Gọi tên mẫu** trong hướng dẫn mà vấn đề thuộc về (ví dụ prompt chaining, evaluator-optimizer, hook thay cho lời dặn).
4. **Đưa 2–3 phương án** trong một bảng: cách làm, ưu, nhược, chi phí. Mỗi nhận định dựa trên hướng dẫn thì kèm link nguồn.
5. **Đối chiếu hướng dẫn:** phương án đề xuất theo đúng điểm nào, khác điểm nào và vì sao khác. Không có điểm khác thì ghi rõ.
6. **Đề xuất một phương án** và nói rõ điều gì sẽ làm thay đổi đề xuất.
7. **Ghi lại khi quyết định lớn** (khó đổi về sau, đổi quy trình hay kiến trúc): mở PR gồm ADR `Proposed` và bản thiết kế theo `docs/design/_template.md`, có mục "Câu hỏi cần quyết" với đề xuất cho từng câu (PRC-05). Quyết định nhỏ thì trả lời trong chat.
8. **Cập nhật danh mục:** gặp nguồn mới hữu ích hoặc nguồn đã đổi nội dung thì sửa `docs/references/anthropic-guidance.md` và ngày kiểm, trong cùng PR.

## Luật

- Không tìm thấy một tính năng trong tài liệu thì nói "không thấy trong tài liệu", không nói "không tồn tại" (AGT-16).
- Thuật ngữ lần đầu xuất hiện trong cuộc trao đổi: nói tên đầy đủ và nghĩa một dòng (PRC-06).
- Hướng dẫn chung không phải luật cứng: khác hướng dẫn được, nhưng phải nói ra và có lý do.
- Kết thúc câu trả lời bằng danh sách "Nguồn" gồm các link đã thật sự mở.
