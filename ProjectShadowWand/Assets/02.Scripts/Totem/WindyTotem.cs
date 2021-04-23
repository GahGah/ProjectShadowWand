using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindyTotem : Totem
{
    [Header("바람의 각도")]
    [Tooltip("바람의 각도입니다.")]
    public float windAngle;

    [Header("바람의 세기")]
    [Tooltip("바람의 세기입니다.")]
    public float windMagnitude;

    [Header("담당할 에어리어 이펙터")]
    public AreaEffector2D areaEffector;

    [Header("담당할 포스 필드")]
    public ParticleSystemForceField forceField;

    private float originalAngle;
    private float originalMagnitude;
    private float originalDirection;


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
                var test = new Vector2(Mathf.Cos(Mathf.Deg2Rad * windAngle), Mathf.Sin(Mathf.Deg2Rad * windAngle));
                forceField.directionX = test.x;
                //forceField.directionY = test.y;

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
        if (InputManager.Instance.buttonSkillUse.wasPressedThisFrame
            && isPlayerIn == true)
        {
            Debug.Log("is Change");
            isOn = !isOn;
        }
    }


}


