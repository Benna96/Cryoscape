using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ColorLock : MonoBehaviour
{
    [SerializeField] private ColorInteractable[] buttons;
    [SerializeField] private ColorExtensions.ColorOption[] correctCombination;

    [SerializeField] private bool _isHavingEffect = false;
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

    private bool _isCorrectCombination;
    private bool isCorrectCombination
    {
        get => _isCorrectCombination;
        set
        {
            if (_isCorrectCombination == value)
                return;

            _isCorrectCombination = value;

            if (_isCorrectCombination)
                Array.ForEach(anims, anim => StartCoroutine(anim.AnimateNormal()));
            else
                Array.ForEach(anims, anim => StartCoroutine(anim.AnimateReversed()));
        }
    }

    [Tooltip("Anims to play with correct combination, or to reverse with incorrect combination")]
    [SerializeField] private SimpleAnim[] anims;

    private void Awake()
    {
        Array.ForEach(buttons, button => button.PropertyChanged += HandleButtonColorChange);
    }

    private void HandleButtonColorChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (isHavingEffect && e.PropertyName == nameof(ColorInteractable.currentColor))
            CheckCombination();
    }

    private void CheckCombination() => isCorrectCombination = buttons.Select(x => x.currentColor).SequenceEqual(correctCombination);
}
