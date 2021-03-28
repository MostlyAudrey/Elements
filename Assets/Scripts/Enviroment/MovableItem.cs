using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 *  Trigger is remarkably similar to the ReturnItem extension of interactable.
 */
class MovableItem : MonoBehaviour
{
    private float positionOffset = 0;
    public float moveDistance = 2;
    public float moveSpeed = 2;
    private int isSupposedToMove = 0; // -1 back, 1 forward, 0 neutral

    private  GameObject target;
    public GameObject buttonHint;       // A corresponding game object that 'holds a hint' related to the completion of the quest.
    public bool showButtonHint    = true;         // Will the corresponding hint be shown?
    public float buttonHintRadius = 5.0f;
    protected bool showingHint;   

    public void pushButton()
    {
        if ( isSupposedToMove == 0 )
            isSupposedToMove = moveDistance <= positionOffset ? -1 : 1;
    }

    void Update()
    {
        if ( showButtonHint )
        {
            if ( !target ) 
                target = GameObject.FindGameObjectWithTag("Player");
            if ( !showingHint && Vector3.Distance(target.transform.position, transform.position) <= buttonHintRadius)
              _displayButtonHint();
            else if ( showingHint && Vector3.Distance(target.transform.position, transform.position) > buttonHintRadius )
              _hideButtonHint();
        }


        if (isSupposedToMove == 0)
        {
            gameObject.tag = "Button";
            return;
        } else if (isSupposedToMove == 1) 
        {
            gameObject.tag = "Untagged";
            if (positionOffset < moveDistance)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                positionOffset += 1 * moveSpeed * Time.deltaTime;
            } else isSupposedToMove = 0;

        } else if (isSupposedToMove == -1) {
            gameObject.tag = "Untagged";
            if (positionOffset > 0)
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                positionOffset += -1 * moveSpeed * Time.deltaTime;
            } else {
                isSupposedToMove = 0;
            }
        }
    }

    private void _displayButtonHint()
    {
        showingHint = true;
        if (showButtonHint) buttonHint.SetActive(true);
    }

    private void _hideButtonHint()
    {
        showingHint = false;
        if (showButtonHint) buttonHint.SetActive(false);
    }
}