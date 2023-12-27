using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Main Camera Bobbing Settings")]
    public bool enableCameraBobbing;
    public float walkBobbingSpeed = 14f;
    public float walkBobbingAmount = 0.05f;
    public float runBobbingSpeed = 24f;
    public float runBobbingAmount = 0.1f;
    private float defaultPosY;
    private float timer = 0f;

    [Header("Weapon Camera Bobbing Settings")]
    public float weaponWalkBobbingMultiplier; // default: 0.5f
    public float weaponRunBobbingMultiplier; // default: 0.5f

    [Header("References")]
    public Transform cameraPosition;
    public Transform weaponCamera;
    public PlayerMovement playerMovementScript;
    private Vector3 originalWeaponPos;

    void Start()
    {
        defaultPosY = cameraPosition.position.y;
        originalWeaponPos = weaponCamera.localPosition;

        if (playerMovementScript == null)
        {
            playerMovementScript = FindObjectOfType<PlayerMovement>();
        }
    }

    void Update()
    {
        HandleCameraPosition();

        if (enableCameraBobbing && (playerMovementScript == null || playerMovementScript.state == PlayerMovement.MovementState.walking || playerMovementScript.state == PlayerMovement.MovementState.sprinting))
        {
            HandleCameraBobbing();
        }
    }

    void HandleCameraPosition()
    {
        transform.position = cameraPosition.position;
    }

    void HandleCameraBobbing()
    {
        float currentYPosition = cameraPosition.position.y;
        float yOffset = 0f;

        float currentWeaponMultiplier = weaponWalkBobbingMultiplier;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            if (playerMovementScript.isSprinting)
            {
                timer += Time.deltaTime * runBobbingSpeed;
                yOffset = Mathf.Sin(timer) * runBobbingAmount;
                currentWeaponMultiplier = weaponRunBobbingMultiplier;
            }
            else
            {
                timer += Time.deltaTime * walkBobbingSpeed;
                yOffset = Mathf.Sin(timer) * walkBobbingAmount;
                currentWeaponMultiplier = weaponWalkBobbingMultiplier;
            }

            transform.position = new Vector3(transform.position.x, currentYPosition + yOffset, transform.position.z);
        }
        else
        {
            timer = 0;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, currentYPosition, transform.position.z), Time.deltaTime * walkBobbingSpeed);
        }

        if (playerMovementScript != null && (playerMovementScript.state == PlayerMovement.MovementState.walking || playerMovementScript.state == PlayerMovement.MovementState.sprinting))
        {
            weaponCamera.localPosition = new Vector3(weaponCamera.localPosition.x, originalWeaponPos.y + yOffset * currentWeaponMultiplier, weaponCamera.localPosition.z);
        }
        else
        {
            weaponCamera.localPosition = Vector3.Lerp(weaponCamera.localPosition, originalWeaponPos, Time.deltaTime * walkBobbingSpeed);
        }
    }

}
