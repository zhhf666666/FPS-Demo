using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginningController : MonoBehaviour
{
    public float RotateSpeed = 180;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

