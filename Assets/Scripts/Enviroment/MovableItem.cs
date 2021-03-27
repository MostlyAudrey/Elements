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

    public void pushButton()
    {
      if (moveDistance <= positionOffset)
      {
        isSupposedToMove = -1;
      } else {
        isSupposedToMove = 1;
      }
    }

    void Update()
    {
      if (isSupposedToMove == 0)
      {
        gameObject.tag = "Button";
        return;
      } else if (isSupposedToMove == 1) {
          gameObject.tag = "Untagged";
          if (positionOffset < moveDistance)
          {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            positionOffset += 1 * moveSpeed * Time.deltaTime;
          } else {
            isSupposedToMove = 0;
          }
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
}
