using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveFileItem : MonoBehaviour, IPointerClickHandler
{
    public Text date;
    public Text time;

    private LoadFromSaveWidget loadWidget;
    private System.DateTime dateTime;

    // Call after instantiation
    public void Init(System.DateTime dt, LoadFromSaveWidget loadWidget)
    {
        this.dateTime = dt;
        date.text = dt.ToShortDateString();
        time.text = dt.ToShortTimeString();

        this.loadWidget = loadWidget;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        loadWidget.OnSaveFileSelected(dateTime);
    }
}
