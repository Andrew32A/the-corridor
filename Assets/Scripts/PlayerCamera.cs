using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;
    public Camera playerCam;

    private float xRotation;
    private float yRotation;

    private Coroutine tiltCoroutine;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        // lock rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate camera and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void DoFov(float endValue, float duration)
    {
        StartCoroutine(SmoothlyLerpFOV(playerCam.fieldOfView, endValue, duration));
    }

    public void DoTilt(float zTilt, float duration)
    {
        // check to see if there's already a tilt coroutine running
        if (tiltCoroutine != null)
        {
            StopCoroutine(tiltCoroutine);
        }

        // start a new tilt coroutine and keep track of it
        tiltCoroutine = StartCoroutine(SmoothlyLerpTilt(transform.rotation.eulerAngles.z, zTilt, duration));
    }

    private IEnumerator SmoothlyLerpFOV(float startValue, float endValue, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            playerCam.fieldOfView = Mathf.Lerp(startValue, endValue, t);
            time += Time.deltaTime;
            yield return null;
        }
        playerCam.fieldOfView = endValue;
    }

    private IEnumerator SmoothlyLerpTilt(float startZTilt, float endZTilt, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.z = Mathf.Lerp(startZTilt, endZTilt, t);
            transform.rotation = Quaternion.Euler(currentRotation);
            time += Time.deltaTime;
            yield return null;
        }
        Vector3 finalRotation = transform.rotation.eulerAngles;
        finalRotation.z = endZTilt;
        transform.rotation = Quaternion.Euler(finalRotation);
    }
}
