using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindyTotem : Totem
{
    public AreaEffector2D areaEffector;
    public ParticleSystemForceField forceField;

    private float originalAngle;
    private float originalMagnitude;
    private float originalDirection;

    [Tooltip("�ٶ��� �����Դϴ�.")]
    public float windAngle;

    [Tooltip("�ٶ��� �����Դϴ�.")]
    public float windMagnitude;

    private void Awake()
    {
        originalAngle = areaEffector.forceAngle;
        originalMagnitude = areaEffector.forceMagnitude;
        originalDirection = forceField.directionX.constant;
        sr = GetComponent<SpriteRenderer>();
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


}


