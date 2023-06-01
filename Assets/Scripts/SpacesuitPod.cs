using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacesuitPod : MonoBehaviour
{
    ButtonScript buttonTwo, buttonSeven, buttonEight, buttonNine, buttonTen, buttonEleven, buttonThirteen, buttonSixteen;

    // Start is called before the first frame update
    void Start()
    {
        buttonTwo = GameObject.Find("button002").GetComponent<ButtonScript>();
        buttonSeven = GameObject.Find("button007").GetComponent<ButtonScript>();
        buttonEight = GameObject.Find("button008").GetComponent<ButtonScript>();
        buttonNine = GameObject.Find("button009").GetComponent<ButtonScript>();
        buttonTen = GameObject.Find("button010").GetComponent<ButtonScript>();
        buttonEleven = GameObject.Find("button011").GetComponent<ButtonScript>();
        buttonThirteen = GameObject.Find("button013").GetComponent<ButtonScript>();
        buttonSixteen = GameObject.Find("button016").GetComponent<ButtonScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            buttonTwo.interacted && buttonSeven.interacted && buttonEight.interacted && buttonNine.interacted &&
            buttonTen.interacted && buttonEleven.interacted && buttonThirteen.interacted && buttonSixteen.interacted)
            {
                Debug.Log("Spacesuit Pod is open.");
            }
    }
}
