using System.Collections;
using UnityEngine;
using TMPro;

// DiceManager.OnTotalsChanged를 구독해 공격/방어 합산 텍스트를 표시한다.
// 합계가 늘어날 때는 1씩 카운트업 연출, 줄어들거나(리롤 시작 등) 그대로면 즉시 반영한다.
public class DiceTotalsView : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private float countUpInterval = 0.05f; // 1 증가당 대기 시간

    [SerializeField] private BattleManager battleManager;
    [SerializeField] private TMP_Text turnValueText;

    private CountUpStat attackStat;
    private CountUpStat defenseStat;

    void Awake()
    {
        attackStat = new CountUpStat(this, attackText);
        defenseStat = new CountUpStat(this, defenseText);
    }

    void OnEnable()
    {
        diceManager.OnTotalsChanged += HandleTotalsChanged;
        battleManager.OnTurnChanged += HandleTurnChanged;

        // 최초 진입(씬 재진입 등) 시에는 카운트업 없이 즉시 동기화
        attackStat.SetImmediate(diceManager.CurrentTotals.Attack);
        defenseStat.SetImmediate(diceManager.CurrentTotals.Defense);
    }

    void OnDisable()
    {
        diceManager.OnTotalsChanged -= HandleTotalsChanged;
        battleManager.OnTurnChanged -= HandleTurnChanged;
    }

    void HandleTotalsChanged(DiceTotals totals)
    {
        attackStat.AnimateTo(totals.Attack, countUpInterval);
        defenseStat.AnimateTo(totals.Defense, countUpInterval);
    }

    private void HandleTurnChanged(int currentTurn)
    {
        if (turnValueText != null)
        {
            turnValueText.text = currentTurn.ToString();
        }
    }

    // 텍스트 하나에 대한 카운트업 상태를 캡슐화 (공격/방어 각각 독립적인 코루틴을 가짐)
    private class CountUpStat
    {
        private readonly MonoBehaviour owner;
        private readonly TMP_Text text;
        private int displayedValue;
        private Coroutine routine;

        public CountUpStat(MonoBehaviour owner, TMP_Text text)
        {
            this.owner = owner;
            this.text = text;
        }

        public void SetImmediate(int value)
        {
            if (routine != null) owner.StopCoroutine(routine);
            displayedValue = value;
            text.text = displayedValue.ToString();
        }

        public void AnimateTo(int target, float interval)
        {
            if (target <= displayedValue)
            {
                SetImmediate(target); // 감소/동일 → 애니메이션 없이 즉시 반영
                return;
            }

            if (routine != null) owner.StopCoroutine(routine);
            routine = owner.StartCoroutine(CountUpRoutine(target, interval));
        }

        private IEnumerator CountUpRoutine(int target, float interval)
        {
            var wait = new WaitForSeconds(interval);
            while (displayedValue < target)
            {
                displayedValue++;
                text.text = displayedValue.ToString();
                yield return wait;
            }
        }
    }
}
