using System;

using UnityEngine;

public static class ColorExtensions
{
    public enum ColorOption
    {
        Red,
        Orange,
        Yellow,
        Green,
        Cyan,
        Blue,
        Magenta
    }

    public static Color ToColor(this ColorOption colorOption) => colorOption switch
    {
        ColorOption.Red => Color.red,
        ColorOption.Orange => new Color(1f, 0.5f, 0f),
        ColorOption.Yellow => Color.yellow,
        ColorOption.Green => Color.green,
        ColorOption.Cyan => Color.cyan,
        ColorOption.Blue => Color.blue,
        ColorOption.Magenta => Color.magenta, // Perhaps something else, like Color32(160, 32, 240, 255)
        _ => Color.clear
    };
}
