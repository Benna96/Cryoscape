using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SimpleAnimColor : SimpleAnim
{
    [Tooltip("Mesh will be set to this color on awake")]
    [SerializeField] private Color startColor = Color.white;

    [SerializeField] private Color endColor = Color.white;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = startColor;
    }

    protected override void AnimateAtPoint(float evaluatedPoint)
        => meshRenderer.material.color = Color.LerpUnclamped(startColor, endColor, evaluatedPoint);
}
