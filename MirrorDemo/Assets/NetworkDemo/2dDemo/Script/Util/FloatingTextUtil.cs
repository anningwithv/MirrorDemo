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
            TextMeshPro textMeshPro;

            var floatingText = GameObjectPoolMgr.S.Allocate(Define.ASSET_EFFECT_POP_TEXT.ToLower());
            //floatingText.name = text;
            floatingText.transform.SetParent(ServerGameMgr.Instance.EntityRoot);
            floatingText.SetActive(true);
            NetworkServer.Spawn(floatingText);

            textMeshPro = floatingText.GetComponent<TextMeshPro>();
            textMeshPro.sortingOrder = 1;
            //textMeshPro.rectTransform.sizeDelta = new Vector2(1.2f, 0.6f);

            floatingText.transform.position = pos ;

            textMeshPro.color = colorCur;
            textMeshPro.fontSize = 4;
            //textMeshPro.enableExtraPadding = true;
            //textMeshPro.enableShadows = false;
            textMeshPro.enableKerning = false;
            textMeshPro.text = text;

            //BattleMgr.Instance.LevelRoot.Mono.StartCoroutine(DisplayTextMeshProFloatingText(textMeshPro));
            textMeshPro.rectTransform.localScale = Vector3.one;
            textMeshPro.rectTransform.DOMoveY(1.2f, 0.8f).SetRelative(true);
            textMeshPro.rectTransform.DOScale(1.4f, 0.8f).SetRelative(false).OnComplete(() => {
                GameObjectPoolMgr.S.Recycle(textMeshPro.gameObject);
                NetworkServer.UnSpawn(textMeshPro.gameObject);
            });

            return floatingText;
        }

    }
}