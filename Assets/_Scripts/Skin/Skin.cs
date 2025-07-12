using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class Skin : IShopItem
{
    public enum Status
    {
        Unlocked,
        Locked
    }
    public SpriteLibraryAsset spriteLibraryAsset;
    public Sprite sprite;
    public string skinName;
    public int price;
    public Status status = Status.Locked;

    // Implement IShopItem interface
    public string Name => skinName;
    public int Price => price;
    public Sprite Icon => sprite;
    public bool IsUnlocked
    {
        get => status == Status.Unlocked;
        set => status = value ? Status.Unlocked : Status.Locked;
    }
}