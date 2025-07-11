using UnityEngine;
using System.Collections.Generic;

public class ShopItemData<T> : ScriptableObject where T : IShopItem
{
    public List<T> itemList;
    public string currentItemName;
}
