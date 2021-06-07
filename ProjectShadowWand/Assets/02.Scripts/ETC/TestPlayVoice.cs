using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayVoice : MonoBehaviour
{

    public void PlayFallVoice()
    {
        AudioManager.Instance.Play_TalkVoice_PitchUp(2);
    }
}
