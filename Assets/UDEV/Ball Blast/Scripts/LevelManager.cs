using System.Collections;
using System.Collections.Generic;
using UDEV;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelDataGroup m_levelDataGroup;
    private int m_levelIndex;

    public int LevelIndex {
        get => m_levelIndex;
        set {
            if (m_levelDataGroup == null) return;
            var levelDatas = m_levelDataGroup.levelDatas;
            if (levelDatas == null || levelDatas.Length <= 0) return;

            m_levelIndex = (value % levelDatas.Length + levelDatas.Length) % levelDatas.Length; //4 0 1 2 3 

            Prefs.levelIndex = m_levelIndex;
        }
    }

    public int ScoreRequired
    {
        get { 
            if(LevelData == null) return 0;
            return LevelData.BallRequired * (m_levelIndex + 1);
        }
    }

    public LevelData LevelData
    {
        get
        {
            if (m_levelDataGroup == null) return null;
            var levelDatas = m_levelDataGroup.levelDatas;
            if (levelDatas == null || levelDatas.Length <= 0 || m_levelIndex < 0 || m_levelIndex >= levelDatas.Length) return null;
            return levelDatas[m_levelIndex];
        }
    }

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    public void LoadData()
    {
        m_levelIndex = Prefs.levelIndex;
    }
}
