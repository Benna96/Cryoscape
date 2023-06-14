using System;
using System.Collections;
using System.ComponentModel;

using UnityEngine;

public abstract class CombinationLock : MonoBehaviour
{
    public delegate void LockStateChangedHandler(CombinationLock sender, LockEventArgs e);
    public event LockStateChangedHandler LockStateChanged;

    [Header("Lock variables")]

    [Tooltip("Anims to play with correct combination, or to reverse with incorrect combination")]
    [SerializeField] protected SimpleAnim[] anims;

    [SerializeField] private bool _isHavingEffect = true;
    public bool isHavingEffect
    {
        get => _isHavingEffect;
        set
        {
            _isHavingEffect = value;

            if (value)
                CheckCombination();
        }
    }

    protected bool isCorrectCombination { get; private set; }

    protected void UpdateCombination()
    {
        if (!isHavingEffect)
            return;

        bool wasCorrectCombination = isCorrectCombination;
        isCorrectCombination = CheckCombination();

        if (wasCorrectCombination == isCorrectCombination)
            return;

        else if (!wasCorrectCombination && isCorrectCombination)
        {
            Array.ForEach(anims, anim => StartCoroutine(anim.AnimateNormal()));
            LockStateChanged?.Invoke(this, new(true));
        }

        else if (wasCorrectCombination && !isCorrectCombination)
        {
            Array.ForEach(anims, anim => StartCoroutine(anim.AnimateReversed()));
            LockStateChanged?.Invoke(this, new(false));
        }
    }

    protected abstract bool CheckCombination();
}

public class LockEventArgs : EventArgs
{
    public bool isLocked { get; private set; }

    public LockEventArgs(bool isLocked)
    {
        this.isLocked = isLocked;
    }
}
