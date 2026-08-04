using UnityEngine;

// 장착한 주사위 슬롯 UI. 슬롯 개수는 EquippedDice.MaxSlots를 따라 동적으로 생성된다
// (DiceInventoryView.Populate()와 동일한 생성 패턴 — 매 갱신마다 자식 전부 파괴 후 다시 생성).
// 채워진 칸은 DiceSlotUI.Setup, 빈 칸은 SetEmpty()로 표시하고, 채워진 슬롯 클릭 시 해당 칸을 해제한다.
public class EquippedDiceView : MonoBehaviour
{
    [SerializeField] private DiceInfoPanel infoPanel;
    [SerializeField] private DiceSlotUI slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private LobbyAudio lobbyAudio;

    void OnEnable()
    {
        infoPanel.OnEquipRequested += HandleEquipRequested;
    }

    void OnDisable()
    {
        infoPanel.OnEquipRequested -= HandleEquipRequested;

        if (EquippedDice.Instance != null)
            EquippedDice.Instance.OnEquippedChanged -= Refresh;
    }

    void Start()
    {
        // Awake/OnEnable 순서가 오브젝트 간 보장되지 않으므로(RerollCountView 사례와 동일 이유),
        // EquippedDice.Instance 접근은 Start에서 수행한다.
        EquippedDice.Instance.OnEquippedChanged += Refresh;
        Refresh();
    }

    void HandleEquipRequested(DiceData data)
    {
        // 실패 이유별 경고 로그는 EquippedDice.TryEquip이 직접 남긴다 (슬롯 초과/보유 개수 초과).
        EquippedDice.Instance.TryEquip(data);
    }

    void Refresh()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        var equippedList = EquippedDice.Instance.Equipped;
        int maxSlots = EquippedDice.Instance.MaxSlots;

        for (int i = 0; i < maxSlots; i++)
        {
            var slot = Instantiate(slotPrefab, slotParent);

            if (i < equippedList.Count)
            {
                slot.Setup(equippedList[i], 1);
                int index = i; // 클로저 캡처 (foreach 변수 재사용 문제 방지)
                slot.OnClicked += _ =>
                {
                    lobbyAudio.PlayUnequipSound();
                    EquippedDice.Instance.UnequipAt(index);
                };
            }
            else
            {
                slot.SetEmpty();
            }
        }
    }
}
