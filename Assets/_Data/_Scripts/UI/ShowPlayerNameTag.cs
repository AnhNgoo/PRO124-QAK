using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPlayerNameTag : MonoBehaviour
{
    private GameObject player;
    private GameObject nameTag;
    private TextMeshProUGUI nameTagText;
    private void Start()
    {
        player = transform.parent.gameObject;
        nameTag = transform.GetChild(0).gameObject;
        nameTagText = nameTag.GetComponent<TextMeshProUGUI>();

        Color color = RandomColor();
        nameTagText.color = color;
        // Set the name tag text to the player's name
        if (player != null)
            nameTagText.text = player.name;
    }

    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1f);
    }
}

//Show tên người chơi khi ở chế độ PVP