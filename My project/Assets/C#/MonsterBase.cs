using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBase : MonoBehaviour
{
    public static MonsterBase instance;
    [Header("����")]
    public Vector3 target; //����Ŀ��
    public float distance_max;//��Զ���룺����
    public float distance_mid;//�еȾ��룺ŭ��
    public float distance_min;//�����빥��
    public NavMeshAgent _nav; //AI�����������
    public float run_speed = 70f;//�ƶ��ٶ�
    public Animator _ani;//�����������

    //��������
    public  bool canAttack = true; // �ܷ񹥻�                                                              //��Ҫ���乥������ȴ�߼����ʵ��������������д�ʹ���
    public bool isAttacking;//����-ing
    public float time = 0;
    public float timaer = 3;
    public float attackpower;

    

    
    
   
}
