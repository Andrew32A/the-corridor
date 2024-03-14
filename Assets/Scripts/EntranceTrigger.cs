using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour
{
    public GameObject entranceDoor;
    public GameObject exitDoor;
    public GameObject exitDoorTrigger;


    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player entered the entrance trigger area");
            opencloseDoor doorScript = entranceDoor.GetComponent<opencloseDoor>();

            // if door is open, close it
            if (doorScript.open)
            {
                StartCoroutine(doorScript.closing());
            }

            // set collider active state to false
            GetComponent<Collider>().enabled = false;

            // disable opencloseDoor script
            entranceDoor.GetComponent<opencloseDoor>().enabled = false;

            // enable exit door's opencloseDoor script
            exitDoor.GetComponent<opencloseDoor>().enabled = true;

            // enable exit door's trigger collider
            exitDoorTrigger.GetComponent<Collider>().enabled = true;
        }
    }
}
