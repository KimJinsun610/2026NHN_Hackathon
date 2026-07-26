using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 추가합니다.

public class StageManager : MonoBehaviour
{
    [Header("Test Settings")]
    // [] 기호를 붙여 배열로 만듭니다. 이제 여러 마리를 등록할 수 있습니다.
    [SerializeField] private Enemy[] enemyPrefabsToSpawn; 
    
    // 적들이 소환될 위치도 여러 개 배열로 받습니다.
    [SerializeField] private Transform[] spawnPoints;

    [Header("Managers")]
    [SerializeField] private BattleManager battleManager;

    void Start()
    {
        SetupTestStage();
    }

    private void SetupTestStage()
    {
        // 생성된 적들을 차곡차곡 담아둘 리스트 바구니입니다.
        List<Enemy> spawnedEnemies = new List<Enemy>();

        // 설정된 프리팹 개수만큼 반복문을 돌며 적을 생성합니다.
        for (int i = 0; i < enemyPrefabsToSpawn.Length; i++)
        {
            if (enemyPrefabsToSpawn[i] == null || spawnPoints.Length <= i || spawnPoints[i] == null)
                continue;

            Enemy spawnedEnemy = Instantiate(enemyPrefabsToSpawn[i], spawnPoints[i].position, spawnPoints[i].rotation);

            // 오브젝트 이름이 헷갈리지 않게 번호를 달아줍니다. (예: Slime_0, Slime_1)
            spawnedEnemy.name = enemyPrefabsToSpawn[i].name + "_" + i;
            
            spawnedEnemies.Add(spawnedEnemy);
        }

        // 완성된 다수의 적 리스트를 전투 매니저에게 통째로 넘겨줍니다.
        battleManager.InitializeBattle(spawnedEnemies);
    }
}