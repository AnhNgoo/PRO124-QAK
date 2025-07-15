using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
            GameManager.Instance.coinTotal -= item.Price;
            item.IsUnlocked = true;
            SelectItem();
        }
        else
        {
            Debug.Log("Not enough coins!");
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
