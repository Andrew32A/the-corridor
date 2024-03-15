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

    public GameObject normalZombie; // may need to rename it?
    public GameObject cursedZombie;

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
        AddRandomCursedObject();
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

    public void AddRandomCursedObject()
    {
        int randomIndex = Random.Range(0, 2);
        if (randomIndex == 0)
        {
            cursedToothbrush.SetActive(true);
            normalToothbrush.SetActive(false);
        }

        if (randomIndex == 1)
        {
            cursedZombie.SetActive(true);
            normalZombie.SetActive(false);
        }
    }

    public void DispelCursedObject(GameObject cursedObject)
    {
        if (cursedObject.name == "cursedToothbrush")
        {
            cursedToothbrush.SetActive(false);
            normalToothbrush.SetActive(true);
        }

        if (cursedObject.name == "cursedZombie")
        {
            cursedZombie.SetActive(false);
            normalZombie.SetActive(true);
        }

        // TODO: add more cursed objects here
    }

}
