using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData", order = 1)]
public class PowerUpData : ScriptableObject
{
    public List<PowerUp> powerUps;
}

[System.Serializable]
public class PowerUp
{
    public Sprite icon;
    public string powerUpName;
    public string description;
    public float duration; // Thời gian hiệu lực của PowerUp
    public int level;
    public int exp;
    public int maxLevel;
}