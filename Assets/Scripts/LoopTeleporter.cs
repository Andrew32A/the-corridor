using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTeleporter : MonoBehaviour
{
    public Observer observer;
    public CharacterController playerController;
    public Transform teleportDestination;
    public Transform gameOverDestination;
    private PlayerMovement playerMovementScript;

    private void Start()
    {
        playerMovementScript = playerController.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && observer.currentCursedObjects <= observer.maxCursedObjectsForGameOver)
        {
            Debug.Log("Teleporting and rotating player with adjusted relative position");

            // notify oberserver that the player entered the teleporter
            observer.PlayerEnteredTeleporter();

            // relative position of the player to the teleporter
            Vector3 currentVelocity = playerMovementScript.GetVelocity();
            Debug.Log("Current velocity: " + currentVelocity);
            Vector3 entryPositionRelativeToTeleporter = other.transform.position - transform.position;

            // adjust the relative position 90 degrees to the right
            Vector3 adjustedRelativePosition = new Vector3(entryPositionRelativeToTeleporter.z, entryPositionRelativeToTeleporter.y, -entryPositionRelativeToTeleporter.x);

            // disable the Character Controller before moving and rotating
            playerController.enabled = false;

            // apply the adjusted relative position to the teleport destination, considering its rotation
            Vector3 newPosition = teleportDestination.position + teleportDestination.rotation * adjustedRelativePosition;
            playerController.transform.position = newPosition;

            // this aligns the player's forward direction with the hallway's forward direction
            playerController.transform.rotation *= Quaternion.Euler(0, 90, 0);

            // re-enable the Character Controller after moving and rotating
            playerController.enabled = true;

            playerMovementScript.SetVelocity(currentVelocity);
        }

        // teleport player to game over room
        else
        {
            // relative position of the player to the teleporter
            Vector3 currentVelocity = playerMovementScript.GetVelocity();
            Debug.Log("Current velocity: " + currentVelocity);
            Vector3 entryPositionRelativeToTeleporter = other.transform.position - transform.position;

            // adjust the relative position 90 degrees to the right
            Vector3 adjustedRelativePosition = new Vector3(entryPositionRelativeToTeleporter.z, entryPositionRelativeToTeleporter.y, -entryPositionRelativeToTeleporter.x);

            // disable the Character Controller before moving and rotating
            playerController.enabled = false;

            // apply the adjusted relative position to the teleport destination, considering its rotation
            Vector3 newPosition = gameOverDestination.position + gameOverDestination.rotation * adjustedRelativePosition;
            playerController.transform.position = newPosition;

            // this aligns the player's forward direction with the hallway's forward direction
            playerController.transform.rotation *= Quaternion.Euler(0, 90, 0);

            // re-enable the Character Controller after moving and rotating
            playerController.enabled = true;

            playerMovementScript.SetVelocity(currentVelocity);
        }
    }
}
