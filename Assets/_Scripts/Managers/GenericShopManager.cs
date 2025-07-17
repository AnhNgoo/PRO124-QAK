using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GenericShopManager<T> where T : IShopItem
{
    public delegate void selectItem(string itemName);
    public selectItem OnSelectItem;
    public ShopItemData<T> itemData;
    public string currentItemName;

    public void LoadShop(ShopItemData<T> data, selectItem OnSelectItem)
    {
        itemData = data;
        currentItemName = data.currentItemName;
        this.OnSelectItem = OnSelectItem;

        if (UIManager.Instance.itemUnlockButtonGameobject != null)
        {
            UIManager.Instance.itemUnlockButtonImage = UIManager.Instance.itemUnlockButtonGameobject.GetComponent<Image>();
        }

        LoadItem();
    }

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
                UIManager.Instance.itemUnlockButtonText.text = "Select";

                if (currentItemName == itemData.currentItemName)
                {
                    UIManager.Instance.itemUnlockButtonGameobject.SetActive(false);
                    UIManager.Instance.itemPriceText.text = "Selected";
                }
                else
                {
                    UIManager.Instance.itemUnlockButtonGameobject.SetActive(true);
                    UIManager.Instance.itemPriceText.text = "Owned";
                }
            }
            else
            {
                UIManager.Instance.itemUnlockButtonImage.sprite = UIManager.Instance.lockedSprite;
                UIManager.Instance.itemUnlockButtonText.text = "Buy";
                UIManager.Instance.itemUnlockButtonGameobject.SetActive(true);
                UIManager.Instance.itemPriceText.text = item.Price.ToString();
            }
        }
    }

    public void NextItem()
    {
        if (itemData.itemList.Count == 0) return;

        int currentIndex = itemData.itemList.FindIndex(i => i.Name == currentItemName);
        if (currentIndex == -1) currentIndex = 0;

        int nextIndex = (currentIndex + 1) % itemData.itemList.Count;
        currentItemName = itemData.itemList[nextIndex].Name;
        LoadItem();
    }

    public void PreviousItem()
    {
        if (itemData.itemList.Count == 0) return;

        int currentIndex = itemData.itemList.FindIndex(i => i.Name == currentItemName);
        if (currentIndex == -1) currentIndex = 0;

        int previousIndex = (currentIndex - 1 + itemData.itemList.Count) % itemData.itemList.Count;
        currentItemName = itemData.itemList[previousIndex].Name;
        LoadItem();
    }

    public void SelectItem()
    {
        itemData.currentItemName = currentItemName;
        UIManager.Instance.itemUnlockButtonGameobject.SetActive(false);
        OnSelectItem?.Invoke(currentItemName);
        LoadItem();
    }

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
