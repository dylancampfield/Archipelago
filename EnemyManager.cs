using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private GameObject cannibal, boar; //Create duplicates from prefabs
    [SerializeField] private int cannibalCount, boarCount;

    private int initialCannibalCount, initialBoarCount;

    public float waitBeforeSpawning = 10f;

    public Transform[] cannibalSpawns, boarSpawns;




    public static EnemyManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        initialCannibalCount = cannibalCount;
        initialBoarCount = boarCount;

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }

    // Update is called once per frame
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();
    }

    void SpawnCannibals()
    {
        int index = 0;


        for (int i = 0; i < cannibalCount; i++)
        {
            if (index >= cannibalSpawns.Length) //Check to prevent index out of bounds error
            {
                index = 0;
            }

            //Iterate through and spawn cannibal at that spawn position
            Instantiate(cannibal, cannibalSpawns[index].position, Quaternion.identity);

            index++;
        }

        cannibalCount = 0;
    }

    void SpawnBoars()
    {
        int index = 0;

        for (int i = 0; i < boarCount; i++)
        {
            if (index >= boarSpawns.Length) //Check to prevent index out of bounds error
            {
                index = 0;
            }

            //Iterate through and spawn boar at that spawn position
            Instantiate(boar, boarSpawns[index].position, Quaternion.identity);

            index++;
        }

        boarCount = 0;
    }

    IEnumerator CheckToSpawnEnemies()
    {
        yield return new WaitForSeconds(waitBeforeSpawning);

        SpawnCannibals();

        SpawnBoars();

        StartCoroutine("CheckToSpawnEnemies");
    }

    public void EnemyDied(bool cannibal) //Call in HealthScript when enemy dies
    {
        //If cannibal dies = true, if boar dies = false
        if (cannibal)
        {
            cannibalCount++; //Allow new cannibal to be created when one dies

            if (cannibalCount > initialCannibalCount)
            {
                cannibalCount = initialCannibalCount;
            }
        }
        else
        {
            boarCount++;//Allow new boar to be created when one dies

            if (boarCount > initialBoarCount)
            {
                boarCount = initialBoarCount;
            }
        }
    }

    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }
}


