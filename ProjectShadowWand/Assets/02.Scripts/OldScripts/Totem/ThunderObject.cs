using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderObject : MonoBehaviour
{

    //    //public GameObject whiteScreen;
    //    public float time = 3f;

    //    ElectricableObject tempElecObject = null;

    //    [Tooltip("번개를 치고 있는 상태인지를...네.")]
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
    //    /// 번개를 칩니다.
    //    /// </summary>
    //    public void GoThunder()
    //    {
    //    }


    //    /// <summary>
    //    /// 번개를 칩니다.
    //    /// </summary>
    //    /// <param name="_pos">번개가 칠 위치입니다.</param>
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

    //            //이펙트 지속시간, 혹은 애니메이션 등의 내부메서 함수 호출을 해야할듯
    //        }

    //        gameObject.SetActive(false);
    //        isThundering = false;
    //    }

    //    private void OnTriggerEnter2D(Collider2D collision)
    //    {
    //        tempElecObject = collision.GetComponent<ElectricableObject>();

    //        //제대로 가져와졌다면
    //        if (tempElecObject != null)
    //        {
    //            Debug.LogError("감전!");
    //            tempElecObject.OnThunder();
    //        }
    //    }

}
