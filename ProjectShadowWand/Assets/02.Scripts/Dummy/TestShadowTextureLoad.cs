using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TestShadowTextureLoad : MonoBehaviour
{
    RenderTexture testRenderTexture;
    // Start is called before the first frame update
    void Start()
    {
        //bits = 0,16,24라는데...24비트로 했을 때만 스텐실 버퍼가 생긴다함
        testRenderTexture = ShadowUtils.GetTemporaryShadowTexture(500, 500, 24);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
