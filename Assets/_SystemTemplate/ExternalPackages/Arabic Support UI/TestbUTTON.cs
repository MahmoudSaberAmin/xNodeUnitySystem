using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestbUTTON : MonoBehaviour
{
    public Button TestButton;
    
    private ArabicText Arabic = new ArabicText();

    private void Start()
    {
        TestButton.transform.GetChild(0).gameObject.AddComponent<ArabicText>();
        TestButton.GetComponentInChildren <ArabicText>().Text = "أشطة عليك";
        //TestButton.GetComponentInChildren<Text>().text = "أشطة عليك";
    }
}
