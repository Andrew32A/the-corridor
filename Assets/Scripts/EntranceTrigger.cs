using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour
{
    public GameObject entranceDoor;

    void Start()
    {
        // opencloseDoor doorScript = entranceDoor.GetComponent<opencloseDoor>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player entered the entrance trigger area");
            opencloseDoor doorScript = entranceDoor.GetComponent<opencloseDoor>();
            StartCoroutine(doorScript.closing());

            // set collider active state to false
            GetComponent<Collider>().enabled = false;

            // disable opencloseDoor script
            entranceDoor.GetComponent<opencloseDoor>().enabled = false;
        }
    }
}
