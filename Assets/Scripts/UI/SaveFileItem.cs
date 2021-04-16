using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveFileItem : MonoBehaviour, IPointerClickHandler
{
    public Text date;
    public Text time;
    public System.DateTime dateTime;

    private LoadFromSaveWidget loadWidget = null;

    /**
     * Call after instantiation.
     * @param dt Date and time of corresponding save file.
     * @param loadWidget Owning widget.
     */
    public void Init(System.DateTime dt, LoadFromSaveWidget loadWidget)
    {
        this.dateTime = dt;
        date.text = dt.ToShortDateString();
        time.text = dt.ToShortTimeString();

        this.loadWidget = loadWidget;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        loadWidget.OnSaveFileSelected(this);
    }
}
