using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class TestShadowTextureLoad : MonoBehaviour
{
    public RawImage rawImage;
    public RenderTexture testRenderTexture;
    // Start is called before the first frame update
    void Start()
    {
        //bits = 0,16,24라는데...24비트로 했을 때만 스텐실 버퍼가 생긴다함

        //rawImage.texture = testRenderTexture;
        //testRenderTexture = ShadowUtils.GetTemporaryShadowTexture(1920, 1080, 24);

        rawImage.texture = testRenderTexture;
        testRenderTexture = ShadowUtils.GetTemporaryShadowTexture(1920, 1080, 24);
        RenderTexture.ReleaseTemporary(testRenderTexture);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
