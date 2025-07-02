using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
public class Task : MonoBehaviour
{
    public static Task intance;
    [Header("�������FreeLook")]
    private CinemachineFreeLook  camera;
    private bool MouseControl = true;
    [Header("����")]
    public Collider _box;
    private float hp;
    private int i = 0;
    private AudioSource Music;
    private Stack<Image> ջ = new Stack<Image>();
    public bool _EscPaused;
    [Header("Image")]
    public Image task;
    public Image main;
    public Image death;
    public Image set;
    public Image vectory;
    [Header("Button")]
    public Button Exit;
    public Button Setting;
    public Slider _music;
    public Slider cameraFOV;
    public Button Restart;
    public Button Continue;
    public Button Accept;
    public Button Disaccept;
    
    // Start is called before the first frame update
    private void Awake()
    {
        
        //MouseControl = true;
        intance = this;
        Music = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        camera = GameObject.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>();
        Exit.onClick.AddListener(() =>exit());
        Restart.onClick.AddListener(() => restart());
        Setting.onClick.AddListener(() => setting());
        Continue.onClick.AddListener(() => continuegame());
        Accept.onClick.AddListener(() => yes());
        Disaccept.onClick.AddListener(() => no());


        if (cameraFOV != null)
        {
            cameraFOV.onValueChanged.AddListener(OnCameraFOVChanged);
            
            cameraFOV.value = camera.m_Lens.FieldOfView;
        }
        if (_music != null && Music != null)
        {
            _music.value = Music.volume;
            // ���ֵ�仯����
            _music.onValueChanged.AddListener(ChangeVolume);
        }
    }
    void Start()
    {
      
        hp = PlayerMove.Instance.Hp;
        _box = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        hp = PlayerMove.Instance.Hp;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerMove.Instance._mouseClickIsCanAttack = !PlayerMove.Instance._mouseClickIsCanAttack;
            _EscPaused = !_EscPaused;
            MouseControl = !MouseControl;


            vectory.gameObject.SetActive(false);
            death.gameObject.SetActive(false);
            if (ջ.Count <= 0)
            {
                main.gameObject.SetActive(true);
                ջ.Push(main);
            }
            else
            {
                ջ.Pop().gameObject.SetActive(false);
            }
        }
        
        if (hp < 1)
        {
            MouseControl = false;
            death.gameObject.SetActive(true);
            StartCoroutine(Death());
        }
        if (MonsterPerfab.instance.residuemonster == 0&&i == 0)
        {
            
            vectory.gameObject.SetActive(true);
            StartCoroutine(Back());
            //Time.timeScale = 0;
            i++;
        }
        if (MouseControl)
        {
            camera.Follow = PlayerMove.Instance.play.transform;
        }
        else
        {
            camera.Follow = null;
        }
    }
    public void yes()           //��������
    {
        MouseControl = true;
        task.gameObject.SetActive(false);
        MonsterPerfab.instance.isspawn = true;
        PlayerMove.Instance._mouseClickIsCanAttack = true;

    }
    public void no()
    {
        MouseControl = true;
        task.gameObject.SetActive(false);
        PlayerMove.Instance._mouseClickIsCanAttack = true;


    }
    public void exit()          //�˳�
    {
        Application.Quit();
    }
    public void restart()       //���¿�ʼ
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void setting()       //����
    {
        ջ.Pop().gameObject.SetActive(false);
        set.gameObject.SetActive(true);
        ջ.Push(set);
    }
    public void continuegame()  //����
    {

        Time.timeScale = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            PlayerMove.Instance._mouseClickIsCanAttack = false;
            MouseControl = false;
            task.gameObject.SetActive(true);
        }
    }
    IEnumerator  Back() 
    {
        yield return new WaitForSeconds(2);
        
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
    }
    private void OnCameraFOVChanged(float value)
    {
        if (camera != null)
        {
            camera.m_Lens.FieldOfView = value;
        }
    }
    private void ChangeVolume(float volume)
    {
        // ��������
        if (Music != null)
        {
            Music.volume = volume;
        }
    }

}
