﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.Serialization;

public class Activatable : Interactable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [FormerlySerializedAs("isActivated")]
    [SerializeField] private bool _isActivated = false;
    public bool isActivated
    {
        get => _isActivated;
        protected set
        {
            _isActivated = value;
            ObservableHelper.OnPropertyChanged(PropertyChanged);
        }
    }

    public bool shouldFail = false;

    [Tooltip("If this is false, the object can only be activated once")]
    [SerializeField] protected bool toggleable = false;

    [SerializeField] protected bool disableInteractionWhileAnimating = false;

    [Tooltip("Delay after activate anims have finished to automatically deactivate.\nLeave as -1 to not auto deactivate.")]
    [SerializeField] protected float autoDeactivateTime = -1;

    [SerializeField] protected SimpleAnim[] activateAnims;

    [Tooltip("Animations to play after activateAnims have completed")]
    [SerializeField] protected SimpleAnim[] activateCompletedAnims;

    [Tooltip("Animations to play upon failed interact (not having required item or such)")]
    [SerializeField] protected SimpleAnim[] failedActivateAnims;

    protected float activateDuration;
    protected float activateCompletedDuration;
    protected float failedActivateDuration;

    protected override void Awake()
    {
        base.Awake();

        activateDuration = activateAnims.Length == 0
            ? 0f
            : activateAnims.Select(x => x.AnimDuration).Max();

        activateCompletedDuration = activateCompletedAnims.Length == 0
            ? 0f
            : activateCompletedAnims.Select(x => x.AnimDuration).Max();

        failedActivateDuration = failedActivateAnims.Length == 0
            ? 0f
            : failedActivateAnims.Select(x => x.AnimDuration).Max();
    }

    public override void Interact()
    {
        if (!isActivated)
            StartCoroutine(Activate());
        else
            StartCoroutine(Deactivate());

        if (shouldFail)
            return;

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
        if (!shouldFail)
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
        }
        else
        {
            Array.ForEach(failedActivateAnims, Animate);
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
