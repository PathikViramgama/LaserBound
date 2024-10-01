using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    [SerializeField] private GameObject platformToReveal;       
    [SerializeField] private ExitDoor linkedExitDoor;           

    private bool hasBeenActivated = false;
    private Renderer platformRenderer;

    private void Awake()
    {
        if (platformToReveal != null)
        {
            platformRenderer = platformToReveal.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogWarning("No platform assigned to TargetBehavior script.");
        }
    }

    private void Start()
    {
        SetPlatformState(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser") && !hasBeenActivated)
        {
            ActivatePlatform();
        }
    }

    public void ActivatePlatform()
    {
        if (hasBeenActivated)
        {
            return;
        }

        SetPlatformState(true);

        if (linkedExitDoor != null)
        {
            linkedExitDoor.ActivateExitDoor();
            Debug.Log("Exit door has been successfully activated.");
        }
        else
        {
            Debug.LogWarning("No exit door linked to TargetBehavior.");
        }

        hasBeenActivated = true;
    }

    private void SetPlatformState(bool isActive)
    {
        if (platformToReveal != null)
        {
            platformToReveal.SetActive(isActive);

            if (platformRenderer != null)
            {
                platformRenderer.enabled = isActive;
            }

            Debug.Log(isActive ? "Platform revealed and visible." : "Platform hidden and invisible.");
        }
    }
}
