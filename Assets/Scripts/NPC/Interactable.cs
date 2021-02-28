using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Interactable : QuestTrigger
{
    //only 1 dialog option can be displayed at one time, the lower the rank the higher its priority
    public int rank = 0;
	public bool getPlayerAttention = true;
	public float getPlayerAttentionRadius = 30.0f;

    // start as soon as the interactable is activated 
    public bool startImmediately = false;

  	protected GameObject target;
    protected Animator anim;

    protected bool gettingAttention = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        if ( buttonHint ) buttonHint.SetActive( false );
    }

    protected void _startWaving()
    {
        gettingAttention = true;
        anim.SetBool("waving", true);
    }

    protected void _stopWaving()
    {
        gettingAttention = false;
        anim.SetBool("waving", false);
    }
}