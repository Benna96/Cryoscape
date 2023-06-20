using UnityEngine;

public class SimpleAnimScalingAxis : SimpleAnim
{
    [Header("Scaling specific")]
    [SerializeField] AxisHelper.Axis axis;
    [SerializeField] private float scaleMultiplier;

    [Tooltip("If wanting to scale to 0 but also negative, just set targetscale here. Axis & scale multiplier will be ignored.")]
    [SerializeField] private Vector3 targetScaleOverride = Vector3.zero;

    private Vector3 origScale;
    private Vector3 targetScale;

    private new Renderer renderer;

    private void Awake()
    {
        origScale = transform.localScale;
        targetScale = Vector3.Scale(transform.localScale, axis.AsVector3Scale(scaleMultiplier));
        if (targetScaleOverride != Vector3.zero)
            targetScale = targetScaleOverride;

        renderer = GetComponent<Renderer>();
    }

    protected override void AnimateAtPoint(float evaluatedPoint)
    {
        var newScale = Vector3.LerpUnclamped(origScale, targetScale, evaluatedPoint);
        bool someScaleAxisIsZero = newScale.z == 0f || newScale.y == 0f || newScale.z == 0f;

        if (renderer.enabled && someScaleAxisIsZero)
            renderer.enabled = false;
        else if (!renderer.enabled && !someScaleAxisIsZero)
            renderer.enabled = true;

        transform.localScale = newScale;
    }
}
