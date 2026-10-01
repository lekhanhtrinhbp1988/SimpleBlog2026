---
name: sync
description: Sau khi người dùng đã squash and merge một PR trên GitHub, đưa máy về main mới nhất và xóa branch đã merge. Chỉ chạy khi người dùng gõ /sync.
argument-hint: "[<branch>]"
disable-model-invocation: true
---

# /sync

Đưa máy về `main` mới nhất sau khi một PR đã được merge, và dọn branch của PR đó.

Tham số: `$ARGUMENTS`

## Chọn branch

- Có tham số: branch là tham số đó.
- Không có: branch là branch hiện tại. Nếu branch hiện tại là `main`, dừng và hỏi người dùng branch nào.
- Branch phải bắt đầu bằng `feature/` hoặc `chore/`. Khác thì dừng.

## Các bước

1. `git status --porcelain` phải rỗng. Không rỗng thì dừng: có thay đổi chưa commit, xóa branch sẽ làm mất chúng.
2. `gh pr view <branch> --json state,number,url`. `state` phải là `MERGED`. Khác thì dừng và báo trạng thái thật (`OPEN`, `CLOSED`, hoặc không có PR).
3. `git switch main`.
4. `git pull --ff-only`. Thất bại nghĩa là `main` trên máy có commit không có trên GitHub: dừng, không tự sửa.
5. `git branch -D <branch>`. Dùng `-D` vì squash merge làm git không nhận ra branch đã merge; bước 2 đã xác nhận PR merged.

Không `git reset`, `git stash`, `git push`, không `--force`.

## Báo cáo

- PR đã merge: số và link.
- `git log --oneline -3` của `main`.
- Branch đã xóa trên máy.
