using QuickOutline;

using UnityEngine;

/// <summary>
/// Base Interactable class.
/// Inherit from this class & override Interact function to implement some kind of interaction.
/// </summary>
[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    protected Outline outline;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    public virtual void EnableOutline() => outline.enabled = true;

    public virtual void DisableOutline() => outline.enabled = false;

    public virtual void Interact() => Debug.Log($"Interacted with {name}");
}
