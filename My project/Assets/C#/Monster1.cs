using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Monster1 : MonsterBase
{
    //�ܻ�
    public float Hp = 100;
    public float MaxHp = 100;
    private float dyingtime = 0f;
    private bool isdyingtime;
    public static Monster1 monster1;
    //��ʼ��
    private void Awake()
    {
        monster1 = this;  
        
        _nav = GetComponent<NavMeshAgent>();
        _ani = GetComponent<Animator>();
    }//��ʼ�����ȡ������ָ��
    void Start()
    {
        
        _nav.speed = 0;
    }//AI�����ٶȳ�ʼ�� = 0



    //��Ϸ����-ing
    void Update()
    {
        target = PlayerMove.Instance.transform.position;
        //print(target);
        _nav.SetDestination(target);

        run();
        if (isAttacking)
        {
            time += Time.deltaTime;
            if (time >= 2.3f)
            {
                isAttacking = false;
                ResetAllTriggers();
                _ani.SetTrigger("Idle");

            }
        }
        if (isAttacking == false && canAttack == false)
        {
            time += Time.deltaTime;
            if (time >= timaer)
            {
                canAttack = true;
                time = 0;

            }
        }

    }

    private void run()
    {
        if (Task.intance._EscPaused)
        {
            return;
        }
        if (PlayerMove.Instance.Hp <= 1)
        {
            return;
        }
        if (isdyingtime)
        {
            return;
        }
        if (isAttacking)
        {                                                                                      
            return;
        }
        float distance = Vector3.Distance(target, transform.position);

        // ÿ��ֻ����һ��״̬
        if (distance > distance_max)
        {
            ResetAllTriggers();
            _ani.SetTrigger("Idle");
            _nav.speed = 0;
        }
        else if (distance > distance_mid && distance < distance_max)
        {
            transform.LookAt(target);
            ResetAllTriggers();
            _ani.SetTrigger("ŭ��");
            _nav.speed = 0;
        }
        else if (distance >= distance_min && distance < distance_mid)
        {
            ResetAllTriggers();

            if (canAttack)
            {
                _ani.SetTrigger("Run");
                _nav.speed = run_speed;

            }
        }
        else if (distance < distance_min)
        {
            if (PlayerMove.Instance.Hp == 0)
            {
                return;
            }
            ResetAllTriggers();

            if (canAttack)
            {
                Attack();//������������
            }

            _nav.speed = 0;
        }
    }//����״̬�л��Ĺؼ�����
    private void Attack() //��������
    {
        _ani.SetTrigger("Attack");
        canAttack = false;
        isAttacking = true;
    }
    public void Hit(float playerdamge)
    {

        if (Hp <= 0.1f)
        {
            return;
        }
        
        if (Hp >playerdamge)
        {
            _ani.SetTrigger("Hurt");
        }
        else
        {
            isdyingtime = true;
            _ani.SetTrigger("Death");
            StartCoroutine(Dying(5f));             
        }

        Hp = Mathf.Max(0, Hp - playerdamge);
         


    }

    IEnumerator  Dying (float timew) 
    {
        yield return new WaitForSeconds(timew);
        gameObject.SetActive(false);
        MonsterPerfab.instance.residuemonster -= 1;

    }
    private void ResetAllTriggers()// ������������������ Trigger
    {
        _ani.ResetTrigger("Idle");
        _ani.ResetTrigger("ŭ��");
        _ani.ResetTrigger("Run");
        _ani.ResetTrigger("Attack");
    }
}

