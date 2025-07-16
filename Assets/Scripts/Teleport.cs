using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{

    public int NumScene;

    private bool _isLoading;

    private static Teleport _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isLoading || !other.CompareTag("Player")) 
            return;
        StartCoroutine(LoadSceneRoutine(NumScene));
    }

    private IEnumerator LoadSceneRoutine(int NumScene)
    {
        _isLoading = true;
        //GetComponent<BoxCollider>().enabled = false;

        var waitFaiding = true;
        Fader.instance.FadeIn(() => waitFaiding = false);

        while (waitFaiding)
            yield return null;

        var async = SceneManager.LoadSceneAsync(NumScene);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
            yield return null;

        async.allowSceneActivation = true;

        waitFaiding = true;
        Fader.instance.FadeOut(() => waitFaiding = false);

        while (waitFaiding)
            yield return null;


        //GetComponent<BoxCollider>().enabled = true;
        _isLoading = false;
    }



}
