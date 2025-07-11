using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("-----UI Shop-----")]
    public Sprite lockedSprite;   // Sprite cho trạng thái khóa
    public Sprite unlockedSprite; // Sprite cho trạng thái mở khóa 

    [Header("Show Properties In List")]
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;

    [Header("Button")]
    public GameObject itemUnlockButtonGameobject;
    public TextMeshProUGUI itemUnlockButtonText; // Text component của nút
    public Image itemUnlockButtonImage { get; set; } // Image component của nút



}
