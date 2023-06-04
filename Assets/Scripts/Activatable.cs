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

    [SerializeField] protected SimpleAnim[] activateAnims;

    [Tooltip("Animations to play after activateAnims have completed")]
    [SerializeField] protected SimpleAnim[] activateCompletedAnims;

    protected float activateDuration;

    protected override void Awake()
    {
        base.Awake();

        activateDuration = activateAnims == null
            ? 0f
            : activateAnims.Select(x => x.AnimDuration).Max();
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
    }

    protected virtual IEnumerator Activate()
    {
        Array.ForEach(activateAnims, animate);
        yield return new WaitForSeconds(activateDuration);

        Array.ForEach(activateCompletedAnims, animate);

        void animate(SimpleAnim anim) => StartCoroutine(anim.AnimateNormal());
    }

    protected virtual IEnumerator Deactivate()
    {
        Array.ForEach(activateAnims, Animate);
        Array.ForEach(activateCompletedAnims, Animate);
        yield return new WaitForSeconds(activateDuration);

        void Animate(SimpleAnim anim) => StartCoroutine(anim.AnimateReversed());
    }
}
