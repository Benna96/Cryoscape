using UnityEngine;

public class SimpleAnimScaling : SimpleAnim
{
    [Header("Scaling specific")]
    [SerializeField] private float scaleMultiplier;

    private Vector3 origScale;
    private Vector3 targetScale;

    private void Awake()
    {
        origScale = transform.localScale;
        targetScale = scaleMultiplier * transform.localScale;
    }

    protected override void AnimateAtPoint(float evaluatedPoint)
        => transform.localScale = Vector3.LerpUnclamped(origScale, targetScale, evaluatedPoint);
}
