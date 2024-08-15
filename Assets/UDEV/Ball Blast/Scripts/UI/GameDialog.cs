using TMPro;
using UnityEngine;

public class GameDialog : Dialog
{
    [SerializeField] private TextMeshProUGUI m_bestLevelTxt;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if (m_bestLevelTxt != null) m_bestLevelTxt.text = Prefs.bestLevel.ToString("00");
    }
}
