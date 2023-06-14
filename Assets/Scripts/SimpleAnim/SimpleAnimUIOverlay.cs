using UnityEngine;

public class SimpleAnimUIOverlay : SimpleAnim
{
    [SerializeField] public EffectOverlaysUI.Overlay overlay;

    protected override void AnimateAtPoint(float evaluatedPoint)
    {
        UIManager.instance.SetOverlayEffectOpacity(overlay, evaluatedPoint);
        
    }
}
