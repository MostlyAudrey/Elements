using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicTrigger : MonoBehaviour
{
    public string eventName;
    public bool always_playing = false;

    void Start()
    {
        if (always_playing)
        {
            BGMusic.instance.ChangeTrack(eventName);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            BGMusic.instance.ChangeTrack(eventName);
        }
    }
}
