using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacesuitPod : MonoBehaviour
{
    ButtonScript buttonTwo, buttonSeven, buttonEight, buttonNine, buttonTen, buttonEleven, buttonThirteen, buttonSixteen;

    // Start is called before the first frame update
    void Start()
    {
        buttonTwo = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(1).GetComponent<ButtonScript>();
        buttonSeven = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(6).GetComponent<ButtonScript>();
        buttonEight = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(7).GetComponent<ButtonScript>();
        buttonNine = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(8).GetComponent<ButtonScript>();
        buttonTen = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(9).GetComponent<ButtonScript>();
        buttonEleven = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(10).GetComponent<ButtonScript>();
        buttonThirteen = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(12).GetComponent<ButtonScript>();
        buttonSixteen = GameObject.Find("SpacesuitPad").transform.GetChild(0).GetChild(15).GetComponent<ButtonScript>();

        Debug.Log(buttonTwo.name);
        Debug.Log(buttonSeven.name);
        Debug.Log(buttonEight.name);
        Debug.Log(buttonNine.name);
        Debug.Log(buttonTen.name);
        Debug.Log(buttonEleven.name);
        Debug.Log(buttonThirteen.name);
        Debug.Log(buttonSixteen.name);
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
