using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EffectOverlayUI : MonoBehaviour
{
    public enum Overlay
    {
        Frost
    }

    private Dictionary<Overlay, VisualElement> overlayElements = new();

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        overlayElements.Add(Overlay.Frost, root.Q("Frost"));
        foreach (var (key, value) in overlayElements)
            value.SetEnabled(false);
    }

    public IEnumerator EnableForDuration(Overlay overlay, float seconds)
    {
        var overlayElement = overlayElements[overlay];

        overlayElement.SetEnabled(true);
        yield return new WaitForSeconds(seconds);
        overlayElement.SetEnabled(false);
    }
}
