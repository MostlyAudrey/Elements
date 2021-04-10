using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LowHealthVignette : MonoBehaviour
{
    [Tooltip("Set to already existing global postprocess volume, otherwise will create its own")]
    public PostProcessVolume m_Volume;
    public PlayerState m_PlayerState;
    public float maxRoundness = 1f;
    public float exp_coeff = -4f;

    private Vignette m_Vignette;
    private int oldHealth = -1;

    void Start()
    {
        if (m_Volume == null)
        {
            m_Vignette = ScriptableObject.CreateInstance<Vignette>();
            m_Vignette.enabled.Override(true);
            m_Vignette.intensity.Override(0.5f);
            m_Vignette.color.Override(Color.red);
            m_Vignette.smoothness.Override(0.25f);
            m_Vignette.roundness.Override(0f);

            m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);
        }
        else
        {
            m_Volume.profile.TryGetSettings(out m_Vignette);
            m_Vignette.enabled.Override(true);
            m_Vignette.roundness.Override(0f);
        }
    }

    void Update()
    {
        int currHealth = m_PlayerState.Health;
        if (currHealth != oldHealth)
        {
            oldHealth = currHealth;

            float healthNormalized = Mathf.Clamp01(((float) currHealth) / m_PlayerState.maxHealth);
            //Use 1-x so that lower health results in greater FX roundness
            m_Vignette.roundness.value = Mathf.Clamp(
                maxRoundness * (1f - Mathf.Exp((1f - healthNormalized) * exp_coeff)),
                0f,
                maxRoundness
            );
        }
    }

    void OnDestroy()
    {
        if (m_Volume != null)
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }
}
