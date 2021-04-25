using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindyTotem : Totem
{
    [Header("������ ������ �ٲ�� �ٶ��� ����")]
    [Tooltip("�ٶ��� �����Դϴ�.")]
    public eWindDirection windDirection;
    private eWindDirection originalWindDirection;

    //[Header("�ٶ��� ����")]
    //[Tooltip("�ٶ��� �����Դϴ�.")]
    //public float windMagnitude;

    //[Header("����� ����� ������")]
    //public AreaEffector2D areaEffector;
    [Header("���� ��Ʈ�ѷ�")]
    public WindController windController;

    [Header("����� ���� �ʵ�")]
    public ParticleSystemForceField forceField;

    //private float originalAngle;
    //private float originalMagnitude;
    private float originalForceFieldDirection;


    private void Awake()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
        // originalAngle = areaEffector.forceAngle;
        // originalMagnitude = areaEffector.forceMagnitude;
        originalForceFieldDirection = forceField.directionX.constant;
    }
    public void Update()
    {
        CheckingInput();
        Execute();
    }
    public override void Execute()
    {
        if (isOn) //���� 
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


