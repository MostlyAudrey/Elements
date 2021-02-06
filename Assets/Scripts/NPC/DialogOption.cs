using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOption : Interactable
{
    //public float textSpeed = 1f;
    public float textBreakTime = 1f;
	public string[] messageText;
    public AudioClip[] audioClips;

    private int currAudioClip = -1;

    private bool isTalking = false;
    private bool playingAudio = false;
    private float talkPauseTimer = 0f;

    private AudioSource audioPlayer;

	void Start()
	{
        audioPlayer = GetComponent<AudioSource>();
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
      		_displayButtonHint();
      	else if ( showingHint && Vector3.Distance(target.transform.position, transform.position) > buttonHintRadius )
      		_hideButtonHint();

        if ( !isTalking && !gettingAttention && Vector3.Distance(target.transform.position, transform.position) <= getPlayerAttentionRadius && getPlayerAttention)
        {
            EventManager.instance.onActionButtonPressed += _startTalking;
            _startWaving();
        }
        else if ( gettingAttention && Vector3.Distance(target.transform.position, transform.position) > getPlayerAttentionRadius )
        {
            EventManager.instance.onActionButtonPressed -= _startTalking;
            _stopWaving();
        }
        if ( playingAudio && !audioPlayer.isPlaying)
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
            time_per_char = (audioClips[index].length)/ char_count;
            EventManager.instance.DisplayText(message, time_per_char, textBreakTime);
            index++;
        }
        _playNextAudioClip();
    }

    void _finishTalking()
    {
        Debug.Log("Here 2: " + progressesQuest);
        AdvanceQuest();
        _stopTalking();
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
        if ( currAudioClip  == audioClips.Length )
        {
            audioPlayer.clip = null;
            currAudioClip = -1;
            playingAudio = false;
            audioPlayer.Stop();
            Debug.Log("Here");
            _finishTalking();
        }
        else
        {
            playingAudio = true;
            audioPlayer.clip = audioClips[currAudioClip];
            audioPlayer.Play();
        }
    }
    
}
