using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    Camera m_GameCamera;

    [SerializeField]
    FoodMgr m_FoodMgr;

    void Start()
    {

    }

    void Update()
    {
        // 鼠标前进后退
        //if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    Log.Info("前进");
        //    transform.position += transform.forward * Time.deltaTime * kSpeed;
        //}else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    Log.Info("后退");
        //    transform.position -= transform.forward * Time.deltaTime * kSpeed;
        //}else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    Log.Info("左移");
        //    transform.position -= transform.right * Time.deltaTime * kSpeed;
        //}
        //else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    Log.Info("右移");
        //    transform.position += transform.right * Time.deltaTime * kSpeed;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("food"))
        {
            Log.Info("eat a food");
            m_FoodMgr.RemoveFood(other.gameObject);
        }
    }
}
