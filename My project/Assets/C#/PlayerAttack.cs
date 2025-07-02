using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack playerattackinstance;
    public Collider _box;
    public Vector3 reference;
    public float PlayerPower = 25;
    private void Awake()
    {
        playerattackinstance = this;
        _box.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }   
    void Update()
    {
        
    }
    public void appear() { _box.gameObject.SetActive(true); }
    public void disappear() { _box.gameObject.SetActive(false); }
    private void OnTriggerEnter(Collider other)//ПлбЊ
    {
        if (other.CompareTag("Monster"))
        {
            reference = other.transform.position;
            Monster1 c = other.GetComponent<Monster1>();
            c.Hit(PlayerPower);
            //Monster1.monster1.Hit(PlayerPower);
        }
    }
}

