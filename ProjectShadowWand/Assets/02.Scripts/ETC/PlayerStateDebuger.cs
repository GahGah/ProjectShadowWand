using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDebuger : MonoBehaviour
{
    [Tooltip("스테이트 제대로 돌아가는지 테스트용의 텍스트 메쉬")]
    private TextMesh stateTextMesh = null;

    PlayerController player;

    private void Start()
    {
        player = PlayerController.Instance;
        CreateStateTextMesh();
    }

    private void Update()
    {
        UpdateStateTextMesh();
    }
    private void CreateStateTextMesh()
    {
        GameObject tempObject = new GameObject("stateTextMesh(Created)");
        tempObject.transform.localScale = new Vector3(0.3f, 0.3f);
        tempObject.transform.position = new Vector3(0f, 0f, -9f);

        stateTextMesh = tempObject.AddComponent<TextMesh>();

        stateTextMesh.fontSize = 20;
        stateTextMesh.characterSize = 0.5f;

        stateTextMesh.anchor = TextAnchor.MiddleCenter;
        stateTextMesh.alignment = TextAlignment.Center;
        stateTextMesh.color = Color.white;

    }
    private void UpdateStateTextMesh()
    {
        stateTextMesh.gameObject.transform.position = new Vector3(player.playerRigidbody.position.x, player.playerRigidbody.position.y + 0.8f, -9f);
        var _text = player.playerStateMachine.GetCurrentStateName();
        stateTextMesh.text = _text;
    }

}
