using DG.Tweening;
using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectX.Logic
{
    public class FloatingText : NetworkBehaviour
    {
        [SyncVar]
        private Color m_Color;
        [SyncVar]
        private string m_Text;
        [SyncVar]
        private Vector3 m_Pos;

        /// <summary>
        /// Call in server
        /// </summary>
        public void Init(Color color, string text, Vector3 pos)
        {
            m_Color = color;
            m_Text = text;
            m_Pos = pos;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            Show();
        }

        private void Show()
        {
            TextMeshPro textMeshPro;

            var floatingText = gameObject;
            floatingText.transform.SetParent(ServerGameMgr.Instance.EntityRoot);
            floatingText.SetActive(true);

            textMeshPro = floatingText.GetComponent<TextMeshPro>();
            textMeshPro.sortingOrder = 1;
            //textMeshPro.rectTransform.sizeDelta = new Vector2(1.2f, 0.6f);

            floatingText.transform.position = m_Pos;

            textMeshPro.color = m_Color;
            textMeshPro.fontSize = 4;
            //textMeshPro.enableExtraPadding = true;
            //textMeshPro.enableShadows = false;
            textMeshPro.enableKerning = false;
            textMeshPro.text = m_Text;

            //BattleMgr.Instance.LevelRoot.Mono.StartCoroutine(DisplayTextMeshProFloatingText(textMeshPro));
            textMeshPro.rectTransform.localScale = Vector3.one;
            textMeshPro.rectTransform.DOMoveY(1.2f, 0.8f).SetRelative(true);
            textMeshPro.rectTransform.DOScale(1.4f, 0.8f).SetRelative(false).OnComplete(() => {
                GameObjectPoolMgr.S.Recycle(textMeshPro.gameObject);
                NetworkServer.UnSpawn(textMeshPro.gameObject);
            });
        }
    }
}