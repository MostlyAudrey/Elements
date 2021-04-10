using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Abstract wrapper class for a slider that can be used to change a settings value.
 * Place the appropriate child of this class on a slider object in a settings menu
 * to make the slider perform a certain function. Put an input field component to have the
 * slider also controllable by text input.
 * 
 * By Aneet Nadella
 */
public abstract class SettingsSlider : MonoBehaviour
{
    [SerializeField]
    protected Slider slider;
    [SerializeField]
    private InputField inputField;

    protected virtual void Awake()
    {
        if (inputField != null)
        {
            // Change slider value when input field changes
            inputField.onEndEdit.AddListener(OnInputTextChanged);
            
            // Update input field with slider value
            slider.onValueChanged.AddListener(UpdateInputText);
        }
    }

    protected virtual void OnEnable()
    {
        Reset();
    }

    /**
     * Set current value of the slider.
     */
    public virtual void SetValue(float val)
    {
        slider.value = val;
    }

    /**
     * Get current value of the slider.
     */
    public virtual float GetValue()
    {
        return slider.value;
    }

    /**
     * Reset slider value to value of its corresponding setting.
     */
    public abstract void Reset();

    /**
     * Apply slider value to corresponding setting and save if appropriate.
     */
    public abstract void ApplyChanges();

    /**
     * Change slider value when text in input field changes.
     */
    private void OnInputTextChanged(string input)
    {
        decimal numericalInput;
        decimal.TryParse(input, out numericalInput);

        // Use set value without notify and update input text
        // manually because if slider value does not change,
        // OnValueChanged will not be called
        slider.SetValueWithoutNotify( (float) numericalInput );
        UpdateInputText(slider.value);
    }

    /**
     * Update input field text box directly from value of slider.
     */
    private void UpdateInputText(float sliderVal)
    {
        inputField.SetTextWithoutNotify(string.Format("{0:0.0}", sliderVal));
    }
}
