using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimType
{
    TRANSLATION,
    ROTATION
}

public class ObjectGroupAnimator : MonoBehaviour
{
    public AnimType animType = AnimType.TRANSLATION;
    public Vector3 targetRelPos;
    public Vector3 rotAxis;
    public float targetRot;
    public LeanTweenType easeType;
    public float animDuration;
    public LeanTweenType loopType;
    public List<GameObject> objsToAnimate;
    public float animDelayBetweenObjs;

    void Start()
    {
        float startDelay = 0f;
        foreach (GameObject obj in objsToAnimate)
        {
            switch (animType)
            {
                case AnimType.TRANSLATION :
                    _TweenTranslation(obj, obj.transform.position + targetRelPos, startDelay);
                    break;
                case AnimType.ROTATION :
                    _TweenRotation(obj, targetRot, startDelay);
                    break;
                default :
                    break;
            }

            startDelay += animDelayBetweenObjs;
        }
    }

    private void _TweenTranslation(GameObject gameObject, Vector3 targetPos, float startDelay)
    {
        LeanTween.move(gameObject, targetPos, animDuration)
        .setEase(easeType)
        .setLoopType(loopType)
        .setDelay(startDelay)
        ;
    }

    private void _TweenRotation(GameObject gameObject, float targetRot, float startDelay)
    {
        LeanTween.rotateAround(gameObject, rotAxis, targetRot, animDuration)
        .setEase(easeType)
        .setLoopType(loopType)
        .setDelay(startDelay)
        ;
    }
}
