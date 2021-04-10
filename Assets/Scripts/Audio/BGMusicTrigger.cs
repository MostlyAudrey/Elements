using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicTrigger : MonoBehaviour
{
    public string eventName;
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            print("Music Trigger");
            print(eventName);
            BGMusic.instance.ChangeTrack(eventName);
        }
    }
}
