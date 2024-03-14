using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;

    [Header("Doors")]
    public GameObject[] doors;

    [Header("Lights")]
    public GameObject[] lights;

    [Header("Teleporters")]
    public GameObject[] teleporters;

    [Header("Cursed Objects")]
    public GameObject normalToothbrush;
    public GameObject cursedToothbrush;

    [Header("Global Values")]
    public int loopCount = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    public void PlayerEnteredTeleporter()
    {
        Debug.Log("Player entered the teleporter's trigger area");
        loopCount++;
        Debug.Log("Loop count: " + loopCount);

        foreach (GameObject door in doors)
        {
            opencloseDoor doorScript = door.GetComponent<opencloseDoor>();
            if (doorScript != null)
            {
                doorScript.silentClosing();
            }
            else
            {
                Debug.LogError("Door does not have an opencloseDoor component.", door);
            }
        }
    }

    public void DispelCursedObject(GameObject cursedObject)
    {
        if (cursedObject.name == "cursedToothbrush")
        {
            cursedToothbrush.SetActive(false);
            normalToothbrush.SetActive(true);
        }

        // TODO: add more cursed objects here
    }

}
