using UnityEngine;
using System.Collections.Generic;
using System.Collections; // 코루틴(IEnumerator)을 사용하기 위해 필요합니다.
using UnityEngine.InputSystem;


public class BattleManager : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private Player player;

    private List<Enemy> enemyList = new List<Enemy>();

    // 턴이 연출되는 동안 스페이스바를 연타해도 중복 실행되지 않도록 막는 안전장치
    private bool isTurnExecuting = false;

    public void InitializeBattle(List<Enemy> spawnedEnemies)
    {
        enemyList = spawnedEnemies;
        Debug.Log($"[BattleManager] 총 {enemyList.Count} 마리의 적과 전투 세팅 완료!");
    }


    void Update()
    {
        // 리스트 안의 적들 중 단 한 마리라도 살아있는지(IsDead가 false인지) 검사합니다.
        bool hasAliveEnemy = enemyList.Exists(e => !e.IsDead);

        // 스페이스바를 눌렀고, 턴이 진행 중이 아니며, 둘 다 살아있을 때만 턴 시작
       if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame && !isTurnExecuting && !player.IsDead && hasAliveEnemy)
        {
            StartCoroutine(ExecuteTurn());
        }
    }

    // 코루틴을 사용하면 코드 실행 중간에 원하는 시간만큼 대기할 수 있습니다.
    private IEnumerator ExecuteTurn()
    {
        isTurnExecuting = true;
        Debug.Log("--- 턴 시작 ---");

        player.CalculateTurnStats();

        // 1. 플레이어 공격 (리스트에서 살아있는 첫 번째 적을 자동으로 타겟팅)
        Enemy targetEnemy = enemyList.Find(e => !e.IsDead);
        if (targetEnemy != null)
        {
            player.Attack(targetEnemy);
            yield return new WaitForSeconds(1.0f);
        }
        else{
            Debug.Log("적이 없습니다.");
        }

        // 2. 모든 적이 죽었는지 체크 (살아있는 적이 하나도 없다면)
        if (!enemyList.Exists(e => !e.IsDead))
        {
            Debug.Log("전투 승리! 모든 적을 처치했습니다.");
            isTurnExecuting = false;
            yield break;
        }

        // 3. 살아있는 적들의 반격
        foreach (Enemy enemy in enemyList)
        {
            // 죽은 적은 공격 턴을 건너뜁니다.
            if (!enemy.IsDead)
            {
                enemy.Attack(player);
                
                // 여러 마리가 동시에 때리지 않고 차례대로 때리도록 1초씩 기다려줍니다.
                yield return new WaitForSeconds(1.0f); 
                
                // 적 하나가 때릴 때마다 플레이어가 죽었는지 즉시 확인합니다.
                if (player.IsDead)
                {
                    Debug.Log("전투 패배... 플레이어가 쓰러졌습니다.");
                    isTurnExecuting = false;
                    yield break;
                }
            }
        }

        isTurnExecuting = false;
        Debug.Log("--- 턴 종료 (다음 스페이스바 대기) ---");
    }
}