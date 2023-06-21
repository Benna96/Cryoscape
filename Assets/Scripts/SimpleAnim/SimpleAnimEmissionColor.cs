using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SimpleAnimEmissionColor : SimpleAnim
{
    [Tooltip("Mesh will be set to this color on awake")]
    [SerializeField] public Color startColor = Color.white;

    [SerializeField] public Color endColor = Color.white;

    [Tooltip("If using multiple animations, only leave this ticked on the \"default\" one")]
    [SerializeField] public bool doSyncOnAwake = true;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        if (doSyncOnAwake)
            SyncEmissionColor();
    }

    public void SyncEmissionColor() => meshRenderer.material.SetColor("_EmissionColor", startColor);

    protected override void AnimateAtPoint(float evaluatedPoint)
        => meshRenderer.material.SetColor("_EmissionColor", Color.LerpUnclamped(startColor, endColor, evaluatedPoint));
}
