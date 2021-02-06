using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class Interactable : MonoBehaviour
{
    //only 1 dialog option can be displayed at one time, the lower the rank the higher its priority
    public int rank = 0;
	public GameObject buttonHint;
	public bool showButtonHint = true;
	public float buttonHintRadius = 15.0f;
	public bool getPlayerAttention = true;
	public float getPlayerAttentionRadius = 30.0f;

    // start as soon as the interactable is activated 
    public bool startImmediately = false;

    public QuestName progressesQuest;

    // If set to anything other than -1 the interactable will be active until the Quest is in a greater phase
    public int progressesQuestToPhase = -1;
  	protected GameObject target;
    protected Animator anim;

    protected bool showingHint = false;
    protected bool gettingAttention = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        buttonHint.SetActive( false );
    }

    protected void _displayButtonHint()
    {
    	showingHint = true;
        if (showButtonHint) buttonHint.SetActive( true );
    }

    protected void _hideButtonHint()
    {
    	showingHint = false;
		if (showButtonHint) buttonHint.SetActive( false );
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

    protected void AdvanceQuest()
    {
        Debug.Log("AdvanceQuest");
        if ( progressesQuest != QuestName.None )
            if ( progressesQuestToPhase == -1)
                QuestManager.ProgressQuest(progressesQuest);
            else
                QuestManager.ProgressQuestToPhase(progressesQuest, progressesQuestToPhase);
    }

}