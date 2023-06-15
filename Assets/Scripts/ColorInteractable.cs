using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using UnityEngine;

using ColorOption = ColorExtensions.ColorOption;

public class ColorInteractable : Interactable, INotifyPropertyChanged
{
    [SerializeField] private SimpleAnimColor colorChangeAnim;

    [SerializeField] private ColorOption _currentColor;
    public ColorOption currentColor
    {
        get => _currentColor;
        private set
        {
            _currentColor = value;
            OnPropertyChanged();
        }
    }

    protected override void Awake()
    {
        if (!interactAnims.Contains(colorChangeAnim))
            interactAnims = interactAnims.Append(colorChangeAnim).ToArray();

        base.Awake();
    }

    private void Start()
    {
        colorChangeAnim.startColor = currentColor.ToColor();
        colorChangeAnim.SyncColor();
    }

    protected override IEnumerator DoInteract()
    {
        var newColor = currentColor + 1;
        if ((int)newColor > EnumExtensions.GetMaxValue<ColorOption>())
            newColor = (ColorOption)EnumExtensions.GetMinValue<ColorOption>();

        colorChangeAnim.startColor = currentColor.ToColor();
        colorChangeAnim.endColor = newColor.ToColor();

        yield return StartCoroutine(base.DoInteract());
        currentColor = newColor;
    }
}
