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

    public static Vector3 AsVector3Scale(this Axis axis, float scale) => axis switch
    {
        Axis.X => new(scale, 1f, 1f),
        Axis.Y => new(1f, scale, 1f),
        Axis.Z => new(1f, 1f, scale),
        _ => new(1f, 1f, 1f)
    };
}