using UnityEngine;

public static class AxisHelper
{
    public enum Axis { X, Y, Z }

    public static Vector3 AsVector3(this Axis axis) => axis switch
    {
        Axis.X => Vector3.right,
        Axis.Y => Vector3.up,
        Axis.Z => Vector3.forward,
        _ => Vector3.zero
    };
}