using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class opencloseDoor : MonoBehaviour
{
	public Animator openandclose;
	public bool open;
	public Transform Player;
	private TextMeshProUGUI interactText;
	private bool playerInRange = false;

	public AudioClip openSound;
	public AudioClip closeSound;
	private AudioSource audioSource;

	void Start()
	{
		// open = false;
		GameObject interactTextObject = GameObject.FindWithTag("InteractText");
		if (interactTextObject != null)
		{
			interactText = interactTextObject.GetComponent<TextMeshProUGUI>();
			if (interactText == null)
			{
				Debug.LogError("No TextMeshProUGUI component found on the object with tag 'InteractText'.");
			}
		}
		else
		{
			Debug.LogError("No object found with tag 'InteractText'.");
		}

		if (interactText != null)
		{
			interactText.enabled = false;
		}

		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			Debug.LogError("No AudioSource component found on the object.");
		}
	}

	void Update()
	{
		if (playerInRange && Player)
		{
			float dist = Vector3.Distance(Player.position, transform.position);
			if (dist < 3.5f)
			{
				ShowInteractText(true);

				if (Input.GetMouseButtonDown(0))
				{
					StartCoroutine(open ? closing() : opening());
				}
			}
			else
			{
				ShowInteractText(false);
			}
		}
	}

	void OnMouseEnter()
	{
		playerInRange = true;
	}

	void OnMouseExit()
	{
		playerInRange = false;
		ShowInteractText(false);
	}

	void ShowInteractText(bool show)
	{
		if (interactText != null)
		{
			interactText.enabled = show;
		}
	}

	IEnumerator opening()
	{
		print("You are opening the door");
		openandclose.Play("Opening");
		audioSource.PlayOneShot(openSound);
		open = true;
		yield return new WaitForSeconds(.5f);
	}

	IEnumerator closing()
	{
		print("You are closing the door");
		openandclose.Play("Closing");
		audioSource.PlayOneShot(closeSound);
		open = false;
		yield return new WaitForSeconds(.5f);
	}
}
