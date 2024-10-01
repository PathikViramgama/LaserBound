using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehavior : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;  
    private Rigidbody mirrorRigidbody;
    private bool isPlayerNearby = false;

    private void Awake()
    {
        mirrorRigidbody = GetComponent<Rigidbody>();
        if (mirrorRigidbody == null)
        {
            Debug.LogError("Rigidbody component missing on Mirror object.");
            return;
        }

        mirrorRigidbody.isKinematic = true;}

    private void Update()
    {if (isPlayerNearby)
        {
            HandleMirrorMovement();
        }}

    private void HandleMirrorMovement()
    {
        Vector3 randomMovement = new Vector3(
            Mathf.Sin(Time.time) * movementSpeed * Time.deltaTime, 
            0f, 
            Mathf.Cos(Time.time) * movementSpeed * Time.deltaTime);

        mirrorRigidbody.MovePosition(transform.position + randomMovement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerNearby = true;
            mirrorRigidbody.isKinematic = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerNearby = false;
            mirrorRigidbody.isKinematic = true;
        }
    }
}
