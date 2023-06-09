using System.Collections;

using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private EffectOverlaysUI effectOverlaysUI;

    public void SetOverlayEffectOpacity(EffectOverlaysUI.Overlay overlay, float opacity) => effectOverlaysUI.SetOverlayOpacity(overlay, opacity);
}
