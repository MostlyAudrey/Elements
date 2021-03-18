using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHUD : MonoBehaviour
{
    public PostProcessVolume globalPostProcessVol;
    public float maxLowHealthFXIntensity = 0.5f;

    private Vignette lowHealthVignette;
    private float oldHealth;

    void Start()
    {
        globalPostProcessVol.profile.TryGetSettings(out lowHealthVignette);
        lowHealthVignette.intensity.value = 0f;
    }

    void Update()
    {
        // // get player health here
        // float maxHealth = 5f;
        // float currHealth = 5f - Time.time % 5f;
        // if (currHealth != oldHealth)
        // {
        //     oldHealth = currHealth;
        //     //Normalize current health and get 1-x so that lower health results in greater intensity
        //     lowHealthVignette.intensity.value = maxLowHealthFXIntensity * (1f - currHealth / maxHealth);
        // }
    }
}
