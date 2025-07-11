using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    //------------------------------------------
    [Header("\t\t----UI Panels----")]
    public GameObject MainPanelGameobject;
    public GameObject SettingPanelGameobject;
    public GameObject PausePanelGameobject;
    public GameObject ShopPanelGameobject;
    public GameObject InGamePanelGameobject;


    //------------------------------------------
    [Header("-----UI Main-----")]
    public GameObject playButtonGameobject; // Nút chơi game
    public GameObject shopButtonGameobject; // Nút vào shop
    public GameObject settingButtonGameobject; // Nút vào setting
    public GameObject quitButtonGameobject; // Nút thoát game

    //Dotween Animation main
    public DOTweenAnimation playButtonDQ { get; private set; }
    public DOTweenAnimation shopButtonDQ { get; private set; }
    public DOTweenAnimation settingButtonDQ { get; private set; }
    public DOTweenAnimation quitButtonDQ { get; private set; }


    //------------------------------------------
    [Header("-----UI Setting-----")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    //Dotween Animation setting
    public DOTweenAnimation settingPanelDQ { get; private set; }


    //------------------------------------------
    [Header("-----UI Pause-----")]
    public int temp = 0; // Biến tạm để tránh lỗi không sử dụng
    // Dotween Animation pause
    public DOTweenAnimation pausePanelDQ { get; private set; }


    //------------------------------------------
    [Header("-----UI InGame-----")]


    //------------------------------------------
    [Header("-----UI Shop-----")]
    public Sprite lockedSprite;   // Sprite cho trạng thái khóa
    public Sprite unlockedSprite; // Sprite cho trạng thái mở khóa 

    [Header("UI Properties")]
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;

    [Header("Button")]
    public GameObject itemUnlockButtonGameobject;
    public TextMeshProUGUI itemUnlockButtonText; // Text component của nút
    public Image itemUnlockButtonImage { get; set; } // Image component của nút
    [Header("Dotween Animation Gameobject")]
    public GameObject menuShopGameobject;
    public GameObject showItemPanelGameobject;
    public GameObject itemStatusGameobject;

    // //Dotween Animation Shop
    public DOTweenAnimation menuShopDQ { get; private set; }
    public DOTweenAnimation showItemPanelDQ { get; private set; }
    public DOTweenAnimation itemStatusDQ { get; private set; }

    //------------------------------------------
    [Header("-----Loading UI-----")]
    public Slider loadingSlider;
    public TextMeshProUGUI percentageText;
    public GameObject loadingPanel;

    [Header("Loading Settings")]
    public float minLoadingTime = 2f; // Thời gian loading tối thiểu
    public float maxLoadingTime = 4f; // Thời gian loading tối đa
    public AnimationCurve loadingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Đường cong loading mượt mà


    //Enum
    public enum StatePanel
    {
        Main,
        Pause,
    }
    public StatePanel statePanel { get; set; } = StatePanel.Main;

    private void Start()
    {
        GetComponentDotween();
    }

    private void GetComponentDotween()
    {
        //Main
        playButtonDQ = playButtonGameobject.GetComponent<DOTweenAnimation>();
        shopButtonDQ = shopButtonGameobject.GetComponent<DOTweenAnimation>();
        settingButtonDQ = settingButtonGameobject.GetComponent<DOTweenAnimation>();
        quitButtonDQ = quitButtonGameobject.GetComponent<DOTweenAnimation>();

        //Setting
        settingPanelDQ = SettingPanelGameobject.GetComponent<DOTweenAnimation>();

        //Pause
        pausePanelDQ = PausePanelGameobject.GetComponent<DOTweenAnimation>();

        //Shop
        menuShopDQ = menuShopGameobject.GetComponent<DOTweenAnimation>();
        showItemPanelDQ = showItemPanelGameobject.GetComponent<DOTweenAnimation>();
        itemStatusDQ = itemStatusGameobject.GetComponent<DOTweenAnimation>();
    }
}
