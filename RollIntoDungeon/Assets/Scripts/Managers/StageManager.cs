using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 추가합니다.

public class StageManager : MonoBehaviour
{
    [Header("Currnet Stage Data")]
    [SerializeField] private StageData currentStageData;

    [Header("Managers")]
    [SerializeField] private BattleManager battleManager;

    private List<Enemy> spawnedEnemies = new List<Enemy>();

    void Start()
    {
        SetupTestStage();
    }

    private void SetupTestStage()
    {
        if (currentStageData == null)
        {
            Debug.LogError("스테이지 데이터가 비어있습니다!");
            return;
        }

        Debug.Log($"--- 스테이지 {currentStageData.stageLevel} 세팅 시작 ---");

        foreach (EnemySpawnInfo info in currentStageData.enemiesToSpawn)
        {
            //Quaternion rotation = Quaternion.Euler(info.spawnRotation);
            Quaternion cameraRotation = Camera.main.transform.rotation;

            Enemy newEnemy = Instantiate(info.enemyPrefab, info.spawnPosition, cameraRotation);
            spawnedEnemies.Add(newEnemy);
        }

        battleManager.InitializeBattle(spawnedEnemies);
    }
}