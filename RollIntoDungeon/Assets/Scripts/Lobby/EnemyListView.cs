using UnityEngine;
using System;
using System.Collections.Generic;

// stageData.enemiesToSpawn을 슬롯으로 나열한다. 동일한 적(같은 프리팹)은 한 번만 표시한다.
public class EnemyListView : MonoBehaviour
{
    [SerializeField] private StageData stageData;
    [SerializeField] private EnemySlotUI slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private LobbyAudio lobbyAudio;

    // 던전 정보창(다음 항목)이 이 이벤트를 구독하면 된다.
    public event Action<Enemy> OnEnemySlotClicked;

    private EnemySlotUI selectedSlot;

    void Start()
    {
        Populate();
    }

    void Populate()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        selectedSlot = null;

        if (stageData == null || stageData.enemiesToSpawn == null) return;

        var seen = new HashSet<Enemy>();

        foreach (var spawnInfo in stageData.enemiesToSpawn)
        {
            Enemy enemy = spawnInfo.enemyPrefab;
            if (enemy == null || !seen.Add(enemy)) continue; // 동일한 적은 한 번만 표시

            var slot = Instantiate(slotPrefab, slotParent);
            slot.Setup(enemy);
            slot.OnClicked += _ => HandleSlotClicked(slot);
        }
    }

    void HandleSlotClicked(EnemySlotUI slot)
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        lobbyAudio.PlaySelectSound();
        OnEnemySlotClicked?.Invoke(slot.Data);
    }
}
