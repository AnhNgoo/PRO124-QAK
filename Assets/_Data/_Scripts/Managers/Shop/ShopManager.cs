using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý cửa hàng trong game <br/>
/// GenericShopManager truyền T (skin, jetpackeffect) vào nên skinShop, jetpackShop có thể gọi các hàm tương tác với shop như nhau <br/>
/// LoadShop: Khi mới bật ShopPanel, hàm này sẽ được tự động gọi để hiển thị shopskin <br/>
/// ShowSkinShop: đặt cửa hàng skin làm cửa hàng hiện tại, gọi hàm LoadShop của skinShop và truyền hàm SetSkin để thay skin <br/>
/// ShowJetpackShop: đặt cửa hàng jetpack làm cửa hàng hiện tại, gọi hàm LoadShop của jetpackShop và truyền hàm SetJetpackEffect để thay jetpack effect <br/>
/// OnNext, OnPrevious, OnActionClick: gọi hàm tương ứng của cửa hàng hiện tại
/// Khi truyền hàm SetSkin, SetJetpackEffect vào GenericShopManager, nó sẽ gán hàm cho Action<string> OnSelectItem để gọi thay skin mỗi khi chọn xong
/// </summary>
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
