using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private Player player;

    private List<Enemy> enemyList = new List<Enemy>();
    private bool isTurnExecuting = false;

    // 새로 추가된 변수: 사용자가 마우스로 클릭해서 선택한 현재 타겟
    private Enemy selectedTarget = null;

    public void InitializeBattle(List<Enemy> spawnedEnemies)
    {
        enemyList = spawnedEnemies;
        Debug.Log($"[BattleManager] 총 {enemyList.Count} 마리의 적과 전투 세팅 완료!");
        
        // 처음 세팅될 때는 살아있는 첫 번째 적을 기본 타겟으로 잡아줍니다.
        selectedTarget = enemyList.Find(e => !e.IsDead);
    }

    void Update()
    {
        // 턴이 진행 중일 때는 마우스 클릭이나 스페이스바 입력을 전부 무시합니다! (안전장치)
        if (isTurnExecuting) return;

        // 1. 마우스 클릭으로 타겟을 변경하는 로직
        HandleMouseClickTargeting();

        // 2. 선택된 타겟 머리 위에만 화살표를 띄우는 로직
        UpdateTargetIndicator();

        bool hasAliveEnemy = enemyList.Exists(e => !e.IsDead);

        // 스페이스바를 누르면 전투 시작
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !player.IsDead && hasAliveEnemy)
        {
            // 혹시라도 타겟이 죽어있거나 없다면, 살아있는 다른 적으로 강제 교체합니다.
            if (selectedTarget == null || selectedTarget.IsDead)
            {
                selectedTarget = enemyList.Find(e => !e.IsDead);
            }

            StartCoroutine(ExecuteTurn());
        }
    }

    // 마우스 클릭을 감지하여 타겟을 바꾸는 함수
    private void HandleMouseClickTargeting()
    {
        // 마우스 왼쪽 버튼을 이번 프레임에 막 눌렀다면
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 화면상의 마우스 위치를 실제 2D 게임 공간의 좌표로 변환합니다.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            // 마우스 위치에서 안쪽으로 광선(Ray)을 쏴서 뭔가 부딪히는지 검사합니다.
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                // 부딪힌 물체에 Enemy 컴포넌트가 있는지 확인합니다.
                Enemy clickedEnemy = hit.collider.GetComponent<Enemy>();
                
                // 클릭한 대상이 적(Enemy)이 맞고, 아직 살아있다면 타겟으로 확정!
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
        // 선택된 타겟이 방금 죽었다면, 다음 살아있는 적으로 화살표를 넘겨줍니다.
        if (selectedTarget == null || selectedTarget.IsDead)
        {
            selectedTarget = enemyList.Find(e => !e.IsDead);
        }

        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                // 현재 선택된 타겟(selectedTarget)과 일치하는 녀석만 화살표를 켜줍니다.
                enemy.SetTargeted(enemy == selectedTarget);
            }
        }
    }

    private IEnumerator ExecuteTurn()
    {
        isTurnExecuting = true;
        Debug.Log("--- 턴 시작 ---");

        player.CalculateTurnStats();

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

        isTurnExecuting = false;
        Debug.Log("--- 턴 종료 (다음 스페이스바 대기) ---");
    }
}