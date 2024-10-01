using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ExitDoor : MonoBehaviour
{
    [SerializeField] private Color doorActivatedColor = Color.green;
    [SerializeField] private Color doorDeactivatedColor = Color.red;

    private Renderer doorRenderer;
    private bool isDoorActive = false;

    private void Awake()
    {
        doorRenderer = GetComponent<Renderer>();
        if (doorRenderer == null)
        {
            Debug.LogError("Renderer component is missing on ExitDoor object.");
        }
    }

    private void Start()
    {
        SetDoorState(false);
    }

    public void ActivateExitDoor()
    {
        if (isDoorActive)
        {
            Debug.Log("Exit door is already activated.");
            return;
        }

        SetDoorState(true);
    }

    private void SetDoorState(bool isActive)
    {
        isDoorActive = isActive;

        if (doorRenderer != null)
        {
            doorRenderer.material.color = isActive ? doorActivatedColor : doorDeactivatedColor;
        }

        Debug.Log(isActive ? "Exit door is now active." : "Exit door is now inactive.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isDoorActive)
        {
            Debug.Log("Player has reached the exit door. Transitioning to the next scene...");
            HandlePlayerExit(other.gameObject);
        }
    }

    private void HandlePlayerExit(GameObject player)
    {
        Destroy(player);
        
    }

   
}
