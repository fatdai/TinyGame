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
        // ���ǰ������
        //if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    Log.Info("ǰ��");
        //    transform.position += transform.forward * Time.deltaTime * kSpeed;
        //}else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    Log.Info("����");
        //    transform.position -= transform.forward * Time.deltaTime * kSpeed;
        //}else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    Log.Info("����");
        //    transform.position -= transform.right * Time.deltaTime * kSpeed;
        //}
        //else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    Log.Info("����");
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
