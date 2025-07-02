using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    
    public static PlayerMove Instance;

    public bool _mouseClickIsCanAttack = true;
    public Rigidbody play;
    public float movespeed = 10f;
    public float horizontalInput;
    public float verticalInput;
    public Vector3 InputDirction;

    [Header("受伤闪烁")]
    private PostProcessVolume _Volume;
    private Vignette _vignette;
    private float max = 0.9f;
    private float min = 0.5f;
    private float flashSpeed = 0.5f;

    [Header("相机引用")]
    public Transform cameraTransform;

    [Header("动画控制")]
    public Animator animator;        
    public string runTrigger = "Run";
    public string idleTrigger = "Idle";

    [Header("玩家旋转速度")]
    public float rotatespeed = 10f;

    private bool isMoving;           

    [Header("受击参数")]
    public float Hp = 100;
    public float MaxHp = 100;
    public bool injuretime;
    private float currentIntensity;

    [Header("血条")]
    public Image 血条;
    public Image healthBar;          
    public Image delayedHealthBar;   
    public float delaySpeed = 0.5f;  

    private float lastHealth;       
    private bool isDelayedDecreasing = false; 

    private void Awake()
    {

        _mouseClickIsCanAttack = true;
        _Volume = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessVolume>();
        if (_Volume && _Volume.profile.TryGetSettings(out _vignette))
        {
            _vignette.enabled.value = false;
            currentIntensity = _vignette.intensity.value;
        }

        Instance = this;
        play = GetComponent<Rigidbody>();
        play.freezeRotation = true;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (healthBar != null && delayedHealthBar != null)
        {
            healthBar.fillAmount = 1f;
            delayedHealthBar.fillAmount = 1f;
            lastHealth = Hp;
        }
    }

    void Update()
    {
        if (isDelayedDecreasing || Hp != lastHealth)
        {
            HpFillAmount();
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        InputDirction.Set(horizontalInput, 0, verticalInput);
        InputDirction.Normalize();

        if (InputDirction.magnitude != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_mouseClickIsCanAttack == false)
            {
                return;
            }
            animator.SetTrigger("Attack");
            if (GameObject.FindWithTag("Monster"))
            {
                if (PlayerAttack.playerattackinstance.reference == new Vector3(0, 0, 0))
                {
                    return;
                }
                if (Vector3.Distance(play.transform.position, PlayerAttack.playerattackinstance.reference) <= 10)
                {
                    Vector3 direction = Monster1.monster1.transform.position - play.transform.position;
                    play.transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
       
    }

    private void FixedUpdate()
    {
        
        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = cameraRight * InputDirction.x + cameraForward * InputDirction.z;

        
        Vector3 newVelocity = moveDir * movespeed;
        newVelocity.y = play.velocity.y; 
        play.velocity = newVelocity;

        
        if (isMoving)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            play.rotation = Quaternion.Slerp(play.rotation, targetRot, 10f * Time.fixedDeltaTime);
        }

        
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (isMoving)
        {
           
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                animator.ResetTrigger(idleTrigger);
                animator.SetTrigger(runTrigger);
            }
        }
        else
        {
           
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.ResetTrigger(runTrigger);
                animator.SetTrigger(idleTrigger);
            }
        }
    }

    public void Hurt(float damge)
    {
        bool 濒死 = true;
        if (injuretime == true )
        {
            return;
        }

       
        lastHealth = Hp;

        if (Hp > damge)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            animator.SetTrigger("Death");
        }

        Hp = Mathf.Max(0, Hp - damge);
        if (Hp<=70&&濒死 == true)
        {
            濒死 = false;
            _vignette.enabled.value = true;
            StartCoroutine(HurtEffect());
        }
        if (Hp <= 30 && Hp != 0)
        {
            injuretime = true;
            StartCoroutine(Injure());
        }

        
        isDelayedDecreasing = true;
    }
    




    //无敌时间
    private IEnumerator Injure() 
    {
        yield return new WaitForSeconds(3);
        injuretime = false;
    }




    //濒死效果
    private IEnumerator HurtEffect()
    {
       
        bool increasing = true;

        while (Hp<=70&&Hp>=0.1f)
        {
            
            currentIntensity += (increasing ? 1f : -1f) * flashSpeed * Time.deltaTime;

            
            if (currentIntensity >= max)
            {
                currentIntensity = max;
                increasing = false;
            }
            else if (currentIntensity <= min)
            {
                currentIntensity = min;
                increasing = true;
            }

           
            _vignette.intensity.value = currentIntensity;
            
            yield return null;
        }
        _vignette.enabled.value = false;
    }




    //心脏血条
    private void HpFillAmount()
    {
        
        healthBar.fillAmount = Hp / MaxHp;

        
        if (isDelayedDecreasing && delayedHealthBar.fillAmount > healthBar.fillAmount)
        {
           
            delayedHealthBar.fillAmount = Mathf.Lerp(
                delayedHealthBar.fillAmount,
                healthBar.fillAmount,
                delaySpeed * Time.deltaTime
            );

            
            if (delayedHealthBar.fillAmount - healthBar.fillAmount < 0.01f)
            {
                delayedHealthBar.fillAmount = healthBar.fillAmount;
                isDelayedDecreasing = false;
            }
        }
        else if (delayedHealthBar.fillAmount < healthBar.fillAmount)
        {
            delayedHealthBar.fillAmount = healthBar.fillAmount;
            isDelayedDecreasing = false;
        }
    }



    //激活血条
    public  void 血条激活() 
    {
        StartCoroutine(血条延迟激活());
    }
    IEnumerator 血条延迟激活() 
    {
        yield return new WaitForSeconds(2);
        血条.gameObject.SetActive(true);
    }
}