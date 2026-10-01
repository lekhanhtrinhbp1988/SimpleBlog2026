# 05 - Test report: about

## Kết quả theo AC

| AC | Test | Kết quả |
|---|---|---|
| AC-1 | `AboutAcceptanceTests.AC1_TrangChu_MenuCoMucGioiThieu` | PASS |
| AC-2 | `AboutAcceptanceTests.AC2_BamMucMenu_MoTrangGioiThieuThanhCong` | PASS |
| AC-3 | `AboutAcceptanceTests.AC3_TrangGioiThieu_HienThiTenBlog` | PASS |
| AC-4 | `AboutAcceptanceTests.AC4_TrangGioiThieu_HienThiMoTa` | PASS |
| AC-5 | `AboutAcceptanceTests.AC5_TrangGioiThieu_MenuCoMucGioiThieu` | PASS |
| AC-6 | `AboutAcceptanceTests.AC6_MoTrucTiep_KhongCanDangNhap` | PASS |

Ghi chú: feature không có DbContext nên bỏ qua database test. `Program` là internal nên test tạo `WebApplicationFactory<Program>` bằng reflection (không sửa `src/`). Test so khớp chuỗi trên HTML thô (UTF-8, NFC), nên cũng xác nhận nội dung không bị encode thành entity.

## Output dotnet test

```
  Determining projects to restore...
  All projects are up-to-date for restore.
  SimpleBlog.Web -> D:\Projects\SimpleBlog2026\src\SimpleBlog.Web\bin\Debug\net10.0\SimpleBlog.Web.dll
  SimpleBlog.Tests -> D:\Projects\SimpleBlog2026\tests\SimpleBlog.Tests\bin\Debug\net10.0\SimpleBlog.Tests.dll
Test run for D:\Projects\SimpleBlog2026\tests\SimpleBlog.Tests\bin\Debug\net10.0\SimpleBlog.Tests.dll (.NETCoreApp,Version=v10.0)
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     8, Skipped:     0, Total:     8, Duration: 705 ms - SimpleBlog.Tests.dll (net10.0)
```

## AC chưa đạt

Không

## Kết luận

Cả 6 AC đạt, dotnet test xanh.
