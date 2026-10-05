# Lộ trình cải tiến quy trình

Những việc còn phải làm cho **quy trình phát triển** (agent, skill, hook, CI, tài liệu), không phải feature của blog. Feature của blog nằm trong `docs/project/backlog.md`.

Xếp theo giai đoạn: làm hết giai đoạn trước rồi mới sang giai đoạn sau, trừ khi có lý do ghi rõ. Mỗi việc khi làm sẽ thành một PR; việc có quyết định lớn thì kèm ADR. Xong việc nào thì đánh dấu và ghi số PR.

Đối chiếu với hướng dẫn của Anthropic: [AI-native SDLC playbook](https://claude.com/blog/the-ai-native-sdlc-playbook), [Building Effective AI Agents](https://www.anthropic.com/engineering/building-effective-agents), [Claude Code: Hooks](https://code.claude.com/docs/en/hooks).

## Đã xong

| Việc | PR |
|---|---|
| Repo trên GitHub, bảo vệ `main`, chỉ squash merge, CI build và test | #1–#3 |
| README, CONTRIBUTING, SECURITY, mẫu PR, ADR | #5 |
| Analyzer, warning là lỗi, CodeQL, Dependabot | #6, #13 |
| Lessons learned theo chủ đề, gắn vào Definition of Done | #4, #9, #17 |
| `/init-project`, agent `product` và `ux`, cổng chặn ba lớp (ADR-0007) | #15, #16 |
| Bảng thuật ngữ | #18 |

## Giai đoạn 1: vá lỗ hổng của workflow hiện tại

| # | Việc | Vì sao | Trạng thái |
|---|---|---|---|
| 1.1 | Chạy `/init-project`, merge năm tài liệu nền | Cổng chặn đang chặn mọi feature cho tới khi có | Việc tiếp theo |
| 1.2 | **Hooks cho agent** (chi tiết bên dưới) | Biến lời dặn trong agent thành luật chặn chắc chắn; playbook dùng hook cho đúng việc này | Ngay sau 1.1 |
| 1.3 | `/feature walking-skeleton` | Feature đầu tiên bắt buộc trong backlog | Sau 1.1 |
| 1.4 | `/fix`: quy trình gọn cho sửa lỗi và việc nhỏ; dùng lần đầu để thêm `public partial class Program {}` | Việc không phải feature chưa có quy trình (AGT-10); bỏ cách lách reflection | |
| 1.5 | Lượt review cho test của tester | Test của tester chưa ai xem (AGT-04) | |
| 1.6 | Bước retro cuối `/feature` và `/init-project` | Bài học chỉ được ghi khi ai đó nhớ (AGT-11) | |
| 1.7 | `/sync` tự kiểm tham số, gợi ý tên nhánh gần đúng | Đã hai lần gõ nhầm tên nhánh (AGT-14) | |

### 1.2 Hooks cho agent

Hook là script Claude Code tự chạy ở thời điểm cố định; hook `PreToolUse` chạy trước mỗi lần dùng tool và chặn được bằng mã thoát 2 hoặc `permissionDecision: "deny"`. Hook khai báo được ngay trong phần đầu file của subagent và chỉ chạy khi subagent đó đang làm việc ([Hooks reference](https://code.claude.com/docs/en/hooks)).

| Agent | Hook | Thay cho |
|---|---|---|
| `developer` | Chặn `Edit`/`Write` vào `docs/features/*/01-requirements.md`, `02-design.md`, và `tests/SimpleBlog.Tests/Acceptance/` | Lời dặn trong `developer.md`; reviewer chỉ bắt được sau khi đã sửa |
| `tester` | Chặn `Edit`/`Write` vào `src/` | Lời dặn trong `tester.md` |
| `product`, `ux`, `architect` (chế độ dự án) | Chặn ghi nội dung có `status: approved` vào `docs/project/` | Lời dặn trong agent và bước kiểm của skill |
| Mọi agent | Chặn `Edit`/`Write` vào `.claude/` | Luật "thay đổi `.claude/` do người duyệt" hiện chỉ nằm trong `CLAUDE.md` |

Cần thử mỗi hook theo cả hai chiều, chặn được và cho qua được (BLD-09), và ghi quyết định dùng hook thành ADR.

## Giai đoạn 2: nền tảng enterprise, rẻ

| # | Việc | Vì sao |
|---|---|---|
| 2.1 | `docs/ai-policy.md`: dữ liệu nào không gửi cho model, ai chịu trách nhiệm code do AI viết, agent không đọc nội dung từ người ngoài nếu chưa có người lọc | Quản trị việc dùng AI; prompt injection trên repo public |
| 2.2 | Eval cho cấu hình agent: 2–3 feature mẫu kèm kết quả mong đợi, chạy lại khi sửa `CLAUDE.md`, skill, agent hay hook | Playbook chạy eval trong CI cho đúng việc này |
| 2.3 | CodeQL thành check bắt buộc; ngưỡng độ phủ test | ADR-0006 hẹn sau vài tuần chạy ổn |
| 2.4 | Mẫu issue; PR ghi `Closes #N` | Truy vết từ yêu cầu tới code |
| 2.5 | Claude review PR trên GitHub | Playbook để Claude review trước, người tập trung vào ý định và rủi ro |

## Giai đoạn 3: khi có người thứ hai

| # | Việc |
|---|---|
| 3.1 | Bắt buộc 1 người duyệt mỗi PR, thêm `CODEOWNERS` |
| 3.2 | Chuyển repo sang organization, phân quyền theo team, bắt buộc 2FA |

## Giai đoạn 4: trước khi đưa blog lên mạng

| # | Việc |
|---|---|
| 4.1 | Database trên CI: SQL Server trong container (ADR mới thay phần còn mở của ADR-0003) |
| 4.2 | CD, môi trường staging, duyệt trước khi lên production, rollback đã tập trước |
| 4.3 | Phiên bản và changelog tự sinh từ Conventional Commits |
| 4.4 | Health check, log có cấu trúc, giám sát, runbook, backup database |
| 4.5 | Giai đoạn Maintain của playbook: sự cố quay lại thành yêu cầu mới trong pipeline |

## Giai đoạn 5: chỉ khi thật sự là enterprise

SSO, SBOM và chứng thực nguồn gốc bản build, chạy agent tập trung trên CI có log kiểm toán, đo các chỉ số DORA.
