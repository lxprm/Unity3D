using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public Collider _box;
    
    private void Awake()
    {
        
    }
    void Update()
    {
        
    }
    public void appear() 
    {
        _box.gameObject.SetActive(true);
    }
    public void Disapear()
    {
        _box.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            PlayerMove.Instance.Hurt(Monster1.monster1.attackpower);
        }
    }
}
