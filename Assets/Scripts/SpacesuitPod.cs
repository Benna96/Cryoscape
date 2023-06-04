using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacesuitPod : MonoBehaviour
{
    SimpleAnim podOpening;
    GameObject padBackground;
    ButtonScript buttonTwo, buttonSeven, buttonEight, buttonNine, buttonTen, buttonEleven, buttonThirteen, buttonSixteen,
            buttonOne, buttonThree, buttonFour, buttonFive, buttonSix, buttonTwelve, buttonFourteen, buttonFifteen;
    bool podIsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        podOpening = gameObject.transform.GetChild(0).gameObject.GetComponent<SimpleAnim>();
        padBackground = GameObject.Find("SpacesuitPad").transform.GetChild(0).gameObject;

        buttonTwo = padBackground.transform.GetChild(1).GetComponent<ButtonScript>();
        buttonSeven = padBackground.transform.GetChild(6).GetComponent<ButtonScript>();
        buttonEight = padBackground.transform.GetChild(7).GetComponent<ButtonScript>();
        buttonNine = padBackground.transform.GetChild(8).GetComponent<ButtonScript>();
        buttonTen = padBackground.transform.GetChild(9).GetComponent<ButtonScript>();
        buttonEleven = padBackground.transform.GetChild(10).GetComponent<ButtonScript>();
        buttonThirteen = padBackground.transform.GetChild(12).GetComponent<ButtonScript>();
        buttonSixteen = padBackground.transform.GetChild(15).GetComponent<ButtonScript>();
        buttonOne = padBackground.transform.GetChild(0).GetComponent<ButtonScript>();
        buttonThree = padBackground.transform.GetChild(2).GetComponent<ButtonScript>();
        buttonFour = padBackground.transform.GetChild(3).GetComponent<ButtonScript>();
        buttonFive = padBackground.transform.GetChild(4).GetComponent<ButtonScript>();
        buttonSix = padBackground.transform.GetChild(5).GetComponent<ButtonScript>();
        buttonTwelve = padBackground.transform.GetChild(11).GetComponent<ButtonScript>();
        buttonFourteen = padBackground.transform.GetChild(13).GetComponent<ButtonScript>();
        buttonFifteen = padBackground.transform.GetChild(14).GetComponent<ButtonScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonTwo.isActivated && buttonSeven.isActivated && buttonEight.isActivated && buttonNine.isActivated &&
            buttonTen.isActivated && buttonEleven.isActivated && buttonThirteen.isActivated && buttonSixteen.isActivated &&
            !buttonOne.isActivated && !buttonThree.isActivated && !buttonFour.isActivated && !buttonFive.isActivated && 
            !buttonSix.isActivated && !buttonTwelve.isActivated && !buttonFourteen.isActivated && !buttonFifteen.isActivated)
            {
                if (!podIsOpen)
                {
                    StartCoroutine(podOpening.AnimateNormal());
                    podIsOpen = true;
                }
            }
    }
}
