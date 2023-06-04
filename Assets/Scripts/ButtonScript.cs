using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ButtonScript : Activatable
{
    public string buttonName;
    // Start is called before the first frame update
    void Start()
    {
        buttonName = gameObject.name;
    }
}
