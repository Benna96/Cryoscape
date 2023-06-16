using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.Serialization;

public class Activatable : Interactable, INotifyPropertyChanged
{
    [SerializeField, FormerlySerializedAs("isActivated")]
    private bool _isActivated = false;
    public bool isActivated
    {
        get => _isActivated;
        protected set
        {
            _isActivated = value;
            OnPropertyChanged();
        }
    }

    [Tooltip("If this is false, the object can only be activated once")]
    [SerializeField, FormerlySerializedAs("toggleable")]
    private bool _toggleable = true;
    protected bool toggleable
    {
        get => _toggleable;
        set
        {
            _toggleable = value;
            OnPropertyChanged();
        }
    }

    [SerializeField] protected bool activationConsumesRequiredItem = false;
    [SerializeField] protected Item resultingItem = null;

    [Tooltip("Delay after activate anims have finished to automatically deactivate.\nLeave as -1 to not auto deactivate.")]
    [SerializeField] protected float autoDeactivateTime = -1;

    [FormerlySerializedAs("activateCompletedAnims")]
    [Tooltip("Animations to play after " + nameof(interactAnims) + " have completed")]
    [SerializeField] protected SimpleAnim[] interactCompletedAnims;

    protected float interactCompletedDuration;

    protected override float applicableInteractDuration => shouldFail ? failedInteractDuration
            : autoDeactivateTime < 0f ? interactDuration + interactCompletedDuration
            : isActivated ? 2f * interactDuration + 2f * interactCompletedDuration + autoDeactivateTime
            : 0f;

    protected override void Awake()
    {
        base.Awake();

        isInteractableConditions.Add(_ => !(isActivated && !toggleable));
        PropertyChanged += (_, e) => { if (e.PropertyName == nameof(isActivated) || e.PropertyName == nameof(toggleable)) UpdateIsInteractable(); };
        UpdateIsInteractable();

        interactCompletedDuration = GetMaxAnimDuration(interactCompletedAnims);

        static float GetMaxAnimDuration(SimpleAnim[] anims)
            => anims.Length == 0 ? 0f : anims.Select(x => x.AnimDuration).Max();
    }

    protected override IEnumerator DoInteract()
    {
        isActivated = !isActivated;

        yield return StartCoroutine(isActivated ? Activate() : Deactivate());
    }

    protected override IEnumerator DoFailedInteract()
    {
        yield return StartCoroutine(!isActivated ? FailedActivate() : FailedDeactivate());
    }

    protected virtual IEnumerator Activate()
    {
        Array.ForEach(interactAnims, Animate);
        if (activationConsumesRequiredItem && requiredItem != null)
            InventoryManager.instance.RemoveItem(requiredItem);
        yield return new WaitForSeconds(interactDuration);

        if (!activationConsumesRequiredItem && resultingItem != null && requiredItem != null)
            InventoryManager.instance.ReplaceItem(requiredItem, resultingItem);


        Array.ForEach(interactCompletedAnims, Animate);

        if (autoDeactivateTime >= 0)
        {
            yield return new WaitForSeconds(interactCompletedDuration);
            yield return new WaitForSeconds(autoDeactivateTime);
            Interact();
        }

        void Animate(SimpleAnim anim) => StartCoroutine(anim.AnimateNormal());
    }

    protected virtual IEnumerator Deactivate()
    {
        Array.ForEach(interactAnims, Animate);
        Array.ForEach(interactCompletedAnims, Animate);
        yield return new WaitForSeconds(interactDuration);

        void Animate(SimpleAnim anim) => StartCoroutine(anim.AnimateReversed());
    }

    protected virtual IEnumerator FailedActivate()
    {
        Array.ForEach(failedInteractAnims, Animate);
        yield break;

        void Animate(SimpleAnim anim) => StartCoroutine(anim.AnimateNormal());
    }

    protected virtual IEnumerator FailedDeactivate()
    {
        Array.ForEach(failedInteractAnims, Animate);
        yield break;

        void Animate(SimpleAnim anim) => StartCoroutine(anim.AnimateReversed());
    }
}
