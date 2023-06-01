using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacesuitPod : MonoBehaviour
{
    GameObject padBackground;
    ButtonScript buttonTwo, buttonSeven, buttonEight, buttonNine, buttonTen, buttonEleven, buttonThirteen, buttonSixteen;

    // Start is called before the first frame update
    void Start()
    {
        padBackground = GameObject.Find("SpacesuitPad").transform.GetChild(0).gameObject;

        buttonTwo = padBackground.transform.GetChild(1).GetComponent<ButtonScript>();
        buttonSeven = padBackground.transform.GetChild(6).GetComponent<ButtonScript>();
        buttonEight = padBackground.transform.GetChild(7).GetComponent<ButtonScript>();
        buttonNine = padBackground.transform.GetChild(8).GetComponent<ButtonScript>();
        buttonTen = padBackground.transform.GetChild(9).GetComponent<ButtonScript>();
        buttonEleven = padBackground.transform.GetChild(10).GetComponent<ButtonScript>();
        buttonThirteen = padBackground.transform.GetChild(12).GetComponent<ButtonScript>();
        buttonSixteen = padBackground.transform.GetChild(15).GetComponent<ButtonScript>();

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
        if (buttonTwo.interacted && buttonSeven.interacted && buttonEight.interacted && buttonNine.interacted &&
            buttonTen.interacted && buttonEleven.interacted && buttonThirteen.interacted && buttonSixteen.interacted)
            {
                Debug.Log("Spacesuit Pod is open.");
            }
    }
}
