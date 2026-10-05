# Hướng dẫn của Anthropic dùng khi thiết kế

Danh mục nguồn chính thức mà mọi đề xuất thiết kế về agent, skill, hook, CI hay quy trình phát triển của SimpleBlog phải đối chiếu. Skill `/design` đọc file này trước, rồi mở trực tiếp các nguồn liên quan, vì nội dung trên web thay đổi nhanh hơn file này.

Mỗi nguồn có: dùng khi nào, vài nguyên tắc chính (trích ngắn), và ngày kiểm gần nhất. Gặp nguồn mới hữu ích thì thêm vào; nguồn đã đổi nội dung thì sửa phần tóm tắt và ngày kiểm.

**Ngày kiểm gần nhất của cả danh mục: 2026-10-05.**

## Tìm tài liệu Claude Code

- Mục lục đầy đủ của Claude Code Docs: https://code.claude.com/docs/llms.txt. Đọc mục lục này trước khi kết luận một tính năng có hay không.
- Câu hỏi về tính năng Claude Code (lệnh, skill, hook, subagent, cài đặt): dùng agent `claude-code-guide` hoặc mở trang tương ứng trên code.claude.com.
- Không tìm thấy thì nói "không thấy trong tài liệu", không nói "không tồn tại" (AGT-16).

## Thiết kế agent và workflow

### [Building Effective AI Agents](https://www.anthropic.com/engineering/building-effective-agents)

Dùng khi: chọn cấu trúc cho một pipeline hay hệ nhiều agent.

- Workflow: "LLMs and tools are orchestrated through predefined code paths". Agent: "LLMs dynamically direct their own processes and tool usage".
- Chỉ dùng agent khi "flexibility and model-driven decision-making are needed at scale"; agent đổi độ trễ và chi phí lấy chất lượng.
- Năm mẫu: prompt chaining ("easily and cleanly decomposed into fixed subtasks"), routing, parallelization, orchestrator-workers ("you can't predict the subtasks needed"), evaluator-optimizer ("clear evaluation criteria, and when iterative refinement provides measurable value").
- Ba nguyên tắc: giữ thiết kế đơn giản; cho thấy rõ các bước lập kế hoạch; đầu tư vào giao diện giữa agent và công cụ (ACI: tài liệu và test cho tool).
- Áp vào SimpleBlog: `/feature` là prompt chaining có cổng chặn cộng một vòng evaluator-optimizer, không phải orchestrator-workers.

### [Effective context engineering for AI agents](https://www.anthropic.com/engineering/effective-context-engineering-for-ai-agents)

Dùng khi: quyết định agent đọc gì, khi nào, và phần nào giao cho subagent.

- Context là tài nguyên có hạn; đưa vào đúng phần cần, lấy thêm dần khi cần (progressive disclosure).
- Subagent có context riêng và chỉ trả về bản tóm tắt.

### [How we built our multi-agent research system](https://www.anthropic.com/engineering/multi-agent-research-system)

Dùng khi: định chạy nhiều agent song song.

- Mỗi việc giao cho subagent cần: mục tiêu, định dạng kết quả, công cụ và nguồn được dùng, ranh giới rõ.

### [Patterns and problems in multiagent systems](https://www.anthropic.com/research/multiagent-systems)

Dùng khi: hệ nhiều agent gặp lỗi phối hợp.

## Quy trình phát triển (SDLC)

### [The AI-native SDLC playbook](https://claude.com/blog/the-ai-native-sdlc-playbook) và [khóa học trên Claude Academy](https://academy.claude.com/courses/ai-native-sdlc-playbook)

Dùng khi: thêm, bỏ hay đổi một giai đoạn của quy trình.

- Sáu giai đoạn: Plan, Design, Build, Test, Deploy, Maintain. Mỗi giai đoạn kết thúc bằng một file trong git; giai đoạn sau bắt đầu bằng việc đọc file đó.
- `CLAUDE.md` là kiến thức chung của nhóm, sửa khi lỗi lặp lại. Skill chứa chuẩn và quy trình. Hook thi hành luật chắc chắn (chặn sửa file được bảo vệ, chặn sửa test khi đang fix). Subagent cho việc cần context riêng.
- Claude review trước, người tập trung vào ý định và rủi ro. Eval trong CI khi sửa `CLAUDE.md`, skill hay hook.
- Mức tự động theo môi trường: dev tự do, staging có cổng chặn vừa, production cần người được chỉ định duyệt.
- Đo: tỉ lệ CI xanh ngay lần đầu, số vòng sửa, lỗi lọt ra production, chỉ số DORA.

### [How Anthropic secures its AI-native SDLC](https://claude.com/blog/how-anthropic-secures-its-ai-native-software-development-lifecycle)

Dùng khi: quyết định về quyền, sandbox, secret, kiểm toán cho agent.

## Cấu hình Claude Code

### [Steering Claude Code: CLAUDE.md, skills, hooks, subagents](https://claude.com/blog/steering-claude-code-skills-hooks-rules-subagents-and-more)

Dùng khi: quyết định một luật hay quy trình nên đặt ở đâu.

- `CLAUDE.md`: "Keep CLAUDE.md under 200 lines, give it an owner, and review changes to it like code". Không đặt quy trình dài 30 dòng ở đây.
- Rules theo đường dẫn: chỉ nạp khi làm việc với thư mục liên quan.
- Skill: "Only the name and description load at session start; the full body loads when Claude invokes the skill".
- Hook: "Hooks are code that the harness runs rather than instructions to Claude". Luật không được phép vỡ thì dùng hook `PreToolUse`, không chỉ dựa vào lời dặn.
- Subagent: "The only thing that returns to your main session is the subagent's final message plus metadata"; không dùng khi cần lái từng bước giữa chừng.
- Quy tắc chung: lời dặn (`CLAUDE.md`, rules, skill) cho hướng dẫn nên theo phần lớn thời gian; hook và permission cho rào chắn không bao giờ được vỡ.

### [Best practices for Claude Code](https://code.claude.com/docs/en/best-practices)

Dùng khi: thiết kế cách agent làm việc và tự kiểm.

- Cho Claude một cách tự kiểm (test, build, ảnh chụp); kiểm càng chắc thì càng ít phải trông.
- Khám phá, lập kế hoạch, rồi mới code; bỏ qua kế hoạch khi thay đổi mô tả được trong một câu.
- `CLAUDE.md` ngắn: với mỗi dòng, hỏi "bỏ dòng này Claude có làm sai không?". File quá dài làm Claude bỏ qua luật thật.
- Review đối kháng: subagent trong context mới chỉ thấy diff và tiêu chí; dặn nó chỉ báo lỗi ảnh hưởng đúng sai hoặc yêu cầu, tránh làm thừa.
- Việc lớn: để Claude phỏng vấn người dùng rồi viết spec, sau đó làm trong phiên mới.

### [Hooks reference](https://code.claude.com/docs/en/hooks)

Dùng khi: biến một lời dặn thành luật chặn.

- `PreToolUse` chạy trước mỗi lần dùng tool; chặn bằng mã thoát 2 hoặc `hookSpecificOutput.permissionDecision: "deny"`.
- Khai báo ở `.claude/settings.json`, trong phần đầu file skill, hoặc trong phần đầu file subagent (chỉ chạy khi subagent đó đang làm việc).
- Có `matcher` theo tên tool và `if` theo cú pháp luật permission, ví dụ `"if": "Edit(src/**)"`.

### [Skills](https://code.claude.com/docs/en/skills) và [Subagents](https://code.claude.com/docs/en/sub-agents)

Dùng khi: tạo hoặc sửa skill, agent.

- Mô tả skill bắt đầu bằng việc nó làm và có cụm từ gợi ý khi nào dùng; dưới 1.536 ký tự. Nội dung skill dưới 500 dòng, tài liệu tham khảo dài để ở file riêng.
- `disable-model-invocation: true` cho quy trình có tác động (push, mở PR) mà người muốn tự gọi.
- Subagent có context riêng, công cụ riêng; dùng cho việc đọc nhiều file hay cần tập trung chuyên biệt.
