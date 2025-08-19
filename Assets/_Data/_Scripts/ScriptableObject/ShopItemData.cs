using UnityEngine;
using System.Collections.Generic;

public class ShopItemData<T> : ScriptableObject where T : IShopItem
{
    public List<T> itemList;
    public string currentItemName;
}

//Class chung cho các ScriptableObject như skin, jetpackeffect
//Khi tạo các ScriptableObject mới như skindata, jetpackeffectdata, chỉ cần kế thừa từ lớp này và truyền kiểu T tương ứng
//Kiểu T là 1 class model định nghĩa các thuộc tính cho skin, jetpackeffect, được implement IShopItem để có các thuộc tính chung
//T truyền vào bắt buộc phải kế thừa từ IShopItem
