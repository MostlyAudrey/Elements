using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LowHealthVignette : MonoBehaviour
{
    [Tooltip("Set to already existing global postprocess volume, otherwise will create its own")]
    public PostProcessVolume m_Volume;
    public PlayerState m_PlayerState;
    public float maxIntensity = 0.5f;

    private Vignette m_Vignette;
    private int oldHealth = -1;

    void Start()
    {
        if (m_Volume == null)
        {
            m_Vignette = ScriptableObject.CreateInstance<Vignette>();
            m_Vignette.enabled.Override(true);
            m_Vignette.intensity.Override(0f);
            m_Vignette.color.Override(Color.red);
            m_Vignette.smoothness.Override(0.4f);

            m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);
        }
        else
        {
            m_Volume.profile.TryGetSettings(out m_Vignette);
            m_Vignette.enabled.Override(true);
            m_Vignette.intensity.Override(0f);
        }
    }

    void Update()
    {
        int currHealth = m_PlayerState.Health;
        if (currHealth != oldHealth)
        {
            oldHealth = currHealth;

            float healthNormalized = Mathf.Clamp01(((float) currHealth) / m_PlayerState.maxHealth);
            //Use 1-x so that lower health results in greater FX intensity
            m_Vignette.intensity.value = maxIntensity * (1f - healthNormalized);
        }
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}
