using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager : MonoBehaviour
{
	private static EventManager eventManager;

	public static EventManager instance {
		get {
			if (!eventManager) {
				eventManager = FindObjectOfType (typeof(EventManager)) as EventManager;

				if (!eventManager) {
					Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
				} else {
					eventManager.Init (); 
				}
			}

			return eventManager;
		}
	}

	private void Awake()
	{
		eventManager = this;
	}

	void Init () {}

	public event Action onActionButtonPressed;
	public void ActionButtonPressed()
	{
		if ( onActionButtonPressed != null ) onActionButtonPressed(); 
	}

	public event Action<string, float, float> onDisplayText;

	public void DisplayText( string text, float textSpeed, float timer )
	{
		if ( onDisplayText != null ) onDisplayText( text, textSpeed, timer );
	}

	public event Action onHideDisplayText;

	public void HideDisplayText()
	{
		if ( onDisplayText != null ) onHideDisplayText();
	}

	public event Action<QuestName, int> onQuestProgressed;

	public void QuestProgressed(QuestName quest, int phase)
	{
		if ( onQuestProgressed != null ) onQuestProgressed(quest, phase);
	}


	/* Events regarding the tracking of mob lifes. 
	 * DeathEvent - Event fired when a mob dies. Passes the mob data to the listener so they can determine if that mob is of interest.
	 */

	public event Action<MobAI> onDeath;
	public void DeathEvent(MobAI mob)
    {
		if (onDeath != null) onDeath(mob);
    }
	// public event Action<QuestName> onProgressQuest;

	// public void ProgressQuest(QuestName quest)
	// {
	// 	if ( onProgressQuest != null ) onProgressQuest(quest);
	// }
}
