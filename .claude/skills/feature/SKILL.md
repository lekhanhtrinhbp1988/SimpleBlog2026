---
name: feature
description: Chạy trọn pipeline BA -> Architect -> Developer -> Reviewer -> Tester cho một feature trên branch feature/<feature>, tự quay lại developer khi reviewer hoặc tester báo FAIL, và commit khi xong. Chỉ chạy khi người dùng gõ /feature.
argument-hint: "[--auto] <feature> <mô tả yêu cầu>  |  <feature> --from <bước>"
disable-model-invocation: true
---

# /feature

Bạn là người điều phối pipeline. Bạn gọi năm agent `ba`, `architect`, `developer`, `reviewer`, `tester` bằng tool Agent, lần lượt, mỗi lần chờ agent xong (`run_in_background: false`).

Bạn không tự viết hay sửa artifact, code hay test; mọi thay đổi trong repo do agent làm. Việc của bạn: chọn bước kế, thao tác git, và báo cáo cho người dùng.

Tham số: `$ARGUMENTS`

## Đọc tham số

- `--auto`: bỏ trạm duyệt sau BA. Chạy không tương tác (`claude -p`) thì bắt buộc có cờ này.
- `--from <bước>`: chạy tiếp một feature đang dở, bắt đầu từ `ba`, `architect`, `developer`, `reviewer` hoặc `tester`.
- Từ đầu tiên còn lại là tên feature; phần còn lại là mô tả yêu cầu. Mô tả bắt buộc khi chạy feature mới hoặc `--from ba`.
- Tên feature phải khớp `^[a-z0-9]+(-[a-z0-9]+)*$`: viết thường, không dấu, nối bằng gạch ngang. Sai thì dừng và nói lý do.

## Chuẩn bị

Feature mới (không có `--from`):

1. `git status --porcelain` phải rỗng. Không rỗng thì dừng: reviewer cần working tree chỉ chứa thay đổi của feature này.
2. `git rev-parse --verify --quiet feature/<feature>` không được tìm thấy branch, và `docs/features/<feature>/` chưa tồn tại. Có rồi thì dừng và gợi ý `--from`.
3. `git check-ignore -q docs/features/<feature>/x`: nếu lệnh trả mã 0 thì tên feature bị `.gitignore` che. Dừng và đề nghị tên khác.
4. `git switch -c feature/<feature> main`.

Chạy tiếp (`--from`):

1. Đang ở branch khác `feature/<feature>` mà `git status --porcelain` không rỗng thì dừng.
2. `git switch feature/<feature>`. Branch không tồn tại thì dừng.
3. Thay đổi chưa commit trên branch này là việc dở của lần chạy trước: giữ nguyên.

## Gọi agent

Lời gọi luôn ghi tên feature:

| Agent | Lời gọi |
|---|---|
| `ba` | `Feature: <feature>. Yêu cầu: <mô tả>`, cộng góp ý của người dùng nếu đang chạy lại |
| `architect` | `Feature: <feature>.` |
| `developer`, lần đầu | `Feature: <feature>. Làm các task trong 03-tasks.md.` |
| `developer`, sửa | `Feature: <feature>. Gọi lại để sửa theo <04-review.md hoặc 05-test-report.md>.` kèm nguyên dòng `FAIL` của agent vừa báo |
| `reviewer` | `Feature: <feature>.` |
| `tester` | `Feature: <feature>.` |

Trạng thái là dòng không rỗng cuối cùng trong câu trả lời của agent. Nó phải đúng dạng `DONE`, `FAIL: ...` hoặc `BLOCKED: ...`; khác thì coi như `BLOCKED: agent không trả dòng trạng thái hợp lệ`.

Sau `DONE`, kiểm file mà bước đó phải để lại. Không đúng thì xử lý như `BLOCKED`.

| Bước | Điều kiện |
|---|---|
| `ba` | `01-requirements.md` tồn tại |
| `architect` | `02-design.md` và `03-tasks.md` tồn tại; 03 có ít nhất một dòng `- [ ] T-` |
| `developer` | 03 không còn dòng `- [ ]` |
| `reviewer` | `04-review.md` tồn tại, mục "Kết luận" ghi `APPROVE` |
| `tester` | `05-test-report.md` tồn tại |

## Luồng

1. **BA.** `BLOCKED` thì dừng và chuyển nguyên câu hỏi của BA cho người dùng. `DONE` thì sang trạm duyệt.
2. **Trạm duyệt** (bỏ qua khi có `--auto`). Tóm tắt cho người dùng: từng AC (mã và một dòng), các giả định, các câu hỏi mở, và đường dẫn tới 01. Kết thúc lượt và chờ.
   - Người dùng đồng ý, hoặc tự sửa 01 xong: sang bước 3.
   - Người dùng góp ý: gọi lại `ba` kèm góp ý, rồi quay lại trạm duyệt.
3. Commit 01: `git add docs/features/<feature>/01-requirements.md`, message `docs(<feature>): requirements`.
4. **Architect.** `BLOCKED` thì dừng. `DONE` thì `git add` 02 và 03, commit với message `docs(<feature>): design and tasks`. Từ đây, nếu developer sửa 01, 02 hay nội dung task trong 03, thay đổi đó hiện trong `git diff HEAD` để reviewer bắt.
5. **Developer.** `DONE` thì sang 6. `BLOCKED` hoặc `FAIL` thì dừng: developer đã tự thử build và test, gọi lại cũng không khác.
6. **Reviewer.** `BLOCKED` thì dừng. `FAIL` thì mở một vòng sửa: gọi developer sửa theo 04, rồi chạy lại bước 6. `DONE` thì sang 7.
7. **Tester.** `BLOCKED` thì dừng. `FAIL` thì đọc mục "AC chưa đạt" của 05:
   - Nếu tester nhận định có AC chưa đạt do yêu cầu sai hoặc mơ hồ, dừng; việc này cần người quyết định.
   - Ngược lại, mở một vòng sửa: gọi developer sửa theo 05, rồi chạy lại bước 6 và 7.

   `DONE` thì sang 8.
8. Commit phần còn lại: `git add -A`, message `feat(<feature>): <tóm tắt một dòng từ 01>`.

Giới hạn: tối đa 3 vòng sửa cho cả feature, tính chung cho reviewer và tester. Cần vòng thứ 4 thì dừng.

Commit dùng message nhiều dòng qua heredoc, kèm dòng attribution theo quy định của phiên. Không `git push`, không merge, không `git reset`, `git stash`, `git checkout -- <file>`, không `--no-verify`.

## Khi dừng giữa chừng

- Không commit thêm, không hoàn tác gì; giữ nguyên working tree để người dùng xem.
- Báo: dừng ở bước nào, nguyên dòng trạng thái của agent, file nên đọc, và lệnh chạy tiếp, ví dụ `/feature <feature> --from developer`.

## Báo cáo cuối

1. Bảng các lần gọi agent theo đúng thứ tự: agent và trạng thái.
2. Số vòng sửa đã dùng.
3. Số AC đạt trên tổng số, lấy từ bảng trong 05.
4. Danh sách commit: `git log --oneline main..HEAD`.
5. Việc của người dùng: đọc lại thay đổi, rồi `git push -u origin feature/<feature>` và `gh pr create --base main --fill`. Merge qua pull request trên GitHub sau khi CI xanh; không merge thẳng vào `main` trên máy.
