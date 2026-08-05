using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 추가합니다.

public class StageManager : MonoBehaviour
{
    [Header("Currnet Stage Data")]
    [SerializeField] private StageData currentStageData;

    [Header("Managers")]
    [SerializeField] private BattleManager battleManager;

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

        // 적을 여기서 소환하지 않고, 배틀 매니저에게 스테이지 데이터 전체를 넘겨줍니다.
        battleManager.InitializeBattle(currentStageData);
    }
}