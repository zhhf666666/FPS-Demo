using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHealth : MonoBehaviour
{
    public float RotateSpeed = 90;
    public GameManager GM;
    
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if(GM.IsPause)
            return;
        this.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GM.GetHealth();
            Destroy(this.gameObject);
        }
    }
}
