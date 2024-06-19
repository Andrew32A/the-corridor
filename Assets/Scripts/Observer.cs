using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Observer : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public GameObject playerFlashlight;
    public GameObject flashlightCharge1;
    public GameObject flashlightCharge2;
    public GameObject flashlightCharge3;

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
    public List<CursedObject> cursedObjects = new List<CursedObject>();

    [Header("Global Values")]
    public int loopCount = 0;
    public int currentCursedObjects = 0;
    public int maxCursedObjectsForGameOver = 3; // need to change it in unity editor // TODO: bug where it's actually +1, dont have time to fix right now. it's because the teleporter looks at this value before it's updated

    [Header("Dev Tools")]
    public bool devMode = false;
    public GameObject devModeTextObject;
    public TextMeshProUGUI devModeText;


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
        loopCount++;
        AddRandomCursedObject();
        playerFlashlight.GetComponent<CrankFlashlight>().RechargeFlashlight();

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
        // list of cursed objects that have not spawned yet
        List<CursedObject> availableCursedObjects = cursedObjects.FindAll(obj => !obj.hasSpawned);

        // if there are no more cursed objects to spawn, return
        if (availableCursedObjects.Count == 0)
        {
            Debug.LogWarning("No more cursed objects to spawn.");
            return;
        }

        // get a random cursed object from the available cursed objects list
        int randomIndex = Random.Range(0, availableCursedObjects.Count);
        CursedObject cursedObject = availableCursedObjects[randomIndex]; // i have doubts that this is truely random... lol

        if (!cursedObject.cursedObject.activeSelf)
        {
            Debug.Log("Added cursed " + cursedObject.name);
            currentCursedObjects++;
            cursedObject.SetCursed(true);
            cursedObject.hasSpawned = true;
        }
    }

    public void DispelCursedObject(GameObject cursedGameObject)
    {
        Debug.Log("Attempting to dispel object: " + cursedGameObject.name);

        foreach (CursedObject cursedObject in cursedObjects)
        {
            Debug.Log("Checking against cursed object: " + cursedObject.cursedObject.name);
            if (cursedObject.IsCursedObject(cursedGameObject))
            {
                currentCursedObjects--;
                cursedObject.SetCursed(false);
                DisplayAnomalyDispelText();
                Debug.Log("Dispelled object: " + cursedGameObject.name);
                return;
            }
        }

        Debug.LogWarning("Cursed object not found: " + cursedGameObject.name);
    }

    public void enableFlashlightTutorialTrigger()
    {
        tutorialTriggers[0].SetActive(true);
    }

    // normally accesed by entrance trigger script
    public void enablePlayerFlashlight()
    {
        playerFlashlight.SetActive(true);
        flashlightCharge1.SetActive(true);
        flashlightCharge2.SetActive(true);
        flashlightCharge3.SetActive(true);
    }
}

[System.Serializable]
public class CursedObject
{
    public string name;
    public GameObject normalObject;
    public GameObject cursedObject;
    public bool hasSpawned = false;

    public void SetCursed(bool isCursed)
    {
        normalObject.SetActive(!isCursed);
        cursedObject.SetActive(isCursed);
    }

    public bool IsCursedObject(GameObject obj)
    {
        // check the primary cursed object and its children
        if (cursedObject == obj || CheckChildren(cursedObject, obj.name))
        {
            Debug.Log("Matched primary cursed object or its child: " + obj.name);
            return true;
        }

        Debug.Log("No match found for object: " + obj.name);
        return false;
    }

    // TODO: this is potentially dangerous and a performance issue. the reason it's here is because the cursed object has a child that is the hitbox that the raycast hits.
    // this is a temporary solution and needs to be refactored. TO BE FAIR, it's nice for when anomalies have multiple hitboxes for dispelling
    private bool CheckChildren(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.name == name)
            {
                return true;
            }
        }
        return false;
    }
}
