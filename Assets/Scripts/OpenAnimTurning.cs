using UnityEngine;

public class OpenAnimTurning : OpenAnim
{

    [Header("Turning specific")]
    [SerializeField] private AxisHelper.Axis axis;
    [SerializeField] private float angle;

    private Vector3 axisVector;
    private Quaternion origRotation;
    private Quaternion targetRotation;

    private void Awake()
    {
        axisVector = axis.AsVector3();

        origRotation = transform.localRotation;
        targetRotation = origRotation * Quaternion.AngleAxis(angle, axisVector);
    }

    protected override void AnimateAtPoint(float evaluatedPoint)
        => transform.localRotation = Quaternion.LerpUnclamped(origRotation, targetRotation, evaluatedPoint);
}
