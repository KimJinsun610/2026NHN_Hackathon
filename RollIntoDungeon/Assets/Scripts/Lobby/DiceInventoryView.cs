using UnityEngine;
using System;

// DiceInventory.OwnedDice 목록을 소유한 종류 수만큼만 슬롯으로 생성한다 (빈 슬롯 없음, 세로 스크롤로 더 보기).
public class DiceInventoryView : MonoBehaviour
{
    [SerializeField] private DiceSlotUI slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private LobbyAudio lobbyAudio;

    // 다음 단계(선택한 주사위 정보 패널)가 이 이벤트를 구독하면 된다.
    public event Action<DiceData> OnDiceSlotClicked;

    private DiceSlotUI selectedSlot;

    void Start()
    {
        Populate();
    }

    void Populate()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        selectedSlot = null;

        if (DiceInventory.Instance == null) return;

        foreach (var stack in DiceInventory.Instance.OwnedDice)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            slot.Setup(stack.data, stack.count);
            slot.OnClicked += _ => HandleSlotClicked(slot);
        }
    }

    void HandleSlotClicked(DiceSlotUI slot)
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        lobbyAudio.PlaySelectSound();
        OnDiceSlotClicked?.Invoke(slot.Data);
    }
}
