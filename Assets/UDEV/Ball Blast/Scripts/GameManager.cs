using System.Collections.Generic;
using UDEV;
using UDEV.SPM;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static GameState state;
    public GameSetting settings;

    private GunController m_gunCtr;
    private List<BallController> m_ballsOnScene = new List<BallController>();

    [SerializeField] float m_gunSpawnOffset;
    private int m_ballRequired;
    private int m_sceneBallCounting;
    private int m_ballKilled;

    public float LevelProgress { get => (float)m_ballKilled / m_ballRequired; }

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    private void Start()
    {
        Init();

        GUIManager.Ins.ShowGameGUI(false);
    }

    private void Init()
    {
        state = GameState.STARTING;
        m_sceneBallCounting = 0;
        m_ballKilled = 0;
        ViewPortUtil.GetWorldPos();
        LevelManager.Ins.LoadData();
        m_ballRequired = LevelManager.Ins.ScoreRequired;

        GUIManager.Ins.UpdateLevelProgressBar(Prefs.levelIndex, LevelProgress);
        AudioController.Ins.PlayBackgroundMusic();
    }

    public void PlayGame()
    {
        state = GameState.PLAYING;
        SpawnGun();
        SpawnSceneBall();

        GUIManager.Ins.ShowGameGUI(true);
    }

    public void Replay_GoNext()
    {
        DeactiveBallsOnScene();
        Init();
        PlayGame();
    }

    private void DeactiveBallsOnScene()
    {
        if (m_ballsOnScene == null || m_ballsOnScene.Count <= 0) return;
        for (int i = 0; i < m_ballsOnScene.Count; i++)
        {
            var ballCtr = m_ballsOnScene[i];
            if (ballCtr == null) continue;
            ballCtr.gameObject.SetActive(false);
        }
        m_ballsOnScene.Clear();
    }

    private void SpawnGun()
    {
        if (m_gunCtr != null) Destroy(m_gunCtr.gameObject);
        Vector3 spawnPos = new Vector3(0, ViewPortUtil.MinY + m_gunSpawnOffset, 0);
        m_gunCtr = Instantiate(settings.gunPrefab, spawnPos, Quaternion.identity);
        if (m_gunCtr != null) m_gunCtr.OnBallCollision.AddListener(Gameover);
    }

    private void SpawnSceneBall()
    {
        if (state != GameState.PLAYING) return;
        if (m_sceneBallCounting >= settings.ballsLimitOnScene) return;
        var ballPoolKeys = settings.ballPoolKeys;
        if(ballPoolKeys == null || ballPoolKeys.Count <= 0) return;

        float randCheck = Random.value;
        bool canSpawnLeftSide = randCheck <= 0.5f;
        Vector3 spawnPos = GetSpawnPos(canSpawnLeftSide);
        string ballPoolKey = Helper.GetRandom(ballPoolKeys.ToArray());
        int smallBallPoolKeyIdx = ballPoolKeys.IndexOf(ballPoolKey) + 1;
        var ballController = SpawnBall(ballPoolKey, spawnPos, canSpawnLeftSide);

        if(ballController == null) return;
        ballController.OnDead.AddListener(ReduceBallCounting);
        ballController.OnDead.AddListener(SpawnSceneBall);
        ballController.OnDeadWithPosition.AddListener((pos) => SpawnSmallBalls(smallBallPoolKeyIdx, pos));
    }

    private Vector3 GetSpawnPos(bool canSpawnLeftSide)
    {
        float spawnPosY = Random.Range(ViewPortUtil.MaxY / 2 - 2f, ViewPortUtil.MaxY / 2);
        Vector3 ballSpawnPosLeft = new Vector3(ViewPortUtil.MinX - 1.5f, spawnPosY, 0f);
        Vector3 ballSpawnPosRight = new Vector3(ViewPortUtil.MaxX + 1.5f, spawnPosY, 0f);

        return canSpawnLeftSide ? ballSpawnPosLeft : ballSpawnPosRight;
    }

    private void SpawnSmallBalls(int ballPoolKeyIndex, Vector3 spawnPos)
    {
        if(state != GameState.PLAYING) return;
        if(ballPoolKeyIndex >= settings.ballPoolKeys.Count)
        {
            SpawnSceneBall();
            IncreaseBallKilled();
            CheckLevelCompleted();
            return;
        }

        string ballPoolKey = settings.ballPoolKeys[ballPoolKeyIndex];
        int ballPoolKeyChildIdx = ballPoolKeyIndex + 1;
        for (int i = 0; i < 2; i++) { 
            var ballCtr = SpawnBall(ballPoolKey, spawnPos, i == 0);
            if(ballCtr == null) continue;
            ballCtr.ActiveCollision();

            ballCtr.OnDead.AddListener(ReduceBallCounting);
            ballCtr.OnDead.AddListener(SpawnSceneBall);
            ballCtr.OnDeadWithPosition.AddListener((pos) => SpawnSmallBalls(ballPoolKeyChildIdx, pos));
        }

    }

    private BallController SpawnBall(string ballPoolKey, Vector3 spawnPos, bool canSpawnLeftSide)
    {
        m_sceneBallCounting++;
        var ballClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE, ballPoolKey, spawnPos, Quaternion.identity);
        var ballCtr = ballClone.GetComponent<BallController>();
        ballCtr?.Init();
        ballCtr?.MoveTrigger(ballCtr.CurBallData.bounceForce, !canSpawnLeftSide);
        m_ballsOnScene.Add(ballCtr);
        return ballCtr;
    }

    private void ReduceBallCounting()
    {
        if(m_sceneBallCounting <= 0) return;
        m_sceneBallCounting--;
    }

    private void CheckLevelCompleted()
    {
        if (m_ballKilled < m_ballRequired) return;
        state = GameState.LEVEL_COMPLETED;
        LevelManager.Ins.LevelIndex++;
        Prefs.bestLevel = LevelManager.Ins.LevelIndex;

        GUIManager.Ins.ShowLevelCompletedDialog();
        Debug.Log(state);
    }

    private void IncreaseBallKilled()
    {
        m_ballKilled++;

        GUIManager.Ins.UpdateLevelProgressBar(Prefs.levelIndex, LevelProgress);
    }

    private void Gameover()
    {
        state = GameState.GAMEOVER;

        GUIManager.Ins.ShowGameoverDialog();
        Debug.Log(state);
    }

    private void OnDisable()
    {
        if (m_gunCtr != null) m_gunCtr.OnBallCollision.RemoveListener(Gameover);
    }
}
