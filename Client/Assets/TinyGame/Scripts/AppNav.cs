using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 登陆
/// </summary>
public class LoginState : FSMState
{
    GameObject m_ObjLogin;
    public LoginState(IFSM ifsm) : base(ifsm)
    {

    }
    public override void OnEnter()
    {
        m_ObjLogin = GameObject.Instantiate(PfRes.instance.pf_LoginLayer, AppNav.instance.transform);
    }

    public override void OnExit()
    {
        GameObject.Destroy(m_ObjLogin);
    }
}


/// <summary>
/// 匹配
/// </summary>
public class MatchState : FSMState
{
    GameObject m_ObjMatch;
    public MatchState(IFSM ifsm) : base(ifsm)
    {
       
    }
    public override void OnEnter()
    {
        m_ObjMatch = GameObject.Instantiate(PfRes.instance.pf_MatchLayer, AppNav.instance.transform);
    }

    public override void OnExit()
    {
        GameObject.Destroy(m_ObjMatch);
    }
}

/// <summary>
/// 游戏
/// </summary>
public class GameNavState : FSMState
{
    public GameNavState(IFSM ifsm) : base(ifsm)
    {

    }
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
}


/// <summary>
/// 总的导航
/// </summary>
public class AppNav : MonoBehaviour,IFSM
{
    public static AppNav instance;

    public FSMState CurState { get; set; }

    LoginState m_LoginState;
    MatchState m_MatchState;
    GameNavState m_GameNavState;

    public void StartFSM(FSMState initState)
    {
        CurState = initState;
        CurState.OnEnter();
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_LoginState = new LoginState(this);
        m_MatchState = new MatchState(this);
        m_GameNavState = new GameNavState(this);
        StartFSM(m_LoginState);
    }

    void Update()
    {
        
    }

    public void ShowMatchLayer() {
        CurState.ChangeState(m_MatchState);
    }
}
