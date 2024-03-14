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

        // flickering effect for cranking and full power
        if (isCranking)
        {
            if (!isFullyPowered)
            {
                // flicker more noticeably while winding up
                flashlight.intensity = Mathf.Lerp(0.2f, 0.5f, Mathf.PingPong(Time.time * 10, 1));
            }
            else
            {
                // subtle flicker when fully powered
                flashlight.intensity = Mathf.Lerp(1.8f, 2f, Mathf.PingPong(Time.time * 10, 1));
            }
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
    }

    private void StopCranking()
    {
        flashlight.intensity = 0f;
        isCranking = false;
        crankingTime = 0f;
        isFullyPowered = false; // reset full power status
    }

    private void EnableFullBrightness()
    {
        isFullyPowered = true; // flashlight is fully powered
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