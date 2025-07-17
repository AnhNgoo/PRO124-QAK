using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataGame
{
    public DataPlayer playerData = new DataPlayer();

    public List<DataSkin> skins = new List<DataSkin>();
    public string currentSkinName;

    public List<DataJetpack> jetpacks = new List<DataJetpack>();
    public string currentJetpackEffectName;

    public DataSettings settings = new DataSettings();
}

[System.Serializable]
public class DataPlayer
{
    public int coinTotal;
    public int distanceBest;
}

[System.Serializable]
public class DataSkin
{
    public enum Status
    {
        Unlocked,
        Locked
    }

    public string skinName;
    public Status status;
}

[System.Serializable]
public class DataJetpack
{
    public enum Status
    {
        Unlocked,
        Locked
    }

    public string jetpackEffectName;
    public Status status;
}

[System.Serializable]
public class DataSettings
{
    public float musicVolume;
    public float sfxVolume;
}

