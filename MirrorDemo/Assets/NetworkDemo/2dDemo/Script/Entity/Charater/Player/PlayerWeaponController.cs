using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : NetworkBehaviour
{
    public GameObject Bullet;

    [Server]
    private void Update()
    {
        if (isServer)
        {
            //Debug.Log("Weapon pos is: " + transform.position);
        }
    }

    [Server]
    public void Fire(Vector3 dir, Vector3 position)
    {
        //GameObject go = GameObject.Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject go = GameObjectPoolMgr.S.Allocate(ObjUtil.BulletAssetId);
        go.transform.parent = ServerGameMgr.Instance.EntityRoot;
        go.transform.position = position;
        go.GetComponent<BulletController>().SetMoveDir(dir);

        NetworkServer.Spawn(go);

    }
}
