using System.Collections;
using System.Collections.Generic;
using UDEV.SPM;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameSetting", menuName = "UDEV/BB/Create GameSetting")]
public class GameSetting : ScriptableObject
{
    public int ballsLimitOnScene;
    [PoolerKeys(target = PoolerTarget.NONE)]
    public List<string> ballPoolKeys;

    public GunController gunPrefab;
}
