using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    PATROL, CHASE, ATTACK
}

public class EnemyController : MonoBehaviour
{

    private EnemyAnimator anim;
    private NavMeshAgent navAgent;
    private EnemyStates enemyState;
    private Transform target;
    private EnemyAudio enemyAudio;

    public GameObject pointOfAttack;

    private float currentAggroDistance; //Keep track of distance when enemy is hit at range
    private float patrolTimer; //Keeping track of how long enemy has been patrolling 
    private float attackTimer; //How long since enemy has attacked

    public float walkSpeed = .5f;
    public float runSpeed = 4f;
    public float aggroDistance = 15f; //Distance before enemy aggros player
    public float attackDistance = 1.8f; //How close enemy needs to be to attack
    public float pursuitDistance = 2f; //How close enemy chases behind player when aggro'd
    public float patrolRadiusMin = 20f, patrolRadiusMax = 60f;
    public float patrolDuration = 15f; //Time enemy patrols before a new random destination is set
    public float waitBeforeAttack = 1.5f; //Time between attacks


    void Awake()
    {
        anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyAudio = GetComponentInChildren<EnemyAudio>();

        target = GameObject.FindWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyStates.PATROL;

        patrolTimer = patrolDuration;

        attackTimer = waitBeforeAttack; 

        currentAggroDistance = aggroDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState == EnemyStates.PATROL)
        {
            Patrol();
        }

        if (enemyState == EnemyStates.CHASE)
        {
            Chase();
        }

        if (enemyState == EnemyStates.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        navAgent.isStopped = false; //Allow nav agent to move
        navAgent.speed = walkSpeed;

        patrolTimer += Time.deltaTime; //Counting up how long enemy has been patroling

        //When timer reaches the set patrol duration
        //calculate a new destination and reset timer
        if (patrolTimer > patrolDuration) 
        {
            RandomDestination();

            patrolTimer = 0f;
        }

        if(navAgent.velocity.sqrMagnitude > 0) //If enemy is moving/has velocity
        {
            anim.Walk(true);
        }
        else
        {
            anim.Walk(false);
        }

        //Check distance between enemy and player
        //If within aggro distance, begin chase
        if(Vector3.Distance(transform.position, target.position) <= aggroDistance)
        {
            anim.Walk(false); //Stop animation

            enemyState = EnemyStates.CHASE;

            //Play enemy reaction audio
            enemyAudio.PlayScream();
        }
    }

    void Chase()
    {
        navAgent.isStopped = false; //Allow nav agent to move
        navAgent.speed = runSpeed;
        navAgent.SetDestination(target.position); //Run towards player

        if (navAgent.velocity.sqrMagnitude > 0) //If enemy is moving/has velocity
        {
            anim.Run(true);
        }
        else
        {
            anim.Run(false);
        }

        //Check distance between enemy and player
        //If less than attack distance, begin attack
        if (Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            //Stop animations
            anim.Run(false);
            anim.Walk(false);

            enemyState = EnemyStates.ATTACK;

            //Handles enemy being hit by player from range
            //Allows enemy to chase and close distance to attack
            //Then reset aggro distance
            if(aggroDistance != currentAggroDistance)
            {
                aggroDistance = currentAggroDistance;
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > aggroDistance) //If player runs away from enemy
        {
            //Stop animation
            anim.Run(false);

            enemyState = EnemyStates.PATROL;

            //Reset patrol timer so new random destination can be calculated
            patrolTimer = patrolDuration;

            if(aggroDistance != currentAggroDistance)
            {
                aggroDistance = currentAggroDistance; //Reset aggro distance
            }
        }
    }

    void Attack()
    {
        navAgent.velocity = Vector3.zero; //Stop enemy completely
        navAgent.isStopped = true; //Not allowed to move

        attackTimer += Time.deltaTime;

        //When attackTimer surpasses wait time, cue attack
        if(attackTimer > waitBeforeAttack)
        {
            anim.Attack();

            attackTimer = 0f;

            //Play attack sounds
            enemyAudio.PlayAttack();
        }

        //Check if player runs away
        //Give player space to run away
        if(Vector3.Distance(transform.position, target.position) > attackDistance + pursuitDistance)
        {
            enemyState = EnemyStates.CHASE;
        }
    }

    void RandomDestination()
    {
        float randomRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);

        Vector3 randomDirection = Random.insideUnitSphere * randomRadius;
        randomDirection += transform.position; //Add random point generated to enemy current position

        NavMeshHit navHit;
        //Check generated random direction within the random radius to make sure it hits inside the navmesh area
        //Prevent enemy from falling off world
        NavMesh.SamplePosition(randomDirection, out navHit, randomRadius, -1);

        navAgent.SetDestination(navHit.position);
    }

    void ActivateAttackPoint()
    {
        pointOfAttack.SetActive(true);
    }

    void DeactivateAttackPoint()
    {
        if (pointOfAttack.activeInHierarchy)
        {
            pointOfAttack.SetActive(false);
        }
    }

    public EnemyStates EnemyState //Accessing EnemyState from outside script
    {
        get; set;
    }
}
