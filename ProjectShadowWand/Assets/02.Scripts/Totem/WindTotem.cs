using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTotem : Totem
{
    public AreaEffector2D areaEffector;
    public ParticleSystemForceField forceField;

    public SpriteRenderer sr;

    private float originalAngle;
    private float originalMagnitude;
    private float originalDirection;

    private bool isPlayerIn = false;

    [Tooltip("바람의 각도입니다.")]
    public float windAngle;

    [Tooltip("바람의 세기입니다.")]
    public float windMagnitude;

    private void Awake()
    {
        originalAngle = areaEffector.forceAngle;
        originalMagnitude = areaEffector.forceMagnitude;
        originalDirection = forceField.directionX.constant;
    }
    public void Update()
    {
        CheckingInput();

        if (isOn)
        {
            sr.color = Color.blue;
            if (windAngle != areaEffector.forceAngle)
            {
                areaEffector.forceAngle = windAngle;
                var test = new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * windAngle), Mathf.Cos(Mathf.Deg2Rad * windAngle));
                forceField.directionX = test.y;

            }
            if (windMagnitude != areaEffector.forceMagnitude)
            {
                areaEffector.forceMagnitude = windMagnitude;
            }
        }
        else
        {
            sr.color = Color.red;
            if (originalAngle != areaEffector.forceAngle)
            {
                areaEffector.forceAngle = originalAngle;
                forceField.directionX = originalDirection;
            }
            if (originalMagnitude != areaEffector.forceMagnitude)
            {
                areaEffector.forceMagnitude = originalMagnitude;
            }
        }
    }

    private void CheckingInput()
    {
        if (InputManager.Instance.buttonCatch.wasPressedThisFrame
            && isPlayerIn == true)
        {
            Debug.Log("is Change");
            isOn = !isOn;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
        }

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //DrawArrow.ForGizmo(gameObject.transform.position, Vector2.right);
    }

}


