using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ECameraState
{
    DEFAULT = 0,
    ZOOMIN = 1,
    ZOOMOUT,
    STAY
}
public class CameraManager : Manager<CameraManager>
{
    [Tooltip("����� ī�޶�. ���� ���� ��� �ڵ����� ���� ī�޶� �ֽ��ϴ�. ")]
    public Camera currentCamera;

    [Tooltip("�ȷο�, ����, �ܾƿ� ���")]
    public Transform target;

    [Tooltip("size�� ���� �������� ��ȭ��ų ������Ʈ...")]
    public GameObject BGObject;

    //�� ������Ʈ�� ������ x,y��.
    private float scObjX = 0f;
    private float scObjY = 0f;

    private Vector3 scObjVel = Vector3.zero;

    [Header("�⺻ ����")]

    [Tooltip("����/�ܾƿ��� ����ϴ°�?")]
    public bool useZoom;

    [Tooltip("ī�޶��� �⺻ z�� �Դϴ�. �ܰ��� ���� �����ϴ�.")]
    public float cameraDefaultPositionZ = -10f;

    [Tooltip("ī�޶��� ����...��...���� ������. ���� ��?")]
    public float cameraDefaultSize = 5f;

    [Tooltip("ī�޶� �� ���� �ִ�ġ")]
    public float cameraZoomInSize = 3f;

    [Tooltip("ī�޶� �� �ƿ��� �ִ�ġ")]
    public float cameraZoomOutSize = 10f;

    [Tooltip("�ȷο� �ӵ�")]
    public float followSpeed;

    [Tooltip("����/�ܾƿ� �ӵ�. �ð������ ��� �ӵ��� �ƴ� '��'�� �ۿ���.")]
    public float zoomSpeed;

    [Tooltip("�ð�����ΰ�? : ����/�ܾƿ� ��, �ӵ��� �Ű澲�� �ʰ� ������ zoomSpeed�� �ȿ� ����/�ܾƿ��� ����.")]
    public bool isTimeMode;


    private float velocity = 0.0f;//�� ���ӵ�

    public float zoomTimer = 0f;
    public float currentZoomSpeed;

    [SerializeField]
    private ECameraState cameraState;

    [Header("ī�޶� ���ѿ��� ����")]
    [Tooltip("���� ���� ������ �� ���ΰ�?")]
    public bool isConfine;

    public Vector2 confinePos;
    public Vector2 confineSize;

    private Vector3 currentConfinePos;
    private Vector3 currentChangeSize;
    private float height;
    private float width;

    private IEnumerator currentCoroutine;

    private float limitCalSize;

    private Vector3 originBGObjectScale;


    public GameObject whiteScreen;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }


    public void Init()
    {


        height = currentCamera.orthographicSize;
        width = height * Screen.width / Screen.height;

        #region if <=0

        if (currentCamera == null)
        {
            Debug.Log("currentCamera�� null");
        }
        if (zoomSpeed <= 0f)
        {
            zoomSpeed = 1f;
        }

        if (followSpeed <= 0f)
        {
            followSpeed = 5f;
        }

        if (BGObject == null)
        {
            BGObject = new GameObject();
            BGObject.transform.parent = gameObject.transform;
            //�׳� ���� ������ ���� ��� �ϳ� �ֱ�.
        }
        #endregion

        limitCalSize = 0.03f;
        currentZoomSpeed = 1f / zoomSpeed;
        cameraState = ECameraState.DEFAULT;
        isTimeMode = true;

        scObjX = BGObject.transform.localScale.x;
        scObjY = BGObject.transform.localScale.y;
        //cameraDefaultPositionZ = -10f;

        originBGObjectScale = BGObject.transform.localScale;
    }



    private void Start()
    {
        if (useZoom == true)
        {
            StartCoroutine(CameraZoom());
            StartCoroutine(CameraControl());

        }

    }

    //private void Update()
    //{
    //    //zoomSpeed�� ����...���� �ֱ� ������.



    //}

    private void FixedUpdate()
    {
        FollowTarget(); // Ÿ�� �ȷ���
    }

    private void FollowTarget() // Ÿ�� �ȷ���
    {
        currentCamera.transform.position = Vector3.Lerp(currentCamera.transform.position, target.position, Time.smoothDeltaTime * followSpeed);


        if (isConfine) //���� ������ �Ǿ������� 
        {
            currentConfinePos = GetConfinePosition();
            currentCamera.transform.position = new Vector3(currentConfinePos.x, currentConfinePos.y, cameraDefaultPositionZ);
        }
        else
        {
            currentCamera.transform.position = new Vector3(currentCamera.transform.position.x, currentCamera.transform.position.y, cameraDefaultPositionZ);
        }

    }
    private IEnumerator CameraZoom()
    {
        while (true)
        {
            currentZoomSpeed = 1f / zoomSpeed;

            if (InputManager.Instance.buttonScroll.ReadValue().y > 0)
            {

                cameraState = ECameraState.ZOOMIN;
            }

            if (InputManager.Instance.buttonScroll.ReadValue().y < 0)
            {

                cameraState = ECameraState.ZOOMOUT;
            }
            yield return null;
        }

    }
    private IEnumerator CameraControl()
    {
        while (true)
        {
            switch (cameraState)
            {
                case ECameraState.DEFAULT:
                    if (currentCamera.orthographicSize > cameraDefaultSize)
                    {
                        currentCoroutine = CameraZoomIn(cameraDefaultSize);
                    }
                    else if (currentCamera.orthographicSize < cameraDefaultSize)
                    {
                        currentCoroutine = CameraZoomOut(cameraDefaultSize);
                    }
                    else
                    {
                        currentCoroutine = null;
                    }
                    break;

                case ECameraState.ZOOMIN:
                    currentCoroutine = CameraZoomIn(cameraZoomInSize);
                    break;

                case ECameraState.ZOOMOUT:
                    currentCoroutine = CameraZoomOut(cameraZoomOutSize);
                    break;

                case ECameraState.STAY:
                    currentCoroutine = null;
                    break;

                default:
                    currentCoroutine = null;
                    break;
            }

            if (currentCoroutine != null)
            {
                yield return StartCoroutine(currentCoroutine);
            }
            else
            {

                yield return null;
            }
        }
    }
    private IEnumerator CameraZoomOut(float _size)
    {
        if (NowCameraState() == ECameraState.ZOOMIN)
        {
            _size = cameraDefaultSize;
        }

        if (isTimeMode) //�ð� ����� ���
        {
            zoomTimer = 0f;
            float oldOrthographicSize = currentCamera.orthographicSize; // ���� ����� ����

            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                zoomTimer += Time.smoothDeltaTime * currentZoomSpeed;
                currentCamera.orthographicSize = Mathf.Lerp(oldOrthographicSize, _size, zoomTimer);
                //������ ���� �ϰ���� ������Ʈ�� ��������...�� �� �ۼ�Ʈ��ŭ���� �����Ų��. �Ƿ���...

                // BG ũ�� ü����
                ChangeScaleBGObject();

                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        else //�ƴ� ���
        {

            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                //zoomTimer += Time.deltaTime;
                currentCamera.orthographicSize = Mathf.SmoothDamp(currentCamera.orthographicSize, _size, ref velocity, zoomSpeed);
                //ChangeScaleThisObject();

                // BG ũ�� ü����
                ChangeScaleBGObject();
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        currentCamera.orthographicSize = _size;
        cameraState = ECameraState.STAY;



    }
    private IEnumerator CameraZoomIn(float _size)
    {
        if (NowCameraState() == ECameraState.ZOOMOUT)
        {
            _size = cameraDefaultSize;
        }

        if (isTimeMode) //�ð� ����� ���
        {
            zoomTimer = 0f;
            float oldOrthographicSize = currentCamera.orthographicSize; // ���� ����� ����

            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                zoomTimer += Time.smoothDeltaTime * currentZoomSpeed;
                currentCamera.orthographicSize = Mathf.Lerp(oldOrthographicSize, _size, zoomTimer);

                // BG ũ�� ü����
                ChangeScaleBGObject();

                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        else //�ƴ� ���
        {
            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                // zoomTimer += Time.deltaTime;
                currentCamera.orthographicSize = Mathf.SmoothDamp(currentCamera.orthographicSize, _size, ref velocity, zoomSpeed);

                // BG ũ�� ü����
                ChangeScaleBGObject();

                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }

        currentCamera.orthographicSize = _size;
        cameraState = ECameraState.STAY;

        // BG ũ�� ü����
        ChangeScaleBGObject();
    }


    private Vector3 limitTest;
    private Vector3 GetConfinePosition()
    {
        height = currentCamera.orthographicSize;
        width = height * Screen.width / Screen.height;
        //�ֵ� ���� �װŸ� �ϱ� ���ؼ��� �� �κп� ���̵��� ��ġ�� ������.
        float localX = confineSize.x * 0.5f - width;

        float localY = confineSize.y * 0.5f - height;

        float clampX = Mathf.Clamp(currentCamera.transform.position.x, -localX + confinePos.x, localX + confinePos.x);
        float clampY = Mathf.Clamp(currentCamera.transform.position.y, -localY + confinePos.y, localY + confinePos.y);

        Vector3 centerBottom = currentCamera.ViewportToWorldPoint(new Vector2(0.5f, 0f));
        //��� �Ʒ��� ��ǥ�� ������....

        //float clampX = Mathf.Clamp(centerBottom.x, -localX + confinePos.x, localX + confinePos.x);
        //float clampY = Mathf.Clamp(centerBottom.y, -localY + confinePos.y, localY + confinePos.y);


        Vector3 confinePosition = new Vector3(clampX, clampY, cameraDefaultPositionZ);
        limitTest = new Vector3(clampX, clampY, cameraDefaultPositionZ);
        return confinePosition;
    }


    private ECameraState NowCameraState()
    {
        if (currentCamera.orthographicSize == cameraZoomOutSize)
        {
            return ECameraState.ZOOMOUT;
        }
        else if (currentCamera.orthographicSize == cameraZoomInSize)
        {
            return ECameraState.ZOOMIN;
        }
        else if (currentCamera.orthographicSize == cameraDefaultSize)
        {
            return ECameraState.DEFAULT;
        }
        else
        {
            return ECameraState.STAY;
        }


    }

    private void ChangeScaleBGObject()
    {
        float BgScaleRatio = (currentCamera.orthographicSize / cameraDefaultSize);
        Vector3 scaleValue = originBGObjectScale * BgScaleRatio;
        BGObject.transform.localScale = new Vector3(scaleValue.x, scaleValue.y, originBGObjectScale.z);
    }

    private void OnDrawGizmos()
    {
        //if (isConfine)
        //{
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(confinePos, confineSize);

        //}

    }

    /// <summary>
    ///�ش� ������Ʈ�� ȭ�� ���� ���� �ִ��� �˻��մϴ�.
    /// </summary>
    /// <param name="_object"></param>
    /// <returns>ȭ�� �ȿ� ������ true, �ƴϸ� false�� �����մϴ�.</returns>
    public bool CheckThisObjectInScreen(GameObject _object)
    {

        Vector3 screenPoint = currentCamera.WorldToViewportPoint(_object.transform.position);
        bool inScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        return inScreen;
    }

    public void SetWhiteScreen(bool _b)
    {
        whiteScreen.SetActive(_b);
    }
}
