using UnityEngine;

// PlayScene 진입 시 로비에서 장착한 주사위(EquippedDice)를 실제로 스폰해서 DiceManager에 등록한다.
public class DiceSpawner : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private Transform[] dropPoints;

    void Start()
    {
        if (EquippedDice.Instance == null) return;

        var equipped = EquippedDice.Instance.Equipped;

        for (int i = 0; i < equipped.Count && i < dropPoints.Length; i++)
        {
            DiceData data = equipped[i];
            if (data == null || data.worldPrefab == null) continue;

            GameObject instance = Instantiate(data.worldPrefab, dropPoints[i].position, dropPoints[i].rotation);

            Dice dice = instance.GetComponent<Dice>();
            DiceDrop drop = instance.GetComponent<DiceDrop>();
            if (dice == null || drop == null) continue;

            drop.SetDropPoint(dropPoints[i]);
            diceManager.RegisterDice(dice);
        }
    }
}
