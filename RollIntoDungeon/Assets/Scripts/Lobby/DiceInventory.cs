using UnityEngine;
using System;
using System.Collections.Generic;

// 플레이어가 소지한 주사위 목록. 같은 종류를 여러 개 소지할 수 있어 (데이터, 개수) 묶음으로 관리한다.
// 씬 전환에도 유지된다 (BgmManager와 동일 패턴).
public class DiceInventory : MonoBehaviour
{
    [Serializable]
    public class DiceStack
    {
        public DiceData data;
        public int count = 1;
    }

    public static DiceInventory Instance { get; private set; }

    [SerializeField] private List<DiceStack> ownedDice = new();

    public IReadOnlyList<DiceStack> OwnedDice => ownedDice;

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
}
