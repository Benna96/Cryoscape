using System;
using System.Collections;

using UnityEngine;

public static class CoroutineHelper
{
    public static IEnumerator StartWaitEnd(Action start, Action end, float duration)
    {
        start();
        yield return new WaitForSeconds(duration);
        end();
    }
}
