using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowArrow : MonoBehaviour
{

    private Rigidbody arrowBody;

    public float speed = 30f;
    public float damage = 15f;
    public float destroyTimer = 3f; //Destroy gameObject after 3sec

    private void Awake()
    {
        arrowBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyArrow", destroyTimer); //After 3sec, call DestroyArrow
    }

    public void Launch(Camera mainCamera)
    {
        arrowBody.velocity = mainCamera.transform.forward * speed; //Called from PlayerAttack script
        transform.LookAt(transform.position + arrowBody.velocity);
    }

    void DestroyArrow()
    {
        //If arrow is active, deactivate/destroy
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //After we hit an enemy, deal damage
        if (collider.tag == "Enemy")
        {
            collider.GetComponent<HealthScript>().DealDamage(damage);

            gameObject.SetActive(false);
        }
    }

 
}
