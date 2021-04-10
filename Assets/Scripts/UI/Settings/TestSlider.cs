using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestSlider : SettingsSlider
{
    public override void Reset()
    {
    }

    public override void ApplyChanges()
    {
    }

    protected override void Awake()
    {
        base.Awake();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
