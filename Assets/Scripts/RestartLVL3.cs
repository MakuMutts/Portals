using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLVL3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        SceneManager.LoadScene("scene 3");
    }
}
