using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class DialogOption : MonoBehaviour
{
	private GameObject target;
    //only 1 dialog option can be displayed at one time, the lower the rank the higher its priority
    public int rank = 0;
	public GameObject buttonHint;
	public bool showButtonHint = true;
	public float buttonHintRadius = 15.0f;
	public bool getPlayerAttention = true;
	public float getPlayerAttentionRadius = 30.0f;

    public QuestName progressesQuest;

    //public float textSpeed = 1f;
    public float textBreakTime = 1f;
	public string[] messageText;
    public AudioClip[] audioClips;

    private int currAudioClip = -1;

	private bool showingHint = false;
    private bool gettingAttention = false;

    private bool isTalking = false;
    private bool playingAudio = false;
    private float talkPauseTimer = 0f;
    private Animator anim;

    private AudioSource audioPlayer;

	void Start()
	{
        target = GameObject.FindGameObjectWithTag("Player");
		buttonHint.SetActive( false );
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !isTalking && !showingHint && Vector3.Distance(target.transform.position, transform.position) <= buttonHintRadius)
      		_displayButtonHint();
      	else if ( showingHint && Vector3.Distance(target.transform.position, transform.position) > buttonHintRadius )
      		_hideButtonHint();

        if ( !isTalking && !gettingAttention && Vector3.Distance(target.transform.position, transform.position) <= getPlayerAttentionRadius && getPlayerAttention)
            _startWaving();
        else if ( gettingAttention && Vector3.Distance(target.transform.position, transform.position) > getPlayerAttentionRadius )
            _stopWaving();

        if ( playingAudio && !audioPlayer.isPlaying)
        {
            if ( talkPauseTimer >= textBreakTime )
                _playNextAudioClip();
            else 
                talkPauseTimer += Time.deltaTime;
        }

    }

    void _displayButtonHint()
    {
    	showingHint = true;
        if (showButtonHint) buttonHint.SetActive( true );
    }

    void _hideButtonHint()
    {
    	showingHint = false;
		if (showButtonHint) buttonHint.SetActive( false );
    }

    void _startWaving()
    {
        EventManager.instance.onActionButtonPressed += _startTalking;
        gettingAttention = true;
        anim.SetBool("waving", true);
    }

    void _stopWaving()
    {
        EventManager.instance.onActionButtonPressed -= _startTalking;
        gettingAttention = false;
        anim.SetBool("waving", false);
    }

    void _startTalking()
    {
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
        if ( progressesQuest != QuestName.None )
            QuestManager.ProgressQuest(progressesQuest);
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
