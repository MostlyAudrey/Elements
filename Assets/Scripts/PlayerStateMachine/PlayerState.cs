using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerState : MonoBehaviour
{
    private Animator anim;
    public int Health = 100; // Default health. 
    public int maxHealth = 100;

	public void Start()
	{
        anim = gameObject.GetComponent<Animator>();
	}
	public void TakeDamage(int amount)
    {
        AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo(0);
        if (!astate.IsName("hit") && !astate.IsName("Death"))
        {
            anim.SetTrigger("hit");
            Debug.Log("Took Damage");
            Health = Health -= amount;
            if (Health < 0)
            {
                onDeath();
                return;
            }
        }
    }

    public void onDeath()
    {
        // tell the animator you are dead.
        anim.SetBool("Dead", true);
        Debug.Log("Player Dead");
        /* And here's where I'd put my respawn logic!*
        * 
        * ... IF I HAD ANY!
        * */
    }
}


