using UnityEngine;

// 주사위 "종류"를 정의하는 데이터. 로비의 소지/장착 목록은 이 데이터를 기준으로 표시된다.
[CreateAssetMenu(fileName = "DiceData", menuName = "RollIntoDungeon/Dice Data")]
public class DiceData : ScriptableObject
{
    public string diceId;
    public string displayName;
    public Sprite icon;
    public GameObject worldPrefab; // PlayScene에서 실제로 굴릴 프리팹 (AttackOnlyDice 등)
    [TextArea] public string description;
}
