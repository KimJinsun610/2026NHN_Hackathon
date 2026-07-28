using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private Player player;

    [Header("Dice System")] // 다이스 컨트롤러 연결을 위한 변수 추가
    [SerializeField] private DiceRoundController diceController;

    private List<Enemy> enemyList = new List<Enemy>();
    private bool isTurnExecuting = false;

    private Enemy selectedTarget = null;

    [Header("Turn System")]
    public int CurrentTurn { get; private set; } = 1; // 현재 턴 (기본값 1)
    public event Action<int> OnTurnChanged; // 턴이 바뀔 때 발생할 이벤트

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
    
    public void InitializeBattle(List<Enemy> spawnedEnemies)
    {
        enemyList = spawnedEnemies;
        Debug.Log($"[BattleManager] 총 {enemyList.Count} 마리의 적과 전투 세팅 완료!");
        
        // 처음 세팅될 때는 살아있는 첫 번째 적을 기본 타겟으로 잡아줍니다.
        selectedTarget = enemyList.Find(e => !e.IsDead);

        CurrentTurn = 1;
        OnTurnChanged?.Invoke(CurrentTurn);
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
        bool hasAliveEnemy = enemyList.Exists(e => !e.IsDead);

        // 턴이 진행 중이 아니고, 플레이어가 살아있으며, 적이 남아있을 때만 턴 시작
        if (!isTurnExecuting && !player.IsDead && hasAliveEnemy)
        {
            if (selectedTarget == null || selectedTarget.IsDead)
            {
                selectedTarget = enemyList.Find(e => !e.IsDead);
            }

            StartCoroutine(ExecuteTurn());
        }
    }

    private IEnumerator ExecuteTurn()
    {
        isTurnExecuting = true;

        yield return null;

        Debug.Log("--- 턴 시작 ---");


        // 선택된 타겟을 공격합니다!
        if (selectedTarget != null && !selectedTarget.IsDead)
        {
            player.Attack(selectedTarget);
            yield return new WaitForSeconds(1.0f);
        }

        if (!enemyList.Exists(e => !e.IsDead))
        {
            Debug.Log("전투 승리! 모든 적을 처치했습니다.");
            isTurnExecuting = false;
            yield break;
        }

        foreach (Enemy enemy in enemyList)
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

        CurrentTurn++; 
        OnTurnChanged?.Invoke(CurrentTurn);

        if (diceController != null)
        {
            diceController.PrepareNextRound();
        }

        isTurnExecuting = false;
        Debug.Log("--- 턴 종료 (다음 스페이스바 대기) ---");
    }
}