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

    public GameObject normalAnimalPaintings;
    public GameObject cursedAnimalPaintings;

    public GameObject normalMirror;
    public GameObject cursedMirror;

    public GameObject normalChessBoard;
    public GameObject cursedChessBoard;

    public GameObject normalPlug;
    public GameObject cursedPlug;

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
        int randomIndex = Random.Range(0, 7); // second arg needs to be +1 for some reason?
        Debug.Log("Random index: " + randomIndex);

        if (randomIndex == 0 && cursedToothbrush.activeSelf == false)
        {
            currentCursedObjects++;
            cursedToothbrush.SetActive(true);
            normalToothbrush.SetActive(false);
        }

        if (randomIndex == 1 && cursedZombie.activeSelf == false)
        {
            currentCursedObjects++;
            cursedZombie.SetActive(true);
            normalZombie.SetActive(false);
        }

        if (randomIndex == 2 && cursedHighFiveMan.activeSelf == false)
        {
            currentCursedObjects++;
            cursedHighFiveMan.SetActive(true);
            normalHighFiveMan.SetActive(false);
        }

        if (randomIndex == 3 && cursedAnimalPaintings.activeSelf == false)
        {
            currentCursedObjects++;
            cursedAnimalPaintings.SetActive(true);
            normalAnimalPaintings.SetActive(false);
        }

        if (randomIndex == 4 && cursedMirror.activeSelf == false)
        {
            currentCursedObjects++;
            cursedMirror.SetActive(true);
            normalMirror.SetActive(false);
        }

        if (randomIndex == 5 && cursedChessBoard.activeSelf == false)
        {
            currentCursedObjects++;
            cursedChessBoard.SetActive(true);
            normalChessBoard.SetActive(false);
        }

        if (randomIndex == 6 && cursedPlug.activeSelf == false)
        {
            currentCursedObjects++;
            cursedPlug.SetActive(true);
            normalPlug.SetActive(false);
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
        if (cursedObject.name == "cursedHighFiveMan1" || cursedObject.name == "cursedHighFiveMan2")
        {
            currentCursedObjects--;
            cursedHighFiveMan.SetActive(false);
            normalHighFiveMan.SetActive(true);
        }

        // animal paintings
        if (cursedObject.name == "cursedAnimalPaintings1" || cursedObject.name == "cursedAnimalPaintings2")
        {
            currentCursedObjects--;
            cursedAnimalPaintings.SetActive(false);
            normalAnimalPaintings.SetActive(true);
        }

        // mirror
        if (cursedObject.name == "cursedMirror")
        {
            currentCursedObjects--;
            cursedMirror.SetActive(false);
            normalMirror.SetActive(true);
        }

        // chess board
        if (cursedObject.name == "cursedChessBoard")
        {
            currentCursedObjects--;
            cursedChessBoard.SetActive(false);
            normalChessBoard.SetActive(true);
        }

        // plug
        if (cursedObject.name == "cursedPlug")
        {
            currentCursedObjects--;
            cursedPlug.SetActive(false);
            normalPlug.SetActive(true);
        }

        // TODO: add more cursed objects here
    }

}
