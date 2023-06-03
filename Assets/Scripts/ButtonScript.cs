using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ButtonScript : Interactable
{
    [SerializeField] private SimpleAnim[] pressTriggeredAnims;

    public bool interacted;
    public string buttonName;
    SimpleAnim[] buttonPressAnims;
    private float buttonPressDuration;
    // Start is called before the first frame update
    void Start()
    {
        interacted = false;
        buttonName = gameObject.name;
        buttonPressAnims = transform.GetChild(0).GetComponents<SimpleAnim>();
        buttonPressDuration = buttonPressAnims.Select(x => x.AnimDuration).Max();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (interacted)
        {
            interacted = false;
            StartCoroutine(Unpress());
        } else if (!interacted)
        {
            interacted = true;
            StartCoroutine(Press());
        }
    }

    private IEnumerator Press()
    {
        Array.ForEach(buttonPressAnims, anim => StartCoroutine(anim.AnimateNormal()));
        yield return new WaitForSeconds(buttonPressDuration);

        Array.ForEach(pressTriggeredAnims, anim => StartCoroutine(anim.AnimateNormal()));
    }

    private IEnumerator Unpress()
    {
        Array.ForEach(buttonPressAnims, anim => StartCoroutine(anim.AnimateReversed()));
        Array.ForEach(pressTriggeredAnims, anim => StartCoroutine(anim.AnimateReversed()));
        yield return new WaitForSeconds(buttonPressDuration);
    }
}
