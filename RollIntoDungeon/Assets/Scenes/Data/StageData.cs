using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Stage", menuName = "Game Data/Stage Data")]
public class StageData : ScriptableObject
{
    [Header("스테이지 기본 정보")]
    public int stageLevel;        
    public bool isBossStage;      

    [Header("등장할 적 목록")]
    public List<EnemySpawnInfo> enemiesToSpawn;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public Enemy enemyPrefab;     
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
}