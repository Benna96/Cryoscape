using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : Interactable
{
    public bool interacted;
    public string buttonName;
    SimpleAnim[] buttonPressAnims;
    // Start is called before the first frame update
    void Start()
    {
        interacted = false;
        buttonName = gameObject.name;
        buttonPressAnims = transform.GetChild(0).GetComponents<SimpleAnim>();
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
            Array.ForEach(buttonPressAnims, anim => StartCoroutine(anim.AnimateNormal()));
            Debug.Log($"{name} unpressed");
        } else if (!interacted)
        {
            interacted = true;
            Array.ForEach(buttonPressAnims, anim => StartCoroutine(anim.AnimateReversed()));
            Debug.Log($"{name} pressed");
        }
    }
}
