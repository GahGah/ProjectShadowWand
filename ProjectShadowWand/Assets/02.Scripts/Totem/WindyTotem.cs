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

    [Tooltip("바람의 각도입니다.")]
    public float windAngle;

    [Tooltip("바람의 세기입니다.")]
    public float windMagnitude;

    private void Awake()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
        originalAngle = areaEffector.forceAngle;
        originalMagnitude = areaEffector.forceMagnitude;
        originalDirection = forceField.directionX.constant;
    }
    public void Update()
    {
        CheckingInput();
        Execute();

    }
    public override void Execute()
    {
        if (isOn)
        {

            if (windAngle != areaEffector.forceAngle)
            {
                sr.color = Color.blue;
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

            if (originalAngle != areaEffector.forceAngle)
            {
                sr.color = Color.red;
                areaEffector.forceAngle = originalAngle;
                forceField.directionX = originalDirection;
            }
            if (originalMagnitude != areaEffector.forceMagnitude)
            {
                areaEffector.forceMagnitude = originalMagnitude;
            }
        }
        ColorChange();
    }


    protected override void CheckingInput()
    {
        if (InputManager.Instance.buttonCatch.wasPressedThisFrame
            && isPlayerIn == true)
        {
            Debug.Log("is Change");
            isOn = !isOn;
        }
    }


}


