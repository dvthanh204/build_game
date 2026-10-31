using UnityEngine;

public sealed class FarmPlot : MonoBehaviour
{
    private Renderer cropRenderer;
    private Material growingMaterial;
    private Material readyMaterial;
    private float plantedAt;
    private const float GrowSeconds = 35f;

    public bool IsPlanted { get; private set; }
    public bool IsReady { get { return IsPlanted && Time.time - plantedAt >= GrowSeconds; } }

    public void Initialize(Material soil, Material growing, Material ready)
    {
        growingMaterial = growing;
        readyMaterial = ready;
        GameObject soilObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        soilObject.name = "Soil";
        soilObject.transform.SetParent(transform, false);
        soilObject.transform.localScale = new Vector3(1.7f, 0.18f, 1.7f);
        soilObject.GetComponent<Renderer>().sharedMaterial = soil;

        GameObject crop = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        crop.name = "Crop";
        crop.transform.SetParent(transform, false);
        crop.transform.localPosition = new Vector3(0f, 0.4f, 0f);
        crop.transform.localScale = new Vector3(0.5f, 0.7f, 0.5f);
        cropRenderer = crop.GetComponent<Renderer>();
        cropRenderer.sharedMaterial = growingMaterial;
        crop.SetActive(false);
    }

    public void Plant()
    {
        IsPlanted = true;
        plantedAt = Time.time;
        cropRenderer.gameObject.SetActive(true);
        cropRenderer.sharedMaterial = growingMaterial;
    }

    public bool Harvest()
    {
        if (!IsReady)
        {
            return false;
        }

        IsPlanted = false;
        cropRenderer.gameObject.SetActive(false);
        return true;
    }

    private void Update()
    {
        if (IsPlanted)
        {
            cropRenderer.sharedMaterial = IsReady ? readyMaterial : growingMaterial;
            float scale = IsReady ? 1f : Mathf.Lerp(0.35f, 0.8f, (Time.time - plantedAt) / GrowSeconds);
            cropRenderer.transform.localScale = new Vector3(0.5f, scale, 0.5f);
        }
    }
}
