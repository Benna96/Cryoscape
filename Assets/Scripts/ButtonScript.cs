using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : Interactable
{
    public bool interacted;
    public string buttonName;
    // Start is called before the first frame update
    void Start()
    {
        interacted = false;
        name = gameObject.name;
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
            Debug.Log($"{name} unpressed");
        } else if (!interacted)
        {
            interacted = true;
            Debug.Log($"{name} pressed");
        }
    }
}
