using UnityEngine;

public class PauseDialog : GameDialog
{
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        Time.timeScale = 0f;
    }

    public void Replay()
    {
        Close();
        GameManager.Ins.Replay_GoNext();
    }

    public void BackHome()
    {
        Close();
        Helper.ReloadCurrentScene();
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }
}
