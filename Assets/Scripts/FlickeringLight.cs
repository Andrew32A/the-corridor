using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1f;
    public float flickerDuration = 0.1f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flickerDuration)
        {
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
            timer = 0f;
        }
    }
}
