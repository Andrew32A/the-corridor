using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Observer : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public GameObject playerFlashlight;

    [Header("Doors")]
    public GameObject[] doors;

    [Header("Lights")]
    public GameObject[] lights;

    [Header("Teleporters")]
    public GameObject[] teleporters;

    [Header("Tutorial Triggers")]
    public GameObject[] tutorialTriggers;
    public GameObject anomalyDispelText;

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
    public int maxCursedObjectsForGameOver = 3; // need to change it in unity editor // TODO: bug where it's actually +1, dont have time to fix right now. it's because the teleporter looks at this value before it's updated

    [Header("Dev Tools")]
    public bool devMode = false;
    public GameObject devModeTextObject;
    public TextMeshProUGUI devModeText;

    // void Start()
    // {

    // }

    void Update()
    {
        // dev mode (press 0 to enable)
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Dev mode enabled");
            devMode = true;
            enablePlayerFlashlight();
            player.GetComponent<PlayerMovement>().speed = 15f;
        }

        if (devMode)
        {
            devModeTextObject.SetActive(true);
            devModeText.text = "Active Anomalies: " + currentCursedObjects;
        }
    }

    public void DisplayAnomalyDispelText()
    {
        anomalyDispelText.GetComponent<DisplayTextTrigger>().DisplayAnomalyDispelText();
    }

    public void PlayerEnteredTeleporter()
    {
        // Debug.Log("Player entered the teleporter's trigger area");
        loopCount++;
        AddRandomCursedObject();
        // Debug.Log("Loop count: " + loopCount);

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

        // check if player looped once, then enable flashlight and tutorial trigger
        if (loopCount == 1)
        {
            enableFlashlightTutorialTrigger();
            enablePlayerFlashlight();
        }
    }

    public void AddRandomCursedObject()
    {
        int randomIndex = Random.Range(0, 7); // second arg needs to be +1 for some reason?
        Debug.Log("Random index: " + randomIndex); // i have doubts that this is truely random... lol

        if (randomIndex == 0 && cursedToothbrush.activeSelf == false)
        {
            Debug.Log("Added cursed toothbrush");
            currentCursedObjects++;
            cursedToothbrush.SetActive(true);
            normalToothbrush.SetActive(false);
        }

        if (randomIndex == 1 && cursedZombie.activeSelf == false)
        {
            Debug.Log("Added cursed zombie");
            currentCursedObjects++;
            cursedZombie.SetActive(true);
            normalZombie.SetActive(false);
        }

        if (randomIndex == 2 && cursedHighFiveMan.activeSelf == false)
        {
            Debug.Log("Added cursed high five man");
            currentCursedObjects++;
            cursedHighFiveMan.SetActive(true);
            normalHighFiveMan.SetActive(false);
        }

        if (randomIndex == 3 && cursedAnimalPaintings.activeSelf == false)
        {
            Debug.Log("Added cursed animal paintings");
            currentCursedObjects++;
            cursedAnimalPaintings.SetActive(true);
            normalAnimalPaintings.SetActive(false);
        }

        if (randomIndex == 4 && cursedMirror.activeSelf == false)
        {
            Debug.Log("Added cursed mirror");
            currentCursedObjects++;
            cursedMirror.SetActive(true);
            normalMirror.SetActive(false);
        }

        if (randomIndex == 5 && cursedChessBoard.activeSelf == false)
        {
            Debug.Log("Added cursed chess board");
            currentCursedObjects++;
            cursedChessBoard.SetActive(true);
            normalChessBoard.SetActive(false);
        }

        if (randomIndex == 6 && cursedPlug.activeSelf == false)
        {
            Debug.Log("Added cursed plug");
            currentCursedObjects++;
            cursedPlug.SetActive(true);
            normalPlug.SetActive(false);
        }

        // Debug.Log("Current cursed objects: " + currentCursedObjects);
    }

    public void DispelCursedObject(GameObject cursedObject)
    {
        // toothbrush
        if (cursedObject.name == "cursedToothbrush")
        {
            currentCursedObjects--;
            cursedToothbrush.SetActive(false);
            normalToothbrush.SetActive(true);
            DisplayAnomalyDispelText();
        }

        // bed zombie
        if (cursedObject.name == "cursedZombie")
        {
            currentCursedObjects--;
            cursedZombie.SetActive(false);
            normalZombie.SetActive(true);
            DisplayAnomalyDispelText();
        }

        // high five man
        if (cursedObject.name == "cursedHighFiveMan1" || cursedObject.name == "cursedHighFiveMan2")
        {
            currentCursedObjects--;
            cursedHighFiveMan.SetActive(false);
            normalHighFiveMan.SetActive(true);
            DisplayAnomalyDispelText();
        }

        // animal paintings
        if (cursedObject.name == "cursedAnimalPaintings1" || cursedObject.name == "cursedAnimalPaintings2")
        {
            currentCursedObjects--;
            cursedAnimalPaintings.SetActive(false);
            normalAnimalPaintings.SetActive(true);
            DisplayAnomalyDispelText();
        }

        // mirror
        if (cursedObject.name == "cursedMirror")
        {
            currentCursedObjects--;
            cursedMirror.SetActive(false);
            normalMirror.SetActive(true);
            DisplayAnomalyDispelText();
        }

        // chess board
        if (cursedObject.name == "cursedChessBoard")
        {
            currentCursedObjects--;
            cursedChessBoard.SetActive(false);
            normalChessBoard.SetActive(true);
            DisplayAnomalyDispelText();
        }

        // plug
        if (cursedObject.name == "cursedPlug")
        {
            currentCursedObjects--;
            cursedPlug.SetActive(false);
            normalPlug.SetActive(true);
            DisplayAnomalyDispelText();
        }

        // TODO: add more cursed objects here
    }

    private void enableFlashlightTutorialTrigger()
    {
        tutorialTriggers[0].SetActive(true);
    }

    private void enablePlayerFlashlight()
    {
        playerFlashlight.SetActive(true);
    }
}
