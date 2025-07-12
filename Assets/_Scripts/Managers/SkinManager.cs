using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Linq;

public class SkinManager : Singleton<SkinManager>
{
    public SkinData skins;
    public string currentSkinName;
    public SpriteLibrary spriteLibrary { get; set; }
    public ChangeSkin changeSkin { get; set; }


    private void Start()
    {
        GetComponent();
    }

    private void GetComponent()
    {
        spriteLibrary = GameObject.Find("Player").GetComponent<SpriteLibrary>();
        changeSkin = GetComponent<ChangeSkin>();
        currentSkinName = skins.currentItemName;
    }
}
