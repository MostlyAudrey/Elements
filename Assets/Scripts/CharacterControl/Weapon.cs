using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Weapon : MonoBehaviour
{

    public int damage = 25;

    private Animator parentAnimator;
    private FMOD.Studio.EventInstance hitAudio;

    void Start() {
        parentAnimator = GetComponentInParent(typeof(Animator)) as Animator;
        hitAudio = RuntimeManager.CreateInstance("event:/Weapons/Sword Hit");
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if ( parentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && other.CompareTag("Breakable"))
        {
            Breakable breakable = other.transform.gameObject.GetComponent<Breakable>();
            breakable.TakeDamage(damage);
            hitAudio.start();
        }

        if ( parentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && other.CompareTag("Enemy") && (other.GetType() == typeof(CapsuleCollider)))
        {
            MobAI enemyAI = other.transform.gameObject.GetComponent<MobAI>();
            enemyAI.TakeDamage(damage);
            hitAudio.start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
