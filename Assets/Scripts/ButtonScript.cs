using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : Interactable
{
    public bool interacted;
    public string buttonName;
    SimpleAnim buttonPress;
    // Start is called before the first frame update
    void Start()
    {
        interacted = false;
        buttonName = gameObject.name;
        buttonPress = transform.GetChild(0).GetComponent<SimpleAnim>();
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
            StartCoroutine(buttonPress.AnimateReversed());
            Debug.Log($"{name} unpressed");
        } else if (!interacted)
        {
            interacted = true;
            StartCoroutine(buttonPress.AnimateNormal());
            Debug.Log($"{name} pressed");
        }
    }
}
