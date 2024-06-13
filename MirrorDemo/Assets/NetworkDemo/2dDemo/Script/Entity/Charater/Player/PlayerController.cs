using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Spine.Unity;

public enum PlayerState
{
    Idle,
    Move,
}
public class PlayerController : PlayerBase
{
    public float MoveSpeed = 5;
    public GameObject TowerPrefab;

    private Rigidbody2D m_Rgb; // 刚体组件
    //private int m_FollowerCount = 0;
    //private int m_MaxFollowerCount = 5;
    private SkeletonAnimation m_Spine;
    private PlayerState m_CurState;

    public Vector3 MoveDir { get; private set; }

    #region Client
    [Client]
    protected override void Awake()
    {
        base.Awake();

        m_Rgb = GetComponent<Rigidbody2D>(); // 获取刚体组件
        m_Spine = GetComponentInChildren<SkeletonAnimation>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //摄像机与角色绑定
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, Camera.main.transform.position.z);
    }

    //速度：每秒移动5个单位长度
    [Client]
    private void Update()
    {
        if (!isLocalPlayer) return; //不应操作非本地玩家
       
        Move();

        SearchTarget();
        //Fire();

        if (Input.GetKeyDown(KeyCode.P))
        {
            CmdSpawnTower();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
        //AddFollower();
    }

    [Client]
    private void Move()
    {
        //通过键盘获取水平轴的值，范围在-1到1
        float Vertical = 0;
        float Horizontal = 0;

        //if (BattleMgr.Instance.LevelMgr.GameTime < 1)
        //    return;

        if (Input.GetKey(KeyCode.W))
        {
            Vertical = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vertical = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Horizontal = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Horizontal = 1;
        }

        MoveDir = new Vector3 (Horizontal, Vertical, 0);
        MoveDir.Normalize();

        m_Rgb.velocity = MoveDir * MoveSpeed; // 设置刚体速度
        if (MoveDir != Vector3.zero)
        {
            SetState(PlayerState.Move);
        }
        else
        {
            SetState(PlayerState.Idle);
        }
        if (Horizontal != 0)
        {
            transform.GetChild(0).localScale = new Vector3(-Horizontal, 1, 1); // 翻转角色
        }
    }

    [Client]
    private void SetState(PlayerState state)
    {
        if (m_CurState != state)
        {
            m_CurState = state;

            switch (m_CurState)
            {
                case PlayerState.Idle:
                    CmdPlayAnim("Idle");
                    break;
                case PlayerState.Move:
                    CmdPlayAnim("Move");
                    break;
            }
        }
    }

    #endregion

    #region Command
    //[Command]
    //private void CmdSpawnFollower()
    //{
    //    GameObject follower = Instantiate(FollowerPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
    //    //follower.GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.localConnection);
    //    NetworkServer.Spawn(follower, connectionToClient);//服务器孵化，同步客户端
    //}

    [Command]
    private void CmdPlayAnim(string animName)
    {
        RpcPlayAnim(animName);
    }

    [Command]
    private void CmdSpawnTower()
    {
        GameObject tower = Instantiate(TowerPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(tower, connectionToClient);//服务器孵化，同步客户端
    }
    #endregion

    #region ClientRpc
    [ClientRpc]
    private void RpcPlayAnim(string animName)
    {
        m_Spine.state.SetAnimation(0, animName, true);
    }
    #endregion

    private Vector3 m_LastMoveDir;
    public override Vector3 GetMoveDir()
    {
        if (MoveDir != Vector3.zero)
        {
            m_LastMoveDir = MoveDir;
            return m_LastMoveDir;
        }

        return m_LastMoveDir;
    }
}
