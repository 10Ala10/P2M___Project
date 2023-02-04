using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishButton : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isClicked = false;
    public void Click()
    {
        isClicked = true;
    }
    // public void desablerClick()
    // {
    //     isClisked = false;
    // }
}
