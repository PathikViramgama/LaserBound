using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float jumpStrength = 7f;
    [SerializeField] private float rotationSpeed = 80f;  
    private Rigidbody playerRigidbody;
    private bool isOnGround = true;
    private Vector3 movementInput;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
    }

    private void Update()
    {
        CaptureMovementInput();
        PerformMovement();
        PerformRotation();  
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            PerformJump();
        }
    }

    private void CaptureMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontal, 0, vertical).normalized;
    }

    private void PerformMovement()
    {
        Vector3 movement = transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    private void PerformRotation()
    {
        // Rotate player based on horizontal input
        float rotationY = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);
    }

    private void PerformJump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        isOnGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Platform"))
        {
            isOnGround = true;
        }
    }
}
