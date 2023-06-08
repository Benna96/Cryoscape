using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacesuitPod : MonoBehaviour
{
    SimpleAnim podOpening;
    GameObject padBackground;
    Activatable buttonTwo, buttonSeven, buttonEight, buttonNine, buttonTen, buttonEleven, buttonThirteen, buttonSixteen,
            buttonOne, buttonThree, buttonFour, buttonFive, buttonSix, buttonTwelve, buttonFourteen, buttonFifteen;
    bool podIsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        podOpening = gameObject.transform.GetChild(0).gameObject.GetComponent<SimpleAnim>();
        padBackground = GameObject.Find("SpacesuitPad").transform.GetChild(0).gameObject;

        buttonTwo = padBackground.transform.GetChild(1).GetComponent<Activatable>();
        buttonSeven = padBackground.transform.GetChild(6).GetComponent<Activatable>();
        buttonEight = padBackground.transform.GetChild(7).GetComponent<Activatable>();
        buttonNine = padBackground.transform.GetChild(8).GetComponent<Activatable>();
        buttonTen = padBackground.transform.GetChild(9).GetComponent<Activatable>();
        buttonEleven = padBackground.transform.GetChild(10).GetComponent<Activatable>();
        buttonThirteen = padBackground.transform.GetChild(12).GetComponent<Activatable>();
        buttonSixteen = padBackground.transform.GetChild(15).GetComponent<Activatable>();
        buttonOne = padBackground.transform.GetChild(0).GetComponent<Activatable>();
        buttonThree = padBackground.transform.GetChild(2).GetComponent<Activatable>();
        buttonFour = padBackground.transform.GetChild(3).GetComponent<Activatable>();
        buttonFive = padBackground.transform.GetChild(4).GetComponent<Activatable>();
        buttonSix = padBackground.transform.GetChild(5).GetComponent<Activatable>();
        buttonTwelve = padBackground.transform.GetChild(11).GetComponent<Activatable>();
        buttonFourteen = padBackground.transform.GetChild(13).GetComponent<Activatable>();
        buttonFifteen = padBackground.transform.GetChild(14).GetComponent<Activatable>();
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
