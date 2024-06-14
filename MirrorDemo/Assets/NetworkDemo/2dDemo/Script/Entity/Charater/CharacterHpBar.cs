using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectX.Logic
{
    public class CharacterHpBar : MonoBehaviour
    {
        public Image m_ProgressBarImg;
        public Image m_ProgressBg;

        //protected CharacterController m_CharacterController;
        protected Color m_InitColor;
        private bool m_IsShowing;

        public virtual void Init()
        {
            //m_CharacterController = character;
            Show(true);

            Refresh(100);

            m_InitColor = m_ProgressBarImg.color;
        }

        public virtual void Refresh(float percent)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            m_ProgressBarImg.fillAmount = percent / 100f;
            //var tween = m_ProgressBarImg.DOFillAmount(percent / 100f, 0.1f);
        }

        public void Show(bool show)
        {
            m_ProgressBarImg.gameObject.SetActive(show);
            m_ProgressBg.gameObject.SetActive(show);
            m_IsShowing = show;
        }

        public bool IsShowing()
        {
            return m_IsShowing;
        }
    }
}