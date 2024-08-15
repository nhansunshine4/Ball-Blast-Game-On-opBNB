using UnityEngine;

public class LevelCompletedDialog : GameDialog
{
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        Time.timeScale = 0f;

        AudioController.Ins.StopPlayMusic();
        AudioController.Ins.PlaySound(AudioController.Ins.levelCompleted);
    }

    public void GoNextLevel()
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
