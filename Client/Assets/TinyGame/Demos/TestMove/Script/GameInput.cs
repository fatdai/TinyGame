using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{

    [SerializeField]
    Player m_Player;

    [SerializeField]
    RectTransform m_joystickTransform;

    // joystick init position;
    Vector3 mJYInitWorldPos;

    // ��¼λ��ƫ��
    Vector3 mLastMousePosition = Vector3.zero;


    bool m_Moving = false;
    float m_Speed = 0f;
    int m_ObstacleMask;

    private void Start()
    {
        mJYInitWorldPos = m_joystickTransform.parent.TransformPoint(new Vector3(m_joystickTransform.anchoredPosition.x, m_joystickTransform.anchoredPosition.y, 0));
        mLastMousePosition = Input.mousePosition;

        m_ObstacleMask = LayerMask.GetMask("Obstacle");
    }

    // joystick move
    public void OnJoyStick_Drag(BaseEventData data)
    {
        m_Moving = true;
        var eventData = data as PointerEventData;
        Vector2 position = eventData.position;

        //Debug.Log($"OnJoyStick_Drag -- position:{position}");

        Vector3 dir = new Vector3(position.x, position.y, 0) - mJYInitWorldPos;
        m_joystickTransform.anchoredPosition = dir = Vector3.ClampMagnitude(dir, 128);

        // �޸ķ�����ٶ�
        m_Speed = dir.magnitude;
        m_Player.transform.forward = new Vector3(dir.x, 0, dir.y);
    }

    public void OnJoyStick_EndDrag(BaseEventData data)
    {
        m_joystickTransform.anchoredPosition = Vector3.zero;
        m_Moving = false;
    }

    private void Update()
    {
        if (!m_Moving)
        {
            return;
        }

        // �ж�ǰ���Ƿ����ϰ���,���ϰ���Ͳ���ǰ��

        Ray ray = new Ray(m_Player.transform.position, m_Player.transform.forward);
        if (Physics.Raycast(ray, 1f, m_ObstacleMask))
        {
            Log.Info("ǰ�����ϰ��� �߲�����!");
        }
        else
        {
            m_Player.transform.Translate(Vector3.forward * m_Speed * Time.deltaTime * 0.1f);
        }
    }
}
