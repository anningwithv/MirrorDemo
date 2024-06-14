using DG.Tweening;
using GameFrame;
using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;


namespace ProjectX.Logic
{
    public static class FloatingTextUtil
    {
        public static GameObject CreateText(string text, Vector3 pos, Color colorCur)
        {
            string assetId = ObjUtil.GetAssetId(Define.ASSET_EFFECT_POP_TEXT);
            var floatingText = GameObjectPoolMgr.S.Allocate(assetId);
            floatingText.GetComponent<FloatingText>()?.Init(colorCur, text, pos);
            NetworkServer.Spawn(floatingText);

            return floatingText;
        }

    }
}