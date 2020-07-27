using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            print("Game Win");
            SceneManager.LoadScene("Game Win");
        }
    }
}
