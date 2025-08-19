using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackEffectManager : Singleton<JetpackEffectManager>
{
    [Header("Jetpack Effect Data")]
    public JetpackEffectData jetpackEffects;

    [Header("Current Settings")]
    public string currentJetpackEffectName;

    public ChangeJetpackEffect changeJetpackEffect { get; set; }
    public ParticleSystem jetpackEffect { get; set; }

    private void Start()
    {
        GetComponent();
    }

    private void GetComponent()
    {
        jetpackEffect = GameObject.Find("JetpackEffect").GetComponent<ParticleSystem>();
        changeJetpackEffect = GetComponent<ChangeJetpackEffect>();
        currentJetpackEffectName = jetpackEffects.currentItemName;
    }
}

/// <summary>
/// Quản lý jetpackeffect cho nhân vật
/// Lấy particleSystem của player để thay đổi jetpackeffect
/// lấy ChangeJetpackEffect để gọi hàm thay jetpackeffect
/// Set tên jetpackeffect hiện tại trong JetpackEffectData
/// </summary>