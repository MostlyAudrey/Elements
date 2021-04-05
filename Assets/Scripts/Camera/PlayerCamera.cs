using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField]
    private Transform cameraCenter;

    [SerializeField]
    private float mouseSensitivity = 25f;
    
    private float camVert = 0f;
    private float camHoriz = 0;

    private Vector3 relativePos;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        relativePos = transform.localPosition - cameraCenter.localPosition;
        Debug.Log("Camera rel pos is " + relativePos);
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float turnMax = 10f;
        mouseX = Mathf.Clamp(mouseX, -turnMax, turnMax);
        mouseY = Mathf.Clamp(mouseY, -turnMax, turnMax);

        camVert += mouseY;
        camVert = Mathf.Clamp(camVert, -50, 90);
        camHoriz += mouseX;
        camHoriz = Mathf.Clamp(camHoriz, -70, 70);

        

        // transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // transform.RotateAroundLocal(Vector3.up, mouseX);
        Quaternion rotation = Quaternion.Euler(camVert, camHoriz, 0);
        transform.localRotation = rotation;
        transform.localPosition = cameraCenter.localPosition + (rotation * relativePos);
        // transform.RotateAround(transform.up, transform.up, mouseX);
    }
}
