using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

// 여러 Dice를 묶어서 굴리기/라운드 리셋을 오케스트레이션한다.
public class DiceManager : MonoBehaviour
{
    [SerializeField] private List<Dice> dices = new();
    [SerializeField] private bool rollOnRKeyForTest = true; // 디버깅용, 실제 흐름 연결 후 꺼도 됨

    void Update()
    {
        if (rollOnRKeyForTest && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            RollUnfixed();
        }
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
}
