using System.Collections;
using System.Collections.Generic;
using TMPro;
using UDEV;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    [SerializeField] private GameObject m_homeGUI;
    [SerializeField] private GameObject m_gameGUI;

    [SerializeField] private TextMeshProUGUI m_currentLevelTxt;
    [SerializeField] private TextMeshProUGUI m_nextLevelTxt;
    [SerializeField] private Image m_lvProgressBarFilled;

    [SerializeField] private Dialog m_levelCompletedDialog;
    [SerializeField] private Dialog m_gameoverDialog;
    [SerializeField] private Dialog m_pauseDialog;

    private Dialog m_activeDialog;

    public Dialog ActiveDialog { get => m_activeDialog; private set => m_activeDialog = value; }

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowGameGUI(bool isShow)
    {
        if(m_gameGUI != null) m_gameGUI.SetActive(isShow);
        if(m_homeGUI != null) m_homeGUI.SetActive(!isShow);
    }

    public void UpdateLevelProgressBar(int currentLevel, float progress)
    {
        int nextLevel = currentLevel + 1;
        if(m_currentLevelTxt != null) m_currentLevelTxt.text = (currentLevel + 1).ToString();
        if(m_nextLevelTxt != null) m_nextLevelTxt.text = (nextLevel + 1).ToString();
        if(m_lvProgressBarFilled != null) m_lvProgressBarFilled.fillAmount = progress; 
    }

    public void ShowGameoverDialog()
    {
        ShowDialog(m_gameoverDialog);
    }

    public void ShowLevelCompletedDialog()
    {
        ShowDialog(m_levelCompletedDialog);
    }

    public void ShowPauseDialog()
    {
        ShowDialog(m_pauseDialog);
    }

    private void ShowDialog(Dialog dialog)
    {
        if(dialog == null) return;
        m_activeDialog = dialog;
        m_activeDialog.Show(true);
    }
}
