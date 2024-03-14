using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public GameObject exitDoor;
    public GameObject entranceDoor;
    public GameObject entranceDoorTrigger;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player entered the exit trigger area");
            opencloseDoor doorScript = exitDoor.GetComponent<opencloseDoor>();
            StartCoroutine(doorScript.closing());

            // set collider active state to false
            GetComponent<Collider>().enabled = false;

            // disable exit door's opencloseDoor script
            exitDoor.GetComponent<opencloseDoor>().enabled = false;

            // enable entrance door's opencloseDoor script
            entranceDoor.GetComponent<opencloseDoor>().enabled = true;

            // enable entrance door's trigger collider
            entranceDoorTrigger.GetComponent<Collider>().enabled = true;
        }
    }
}
