using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private EffectOverlayUI effectOverlaysUI;

    public void OverlayEffectForDuration    (EffectOverlayUI.Overlay overlay, float seconds)
        => effectOverlaysUI.EnableForDuration(overlay, seconds);
}
