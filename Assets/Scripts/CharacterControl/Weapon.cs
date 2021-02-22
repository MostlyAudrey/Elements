using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int damage = 25;

    private Animator parentAnimator;

    void Start() {
        parentAnimator = GetComponentInParent(typeof(Animator)) as Animator;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if ( parentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && other.CompareTag("Breakable"))
        {
            Breakable breakable = other.transform.gameObject.GetComponent<Breakable>();
            breakable.TakeDamage(damage);
        }

        if ( parentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && other.CompareTag("Enemy") && (other.GetType() == typeof(CapsuleCollider)))
        {
            MobAI enemyAI = other.transform.gameObject.GetComponent<MobAI>();
            enemyAI.TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
