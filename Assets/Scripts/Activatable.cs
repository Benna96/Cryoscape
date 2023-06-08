using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;

public class Activatable : Interactable
{
    [field: SerializeField] public bool isActivated { get; protected set; } = false;

    [Tooltip("If this is false, the object can only be activated once")]
    [SerializeField] protected bool toggleable = false;

    [SerializeField] protected bool disableInteractionWhileAnimating = false;

    [Tooltip("Delay after activate anims have finished to automatically deactivate.\nLeave as -1 to not auto deactivate.")]
    [SerializeField] protected float autoDeactivateTime = -1;

    [SerializeField] protected SimpleAnim[] activateAnims;

    [Tooltip("Animations to play after activateAnims have completed")]
    [SerializeField] protected SimpleAnim[] activateCompletedAnims;

    protected float activateDuration;
    protected float activateCompletedDuration;

    protected override void Awake()
    {
        base.Awake();

        activateDuration = activateAnims == null
            ? 0f
            : activateAnims.Select(x => x.AnimDuration).Max();

        activateCompletedDuration = activateCompletedAnims == null
            ? 0f
            : activateCompletedAnims.Select(x => x.AnimDuration).Max();
    }

    public override void Interact()
    {
        if (!isActivated)
            StartCoroutine(Activate());
        else
            StartCoroutine(Deactivate());

        isActivated = !isActivated;
        if (isActivated && !toggleable)
            isInteractable = false;

        if (isInteractable && disableInteractionWhileAnimating)
        {
            if (autoDeactivateTime < 0f)
                StartCoroutine(CoroutineHelper.StartWaitEnd(
                    () => isInteractable = false,
                    () => isInteractable = true,
                    activateDuration + activateCompletedDuration));
            else if (isActivated)
                StartCoroutine(CoroutineHelper.StartWaitEnd(
                    () => isInteractable = false,
                    () => isInteractable = true,
                    2f * activateDuration + 2f * activateCompletedDuration + autoDeactivateTime));
        }
    }

    protected virtual IEnumerator Activate()
    {
        Array.ForEach(activateAnims, Animate);
        yield return new WaitForSeconds(activateDuration);

        Array.ForEach(activateCompletedAnims, Animate);

        if (autoDeactivateTime >= 0)
        {
            yield return new WaitForSeconds(activateCompletedDuration);
            yield return new WaitForSeconds(autoDeactivateTime);
            Interact();
        }

        void Animate(SimpleAnim anim) => StartCoroutine(anim.AnimateNormal());
    }

    protected virtual IEnumerator Deactivate()
    {
        Array.ForEach(activateAnims, Animate);
        Array.ForEach(activateCompletedAnims, Animate);
        yield return new WaitForSeconds(activateDuration);

        void Animate(SimpleAnim anim) => StartCoroutine(anim.AnimateReversed());
    }
}
