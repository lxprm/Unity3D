using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPerfab : MonoBehaviour
{
    public static MonsterPerfab instance;
    public GameObject MonsterPrefab;
    public List<Transform> spwanpos1 = new List<Transform>();
    public List<Transform> spwanpos2 = new List<Transform>();

    public int residuemonster = 4;
    public bool isspawn;
    private bool twospawn;
    // Start is called before the first frame update
    private void Awake()
    {
        //isspawn = true;
        instance = this;
        MonsterPrefab = Resources.Load<GameObject>("MonsterPrefab/Monster");
        
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        if (isspawn)
        {
            //print(123);
            spawn(spwanpos1);
            isspawn = false;
            twospawn = true;
        }
        if (residuemonster == 2 &&twospawn)
        {
            spawn(spwanpos2);
            twospawn = false;
        }
    }
    public void spawn(List<Transform> list) 
    {
        foreach (var pos in list)
        {
            Instantiate(MonsterPrefab,pos);
        }
    }
}
