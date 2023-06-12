using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoebox : Interactable
{
    [SerializeField] private SimpleAnim[] doorSlideAnims;
    [SerializeField] private ColorLock colorLock;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(OpenDoor());

        IEnumerator OpenDoor()
        {
            yield return new WaitUntil(() => Array.TrueForAll(doorSlideAnims, anim => anim.initDone));
            Array.ForEach(doorSlideAnims, anim => StartCoroutine(anim.AnimateNormal()));
        }
    }

    protected override IEnumerator DoInteract()
    {
        yield return base.DoInteract();
        Array.ForEach(doorSlideAnims, anim => StartCoroutine(anim.AnimateReversed()));
        colorLock.isHavingEffect = true;
    }
}
