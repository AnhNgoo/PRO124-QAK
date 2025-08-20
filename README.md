# 🧠 Luồng Hoạt Động Của Game

## 📦 Cửa Hàng Skin & Jetpack

### 1. Cấu trúc dữ liệu tổng quát

#### ✅ IShopItem (Interface)

- Lớp định nghĩa chung cho các thuộc tính của các Item có trong shop: `Name`, `Price`, `Icon`, `IsUnlocked`.
- Giúp shop có 1 kiểu dữ liệu chung để xử lí.

#### ✅ Skin, JetpackEffect

- Là các class cụ thể kế thừa `IShopItem`.
- Có thêm thuộc tính riêng như `SpriteLibraryAsset`, `Material` để dùng riêng cho từng item.

#### ✅ ShopItemData<T>

- `ScriptableObject` chung sử dụng kiểu `Generic<T>` để truyền đúng kiểu dữ liệu vào.
- Kiểu T phải kế thừa từ `IShopItem`.
- Chứa:
  - `List<T> itemList`: danh sách tất cả item (skin/jetpack).
  - `string currentItemName`: tên item đang được chọn.
- Các class kế thừa:
  - `SkinData` : `ShopItemData<Skin>`
  - `JetpackEffectData` : `ShopItemData<JetpackEffect>`

---

### 2. Lớp quản lý chung: GenericShopManager<T>

- Dùng Generic để truyền vào các class vật phẩm kế thừa từ `IShopItem`.
- Các chức năng chính:
  - `LoadShop(data, callback)` → Load UI từ ScriptableObject, truyền callback để xử lý khi chọn.
  - `LoadItem()` → Cập nhật UI dựa trên item đang chọn.
  - `NextItem()` / `PreviousItem()` → Chuyển item đang xem.
  - `OnActionButtonClick()` → Gọi `SelectItem()` nếu đã unlock, gọi `BuyItem()` nếu chưa.
  - `SelectItem()` → Cập nhật item hiện tại và gọi callback.
  - `BuyItem()` → Kiểm tra đủ tiền → trừ tiền bằng DOTween → mở khóa → gọi `SelectItem()`.

---

### 3. Lớp điều phối: ShopManager

- Điều phối chuyển qua lại giữa các shop (Skin / Jetpack).
- Sử dụng `GenericShopManager<T>` với kiểu cụ thể là `Skin` và `JetpackEffect`.

---

### 4. Tóm tắt cơ chế hoạt động

- Các vật phẩm muốn hiển thị trong shop (như skin, jetpack...) cần được tạo thành các class riêng kế thừa từ interface IShopItem, nhằm đảm bảo có các thuộc tính chung như: Name, Price, Icon, IsUnlocked
- Tiếp theo, ta sử dụng một class Generic (GenericShopManager<T>) để xử lý chung tất cả các loại item. Kiểu T bắt buộc phải kế thừa IShopItem để đảm bảo có thể truy cập các thuộc tính chung.
- Nhờ vậy, shop chỉ cần xử lý qua interface IShopItem với các thuộc tính: Name, Price, Icon, IsUnlocked, không cần quan tâm item cụ thể là gì → giúp tái sử dụng, mở rộng linh hoạt hơn.

---

## 💾 Save/Load Dữ Liệu Trong Game

### 1. 🧱 DataGame

- Class chứa toàn bộ dữ liệu cần lưu, gồm:
  - `DataPlayer`
  - `List<DataSkin>` + `currentSkinName`
  - `List<DataJetpack>` + `currentJetpackEffectName`
  - `DataSettings`

---

### 2. 💾 SaveManager

#### Khi thoát game:

```text
SaveManager.OnApplicationQuit() (Gọi khi out game đột ngột)
├─ Nếu đang chơi → cập nhật coin/distance
└─ Gọi Save():
   ├─ SavePlayerData() ← coin/distance
   ├─ SaveSkins() ← danh sách skin
   ├─ SaveJetpacks() ← danh sách jetpack
   └─ SaveSettings() ← âm lượng
⇒ Ghi xuống file .es3
```

#### Khi mở game:

```text
SaveManager.Load() (dùng singleton để gọi)
├─ LoadPlayerData() → coin, distance
├─ LoadSkins() → danh sách skin đã mở, skin đang chọn
├─ LoadJetpacks() → tương tự cho jetpack
└─ LoadSettings() → âm lượng từ slider
```

#### Khi muốn lưu thủ công:

```text
SaveManager.Save() (dùng singleton để gọi)
├─ SavePlayerData() → coin, distance
├─ SaveSkins() → danh sách skin
├─ SaveJetpacks() → danh sách jetpack
└─ SaveSettings() → âm lượng
⇒ Ghi xuống file .es3
```

---

## 🧠 Tổng Quan Cơ Chế Hoạt Động Của Wrecker

### 🎯 Mục tiêu:

Wrecker là enemy đặc biệt xuất hiện tạm thời để tấn công người chơi theo chu kỳ **né → bắn → thoát**. Nếu có obstacle chắn đường thì né, không bắn.

---

### 1. 🔄 Vòng đời hoạt động chính

```text
[OnEnable()]
 └──▶ GetComponent()
 └──▶ StartMove()
      ├── Đợi 3s
      ├── Tìm điểm an toàn để xuất hiện
      └── Gọi MainBehaviorWithCallback()
          ├── Lặp attackCount lần:
              ├── Né obstacle (AvoidancePhase)
              └── Nếu không bị cản → Attack
          └── Sau khi đủ lượt → StartExitSequence()
```

---

### 2. 🪨 Cơ chế né vật cản

```text
AvoidancePhase():
 └── Trong thời gian attackInterval:
     └── Mỗi 0.1s → raycast kiểm tra obstacle phía trước
         └── Nếu có → gọi FindSafeFlight() → tween tới điểm an toàn
```

---

### 3. 🎯 Cơ chế tấn công

```text
AttackPhase():
 ├── Nếu đang bị obstacle cản → skip
 └── Nếu không:
     ├── Random 1 player
     ├── Tính hướng từ Wrecker đến player
     ├── Bật bullet, đặt vị trí = Wrecker
     └── Gọi bullet.Init(hướng)
```

---

### 4. 🚪 Cơ chế rút lui khi xong hành vi

```text
StartExitSequence():
 ├── Tween quay về vị trí gốc
 ├── Đợi 1 giây
 └── Tắt GameObject
```

---

### 5. 🛑 Cơ chế dừng khi player chết

```text
Stop():
 ├── Nếu player chết hoặc có BigEvent
 ├── Dừng coroutine, dừng tween
 ├── Tween về vị trí gốc
 └── Tắt đạn và tắt GameObject
```

---

### 6. 🔎 Các component hỗ trợ

| Tên biến                  | Ý nghĩa                                     |
| ------------------------- | ------------------------------------------- |
| `wrecker`                 | GameObject con để tween, raycast            |
| `bulletWreckerGameobject` | Đạn bắn ra từ wrecker                       |
| `flightPoints`            | Danh sách điểm có thể bay tới tránh vật cản |
| `obstacleLayerMask`       | Layer dùng để raycast                       |
| `safePosition`            | Vị trí an toàn hiện tại (đã tìm được)       |

## 🔄 Cơ Chế Hoạt Động SceneLoader

🧩 Khi cần load lại scene, gọi:  
`SceneLoader.Instance.ReloadSceneWithLoading(true/false)`

- ✅ Nếu truyền `false`: **không gán callback replay**, dùng mặc định về màn hình chính (`DefaultOnHidden`)
- 🔁 Nếu truyền `true`: **gán callback replay** tương ứng với chế độ chơi hiện tại (`ReplayGame` hoặc `PVPReplayGame`)

📌 Callback sẽ được lưu tạm vào biến `staticOnLoadingComplete` để sử dụng sau khi load lại scene.

🎬 Khi scene được load lại:

- `Start()` sẽ tự động gọi `StartLoading()`
- `StartLoading()` kiểm tra `staticOnLoadingComplete`:
  - Nếu **null** → gán callback mặc định (`DefaultOnHidden`)
  - Nếu **đã có** → giữ nguyên, không thay đổi
- Gọi coroutine `LoadingCoroutine()` để hiển thị thanh loading
- Khi loading hoàn tất → gọi `staticOnLoadingComplete.Invoke()` và reset về `null`

  ##Dotween
  - DOTween trong Unity là một thư viện tweening giúp bạn tạo hiệu ứng chuyển động (animation) cho các giá trị (position, scale, rotation, màu sắc, số, v.v.) một cách mượt mà, ngắn gọn và dễ viết code hơn so với tự làm
  - Sequence trong dotween giúp chạy các chuyển động hoặc các phương thức 1 cách tuần tự hoặc song song với nhau
