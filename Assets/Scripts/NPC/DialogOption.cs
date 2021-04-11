using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DialogOption : Interactable
{
    //public float textSpeed = 1f;
    public float textBreakTime = 1f;
	public string[] messageText;
    public int[] audioIndices;
    public float[] audioLengths;
    public string eventPath;
    public bool repeatable = false;
    public bool darkmode = false;

    private int currAudioClip = -1;

    private bool isTalking = false;
    private bool playingAudio = false;
    private float talkPauseTimer = 0f;

    // private AudioSource audioPlayer;
    private FMOD.Studio.EventInstance eventInstance;

	void Start()
	{
        // audioPlayer = GetComponent<AudioSource>();
        
        if (messageText.Length != audioIndices.Length || messageText.Length != audioLengths.Length ) throw new Exception("messageText, audioIndicies, and audioLengths should all be the same size");


        try {
            eventInstance = RuntimeManager.CreateInstance(eventPath);
        } catch (EventNotFoundException) {
            Debug.Log("Event not found.");
        }

        if ( !buttonHint ) showButtonHint = false;
        
        if (showButtonHint) _hideButtonHint();
        anim = GetComponent<Animator>();

        if ( startImmediately )
        {
            _startTalking();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( !target ) target = GameObject.FindGameObjectWithTag("Player");
        if ( !anim ) anim = GetComponent<Animator>();

        if ( !isTalking && !showingHint && Vector3.Distance(target.transform.position, transform.position) <= buttonHintRadius)
        {
            EventManager.instance.onActionButtonPressed += _startTalking;
      		_displayButtonHint();
        }    
      	else if ( showingHint && Vector3.Distance(target.transform.position, transform.position) > buttonHintRadius )
      	{
            EventManager.instance.onActionButtonPressed -= _startTalking;
            _hideButtonHint();
        }
        if ( !isTalking && !gettingAttention && Vector3.Distance(target.transform.position, transform.position) <= getPlayerAttentionRadius && getPlayerAttention)
        {
            _startWaving();
        }
        else if ( gettingAttention && Vector3.Distance(target.transform.position, transform.position) > getPlayerAttentionRadius )
        {
            _stopWaving();
        }
        FMOD.Studio.PLAYBACK_STATE audioState;
        eventInstance.getPlaybackState(out audioState);
        if ( playingAudio && audioState == FMOD.Studio.PLAYBACK_STATE.STOPPED)//!audioPlayer.isPlaying)
        {
            if ( talkPauseTimer >= textBreakTime )
                _playNextAudioClip();
            else 
                talkPauseTimer += Time.deltaTime;
        }

    }

    void _startTalking()
    {
        EventManager.instance.onActionButtonPressed -= _startTalking;
        isTalking = true;
        _stopWaving();
        _hideButtonHint();
        EventManager.instance.onActionButtonPressed += _stopTalking;
        anim.SetBool( "talking_happy", true );
        int index = 0;
        float time_per_char = 0f;
        foreach ( string message in messageText ) {
            int char_count = message.Length - (message.Split(' ').Length - 1);
            time_per_char = (audioLengths[index])/ char_count;
            EventManager.instance.DisplayText(message, time_per_char, textBreakTime, darkmode);
            index++;
        }
        _playNextAudioClip();
    }

    void _finishTalking()
    {
        _stopTalking();
        if (!repeatable) 
        {
            NPC npc = gameObject.GetComponent<NPC>();
            int index = npc.interactables.IndexOf(this);
            npc.endQuestPhase[index] = QuestManager.GetQuestPhase( npc.quests[index] );
        }        
        AdvanceQuest();
    }

    void _stopTalking()
    {
        EventManager.instance.onActionButtonPressed -= _stopTalking;
        isTalking = false;
        anim.SetBool( "talking_happy", false );
    }

    void _playNextAudioClip()
    {
        talkPauseTimer = 0f;
        currAudioClip += 1;
        if ( audioIndices.Length == 0 || currAudioClip == audioIndices.Length )
        {
            // audioPlayer.clip = null;
            currAudioClip = -1;
            playingAudio = false;
            // audioPlayer.Stop();
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _finishTalking();
        }
        else
        {
            playingAudio = true;
            // audioPlayer.clip = audioClips[currAudioClip];
            // audioPlayer.Play();
            
            eventInstance.setParameterByName("Dialogue Option", audioIndices[currAudioClip]);
            
            eventInstance.start();
            
        }
    }
    
}
