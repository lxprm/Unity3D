using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPerticalSystem : MonoBehaviour
{
    public ParticleSystem one;
    public ParticleSystem two;
    public ParticleSystem three;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void  PerticalPlayerone() 
    {
        one.Play();
    }
    public void PerticalPlayertwo()
    {
        two.Play();
    }
    public void PerticalPlayerthree()
    {
        three.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
