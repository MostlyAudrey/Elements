using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Singleton class for adding and removing UI elements from the screen.
 * Makes it convenient to clear all UI for cases such as loading another scene.
 * If placing a UI element in the scene instead of instantiating it by script,
 * make sure it is a child of this canvas.
 *
 * Made by Aneet Nadella
 */
public class UICanvas : MonoBehaviour
{
    // Singleton behavior
    private static UICanvas instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static UICanvas Get()
    {
        if (instance == null)
        {
            Debug.LogError("UICanvas does not exist within scene");
        }
        return instance;
    }

    // Canvas for parenting UI elements
    public Transform canvasTransform;

    /**
     * Add a UI element to the screen.
     * @param widgetPrefab User interface prefab to instantiate.
     * @return sibling index of element.
     */
    public int AddUIElement(GameObject widgetPrefab)
    {
        return Instantiate(widgetPrefab, canvasTransform).transform.GetSiblingIndex();
    }

    /**
     * Remove a UI element from the screen.
     * @param index Sibling index of element to remove.
     */
    public void RemoveUIElement(int index)
    {
        Destroy(canvasTransform.GetChild(index));
    }

    /**
     * Remove all UI elements from the screen of the specified type.
     */
    public void RemoveUIElement<T>() where T : MonoBehaviour
    {
        T uiElement = null;
        canvasTransform.TryGetComponent<T>(out uiElement);
        if (uiElement != null) 
        {
            Destroy(uiElement.gameObject);
        }
        else
        {
            Debug.LogError("UICanvas: Could not find UI element of type " + typeof(T));
        }
    }

    /**
     * Clear all UI elements from screen.
     */
    public void ClearScreen()
    {
        for (int i = canvasTransform.childCount - 1; i >= 0; --i)
        {
            Destroy(canvasTransform.GetChild(i).gameObject);
        }
    }
}
