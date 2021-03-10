using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerState: MonoBehaviour
{
    private Animator anim;
    public int Health = 10;

    // This actually might be handles by the animator already,
    // based on the length of the Exit time of the animation state 'Hit'

    public bool Hurt = false; // Hurt handles whether the character is invincible. (invincibility frames)
    public float invincibleTime = 1.5f;

    public void TakeDamage(int amount)
    {
        AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo(0);
        if (!astate.IsName("hit") && !astate.IsName("Death")){
            Health = Health -= amount;
            if (Health < 0)
            {
                onDeath();
                return;
            } 
            else 
            {
                Timer timer = new Timer();
                timer.setTimer(invincibleTime, notHurtAnymore);
            }
        }
    }

    public void onDeath()
    {
        // tell the animator you are dead.
        anim.SetBool("Dead", true);

        /* And here's where I'd put my respawn logic!*
         * 
         * ... IF I HAD ANY!
         * */
    }

    public void notHurtAnymore()
    {
        Hurt = false;
    }

}
