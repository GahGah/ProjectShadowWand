using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderObject : MonoBehaviour
{

    //    //public GameObject whiteScreen;
    //    public float time = 3f;

    //    ElectricableObject tempElecObject = null;

    //    [Tooltip("������ ġ�� �ִ� ����������...��.")]
    //    public bool isThundering = false;


    //    public Animator animator;
    //    private void Start()
    //    {
    //        gameObject.SetActive(false);
    //        if (animator == null)
    //        {
    //            animator = GetComponent<Animator>();
    //        }

    //    }

    //    /// <summary>
    //    /// ������ Ĩ�ϴ�.
    //    /// </summary>
    //    public void GoThunder()
    //    {
    //    }


    //    /// <summary>
    //    /// ������ Ĩ�ϴ�.
    //    /// </summary>
    //    /// <param name="_pos">������ ĥ ��ġ�Դϴ�.</param>
    //    /// <returns></returns>
    //    public IEnumerator DoThunder(Vector2 _pos)
    //    {
    //        gameObject.transform.position = _pos;
    //        isThundering = true;
    //        var timer = 0f;

    //        //GoThunder();
    //        gameObject.SetActive(true);

    //        while (timer < time)
    //        {
    //            timer += Time.deltaTime;
    //            yield return null;

    //            //����Ʈ ���ӽð�, Ȥ�� �ִϸ��̼� ���� ���θ޼� �Լ� ȣ���� �ؾ��ҵ�
    //        }

    //        gameObject.SetActive(false);
    //        isThundering = false;
    //    }

    //    private void OnTriggerEnter2D(Collider2D collision)
    //    {
    //        tempElecObject = collision.GetComponent<ElectricableObject>();

    //        //����� ���������ٸ�
    //        if (tempElecObject != null)
    //        {
    //            Debug.LogError("����!");
    //            tempElecObject.OnThunder();
    //        }
    //    }

}
