using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    private Animator anim;


    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Walk(bool isWalking)
    {
        anim.SetBool("isWalking", isWalking);
    }

    public void Run(bool isRunning)
    {
        anim.SetBool("isRunning", isRunning);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void Dead()
    {
        anim.SetTrigger("Dead");
    }
}
