using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginningController : MonoBehaviour
{
    public float RotateSpeed = 180;
    public GameObject Player;
    public GameObject BeginningCanvas;
    public GameObject GameCanvas;


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
        this.gameObject.SetActive(false);
        BeginningCanvas.SetActive(false);
        GameCanvas.SetActive(true);
        Player.transform.position = new Vector3(-29, 0, -29);
        Player.transform.localEulerAngles = new Vector3(0, 45, 0);
        Player.SetActive(true);
    }
}

