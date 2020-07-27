using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            print("Item pickup");
            Destroy(gameObject);
        }
    }
}
