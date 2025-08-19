using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeJetpackEffect : MonoBehaviour
{
    void Start()
    {
        Set(JetpackEffectManager.Instance.currentJetpackEffectName);
    }

    /// <summary>
    /// Thay đổi jetpackeffect cho nhân vật theo tên <br/>
    /// Lấy jetpackeffect trong JetpackEffectData theo tên và đã mở khoá <br/>
    /// đặt lại tên jetpack hiện tại <br/>
    /// </summary>
    public void Set(string jetpackEffectName)
    {
        var jetpackEffect = JetpackEffectManager.Instance.jetpackEffects.itemList
                        .FirstOrDefault(effect => effect.jetpackEffectName == jetpackEffectName &&
                                        effect.status == JetpackEffect.Status.Unlocked);

        if (jetpackEffect != null)
        {
            var renderer = JetpackEffectManager.Instance.jetpackEffect.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.material = jetpackEffect.material;
        }
        else
        {
            var defaultEffect = JetpackEffectManager.Instance.jetpackEffects.itemList
                                                      .FirstOrDefault(effect => effect.jetpackEffectName == "Default");
            if (defaultEffect != null)
            {
                var renderer = JetpackEffectManager.Instance.jetpackEffect.GetComponent<ParticleSystemRenderer>();
                if (renderer != null)
                    renderer.material = defaultEffect.material;
            }
        }

        JetpackEffectManager.Instance.currentJetpackEffectName = jetpackEffectName;
    }
}
