using UnityEngine;

public class CrankFlashlight : MonoBehaviour
{
    public Observer observer;
    public Light flashlight;
    private bool isCranking = false;
    private float crankingTime = 0f;
    private float maxCrankingTime = 5f; // maximum time the flashlight stays on fully charged
    private float cooldownTime = 0.1f; // time the flashlight stays off after being used too long
    private bool isOnCooldown = false;
    private bool isFullyPowered = false;
    public AudioClip windupSound;
    private AudioSource audioSource;

    public GameObject flashlightCharge1;
    public GameObject flashlightCharge2;
    public GameObject flashlightCharge3;

    // TODO: implement flashlight charges to prevent player from spamming
    public int currentFlashlightCharges = 0;
    public int maxFlashlightCharges = 3;

    private void Start()
    {
        flashlight.intensity = 0f;

        currentFlashlightCharges = maxFlashlightCharges;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the object.");
        }
    }

    private void Update()
    {
        // check for left mouse button input
        if (Input.GetMouseButton(0) && !isOnCooldown)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(windupSound);
            }
            StartCranking();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
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
                CastRay();
            }
        }

        // update flashlight charge UI
        switch (currentFlashlightCharges)
        {
            case 3:
                flashlightCharge1.SetActive(true);
                flashlightCharge2.SetActive(true);
                flashlightCharge3.SetActive(true);
                break;
            case 2:
                flashlightCharge1.SetActive(true);
                flashlightCharge2.SetActive(true);
                flashlightCharge3.SetActive(false);
                break;
            case 1:
                flashlightCharge1.SetActive(true);
                flashlightCharge2.SetActive(false);
                flashlightCharge3.SetActive(false);
                break;
            case 0:
                flashlightCharge1.SetActive(false);
                flashlightCharge2.SetActive(false);
                flashlightCharge3.SetActive(false);
                break;
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
        currentFlashlightCharges--; // reduce flashlight charge by 1
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("DispelHitbox"))
            {
                Debug.Log(hit.collider.gameObject);
                observer.DispelCursedObject(hit.collider.gameObject);
            }
        }
    }
}