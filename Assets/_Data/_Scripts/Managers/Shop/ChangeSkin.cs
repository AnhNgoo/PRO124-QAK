using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ChangeSkin : MonoBehaviour
{
    void Start()
    {
        Set(SkinManager.Instance.currentSkinName);
    }

    /// <summary>
    /// Thay đổi skin cho nhân vật theo tên <br/>
    /// Lấy skin trong SkinData theo tên và đã mở khoá <br/>
    /// đặt lại tên skin hiện tại 
    /// </summary>
    public void Set(string skinName)
    {
        var skin = SkinManager.Instance.skins.itemList
                        .FirstOrDefault(skin => skin.skinName == skinName &&
                                        skin.status == Skin.Status.Unlocked);

        if (skin != null)
            SkinManager.Instance.spriteLibrary.spriteLibraryAsset = skin.spriteLibraryAsset;
        else
            SkinManager.Instance.spriteLibrary.spriteLibraryAsset = SkinManager.Instance.skins.itemList
                                                      .FirstOrDefault(skin => skin.skinName == "JetBoy")
                                                      .spriteLibraryAsset;
        SkinManager.Instance.currentSkinName = skinName;
    }
}
