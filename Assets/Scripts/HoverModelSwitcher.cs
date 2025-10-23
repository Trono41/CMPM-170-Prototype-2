using UnityEngine;


//** For swapping models in the building prefabs **//
public class HoverModelSwitcher : MonoBehaviour
{
    public GameObject replacementPrefab;
    public Mesh replacementMesh;

    private GameObject spawnedReplacement;
    private Mesh originalMesh;
    private MeshFilter targetMeshFilter;
    private Renderer[] originalRenderers;

    void Awake()
    {
        originalRenderers = GetComponentsInChildren<Renderer>(true);

        if (replacementMesh != null)
        {
            targetMeshFilter = GetComponentInChildren<MeshFilter>();
            if (targetMeshFilter != null)
            {
                originalMesh = targetMeshFilter.sharedMesh;
            }
        }
    }

    public void Activate()
    {
        if (replacementPrefab != null && spawnedReplacement == null)
        {
            SetOriginalRenderersEnabled(false);
            spawnedReplacement = Instantiate(replacementPrefab, transform);
            spawnedReplacement.transform.localPosition = Vector3.zero;
            spawnedReplacement.transform.localRotation = Quaternion.identity;
            spawnedReplacement.transform.localScale = Vector3.one;
        }

        if (replacementMesh != null && targetMeshFilter != null)
        {
            targetMeshFilter.sharedMesh = replacementMesh;
        }
    }

    public void Deactivate()
    {
        if (spawnedReplacement != null)
        {
            Destroy(spawnedReplacement);
            spawnedReplacement = null;
            SetOriginalRenderersEnabled(true);
        }

        if (replacementMesh != null && targetMeshFilter != null)
        {
            targetMeshFilter.sharedMesh = originalMesh;
        }
    }

    private void SetOriginalRenderersEnabled(bool enabled)
    {
        if (originalRenderers == null) return;
        foreach (var r in originalRenderers)
        {
            if (r != null) r.enabled = enabled;
        }
    }
}

