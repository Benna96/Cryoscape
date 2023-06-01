using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAnimTurning : OpenAnim
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [Header("Turning specific")]
    [SerializeField] private Axis axis;
    [SerializeField] private float angle;

    private Vector3 axisVector;
    private Quaternion origRotation;
    private Quaternion targetRotation;

    private void Awake()
    {
        axisVector = axis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.zero
        };

        origRotation = transform.rotation;
        targetRotation = origRotation * Quaternion.AngleAxis(angle, axisVector);
    }

    protected override void AnimateAtPoint(float evaluatedPoint)
        => transform.rotation = Quaternion.LerpUnclamped(origRotation, targetRotation, evaluatedPoint);
}
