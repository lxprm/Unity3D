using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Monster1 : MonsterBase
{
    //受击
    public float Hp = 100;
    public float MaxHp = 100;
    private float dyingtime = 0f;
    private bool isdyingtime;
    public static Monster1 monster1;
    //初始化
    private void Awake()
    {
        monster1 = this;  
        
        _nav = GetComponent<NavMeshAgent>();
        _ani = GetComponent<Animator>();
    }//初始组件获取、单例指向
    void Start()
    {
        
        _nav.speed = 0;
    }//AI导航速度初始化 = 0



    //游戏运行-ing
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

        // 每次只触发一个状态
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
            _ani.SetTrigger("怒吼");
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
                Attack();//攻击函数调用
            }

            _nav.speed = 0;
        }
    }//各种状态切换的关键函数
    private void Attack() //攻击函数
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
    private void ResetAllTriggers()// 辅助方法：重置所有 Trigger
    {
        _ani.ResetTrigger("Idle");
        _ani.ResetTrigger("怒吼");
        _ani.ResetTrigger("Run");
        _ani.ResetTrigger("Attack");
    }
}

