using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cameraComponent;
    public Transform target;
    public Transform player;
    public Vector3 offset;
    [Range(1, 10)]
    public int smoothFactor;

    public SpriteRenderer fadeObject;

    public bool End = false;

    private void Start()
    {
        cameraComponent = gameObject.GetComponent<Camera>();
        player = target;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            target = player;
            cameraComponent.orthographicSize = 5f;
        }
        Follow();
        if (End && Vector2.Distance(transform.position, target.position) <= 0.2f)
        {
            // End the game
            fadeObject.color = new Color(0f, 0f, 0f, 0.8f);
        }
    }

    private void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}