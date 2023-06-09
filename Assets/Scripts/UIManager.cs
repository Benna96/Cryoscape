using System.Collections;

using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private EffectOverlaysUI effectOverlaysUI;

    /// <summary>
    /// Remember to call with StartCoroutine
    /// </summary>
    /// <param name="overlay"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public IEnumerator OverlayEffectForDuration (EffectOverlaysUI.Overlay overlay, float seconds)
        => effectOverlaysUI.EnableForDuration(overlay, seconds);
}
