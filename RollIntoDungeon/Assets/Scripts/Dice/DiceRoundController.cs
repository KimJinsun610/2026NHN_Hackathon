using System;
using UnityEngine;
using UnityEngine.UI;

// "다시 굴리기"/"이대로 공격하기" 버튼 입력을 받아 리롤 횟수를 관리하고,
// 공격 확정 시 최종 공격/방어값(AttackResult)을 전투 시스템에 이벤트로 전달한다.
public class DiceRoundController : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private Button rerollButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private int maxRerollCount = 3;
    [SerializeField] private int criticalMultiplier = 3;

    public int RerollsRemaining { get; private set; }
    public int MaxRerollCount => maxRerollCount;

    // 전투 시스템(타 팀원)이 구독할 이벤트
    public event Action<AttackResult> OnAttackConfirmed;
    // 리롤 남은 횟수 UI 표시용
    public event Action<int> OnRerollsRemainingChanged;

    void Awake()
    {
        RerollsRemaining = maxRerollCount;
    }

    void OnEnable()
    {
        diceManager.OnTotalsChanged += HandleDiceUpdated;
        rerollButton.onClick.AddListener(OnRerollButtonClicked);
        attackButton.onClick.AddListener(OnAttackButtonClicked);
        RefreshButtons();
    }

    void OnDisable()
    {
        diceManager.OnTotalsChanged -= HandleDiceUpdated;
        rerollButton.onClick.RemoveListener(OnRerollButtonClicked);
        attackButton.onClick.RemoveListener(OnAttackButtonClicked);
    }

    void HandleDiceUpdated(DiceTotals _) => RefreshButtons();

    void OnRerollButtonClicked()
    {
        if (RerollsRemaining <= 0 || diceManager.AnyRolling) return;

        RerollsRemaining--;
        OnRerollsRemainingChanged?.Invoke(RerollsRemaining);
        diceManager.RollUnfixed();
        RefreshButtons();
    }

    void OnAttackButtonClicked()
    {
        if (!diceManager.AllSettled()) return;
        if (RerollsRemaining == maxRerollCount) return; // 이번 라운드에 한 번도 안 굴렸으면 공격 불가

        var totals = diceManager.CurrentTotals;
        bool isCritical = diceManager.AllSameValue();
        int finalAttack = isCritical ? totals.Attack * criticalMultiplier : totals.Attack;
        int finalDefense = isCritical ? totals.Defense * criticalMultiplier : totals.Defense;
        var result = new AttackResult(finalAttack, finalDefense, isCritical);

        Debug.Log($"[공격 확정] Attack={result.Attack}, Defense={result.Defense}, Critical={result.IsCritical}");
        OnAttackConfirmed?.Invoke(result);

        RerollsRemaining = maxRerollCount;
        OnRerollsRemainingChanged?.Invoke(RerollsRemaining);
        diceManager.StartNewRound(); // 고정 해제 등 다음 턴 준비
        RefreshButtons();
    }

    void RefreshButtons()
    {
        rerollButton.interactable = RerollsRemaining > 0 && !diceManager.AnyRolling;
        attackButton.interactable = diceManager.AllSettled() && RerollsRemaining < maxRerollCount;
    }
}
