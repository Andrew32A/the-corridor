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

    public GameObject normalHighFiveMan;
    public GameObject cursedHighFiveMan;

    [Header("Global Values")]
    public int loopCount = 0;
    public int currentCursedObjects = 0;

    // void Start()
    // {

    // }

    // void Update()
    // {
    //     Debug.Log("Current cursed objects: " + currentCursedObjects);
    // }

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
        currentCursedObjects++;

        int randomIndex = Random.Range(0, 3);
        Debug.Log("Random index: " + randomIndex);
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

        if (randomIndex == 2)
        {
            cursedHighFiveMan.SetActive(true);
            normalHighFiveMan.SetActive(false);
        }
    }

    public void DispelCursedObject(GameObject cursedObject)
    {
        // toothbrush
        if (cursedObject.name == "cursedToothbrush")
        {
            currentCursedObjects--;
            cursedToothbrush.SetActive(false);
            normalToothbrush.SetActive(true);
        }

        // bed zombie
        if (cursedObject.name == "cursedZombie")
        {
            currentCursedObjects--;
            cursedZombie.SetActive(false);
            normalZombie.SetActive(true);
        }

        // high five man
        if (cursedObject.name == "cursedHighFiveMan" || cursedObject.name == "cursedHighFiveMan2")
        {
            currentCursedObjects--;
            cursedHighFiveMan.SetActive(false);
            normalHighFiveMan.SetActive(true);
        }

        // TODO: add more cursed objects here
    }

}
