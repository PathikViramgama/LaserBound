using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int maxReflections = 20;            
    [SerializeField] private float laserLength = 300f;           
    [SerializeField] private Vector3 laserOffset;                

    private LineRenderer laserRenderer;

    private void Awake()
    {
        laserRenderer = GetComponent<LineRenderer>();

        if (laserRenderer == null) {
            Debug.LogError("LineRenderer component is missing on Laser object.");
            return;
        }

        Material laserMaterial = new Material(Shader.Find("Unlit/Color"));
        laserMaterial.color = Color.red;  
        laserRenderer.material = laserMaterial;

        laserRenderer.startColor = Color.red;
        laserRenderer.endColor = Color.red;
        laserRenderer.positionCount = maxReflections + 1;
        laserRenderer.useWorldSpace = true;
        laserRenderer.startWidth = 0.1f;
        laserRenderer.endWidth = 0.1f;

        for (int i = 0; i <= maxReflections; i++) 
        {
            laserRenderer.SetPosition(i, transform.position);
        }
    }

    private void Start()
    {
        FireLaser(transform.position + laserOffset, transform.forward);
    }

    private void Update()
    {
        FireLaser(transform.position + laserOffset, transform.forward);
    }

    private void FireLaser(Vector3 startPosition, Vector3 direction)
    {
        int currentReflection = 0;
        laserRenderer.SetPosition(0, startPosition);

        while (currentReflection < maxReflections)
        {
            if (TryReflectLaser(startPosition, direction, out Vector3 hitPosition, out Vector3 newDirection))
            {
                startPosition = hitPosition;
                direction = newDirection;
                laserRenderer.SetPosition(currentReflection + 1, hitPosition);
                currentReflection++;
            }
            else
            {
                laserRenderer.SetPosition(currentReflection + 1, startPosition + direction * laserLength);
                break;
            }
        }

        for (int i = currentReflection + 1; i <= maxReflections; i++) 
        {
            laserRenderer.SetPosition(i, startPosition + direction * laserLength);
        }
    }

    private bool TryReflectLaser(Vector3 position, Vector3 direction, out Vector3 hitPosition, out Vector3 newDirection)
    {
        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserLength)) {
            hitPosition = hit.point;
            newDirection = Vector3.Reflect(direction, hit.normal);
            HandleLaserHit(hit);

            if (hit.transform.CompareTag("Mirror")) {
                return true;
            } else {
                return false;
            }
        }
        else {
            hitPosition = position + direction * laserLength;
            newDirection = direction;
            return false;
        }
    }

    private void HandleLaserHit(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Player")) {
            Debug.Log("Player hit by laser. Game Over.");
            Destroy(hit.transform.gameObject);
        }
        else if (hit.transform.CompareTag("Target")) {
            var targetScript = hit.transform.GetComponent<TargetBehavior>();
            if (targetScript != null) {
                targetScript.ActivatePlatform();
            }
        }
    }
}
