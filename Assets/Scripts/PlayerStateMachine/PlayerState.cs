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
    public bool Hurt = false;

    public void TakeDamage(int amount)
    {
        AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo(0);
        if (! astate.IsName("hit")){
            Health = Health -= amount;
            if (Health < 0)
            {
                onDeath();
                return;
            } else
            {
                Health--;
                Timer timer = new Timer();
                timer.setTimer(90f,notHurtAnymore);
                timer.active = true;
            }
        }
    }

    public void onDeath()
    {
        // tell the animator you are dead.
        anim.SetBool("Dead", true);
    }

    public void notHurtAnymore()
    {
        Hurt = true;
    }

}
