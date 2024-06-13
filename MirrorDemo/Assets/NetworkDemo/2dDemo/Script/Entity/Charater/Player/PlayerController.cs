using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Spine.Unity;
using GameFrame;

public enum PlayerState
{
    Idle,
    Move,
}
public class PlayerController : CharacterController
{
    public float MoveSpeed = 5;
    public GameObject TowerPrefab;

    //private SkeletonAnimation m_Spine;
    private PlayerState m_CurState;

    protected PlayerWeaponController m_WeaponController;
    protected EnemyController m_AtkTarget;

    #region Client
    [Client]
    protected override void Awake()
    {
        base.Awake();

        m_WeaponController = GetComponentInChildren<PlayerWeaponController>();

        //m_Spine = GetComponentInChildren<SkeletonAnimation>();
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
    protected override void Update()
    {
        if (!isLocalPlayer) return; //不应操作非本地玩家

        base.Update();

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

        Rgb.velocity = MoveDir * MoveSpeed; // 设置刚体速度
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

    [Client]
    protected void SearchTarget()
    {
        if (m_AtkTarget != null)
            return;

        var enemies = FindObjectsOfType<EnemyController>();
        if (enemies.Length > 0)
        {
            m_AtkTarget = enemies[0];
        }
    }

    protected float m_LastAtkTime;
    [Client]
    protected void Fire()
    {
        if (m_AtkTarget == null) return;

        //if (Time.time - m_LastAtkTime > 3)
        {
            m_LastAtkTime = Time.time;

            Vector3 fireDir = m_AtkTarget.transform.position - transform.position;
            fireDir.Normalize();

            CmdFire(fireDir, transform.position);
        }
    }

    [Command]
    protected void CmdFire(Vector3 dir, Vector3 position)
    {
        m_WeaponController.Fire(dir, position);
        //GameObject go = GameObject.Instantiate(Bullet, position, Quaternion.identity);
        //NetworkServer.Spawn(go);

        //go.GetComponent<BulletController>().SetMoveDir(dir);
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
        //GameObject tower = Instantiate(TowerPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        //GameObject tower = GameObjectPoolMgr.S.Allocate(ObjUtil.TowerAssetId);
        string assetId = ObjUtil.GetAssetId("BuildingTower");
        GameObject go = GameObjectPoolMgr.S.Allocate(assetId);
        go.transform.parent = ServerGameMgr.Instance.EntityRoot;
        NetworkServer.Spawn(go, connectionToClient);//服务器孵化，同步客户端
    }
    #endregion

    #region ClientRpc
    [ClientRpc]
    private void RpcPlayAnim(string animName)
    {
        m_SpineAnim.state.SetAnimation(0, animName, true);
    }
    #endregion

    private Vector3 m_LastMoveDir;
    public Vector3 GetMoveDir()
    {
        if (MoveDir != Vector3.zero)
        {
            m_LastMoveDir = MoveDir;
            return m_LastMoveDir;
        }

        return m_LastMoveDir;
    }
}
