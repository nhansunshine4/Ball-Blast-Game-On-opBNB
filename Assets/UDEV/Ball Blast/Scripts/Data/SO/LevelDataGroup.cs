using UnityEngine;

[System.Serializable]
public class LevelData {
    public int minBallRequired;
    public int maxBallRequired;

    public int BallRequired
    {
        get => Random.Range(minBallRequired, maxBallRequired);
    }
}

[CreateAssetMenu(fileName = "New LevelDataGroup", menuName = "UDEV/BB/Create LevelDataGroup")]
public class LevelDataGroup : ScriptableObject
{
    public LevelData[] levelDatas;
}
