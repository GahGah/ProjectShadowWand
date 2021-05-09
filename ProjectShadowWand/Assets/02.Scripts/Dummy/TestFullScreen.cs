using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFullScreen : MonoBehaviour
{


    public void ButtonOnFullScreen()
    {
        Screen.fullScreen = true;
    }

    public void ButtonOnWindowScreen()
    {
        Screen.fullScreen = false;
    }

    public void ButtonOn_EF()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

    public void ButtonOn_VF()
    {
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

}
