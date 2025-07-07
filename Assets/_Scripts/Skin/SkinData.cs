using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "Skins", menuName = "ScriptableObjects/Skins", order = 1)]
public class SkinData : ScriptableObject
{
    public List<Skin> skinList = new List<Skin>();
}

[System.Serializable]
public class Skin
{
    public enum Status
    {
        Unlocked,
        Locked
    }
    public SpriteLibraryAsset spriteLibraryAsset;
    public string skinName;
    public int price;
    public Status status = Status.Locked;
}