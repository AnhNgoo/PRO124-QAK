using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

/// <summary>
/// Quản lý cửa hàng chung <br/>
/// LoadShop: Tải dữ liệu item vào cửa hàng <br/>
/// LoadItem: Tải item hiện tại và cập nhật UI <br/>
/// SelectItem: Chọn item hiện tại <br/>
/// BuyItem: Mua item hiện tại <br/>
/// OnActionButtonClick: Gọi SelectItem/BuyItem <br/>
/// </summary>
public class GenericShopManager<T> where T : IShopItem
{
    public Action<string> OnSelectItem;
    public ShopItemData<T> itemData;
    public string currentItemName;

    /// <summary>
    /// LoadShop: Tải data cửa hàng và truyền hàm OnSelectItem để xử lý sự kiện chọn mặt hàng, lấy component Image, gọi LoadItem để load item đầu tiên <br/>
    /// </summary>
    public void LoadShop(ShopItemData<T> data, Action<string> OnSelectItem)
    {
        itemData = data;
        currentItemName = itemData.currentItemName;
        this.OnSelectItem = OnSelectItem;

        if (UIManager.Instance.itemUnlockButtonGameobject != null)
        {
            UIManager.Instance.itemUnlockButtonImage = UIManager.Instance.itemUnlockButtonGameobject.GetComponent<Image>();
        }

        LoadItem();
    }

    /// <summary>
    /// LoadItem: Tải item hiện tại và cập nhật UI (icon, name, trạng thái giá, trạng thái trên button)
    /// </summary>
    public void LoadItem()
    {
        var item = itemData.itemList.FirstOrDefault(i => i.Name == currentItemName)
                 ?? itemData.itemList.FirstOrDefault();


        if (item != null)
        {
            UIManager.Instance.itemImage.sprite = item.Icon;
            UIManager.Instance.itemNameText.text = item.Name;

            if (item.IsUnlocked)
            {
                UIManager.Instance.itemUnlockButtonImage.sprite = UIManager.Instance.unlockedSprite;
                UIManager.Instance.itemUnlockButtonText.text = "Select"; //trạng thái trên button

                if (currentItemName == itemData.currentItemName)
                {
                    UIManager.Instance.itemUnlockButtonGameobject.SetActive(false);
                    UIManager.Instance.itemPriceText.text = "Selected"; //trạng thái giá
                }
                else
                {
                    UIManager.Instance.itemUnlockButtonGameobject.SetActive(true);
                    UIManager.Instance.itemPriceText.text = "Owned"; //trạng thái giá
                }
            }
            else
            {
                UIManager.Instance.itemUnlockButtonImage.sprite = UIManager.Instance.lockedSprite;
                UIManager.Instance.itemUnlockButtonText.text = "Buy"; //trạng thái trên button
                UIManager.Instance.itemUnlockButtonGameobject.SetActive(true);
                UIManager.Instance.itemPriceText.text = item.Price.ToString(); //trạng thái giá
            }
        }
    }

    /// <summary>
    /// NextItem: Chuyển đến item tiếp theo trong danh sách, nếu đã đến cuối thì quay lại đầu <br/>
    /// </summary>
    public void NextItem()
    {
        if (itemData.itemList.Count == 0) return;

        int currentIndex = itemData.itemList.FindIndex(i => i.Name == currentItemName);
        if (currentIndex == -1) currentIndex = 0;

        int nextIndex = (currentIndex + 1) % itemData.itemList.Count;
        currentItemName = itemData.itemList[nextIndex].Name;
        LoadItem();
    }

    /// <summary>
    /// PreviousItem: Chuyển đến item trước đó trong danh sách, nếu đã đến đầu thì quay lại cuối <br/>
    /// </summary>
    public void PreviousItem()
    {
        if (itemData.itemList.Count == 0) return;

        int currentIndex = itemData.itemList.FindIndex(i => i.Name == currentItemName);
        if (currentIndex == -1) currentIndex = 0;

        int previousIndex = (currentIndex - 1 + itemData.itemList.Count) % itemData.itemList.Count;
        currentItemName = itemData.itemList[previousIndex].Name;
        LoadItem();
    }

    /// <summary>
    /// SelectItem: Chọn item hiện tại, thay skin và gọi LoadItem
    /// </summary>
    public void SelectItem()
    {
        itemData.currentItemName = currentItemName;
        UIManager.Instance.itemUnlockButtonGameobject.SetActive(false);
        OnSelectItem?.Invoke(currentItemName);
        LoadItem();
    }

    /// <summary>
    /// BuyItem: Mua item hiện tại nếu chưa được mở khóa
    /// </summary>
    public void BuyItem()
    {
        var item = itemData.itemList.FirstOrDefault(i => i.Name == currentItemName);
        if (item == null || item.IsUnlocked) return;

        if (GameManager.Instance.coinTotal >= item.Price)
        {
            // Phát SFX bắt đầu mua hàng
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("PurchaseStart");
            UIManager.Instance.itemUnlockButtonGameobject.SetActive(false);
            // Lưu giá trị ban đầu và target
            int startCoin = GameManager.Instance.coinTotal;
            int targetCoin = GameManager.Instance.coinTotal - item.Price;

            // Tạo hiệu ứng trừ tiền từ từ
            DOTween.To(() => (float)startCoin, x =>
            {
                int currentCoin = Mathf.RoundToInt(x);
                GameManager.Instance.coinTotal = currentCoin;

                // Cập nhật UI coin - sử dụng coinTotalText thay vì coinText
                if (UIManager.Instance.coinTotalText != null)
                    UIManager.Instance.coinTotalText.text = currentCoin.ToString();

            }, (float)targetCoin, 1f) // Cast to float
            .SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                // Khi hoàn thành việc trừ tiền
                item.IsUnlocked = true;
                SelectItem();
            });
        }
        else
        {
            UIManager.Instance.FailedPurchasePanelGameobject.SetActive(false);
            UIManager.Instance.FailedPurchasePanelGameobject.SetActive(true);
        }
    }

    /// <summary>
    /// OnActionButtonClick: Gọi hàm tương ứng với trạng thái của item hiện tại <br/>
    /// </summary>
    public void OnActionButtonClick()
    {
        var item = itemData.itemList.FirstOrDefault(i => i.Name == currentItemName);
        if (item == null) return;

        if (item.IsUnlocked)
            SelectItem();
        else
            BuyItem();
    }
}
