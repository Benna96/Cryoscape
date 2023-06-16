using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EffectOverlaysUI : MonoBehaviour
{
    public enum Overlay
    {
        Frost,
        Spill,
        Light
    }

    private Dictionary<Overlay, VisualElement> overlayElements = new();

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        overlayElements.Add(Overlay.Frost, root.Q("Frost"));
        overlayElements.Add(Overlay.Spill, root.Q("Spill"));
        overlayElements.Add(Overlay.Light, root.Q("Light"));
        foreach (var (key, value) in overlayElements)
            value.SetEnabled(false);
    }

    public void SetOverlayOpacity(Overlay overlay, float opacity)
        => overlayElements[overlay].style.opacity = opacity;
}
