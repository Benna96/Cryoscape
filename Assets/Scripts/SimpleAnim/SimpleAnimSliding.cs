using UnityEngine;

public class SimpleAnimSliding : SimpleAnim
{
    [SerializeField] private AxisHelper.Axis axis;
    [SerializeField] private float distance;

    private Vector3 axisVector;
    private Vector3 origPosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        axisVector = axis.AsVector3();

        origPosition = transform.localPosition;
        targetPosition = transform.localPosition + distance * axisVector;
    }

    protected override void AnimateAtPoint(float evaluatedPoint)
        => transform.localPosition = Vector3.LerpUnclamped(origPosition, targetPosition, evaluatedPoint);
}
