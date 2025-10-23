using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoverHandler : MonoBehaviour
{   

    public LayerMask hoverLayer;
    private GameObject currentHoverObject;
    public float jitterAmplitude = 5f;
    public float jitterFrequency = 20f;
    private Transform jitterTransform;
    private Vector3 jitterBasePosition;
    private Vector3 jitterSeed;

    void Update()
    {
        RaycastHit hit;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());


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
        // nothing hovered
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
        if (jitterTransform == null)
        {
            jitterTransform = obj.transform;
            jitterBasePosition = jitterTransform.position;
            jitterSeed = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);
        }

        float t = Time.time * jitterFrequency;
        float dx = (Mathf.PerlinNoise(jitterSeed.x, t))* jitterAmplitude;
        float dz = (Mathf.PerlinNoise(jitterSeed.z, t)) *  jitterAmplitude;
        float dy = 0; /* no need to have them going up and down */

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
    }
}
