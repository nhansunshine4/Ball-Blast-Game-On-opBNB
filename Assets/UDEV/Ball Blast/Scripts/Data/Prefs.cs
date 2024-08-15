using UnityEngine;

public static class Prefs
{
    public static int levelIndex { 
        get => PlayerPrefs.GetInt(PrefKey.level_index.ToString(), 0);
        set => PlayerPrefs.SetInt(PrefKey.level_index.ToString(), value);
    }

    public static int bestLevel
    {
        get => PlayerPrefs.GetInt(PrefKey.best_level.ToString(), 0);
        set
        {
            int bestLevelUserData = PlayerPrefs.GetInt(PrefKey.best_level.ToString(), 0);
            if (bestLevelUserData >= value) return;
            PlayerPrefs.SetInt(PrefKey.best_level.ToString(), value);
        }
    }
}
