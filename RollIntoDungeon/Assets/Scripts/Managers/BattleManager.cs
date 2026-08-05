using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private Player player;

    [Header("Dice System")] 
    [SerializeField] private DiceRoundController diceController;

    private List<Enemy> enemyList = new List<Enemy>();
    private bool isTurnExecuting = false;

    private Enemy selectedTarget = null;

    [Header("Turn System")]
    public int CurrentTurn { get; private set; } = 1; // 현재 턴 (기본값 1)
    public event Action<int> OnTurnChanged; // 턴이 바뀔 때 발생할 이벤트

    private StageData currentStageData;
    private int currentWaveIndex = 0;

    private void OnEnable()
    {
        if (diceController != null)
        {
            diceController.OnAttackConfirmed += OnDiceAttackConfirmed;
        }
    }

    private void OnDisable()
    {
        if (diceController != null)
        {
            diceController.OnAttackConfirmed -= OnDiceAttackConfirmed;
        }
    }
    
    public void InitializeBattle(StageData stageData)
    {
        currentStageData = stageData;
        currentWaveIndex = 0;
        enemyList.Clear();

        CurrentTurn = 1;
        OnTurnChanged?.Invoke(CurrentTurn);

        SpawnNextEnemy();
    }

    private void SpawnNextEnemy()
    {
        if (currentStageData == null || currentWaveIndex >= currentStageData.enemiesToSpawn.Count) return;

        EnemySpawnInfo info = currentStageData.enemiesToSpawn[currentWaveIndex];
        Quaternion cameraRotation = Camera.main.transform.rotation;

        Enemy newEnemy = Instantiate(info.enemyPrefab, info.spawnPosition, cameraRotation);
        enemyList.Add(newEnemy);

        selectedTarget = newEnemy;

        currentWaveIndex++;
        Debug.Log($"[웨이브 {currentWaveIndex}/{currentStageData.enemiesToSpawn.Count}] {newEnemy.name} 등장!");
    }

    void Update()
    {
        // 턴이 진행 중일 때는 마우스 클릭이나 스페이스바 입력을 전부 무시합니다! (안전장치)
        if (isTurnExecuting) return;

        // 1. 마우스 클릭으로 타겟을 변경하는 로직
        HandleMouseClickTargeting();

        // 2. 선택된 타겟 머리 위에만 화살표를 띄우는 로직
        UpdateTargetIndicator();

    }

    // 마우스 클릭을 감지하여 타겟을 바꾸는 함수
    private void HandleMouseClickTargeting()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                Enemy clickedEnemy = hit.collider.GetComponent<Enemy>();
                
                if (clickedEnemy != null && !clickedEnemy.IsDead)
                {
                    selectedTarget = clickedEnemy;
                    Debug.Log($"타겟 변경: {selectedTarget.name}");
                }
            }
        }
    }

    // 화살표(타겟 마커)를 갱신하는 함수
    private void UpdateTargetIndicator()
    {
        if (selectedTarget == null || selectedTarget.IsDead)
        {
            selectedTarget = enemyList.Find(e => !e.IsDead);
        }

        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.SetTargeted(enemy == selectedTarget);
            }
        }
    }

    // 다이스 컨트롤러에서 이벤트가 발생하면 자동으로 호출되는 함수
    private void OnDiceAttackConfirmed(AttackResult result)
    {
        if (!isTurnExecuting && !player.IsDead && enemyList.Exists(e => !e.IsDead))
        {
            StartCoroutine(ExecuteTurn());
        }
    }

    public void AddSpawnedEnemy(Enemy newEnemy)
    {
        if (newEnemy != null)
        {
            enemyList.Add(newEnemy);
            Debug.Log($"[BattleManager] 난입! 새로운 적 {newEnemy.name} 추가됨. 현재 남은 적: {enemyList.Count}마리");
        }
    }

    private IEnumerator ExecuteTurn()
    {
        isTurnExecuting = true;
        yield return null;
        Debug.Log("--- 턴 시작 ---");

        // 1. 플레이어 공격턴
        if (selectedTarget != null && !selectedTarget.IsDead)
        {
            player.Attack(selectedTarget);
            yield return new WaitForSeconds(1.0f);
        }

        //죽은 적을 리스트에서 제거 
        enemyList.RemoveAll(e => e == null || e.IsDead);

        // 2. 필드에 남은 적이 하나도 없는지 검사
        if (enemyList.Count == 0)
        {
            if (currentWaveIndex < currentStageData.enemiesToSpawn.Count)
            {
                Debug.Log("적을 쓰러뜨렸습니다! 다음 적 등장 대기 중...");
                yield return new WaitForSeconds(1.0f); 

                SpawnNextEnemy(); // 다음 적 소환

                FinishTurn();
                yield break;
            }
            else
            {
                Debug.Log("전투 승리! 모든 웨이브를 클리어했습니다.");
                isTurnExecuting = false;
                yield break;
            }
        }

        // 3. 적 공격턴
        List<Enemy> currentTurnEnemies = new List<Enemy>(enemyList);
        foreach (Enemy enemy in currentTurnEnemies)
        {
            if (!enemy.IsDead)
            {
                enemy.Attack(player);
                yield return new WaitForSeconds(1.0f);

                if (player.IsDead)
                {
                    Debug.Log("전투 패배... 플레이어가 쓰러졌습니다.");
                    isTurnExecuting = false;
                    yield break;
                }
            }
        }

        FinishTurn();
    }

    private void FinishTurn()
    {
        CurrentTurn++;
        OnTurnChanged?.Invoke(CurrentTurn);

        if (diceController != null)
        {
            diceController.PrepareNextRound();
        }

        isTurnExecuting = false;
        Debug.Log("--- 턴 종료  ---");
    }
}