using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{

    public void PlayPush()
    {
        AudioManager.Instance.Play_UI_Button_Push();
    }

    public void PlayImagePush()
    {
        AudioManager.Instance.Play_UI_ImageButton_Push();
    }
}
