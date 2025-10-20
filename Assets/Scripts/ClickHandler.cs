using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleClickToHole : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject holePrefab;   
    public LayerMask groundLayer;

    /* feel free to mess with these variables to change sink animation and hole size */ 
    public float holeRadius = 3f;    
    public float sinkDepth = 8f;     
    public float sinkDuration = 0.8f;
    public float holeLifetime = 1.0f;
    public bool scaleHoleToRadius = true;

    void Update()
    {
        /* left click = generate hole */
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        /* raycasts to the the floor only */
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            return;
        Vector3 holePos = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);


        GameObject hole = null;
        if (holePrefab != null)
        {
            hole = Instantiate(holePrefab, holePos, Quaternion.identity);
            if (scaleHoleToRadius)
            {
                Vector3 s = hole.transform.localScale;
                float diameter = holeRadius * 2f;
                hole.transform.localScale = new Vector3(diameter, s.y, diameter);
            }
        }

        /* if you add new buildings, make sure to set layer and tag to "Building" */
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        int sunkCount = 0;

        foreach (var b in buildings)
        {
            Collider col = b.GetComponent<Collider>();
            Vector3 testPoint = (col != null)
                ? col.ClosestPoint(new Vector3(holePos.x, col.bounds.center.y, holePos.z))
                : b.transform.position;

            Vector2 d = new Vector2(testPoint.x - holePos.x, testPoint.z - holePos.z);
            if (d.magnitude <= holeRadius)
            {
                if (col) col.enabled = false;
                StartCoroutine(SinkAndDestroy(b));
                sunkCount++;
            }
        }

        if (hole != null)
            Destroy(hole, (sunkCount > 0) ? (sinkDuration + 0.4f) : holeLifetime);
    }

    private IEnumerator SinkAndDestroy(GameObject building)
    {
        float t = 0f;
        Vector3 start = building.transform.position;
        Vector3 end   = start + Vector3.down * sinkDepth;

        while (t < 1f)
        {
            t += Time.deltaTime / sinkDuration;
            building.transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        Destroy(building);
    }
}
