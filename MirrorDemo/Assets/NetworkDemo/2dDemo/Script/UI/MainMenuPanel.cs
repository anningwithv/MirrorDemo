using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_IpInput;
    [SerializeField] private Button m_ClientStartBtn;
    //[SerializeField] private Button m_HostStartBtn;
    //[SerializeField] private Button m_ServerStartBtn;

    //private NetworkMgr m_NetworkMgr;
    private string m_GameSceneName = "Game";
    private void Awake()
    {
        //m_NetworkMgr = FindObjectOfType<NetworkMgr>();

        //m_NetworkMgr.networkAddress = m_IpInput.text;

        m_ClientStartBtn.onClick.AddListener(() =>
        {
            //m_NetworkMgr?.StartClient();
            ShowPanel(false);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(m_GameSceneName, LoadSceneMode.Additive);
        });


        //m_HostStartBtn.onClick.AddListener(() => {
        //    m_NetworkMgr?.StartHost();
        //    ShowPanel(false);
        //});

        //m_ServerStartBtn.onClick.AddListener(() => {
        //    m_NetworkMgr?.StartServer();
        //    ShowPanel(false);
        //});
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneType)
    {
        if (scene.name == m_GameSceneName)
        {
            var networkMgr = FindObjectOfType<NetworkMgr>();
            if (networkMgr == null)
            {
                Debug.LogError("NetworkMgr not found!");
                return;
            }
            networkMgr.networkAddress = "192.168.20.108";
            networkMgr?.StartClient();
        }
    }

    private void ShowPanel(bool show)
    {
        gameObject.SetActive(show); 
    }
}
