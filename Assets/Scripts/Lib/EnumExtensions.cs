using System;
using System.Collections;
using System.Linq;

using UnityEngine;

public static class EnumExtensions
{
    public static int GetMaxValue<TEnum>() where TEnum : Enum
        => Enum.GetValues(typeof(TEnum)).Cast<int>().Max();

    public static int GetMinValue<TEnum>() where TEnum : Enum
        => Enum.GetValues(typeof(TEnum)).Cast<int>().Min();
}
