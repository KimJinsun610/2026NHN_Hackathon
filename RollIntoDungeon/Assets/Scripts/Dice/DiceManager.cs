using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

// 여러 Dice를 묶어서 굴리기/라운드 리셋을 오케스트레이션한다.
public class DiceManager : MonoBehaviour
{
    [SerializeField] private List<Dice> dices = new();
    [SerializeField] private bool rollOnRKeyForTest = true; // 디버깅용, 실제 흐름 연결 후 꺼도 됨
    [SerializeField] private int maxFixedCount = 3; // 동시에 고정 가능한 최대 개수

    // 확정(Settled)된 주사위들의 공격/방어 합산값. UI와 전투 시스템이 동일한 값을 공유한다.
    public event Action<DiceTotals> OnTotalsChanged;
    public DiceTotals CurrentTotals { get; private set; }

    void Awake()
    {
        foreach (var dice in dices)
            dice.OnStateChanged += HandleDiceStateChanged;
    }

    void OnDestroy()
    {
        foreach (var dice in dices)
            dice.OnStateChanged -= HandleDiceStateChanged;
    }

    void Update()
    {
        if (rollOnRKeyForTest && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            RollUnfixed();
        }
    }

    void HandleDiceStateChanged(Dice.DiceState _) => RecalculateTotals();

    // 현재 Settled 상태인 주사위들만 모아 공격/방어 합산값을 다시 계산하고 발행한다.
    void RecalculateTotals()
    {
        int atk = 0, def = 0;

        foreach (var dice in dices)
        {
            if (dice.State != Dice.DiceState.Settled) continue;

            var face = dice.LastResult;
            if (face.faceType == DiceFaceReader.DiceFaceType.Attack)
                atk += face.value;
            else
                def += face.value;
        }

        CurrentTotals = new DiceTotals(atk, def);
        OnTotalsChanged?.Invoke(CurrentTotals);
    }

    // 고정되지 않은 주사위만 굴린다 (고정된 주사위는 Dice.Roll() 내부에서 스스로 스킵)
    public void RollUnfixed()
    {
        foreach (var dice in dices)
            dice.Roll();
    }

    // 새 라운드 시작 시 모든 고정을 해제
    public void StartNewRound()
    {
        foreach (var dice in dices)
            dice.ResetFix();
    }

    public bool AllSettled()
    {
        return dices.Count > 0 && dices.All(d => d.State == Dice.DiceState.Settled);
    }

    public IEnumerable<Dice> FixedDices => dices.Where(d => d.IsFixed);

    public int FixedCount => dices.Count(d => d.IsFixed);
    public bool CanFixMore => FixedCount < maxFixedCount;

    // DiceSelector가 클릭 시 이 메서드를 통해 고정을 시도한다 (dice.ToggleFix() 직접 호출 금지).
    // 해제는 항상 허용하고, 신규 고정은 개수 제한을 넘으면 막는다.
    public bool TryToggleFix(Dice dice)
    {
        bool wantFixed = !dice.IsFixed;

        if (wantFixed && !CanFixMore)
        {
            Debug.Log($"고정 가능 개수({maxFixedCount}개)를 초과했습니다.");
            return false;
        }

        return dice.SetFixed(wantFixed);
    }
}
