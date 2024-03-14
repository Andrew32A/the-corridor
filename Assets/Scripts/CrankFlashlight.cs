using UnityEngine;

public class CrankFlashlight : MonoBehaviour
{
    public Light flashlight;
    private bool isCranking = false;
    private float crankingTime = 0f;
    private float maxCrankingTime = 5f; // maximum time the flashlight stays on fully charged
    private float cooldownTime = 3f; // time the flashlight stays off after being used too long
    private bool isOnCooldown = false;
    private bool isFullyPowered = false;

    private void Start()
    {
        flashlight.intensity = 0f;
    }

    private void Update()
    {
        // check for left mouse button input
        if (Input.GetMouseButton(0) && !isOnCooldown)
        {
            StartCranking();
        }
        else
        {
            StopCranking();
        }

        // handle cooldown
        if (isOnCooldown)
        {
            flashlight.intensity = 0f;
            isFullyPowered = false;
            return;
        }

        // flickering effect
        if (isCranking && !isFullyPowered)
        {
            float intensity = Mathf.Lerp(0.01f, 0.2f, Mathf.PingPong(Time.time, 0.1f));
            flashlight.intensity = intensity;
        }
    }

    private void StartCranking()
    {
        if (!isCranking)
        {
            // start dim and flicker for 1.5 seconds
            flashlight.intensity = 0.2f;
            Invoke(nameof(EnableFullBrightness), 1.5f);
        }

        isCranking = true;
        crankingTime += Time.deltaTime;

        // turn off and lockout the flashlight if cranking time exceeds maxCrankingTime
        if (crankingTime >= maxCrankingTime)
        {
            isCranking = false;
            isOnCooldown = true;
            flashlight.intensity = 0f;
            Invoke(nameof(ResetCooldown), cooldownTime);
        }
        else if (isFullyPowered)
        {
            // only cast ray when fully charged
            CastRay();
        }
    }

    private void StopCranking()
    {
        flashlight.intensity = 0f;
        isCranking = false;
        crankingTime = 0f;
        isFullyPowered = false; // Reset full power status
    }

    private void EnableFullBrightness()
    {
        if (isCranking) // check if still cranking after 1.5 seconds
        {
            flashlight.intensity = 2f; // set to full brightness
            isFullyPowered = true; // flashlight is fully powered
        }
    }

    private void ResetCooldown()
    {
        isOnCooldown = false;
    }

    private void CastRay()
    {
        if (!isFullyPowered) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // TODO: check for cursed object tag trigger then send data (eg. dispel(objectHit)) to observer so it can handle the dispelling
            // Debug.Log(hit.transform.name);
        }
    }
}