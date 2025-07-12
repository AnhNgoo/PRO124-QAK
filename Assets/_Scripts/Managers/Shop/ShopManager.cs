using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{

    private GenericShopManager<Skin> skinShop = new GenericShopManager<Skin>();
    private GenericShopManager<JetpackEffect> jetpackShop = new GenericShopManager<JetpackEffect>();

    private enum ShopType { Skin, Jetpack }
    private ShopType currentShop;

    public void LoadShop()
    {
        ShowSkinShop(); // mặc định là skin
    }

    public void ShowSkinShop()
    {
        currentShop = ShopType.Skin;
        skinShop.LoadShop(SkinManager.Instance.skins, SetSkin);
    }

    public void ShowJetpackShop()
    {
        currentShop = ShopType.Jetpack;
        jetpackShop.LoadShop(JetpackEffectManager.Instance.jetpackEffects, SetJetpackEffect);
    }

    public void OnNext()
    {
        if (currentShop == ShopType.Skin)
            skinShop.NextItem();
        else
            jetpackShop.NextItem();
    }

    public void OnPrevious()
    {
        if (currentShop == ShopType.Skin)
            skinShop.PreviousItem();
        else
            jetpackShop.PreviousItem();
    }

    public void OnActionClick()
    {
        if (currentShop == ShopType.Skin)
            skinShop.OnActionButtonClick();
        else
            jetpackShop.OnActionButtonClick();
    }

    private void SetSkin(string skinName)
    {
        SkinManager.Instance.changeSkin.Set(skinName);
    }

    private void SetJetpackEffect(string effectName)
    {
        JetpackEffectManager.Instance.changeJetpackEffect.Set(effectName);
    }
}
