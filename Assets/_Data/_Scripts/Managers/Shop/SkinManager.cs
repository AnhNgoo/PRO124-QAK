using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Linq;

/// <summary>
/// Quản lý skin cho nhân vật <br/>
/// Lấy spriteLibrary của player để thay đổi skin <br/>
/// lấy ChangeSkin để gọi hàm thay skin <br/>
/// Set tên skin hiện tại trong SkinData
/// </summary>
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
        spriteLibrary = GameObject.Find("Player 1").GetComponent<SpriteLibrary>();
        changeSkin = GetComponent<ChangeSkin>();
        currentSkinName = skins.currentItemName;
    }
}

