using UnityEngine;
using System.Collections;

public class ShowFPS_OnGUI : MonoBehaviour
{

    public float fpsMeasuringDelta = 2.0f;

    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;

    private void Start()
    {
        timePassed = 0.0f;
    }

    private void Update()
    {
        m_FrameCount++;
        timePassed += Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / fpsMeasuringDelta;
            timePassed -= fpsMeasuringDelta;
            m_FrameCount = 0;
        }
    }

    private void OnGUI()
    {
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;    //�������ñ�������
        bb.normal.textColor = new Color(1.0f, 0.5f, 0.0f);   //����������ɫ��
        bb.fontSize = 40;       //��Ȼ�����������С

        //������ʾFPS
        GUI.Label(new Rect((Screen.width / 2) - 40, 0, 200, 200), "FPS: " + m_FPS, bb);
    }
}
