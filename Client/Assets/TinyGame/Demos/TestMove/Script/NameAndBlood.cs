using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameAndBlood : MonoBehaviour
{
    [SerializeField]
    Camera m_GameCamera;

    [SerializeField]
    Player m_Player;

    Vector3 m_PlayerCameraOffset = Vector3.zero;
    Vector3 m_SelfWithPlayerOffset = Vector3.zero;

    void Start()
    {
        m_PlayerCameraOffset = m_GameCamera.transform.position - m_Player.transform.position;
        m_SelfWithPlayerOffset = transform.position - m_Player.transform.position;
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        // 更新相机位置
        m_GameCamera.transform.position = m_PlayerCameraOffset + transform.position;

        // billboard
        transform.position = m_Player.transform.position + m_SelfWithPlayerOffset;
        transform.rotation = m_GameCamera.transform.rotation;
    }
}
