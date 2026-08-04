using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// 이번 게임에 들고 갈 장착 주사위 목록. 슬롯 개수는 Inspector(maxSlots)에서 조절 가능 —
// 기획 변경으로 3개 → N개가 되어도 이 클래스와 EquippedDiceView는 수정할 필요 없다.
public class EquippedDice : MonoBehaviour
{
    public static EquippedDice Instance { get; private set; }

    [SerializeField] private int maxSlots = 3;
    [SerializeField] private List<DiceData> equipped = new();

    public int MaxSlots => maxSlots;
    public IReadOnlyList<DiceData> Equipped => equipped;
    public bool IsFull => equipped.Count >= maxSlots;

    public event Action OnEquippedChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 같은 종류가 이미 몇 개 장착되어 있는지
    public int CountEquipped(DiceData data) => equipped.Count(d => d == data);

    // DiceInventory에 소지한 개수(count)만큼만 장착 가능한지
    public bool CanEquipMore(DiceData data)
    {
        if (data == null || DiceInventory.Instance == null) return false;

        var stack = DiceInventory.Instance.OwnedDice.FirstOrDefault(s => s.data == data);
        int owned = stack?.count ?? 0;

        return CountEquipped(data) < owned;
    }

    // 고정 개수 제한 판정(DiceManager.TryToggleFix)과 동일하게, 이유별로 여기서 직접 경고 로그를 남기고 false를 반환한다.
    public bool TryEquip(DiceData data)
    {
        if (data == null) return false;

        if (IsFull)
        {
            Debug.Log($"장착 슬롯({maxSlots}개)이 가득 찼습니다.");
            return false;
        }

        if (!CanEquipMore(data))
        {
            Debug.Log($"'{data.displayName}'을(를) 보유한 개수만큼 이미 장착했습니다.");
            return false;
        }

        equipped.Add(data);
        OnEquippedChanged?.Invoke();
        return true;
    }

    public void UnequipAt(int index)
    {
        if (index < 0 || index >= equipped.Count) return;

        equipped.RemoveAt(index);
        OnEquippedChanged?.Invoke();
    }
}
