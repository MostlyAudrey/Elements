using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;

public class CharacterCommon {

    public static bool CheckGroundNear(
        Vector3 charPos,       
        float jumpableGroundNormalMaxAngle, 
        float rayDepth, //how far down from charPos will we look for ground?
        float rayOriginOffset, //charPos near bottom of collider, so need a fudge factor up away from there
        out bool isJumpable,
        out int surface
    ) 
    {

        bool ret = false;
        bool _isJumpable = false;
        surface = 0;


        float totalRayLen = rayOriginOffset + rayDepth;

        Ray ray = new Ray(charPos + Vector3.up * rayOriginOffset, Vector3.down);

        int layerMask = 1 << LayerMask.NameToLayer("Default");


        RaycastHit[] hits = Physics.RaycastAll(ray, totalRayLen, layerMask);

        RaycastHit groundHit = new RaycastHit();

        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag.EndsWith("grass")) {
                surface = 1;
            } else if (hit.collider.gameObject.tag.EndsWith("concrete")) {
                surface = 0;
            } else if (hit.collider.gameObject.tag.EndsWith("snow")) {
                surface = 1;
            }else if (hit.collider.gameObject.tag.EndsWith("dirt")) {
                surface = 1;
            }

            if (hit.collider.gameObject.tag.StartsWith("ground"))
            {           

                ret = true;

                groundHit = hit;

                _isJumpable = Vector3.Angle(Vector3.up, hit.normal) < jumpableGroundNormalMaxAngle;

                break; //only need to find the ground once

            }

            

        }

        Helper.DrawRay(ray, totalRayLen, hits.Length > 0, groundHit, Color.magenta, Color.green);

        isJumpable = _isJumpable;

        return ret;
    }


    public static GameObject CheckForNearestPickupableItem(
        Transform charPos,       
        float rayDepth //how far down from charPos will we look for ground?
    ) 
    {
        List<GameObject> items = CheckForNearestItems( charPos, rayDepth );
        GameObject pickup_item = null;
        foreach( GameObject item in items ){
            if ( item.CompareTag("pickupable") )
            {
                pickup_item = item;
            }
        }
        return pickup_item;
    }

    public static List<GameObject> CheckForNearestItems(
        Transform charPos,       
        float rayDepth //how far down from charPos will we look for ground?
    ) 
    {
        List<GameObject> items = new List<GameObject>();

        for (float i = -1.0f; i < 1.0f; i += 0.2f)
        {
            for (float j = 0f; j < 2.0f; j += 0.3f)
            {
                Ray ray = new Ray( charPos.position + ( Vector3.left * i ) + ( Vector3.up * j ), charPos.forward );

                // int layerMask = 1 << LayerMask.NameToLayer("Default");


                RaycastHit[] hits = Physics.RaycastAll(ray, rayDepth);

                foreach(RaycastHit hit in hits)
                {
                    // Renderer rend = hit.transform.GetComponent<Renderer>();

                    // if (rend)
                    // {
                    //     // Change the material of all hit colliders
                    //     // to use a transparent shader.
                    //     rend.material.shader = Shader.Find("Transparent/Diffuse");
                    //     Color tempColor = rend.material.color;
                    //     tempColor.a = 0.3F;
                    //     rend.material.color = tempColor;
                    // }

                    if ( !items.Contains(hit.collider.gameObject) )
                    {           
                        items.Add(hit.collider.gameObject);
                    }

                    Helper.DrawRay(ray, rayDepth, true, hit, Color.magenta, Color.green);

                }
            }
        }

        

        return items;
    }
}
