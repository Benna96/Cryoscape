using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SimpleAnimEmissionColor : SimpleAnim
{
    [Tooltip("Mesh will be set to this color on awake")]
    [SerializeField] public Color startColor = Color.white;

    [SerializeField] public Color endColor = Color.white;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        SyncEmissionColor();
    }

    public void SyncEmissionColor() => meshRenderer.material.SetColor("_EmissionColor", startColor);

    protected override void AnimateAtPoint(float evaluatedPoint)
        => meshRenderer.material.SetColor("_EmissionColor", Color.LerpUnclamped(startColor, endColor, evaluatedPoint));
}
