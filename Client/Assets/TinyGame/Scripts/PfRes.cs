using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PfRes : MonoBehaviour
{
    [SerializeField]
    public GameObject pf_LoginLayer;

    [SerializeField]
    public GameObject pf_MatchLayer;


    public static PfRes instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
