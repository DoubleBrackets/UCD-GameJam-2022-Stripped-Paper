using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData")]
public class LevelDataAsset : ScriptableObject
{
    public List<LevelData> levelData;

    public LevelData this[int key]
    {
        get => levelData[key];
    }
}

[System.Serializable]
public class LevelData
{
    public Vector3 respawnPosition;
}