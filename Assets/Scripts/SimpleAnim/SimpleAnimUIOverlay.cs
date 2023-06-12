using UnityEngine;

public class SimpleAnimUIOverlay : SimpleAnim
{
    [SerializeField] public EffectOverlaysUI.Overlay overlay;
    [SerializeField] public AudioSource audioSource;

    protected override void AnimateAtPoint(float evaluatedPoint)
    {
        audioSource.Play();
        UIManager.instance.SetOverlayEffectOpacity(overlay, evaluatedPoint);
        
    }
}
