---
name: init-project
description: Giai đoạn khởi động dự án (ADR-0007). Soạn năm tài liệu nền trong docs/project/ (vision, nfr, ui-guidelines, architecture, backlog) với agent product, ux, architect, có trạm duyệt của người sau mỗi file, rồi mở PR. Chỉ chạy khi người dùng gõ /init-project.
argument-hint: "[--from <vision|nfr|ui|architecture|backlog>] [yêu cầu ban đầu]"
disable-model-invocation: true
---

# /init-project

Bạn là người điều phối giai đoạn khởi động dự án. Bạn gọi agent `product`, `ux`, `architect` bằng tool Agent, lần lượt, mỗi lần chờ agent xong (`run_in_background: false`), và đưa câu hỏi của agent cho người dùng.

Bạn không tự viết nội dung tài liệu nền hay ADR; nội dung do agent soạn. Ngoại lệ duy nhất: sau khi người dùng duyệt, bạn ghi phần đầu file (`status`, `approved_by`, `approved_on`) và đổi ADR người đã chọn sang `Accepted`. Thiết kế chi tiết: `docs/design/project-foundation.md`.

Tham số: `$ARGUMENTS`

## Đọc tham số

- `--from <bước>`: chạy tiếp từ `vision`, `nfr`, `ui`, `architecture` hoặc `backlog`.
- Phần còn lại là yêu cầu ban đầu của người dùng (blog để làm gì, cho ai...). Không có cũng được; agent `product` sẽ hỏi.
- Skill này cần người trả lời và duyệt. Chạy không tương tác (`claude -p`) thì dừng ngay.

## Chuẩn bị

Chạy mới (không có `--from`):

1. `git status --porcelain` phải rỗng.
2. `docs/project/` chưa có file nào ngoài `_template/`. Có rồi thì dừng và gợi ý `--from`.
3. Branch `chore/project-foundation` chưa tồn tại. Có rồi thì dừng và gợi ý `--from`.
4. `git switch -c chore/project-foundation main`.

Chạy tiếp (`--from`):

1. Đang ở branch khác `chore/project-foundation` mà `git status --porcelain` không rỗng thì dừng.
2. `git switch chore/project-foundation`. Không có thì dừng.
3. Thay đổi chưa commit trên branch này là việc dở của lần chạy trước: giữ nguyên.

## Các bước

| Bước | Agent | File | Message commit |
|---|---|---|---|
| `vision` | `product` | `docs/project/vision.md` | `docs(project): vision` |
| `nfr` | `product` | `docs/project/nfr.md` | `docs(project): non-functional requirements` |
| `ui` | `ux` | `docs/project/ui-guidelines.md` | `docs(project): UI guidelines` |
| `architecture` | `architect` | `docs/project/architecture.md` và ADR mới | `docs(project): architecture and ADRs` |
| `backlog` | `product` | `docs/project/backlog.md` | `docs(project): backlog` |

Bước sau chỉ chạy khi file của bước trước đã `approved`.

## Gọi agent

| Agent | Lời gọi |
|---|---|
| `product` | `File: <vision, nfr hoặc backlog>. Yêu cầu ban đầu: <tham số>. Câu trả lời của người dùng: <tất cả câu trả lời và góp ý cho file này, theo thứ tự>` |
| `ux` | `Câu trả lời của người dùng: <...>` |
| `architect` | `Chế độ: dự án. Lựa chọn của người dùng: <...>` |

Trạng thái là dòng không rỗng cuối cùng trong câu trả lời của agent: `DONE`, `BLOCKED: ...` hoặc `FAIL: ...`. Khác thì coi như `FAIL: agent không trả dòng trạng thái hợp lệ`.

## Mỗi bước

1. Gọi agent.
2. **`BLOCKED`** (vòng hỏi đáp): đưa nguyên các câu hỏi của agent cho người dùng, kết thúc lượt và chờ. Khi người dùng trả lời, gọi lại agent kèm **toàn bộ** câu trả lời đã có cho file này. Tối đa 3 vòng mỗi file; cần vòng thứ 4 thì dừng và đề nghị người dùng tự sửa file rồi chạy `--from <bước>`.
3. **`FAIL`**: dừng.
4. **`DONE`**: kiểm file tồn tại và vẫn `status: draft`. Agent ghi `approved` thì coi như `FAIL: agent tự duyệt`.
5. **Trạm duyệt.** Tóm tắt cho người dùng: các quyết định chính, các giả định, câu hỏi mở kèm hạn chót, đường dẫn file. Với bước `architecture`: liệt kê từng ADR mới và phương án đã chọn. Kết thúc lượt và chờ.
   - Người dùng gõ **`duyệt`** (hoặc câu rõ nghĩa tương đương, như "duyệt vision"): sang 6.
   - Người dùng góp ý: gọi lại agent kèm góp ý, rồi quay lại trạm duyệt. Góp ý cũng tính vào giới hạn 3 vòng.
   - Người dùng tự sửa file: đọc lại, tóm tắt lại, chờ duyệt.
   - Câu trả lời mơ hồ (ví dụ "ok" khi vừa hỏi một câu khác): hỏi lại cho rõ, không tự coi là duyệt.
6. Ghi phần đầu file: `status: approved`, `approved_by: <git config user.name>`, `approved_on: <ngày hôm nay, YYYY-MM-DD>`. Với bước `architecture`: đổi mỗi ADR mới mà người dùng đã chọn phương án từ `Proposed` sang `Accepted`, cả trong file ADR lẫn bảng trong `docs/adr/README.md`.
7. Commit file của bước (và ADR với bước `architecture`) với message trong bảng, nhiều dòng qua heredoc, kèm dòng attribution theo quy định của phiên.

## Kết thúc

Sau bước `backlog`:

1. Kiểm năm file đều `status: approved`.
2. `git push -u origin chore/project-foundation`.
3. `gh pr create --base main --head chore/project-foundation`, tiêu đề `docs(project): project foundation`. Mô tả đi theo đúng các mục của `.github/pull_request_template.md` và giữ **mọi** checkbox Definition of Done (AGT-15). Nội dung: tóm tắt từng file, danh sách ADR mới, câu hỏi mở còn lại và hạn chót, feature đầu tiên trong backlog, và dòng attribution PR theo quy định của phiên. Trước khi tạo PR, kiểm lần chạy này có sinh bài học không; có thì thêm vào `docs/lessons-learned/` và commit trước.
4. Push hoặc tạo PR bị từ chối thì không thử cách khác; giữ nguyên commit, báo lỗi và đưa lệnh cho người dùng.

Không push `main`, không `--force`, không `gh pr merge`, không `git reset`, `git stash`, `git checkout -- <file>`, không `--no-verify`.

## Khi dừng giữa chừng

- Không commit thêm, không hoàn tác; giữ nguyên working tree.
- Báo: dừng ở bước nào, nguyên dòng trạng thái của agent, file nên đọc, lệnh chạy tiếp, ví dụ `/init-project --from ui`.

## Báo cáo cuối

1. Bảng năm file: trạng thái, số vòng hỏi đáp.
2. Danh sách ADR mới và trạng thái.
3. Câu hỏi mở còn lại và hạn chót.
4. Link PR.
5. Việc của người dùng: đọc PR, merge, `/sync chore/project-foundation`, rồi `/feature walking-skeleton <mô tả>`.
