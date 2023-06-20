using UnityEngine;

public class SimpleAnimScalingAxis : SimpleAnim
{
    [Header("Scaling specific")]
    [SerializeField] AxisHelper.Axis axis;
    [SerializeField] private float scaleMultiplier;

    private Vector3 origScale;
    private Vector3 targetScale;

    private void Awake()
    {
        origScale = transform.localScale;
        targetScale = Vector3.Scale(transform.localScale, axis.AsVector3Scale(scaleMultiplier));
    }

    protected override void AnimateAtPoint(float evaluatedPoint)
        => transform.localScale = Vector3.LerpUnclamped(origScale, targetScale, evaluatedPoint);
}
