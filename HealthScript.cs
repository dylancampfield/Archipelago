using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{

    private EnemyAnimator enemyAnim;
    private EnemyController enemyController;
    private NavMeshAgent navAgent;
    private EnemyAudio enemyAudio;
    private PlayerStats playerStats;

    private bool isDead;

    public float health = 100f;
    public bool isPlayer, isBoar, isCannibal; //Identifying GameObjects


    
    void Awake()
    {
        if(isBoar || isCannibal)
        {
            enemyAnim = GetComponent<EnemyAnimator>();
            enemyController = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            //Get enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if(isPlayer)
        {
            //Get player stats/health value
            playerStats = GetComponent<PlayerStats>();
        }
    }

    
    public void DealDamage(float damage)
    {
        if(isDead)
            return;

        health -= damage; //Call health and apply the damage passed as parameter

        if(isPlayer)
        {
            //Display health UI
            playerStats.DisplayHealthStats(health);
        }

        if(isBoar || isCannibal)
        {
            if(enemyController.EnemyState == EnemyStates.PATROL) //When enemy is in patrol state/not aggro'd
            {
                //If player attacks enemy from distance, set aggroDistance to higher value than default
                //so that enemy can immediately target and pursue the player
                enemyController.aggroDistance = 20f; 
            }
        }

        if(health <= 0f)
        {
            Dead();

            isDead = true;
        }
    }

    void Dead()
    {
        if(isBoar)
        {
            navAgent.velocity = Vector3.zero; //Stop enemy movement
            navAgent.isStopped = true; //Turn off nav agent

            enemyController.enabled = false; //Turn off enemy movement script

            enemyAnim.Dead(); //Play death animation

            //Start Coroutine
            StartCoroutine(DeathSound());

            //Spawn more enemies
            //EnemyManager.instance.EnemyDied(true); //Spawn cannibal
            EnemyManager.instance.EnemyDied(false); //Spawn boar
        }

        if (isCannibal)
        {
            navAgent.velocity = Vector3.zero; //Stop enemy movement
            navAgent.isStopped = true; //Turn off nav agent

            enemyController.enabled = false; //Turn off enemy movement script

            enemyAnim.Dead(); //Play death animation

            //Start Coroutine
            StartCoroutine(DeathSound());

            //Spawn more enemies
            EnemyManager.instance.EnemyDied(true); //Spawn cannibal
            //EnemyManager.instance.EnemyDied(false); //Spawn boar
        }

        if (isPlayer)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //Turn off enemies so they stop attacking dead player
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            //Stop spawning enemies
            EnemyManager.instance.StopSpawning();

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentWeapon().gameObject.SetActive(false); //Hide weapon when dead
        }

        if(tag == "Player")
        {
            Invoke("GameOver", 3f); //Player dies, restart game
        }
        else
        {
            Invoke("TurnOffGameObject", 10f); //Enemy dies, destroy GameObject
        }
    }

    void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeathSound()
    {
        yield return new WaitForSeconds(0.3f);

        enemyAudio.PlayDeath();
    }
}
