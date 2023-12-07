using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] float xSensitivity = 1.0f;
    [SerializeField] float ySensitivity = 1.0f;
    [SerializeField] float xMaxAngle = 70.0f;
    [SerializeField] float xMinAngle = -20.0f;
    [SerializeField] GameObject cameraAxis;

    private float rotationX = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;

        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX += mouseY * xSensitivity;
        if (rotationX > xMaxAngle) rotationX = xMaxAngle;
        if (rotationX < xMinAngle) rotationX = xMinAngle;

        cameraAxis.transform.rotation = Quaternion.Euler(rotationX, cameraAxis.transform.rotation.eulerAngles.y, 0);

        transform.Rotate(0, mouseX * ySensitivity, 0);
    }
}
