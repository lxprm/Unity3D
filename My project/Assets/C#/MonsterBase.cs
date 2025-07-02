using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBase : MonoBehaviour
{
    public static MonsterBase instance;
    [Header("参数")]
    public Vector3 target; //攻击目标
    public float distance_max;//最远距离：发呆
    public float distance_mid;//中等距离：怒吼
    public float distance_min;//近距离攻击
    public NavMeshAgent _nav; //AI导航代理参数
    public float run_speed = 70f;//移动速度
    public Animator _ani;//动画代理参数

    //攻击参数
    public  bool canAttack = true; // 能否攻击                                                              //需要记忆攻击、冷却逻辑，适当参数方便后续编写和处理。
    public bool isAttacking;//攻击-ing
    public float time = 0;
    public float timaer = 3;
    public float attackpower;

    

    
    
   
}
