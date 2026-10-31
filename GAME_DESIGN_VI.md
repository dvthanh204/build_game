# Happy Hen Ranch — Tài liệu thiết kế game và đặc tả kỹ thuật

## 1. Tầm nhìn sản phẩm

**Thể loại:** mô phỏng nông trại 3D thư giãn, sưu tầm thú và hoạt động arcade ngắn.  
**Nền tảng:** Android trước; iOS sau khi kiểm thử thiết bị và cửa hàng.  
**Đối tượng:** người chơi casual thích sưu tầm, trang trí, tương tác xã hội nhẹ và các phiên chơi 2–10 phút.  
**Lời hứa sản phẩm:** xây nông trại nhiều màu sắc, kết bạn với những giống gà độc đáo và biến việc chăm sóc thường ngày thành một công viên giải trí vui nhộn.

Phong cách hướng tới 3D stylized nguyên bản: hình khối bo tròn, silhouette dễ đọc, bảng màu tươi có kiểm soát, nhân vật biểu cảm, ánh sáng ấm và cây cối có chiều sâu. Các tựa game được nêu trong brief chỉ là tham khảo cảm xúc; nhân vật, tài sản, giao diện và thương hiệu khi sản xuất phải là thiết kế riêng.

### Mục tiêu MVP

| Mục tiêu | Tiêu chí kiểm chứng |
|---|---|
| Nông trại dễ hiểu ngay | Trong phút đầu người chơi nhận ra chuồng, luống cây, gà và HUD |
| Vòng lặp ngắn có thưởng | Vuốt ve/cho ăn, nhặt trứng, gieo hạt rồi quay lại thu hoạch |
| Điều khiển cảm ứng thoải mái | Bố cục ngang, nút đủ lớn, chạm để di chuyển, không cần thao tác chính xác |
| Nền tảng Android có thể mở rộng | Unity URP, cảnh tự tạo, hướng dẫn build Android |
| Phạm vi sản xuất minh bạch | Multiplayer, kiếm tiền và nội dung lớn chỉ làm sau khi kiểm chứng core loop |

## 2. Core loop và tiến trình

```text
Khám phá nông trại → chăm sóc gà → nhặt trứng / trồng cây
        ↑                                      ↓
Mở khu mới ← nhận xu, thức ăn, hạt giống ← bán và thu hoạch
        ↓
Sưu tầm giống → trang trí nông trại → tham gia minigame / sự kiện
```

### Vòng lặp tương tác MVP

1. Chạm mặt đất hoặc dùng D-pad để đi đến một chú gà.
2. Vuốt ve để tăng nhẹ độ vui vẻ, hoặc tiêu 1 phần ngũ cốc để cho ăn và tăng nhiều hơn.
3. Đến gần chuồng để nhặt trứng và nhận xu.
4. Gieo hạt ở luống gần đó. Cây lớn theo thời gian; thu hoạch nhận ngũ cốc và xu.
5. Dùng phần thưởng cho thức ăn, hạt giống, mỹ phẩm và mở rộng nông trại ở các bản sau.

### Nguyên tắc kinh tế

- **Tài nguyên:** xu mềm, ngũ cốc/thức ăn, hạt giống và (tương lai) tiền tệ cao cấp thiên về mỹ phẩm.
- Trứng và cây trồng là nguồn xu ổn định, dễ hiểu; không tạo sức mạnh phụ thuộc vào may rủi trả phí.
- Vật phẩm trả phí ưu tiên mỹ phẩm và tiện ích minh bạch, có giới hạn rõ ràng.
- Quảng cáo thưởng phải tự chọn, ghi nhãn rõ, giới hạn tần suất và không bắt buộc để hoàn tất tiến trình.
- MVP không dùng loot box trả phí. Nếu sau này có ngẫu nhiên hóa, phải công bố xác suất và tuân thủ quy định từng thị trường.

| Nguồn | Thưởng prototype | Mục đích sử dụng |
|---|---:|---|
| Nhặt một trứng | 8 xu | Trang trí/mở khóa nông trại (tương lai) |
| Thu hoạch một luống | 5 xu + 2 ngũ cốc | Cho gà ăn hoặc chế tạo hạt |
| Gieo một luống | 1 hạt giống | Tạo tiến trình cây trồng |
| Cho một gà ăn | 1 ngũ cốc | Tăng vui vẻ và tiến trình chăm sóc |

Đây là số liệu thử nghiệm, không phải cân bằng live-game. Cần đo playtest trước khi chốt kinh tế lưu trữ lâu dài.

## 3. Sưu tầm, lai tạo và tùy biến

Nên ra mắt với khoảng 24 giống gà được làm thủ công; chỉ mở rộng dần tới 100+ khi năng lực art, bộ nhớ và cân bằng sưu tầm cho phép. Độ hiếm biểu thị vẻ ngoài/khả năng sưu tầm, không phải sức mạnh bắt buộc:

| Bậc | Hướng ví dụ | Nguồn nhận |
|---|---|---|
| Thường | Gà quê, gà lông đốm | Hướng dẫn, tiến trình nông trại |
| Khá hiếm | Hướng dương, băng giá, lông đồng | Chế tạo, sự kiện |
| Hiếm | Pixel, biển sâu, ngắm sao | Cột mốc minigame |
| Huyền thoại | Phượng hoàng, đồng hồ máy, vũ trụ | Sự kiện dài hạn hoặc gói mua trực tiếp minh bạch |

Lai tạo nên có thể dự tính: chọn bố mẹ phù hợp, xem trước nhóm đặc tính có thể xuất hiện, dùng vật liệu ấp kiếm được trong game. Đột biến ngẫu nhiên chỉ là phần thưởng thêm, không phải bảo đảm trả phí. Lưu phả hệ và ngoại hình; không biến đặc tính hiếm thành rào cản sức mạnh.

Mỹ phẩm gà gồm mũ, kính, khăn, hiệu ứng cánh và biểu cảm. Chủ đề nông trại gồm khu vườn hiện đại lúc ra mắt, sau đó là miền Tây, cổ tích và neon. Dùng chung rig/material atlas khi có thể; silhouette phải đọc được trên màn hình điện thoại và hỗ trợ người khó phân biệt màu.

## 4. Hoạt động và tính năng xã hội

| Hoạt động | Thời lượng | Trạng thái |
|---|---:|---|
| Chăm nông trại, trồng cây | 2–10 phút | Có prototype |
| Hồ câu cá | 3–5 phút | Sau MVP: ném câu, chờ, canh lực, sổ sưu tầm |
| Mê cung trứng vàng | 5–8 phút | Sau MVP: phòng thiết kế sẵn, câu đố dễ đọc, checkpoint |
| Đua gà kart | 2–4 phút | Sau MVP: điều hướng, nhảy, tăng tốc, replay bóng ma |

Không bắt đầu với một phòng “hàng nghìn người”. Nên thử phòng xã hội giới hạn khoảng 16–32 người, đo đồng thời rồi chia shard theo khu vực/sự kiện. Trạng thái nông trại phải hoạt động khi offline; dịch vụ máy chủ xác thực giao dịch, phần thưởng và tiến trình có giá trị.

| Tính năng | Thiết kế khởi đầu |
|---|---|
| Quảng trường | Phòng theo instance, emote, chat preset an toàn; chỉ mở chat chữ sau khi có kiểm duyệt |
| Thăm nông trại | Bắt đầu bằng ảnh chụp trạng thái bất đồng bộ; co-op thời gian thực làm sau |
| Hội thi sắc đẹp | Thời gian gửi bài do server quản lý, giới hạn tần suất bình chọn |
| Nhiệm vụ co-op | Mục tiêu chung, phần thưởng xác thực phía server |
| “Trộm trứng” | Minigame nhóm có đồng thuận; không xóa vật phẩm lưu trữ của người khác |

Có thể đánh giá Photon Fusion/Cloud hoặc backend server-authoritative tự quản dựa trên CCU, vùng máy chủ, lưu trữ, kiểm duyệt và chi phí. Mirror là thư viện mạng, không phải backend live-service được host sẵn.

## 5. Đồ họa, render, âm thanh

### Định hướng hình ảnh

- 3D stylized, hình khối mềm, silhouette rõ, màu tươi nhưng có kiểm soát và thiết kế nhân vật nguyên bản.
- PBR chọn lọc: roughness/specular đơn giản để tạo chiều sâu mà không phụ thuộc texture quá dày.
- “Lông mềm” MVP dùng các lớp hình khối và ánh sáng. SSS thời gian thực không phải mặc định cho mobile; mô phỏng bằng ánh sáng ngược ấm, rim light nhẹ và vật liệu feather được thiết kế cẩn thận.
- Thứ tự thời tiết: nắng buổi sáng trước; mưa/tuyết là giai đoạn sau. Không để hiệu ứng wetness/tuyết động cản trở core loop.
- Hạt lấp lánh khi thu hoạch và lửa trại phải ngắn, tái sử dụng bằng pool; có tùy chọn giảm hiệu ứng.

### Mục tiêu render URP

| Hạng mục | Mục tiêu / giải pháp |
|---|---|
| Render | URP Forward; một directional light chính; baked/mixed light cho vật thể tĩnh |
| FPS | Mục tiêu 60 FPS trên thiết bị upper-midrange đã kiểm thử; có mức 30 FPS dự phòng |
| Độ phân giải | Chỉ dùng dynamic render scale sau khi profiler xác nhận; giữ UI sắc nét |
| Bóng | Một nguồn bóng chính, khoảng cách ngắn, độ phân giải thấp/vừa trên mobile |
| Vật liệu | Dùng chung material, GPU instancing cho vật thể lặp, texture atlas |
| LOD | LOD0 gần camera, LOD1/2 xa; cull khuất tầm nhìn |
| Đám đông | Animation LOD, giảm tần suất cập nhật theo khoảng cách, pool actor; đo draw call |
| Batching | Ưu tiên SRP Batcher và GPU instancing; Dynamic Batching không phải lúc nào cũng nhanh hơn |
| Nhiệt/pin | Đo phiên 15–20 phút trên thiết bị đại diện; giảm bóng, hạt, FPS khi máy nóng |

Prototype hiện có cảnh nhỏ; chưa chứng minh stress test 100 gà và chưa xác nhận FPS trên thiết bị.

### Âm thanh và haptic

Phân lớp tiếng gà gần, tiếng lông/chân theo bề mặt, chuồng, thời tiết và UI; dùng spatial audio, attenuation theo khoảng cách và giới hạn số voice. Footstep đổi theo vật liệu có thể đồng bộ animation event. Haptic nhẹ khi vuốt ve, xác nhận khi nhặt trứng, tín hiệu câu cá riêng nhưng vừa phải. Trước khi phát hành cần tùy chọn tắt tiếng, chỉnh nhạc/hiệu ứng và tắt rung.

## 6. Tương tác và UX

- Ưu tiên màn hình ngang cho góc nhìn nông trại; chỉ cân nhắc dọc nếu điều hướng vẫn thoải mái.
- Chạm mặt đất để đi, D-pad cảm ứng dự phòng, nút hành động theo ngữ cảnh, vùng chạm khoảng 48 dp trở lên.
- Camera bám ổn định; zoom do người chơi điều khiển là tính năng tương lai.
- Phản hồi ngắn, có thể bỏ qua; không dùng màu sắc đơn lẻ để truyền thông tin cốt yếu.
- Tutorial: di chuyển → vuốt ve → nhặt trứng → gieo hạt → thu hoạch → giới thiệu sổ sưu tầm.
- Tôn trọng safe area, nút back Android, pause/resume, khởi chạy lại khi thiếu bộ nhớ và gián đoạn mạng.

## 7. Kiến trúc kỹ thuật

### Client

- Unity 2022.3 LTS + URP 14; gameplay C#; khi nội dung tăng, chuyển định nghĩa nhân vật/vật phẩm sang ScriptableObject.
- Tách presentation, simulation, persistence trước khi sản xuất. Prototype hiện gom setup/UI để thử nhanh.
- Chỉ thêm Addressables khi quy mô asset cần; pool hạt, pickup và NPC lặp.
- Lưu profile local có version, ghi file an toàn và test migration. Không tin trạng thái mua hàng do client tự khai.
- Chỉ thêm analytics sau khi xác định event cần đo, có consent và tối thiểu hóa dữ liệu.

### Lựa chọn backend

| Nhu cầu | Ứng viên | Tiêu chí |
|---|---|---|
| Đăng nhập/profile/cloud save | PlayFab hoặc Firebase | Danh tính nền tảng, xử lý xung đột, vùng dữ liệu, chi phí |
| Phòng xã hội thời gian thực | Photon Fusion/Cloud hoặc server riêng | Giá CCU, vùng, reconnect, quyền xác thực, kiểm duyệt |
| Push/remote config | Firebase hoặc tương đương | Consent, giới hạn tần suất, rollout theo nhóm |
| Mua hàng | Google Play Billing qua Unity IAP | Xác minh receipt, refund, restore, tuân thủ địa phương |

Không tin client gửi số xu, kết quả lai giống, điểm bảng xếp hạng hoặc quyền mua hàng. Server xác thực trạng thái có giá trị; chơi đơn vẫn nên dùng được khi offline.

### Dữ liệu và quyền riêng tư

Chỉ thu dữ liệu cần cho gameplay, hỗ trợ, an toàn và vận hành. Trước khi có xã hội/analytics, cần thiết kế phù hợp độ tuổi, quy trình đồng ý của phụ huynh khi luật yêu cầu, xóa/xuất dữ liệu và thông báo quyền riêng tư. Mặc định dùng emote preset; chat cần report, block, chính sách lưu trữ và quy trình phản hồi.

## 8. Giữ chân và doanh thu

- Hành trình tuần đầu: mục tiêu ngắn mỗi ngày, tiến trình rõ và mỹ phẩm có ý nghĩa; không phạt người bỏ lỡ ngày.
- Chicken Pass mùa: nhánh miễn phí và premium tùy chọn, thiên về mỹ phẩm; công bố ngày mùa.
- Corgi/Shiba đồng hành ban đầu nên là companion trang trí. Tính năng chống cáo/diều hâu làm sau và không biến thành “phí phòng thủ”.
- Daily/weekly goal khuyến khích chăm sóc, khám phá, sáng tạo thay vì ép mở game quá thường xuyên.
- Rewarded ad: thưởng tùy chọn để nhận bonus/rút ngắn có giới hạn; không chèn interstitial khi đang chăm gà hoặc chơi minigame.
- IAP: gói mỹ phẩm bán trực tiếp và pass; hỗ trợ giá theo vùng, restore, entitlement an toàn khi hoàn tiền.

## 9. Lộ trình sản xuất

| Giai đoạn | Ước lượng | Bàn giao / tiêu chí thoát |
|---|---:|---|
| Prototype | 2–4 tuần | Di chuyển, camera, một vòng lặp nông trại, UX cảm ứng; test hiểu và độ vui với 10–20 người |
| Alpha | 8–12 tuần | Art direction, save/load, tutorial, 10–15 giống, telemetry kinh tế, ma trận Android; không còn crash chặn |
| Beta / Soft launch | 8–12 tuần | Thị trường giới hạn, remote config, hỗ trợ/kiểm duyệt, store flow, privacy review, cohort retention |
| Ra mắt chính thức | Chuẩn bị 4–8 tuần | Lịch nội dung, localization, runbook live ops, rollback/sự cố, compliance và review readiness |

Nhóm production tham khảo: producer, game/economy designer, Unity client engineer, technical artist, 3D artist/animator, UI/UX, QA/device tester và backend/live-ops. Cần ước lượng lại nhân lực/lịch sau prototype.

### Cổng chất lượng

1. Chốt ma trận Android theo GPU, RAM, OS và nhóm nhiệt.
2. Profile cold start, bộ nhớ, draw call, frame pacing, nhiệt/pin và phiên bị ngắt.
3. Test migration save, offline, xung đột cloud, receipt mua hàng và reconnect.
4. Review accessibility, privacy, localization, store policy và moderation trước soft launch.
5. Mỗi bản phát hành phải đạt ngưỡng crash-free/performance dựa trên telemetry thiết bị thật.

## 10. Phạm vi prototype hiện tại

Source trong dự án là bộ khung tương tác có thể chơi, dùng hình khối primitive tạo runtime để không cần tải asset ngoài. Chưa có art production, cloud save, multiplayer, lai giống, đua gà, câu cá, mê cung, battle pass, quảng cáo, IAP, localization hay analytics. Unity Editor cần tạo cảnh bootstrap ở lần mở đầu; môi trường hiện chưa có Unity nên APK và hiệu năng thiết bị chưa được xác minh.
