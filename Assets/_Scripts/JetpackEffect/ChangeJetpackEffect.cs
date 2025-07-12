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

    public void Set(string jetpackEffectName)
    {
        var jetpackEffect = JetpackEffectManager.Instance.jetpackEffects.itemList
                        .FirstOrDefault(effect => effect.jetpackEffectName == jetpackEffectName &&
                                        effect.status == JetpackEffect.Status.Unlocked);

        if (jetpackEffect != null)
        {
            // Apply the material to the jetpack effect particle system
            var renderer = JetpackEffectManager.Instance.jetpackEffect.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.material = jetpackEffect.material;
        }
        else
        {
            // Apply default jetpack effect
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
