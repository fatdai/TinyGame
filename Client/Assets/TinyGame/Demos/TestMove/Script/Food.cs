using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Food : MonoBehaviour
{
    Vector3 m_Axis = Vector3.zero;
    private void Start()
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);
        m_Axis.x = x;
        m_Axis.y = y;
        m_Axis.z = z;
    }
    private void Update()
    {
        transform.Rotate(m_Axis, Time.deltaTime * 100f);
    }
}
