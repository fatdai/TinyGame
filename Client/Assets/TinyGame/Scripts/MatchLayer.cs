using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchLayer : MonoBehaviour
{
    [SerializeField]
    Text m_TextMatch;

    [SerializeField]
    Button m_BtnMatch;

    void Start()
    {
        m_TextMatch.gameObject.SetActive(false);
        m_BtnMatch.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Match() {

        // ¿ªÊ¼match
        NetGlobal.instance.Send(CS_ID.MATCH, "");
        m_TextMatch.gameObject.SetActive(true);
        m_BtnMatch.gameObject.SetActive(false);
    }
}
