
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class JetpackEffect : IShopItem
{
    public enum Status
    {
        Unlocked,
        Locked
    }

    public Material material;
    public Sprite sprite;
    public string jetpackEffectName;
    public int price;
    public Status status = Status.Locked;

    // Implement IShopItem interface
    public string Name => jetpackEffectName;
    public int Price => price;
    public Sprite Icon => sprite;
    public bool IsUnlocked
    {
        get => status == Status.Unlocked;
        set => status = value ? Status.Unlocked : Status.Locked;
    }
}