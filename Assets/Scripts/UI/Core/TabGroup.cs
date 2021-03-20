using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    
    public List<GameObject> objectsToSwap;

    private List<TabButton> tabButtons;
    private TabButton selectedTab;

    void OnEnable()
    {
        if (tabButtons != null)
        {
            ResetTabs();
            OnTabSelected(tabButtons[0]);
        }
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();        
        }

        tabButtons.Add(button);
    }

    // Call when tab button is hovered over
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();

        if (button != selectedTab)
        {
            button.background.color = tabHover;
        }
    }

    // Call when tab button is no longer hovered over
    public void OnTabExit(TabButton button)
    {  
        ResetTabs();
    }

    // Call when tab button is clicked
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; ++i)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }

        ResetTabs();
        button.background.color = tabActive;
    }

    private void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if (button != selectedTab) 
            {
                button.background.color = tabIdle;
            }
        }
    }
}
