using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform cameraCenter;

    [Tooltip("Set to 0 or negative to not cap yaw rotation.")]
    public float maxYawRotation = 90f;
    [Tooltip("Set to 0 or negative to not cap pitch rotation.")]
    public float maxPitchRotation = 90f;

    private float yawRotation = 0f;
    private float pitchRotation = 0f;

    private CharacterInputController cInputController;

    void Start() {
        cInputController = GetComponentInParent<CharacterInputController>();
        if (cInputController == null)
        {
            Debug.LogError("PlayerCamera cannot get CharacterInputController!");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() 
    {
        AddCameraYawRotation(cInputController.Turn);
        AddCameraPitchRotation(cInputController.LookUp);
    }

    private void AddCameraYawRotation(float deltaRot)
    {
        if (maxYawRotation > 0f && Mathf.Abs(deltaRot + yawRotation) > maxYawRotation)
        {
            deltaRot = Mathf.Sign(deltaRot) * maxYawRotation - yawRotation;
        }
        transform.RotateAround(cameraCenter.position, Vector3.up, deltaRot);
        yawRotation += deltaRot;
    }

    private void AddCameraPitchRotation(float deltaRot)
    {
        if (maxPitchRotation > 0f && Mathf.Abs(deltaRot + pitchRotation) > maxPitchRotation)
        {
            deltaRot = Mathf.Sign(deltaRot) * maxPitchRotation - pitchRotation;
        }
        transform.RotateAround(cameraCenter.position, transform.right, deltaRot);
        pitchRotation += deltaRot;
    }
}
