//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LightObjectSun : LightObject
//{

//    public float speed = 1f;
//    public Transform target;
//    private void Update()
//    {
//        if (InputManager.Instance.keyboard.zKey.isPressed)
//        {
//            transform.position += new Vector3(-speed, 0, 0);
//        }
//        if (InputManager.Instance.keyboard.cKey.isPressed)
//        {
//            transform.position += new Vector3(speed, 0, 0);

//        }

//        Vector2 direction = target.position - transform.position;

//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

//        //천천히 움직일때(Slerp)사용
//        //Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 1f * Time.deltaTime);

//        //하지만 난 바로바로 움직이게 하고 싶기 때문에
//        Quaternion rotation = angleAxis;
//        transform.rotation = rotation;

//    }
//    public override void UpdateShadowJudgement()
//    {
//        if (shadowJudgment.Length != MonsterManager.Instance.monsterList.Count) //갯수가 다르면 
//        {
//            shadowJudgment = new bool[MonsterManager.Instance.monsterList.Count]; //새로 해주기.
//        }

//        monsterCount = MonsterManager.Instance.monsterList.Count;

//        for (int i = 0; i < monsterCount; i++)
//        {
//            Monster nowMonster = MonsterManager.Instance.monsterList[i];

//            nowMonster.directions = new Vector2[] //거꾸로 하면 빛에서부터 몬스터로의 방향을 구하지 않을까? 싶어서...
//            {
//                nowMonster.path[0] - transform.position,
//                nowMonster.path[1] - transform.position,
//                nowMonster.path[2] - transform.position,
//                nowMonster.path[3] - transform.position,
//            };

//            for (int j = 0; j < 4; j++) //라이트에서 몬스터를 향해 레이캐스트
//            {
//                Debug.DrawRay(transform.position, nowMonster.directions[j], nowMonster.colors[j], 0.5f);
//                nowMonster.hits[j] = Physics2D.Raycast(transform.position, nowMonster.directions[j], nowMonster.directions[j].magnitude, layerMask);
//            }

//            //순서 :
//            // 3 2
//            // 0 1


//            nowMonster.UpdateHitsLog();

//            shadowJudgment[i] = nowMonster.isAllHitsTrue();
//        }
//    }
//}
