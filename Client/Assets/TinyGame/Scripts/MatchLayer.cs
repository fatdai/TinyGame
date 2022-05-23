using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Match() {

        // ¿ªÊ¼match
        NetGlobal.instance.Send(CS_ID.MATCH, "");
    }
}
