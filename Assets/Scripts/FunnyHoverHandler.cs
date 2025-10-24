using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class FunnyHoverHandler : MonoBehaviour
{
    public LayerMask hoverLayer;

    private GameObject currentHoverObject;
    // No swap state: model swapping is handled by HoverModelSwitcher attached on objects

    [Tooltip("Maximum jitter distance from base position")] public float jitterAmplitude = 0.05f;
    [Tooltip("Oscillation speed for jitter")] public float jitterFrequency = 12f;
    private Transform jitterTransform;
    private Vector3 jitterBasePosition;
    private Vector3 jitterSeed;

    [Header("Funny Teleport")] 
    public float teleportChancePerSecond = 10f;
    public float teleportRadius = 60.0f;
    public float teleportCooldown = 1.0f;
    public AudioClip teleportClip;
    public AudioSource teleportAudioSource;
    private float lastTeleportTime = -999f;

    void Update()
    {
        RaycastHit hit;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hoverLayer))
        {
            GameObject newHoverObject = hit.collider.gameObject;

            if (newHoverObject != currentHoverObject)
            {
                if (currentHoverObject != null)
                {
                    OnHoverExit(currentHoverObject);
                }

                OnHoverEnter(newHoverObject);
                currentHoverObject = newHoverObject;
            }
            else
            {
                OnHoverStay(currentHoverObject);
            }
        }
        else
        {
            if (currentHoverObject != null)
            {
                OnHoverExit(currentHoverObject);
                currentHoverObject = null;
            }
        }
    }

    private void OnHoverEnter(GameObject obj)
    {
        Debug.Log("hovering! " + obj.name);
        var switcher = obj.GetComponentInParent<HoverModelSwitcher>();
        if (switcher != null)
        {
            switcher.Activate();
        }

        jitterTransform = obj.transform;
        jitterBasePosition = jitterTransform.position;
        jitterSeed = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);

    }

    private void OnHoverStay(GameObject obj)
    {
        if (Time.time - lastTeleportTime >= teleportCooldown)
        {
            float p = teleportChancePerSecond * Time.deltaTime;
            if (Random.value < p)
            {
                Vector2 circle = Random.insideUnitCircle * teleportRadius;
                Vector3 newPos = jitterBasePosition + new Vector3(circle.x, 0f, circle.y);

                jitterTransform.position = newPos;
                jitterBasePosition = newPos;
                lastTeleportTime = Time.time;

                if (teleportClip != null)
                {
                    if (teleportAudioSource != null)
                    {
                        teleportAudioSource.PlayOneShot(teleportClip);
                    }
                    else
                    {
                        AudioSource.PlayClipAtPoint(teleportClip, newPos);
                    }
                }

                return;
            }
        }

        if (jitterTransform == null)
        {
            jitterTransform = obj.transform;
            jitterBasePosition = jitterTransform.position;
            jitterSeed = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);
        }

        float t = Time.time * jitterFrequency;
        float dx = (Mathf.PerlinNoise(jitterSeed.x, t) - 0.5f) * 2f * jitterAmplitude;
        float dz = (Mathf.PerlinNoise(jitterSeed.z, t + 23.17f) - 0.5f) * 2f * jitterAmplitude;
        float dy = 0f;

        Vector3 offset = new Vector3(dx, dy, dz);
        jitterTransform.position = jitterBasePosition + offset;
    }

    private void OnHoverExit(GameObject obj)
    {
        if (jitterTransform != null)
        {
            jitterTransform.position = jitterBasePosition;
            jitterTransform = null;
        }

        var switcher = obj.GetComponentInParent<HoverModelSwitcher>();
        if (switcher != null)
        {
            switcher.Deactivate();
        }


        Debug.Log("Mouse exited: " + obj.name);
    }
}
