using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{

    void Start()
    {
        Set(SkinManager.Instance.currentSkinName);
    }

    public void Set(string skinName)
    {
        var skin = SkinManager.Instance.skins.skinList
                        .FirstOrDefault(skin => skin.skinName == skinName &&
                                        skin.status == Skin.Status.Unlocked);

        if (skin != null)
            SkinManager.Instance.spriteLibrary.spriteLibraryAsset = skin.spriteLibraryAsset;
        else
            SkinManager.Instance.spriteLibrary.spriteLibraryAsset = SkinManager.Instance.skins.skinList
                                                      .FirstOrDefault(skin => skin.skinName == "Default")
                                                      .spriteLibraryAsset;
        SkinManager.Instance.currentSkinName = skinName;
    }
}
