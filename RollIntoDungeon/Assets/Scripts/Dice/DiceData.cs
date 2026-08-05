using UnityEngine;
using System;

// 주사위 "종류"를 정의하는 데이터. 로비의 소지/장착 목록, 선택 정보 패널은 이 데이터를 기준으로 표시된다.
[CreateAssetMenu(fileName = "DiceData", menuName = "RollIntoDungeon/Dice Data")]
public class DiceData : ScriptableObject
{
    [Serializable]
    public struct FaceInfo
    {
        public Sprite image; // 공격/방어 수치를 나타내는 이미지 (텍스트 대신 이미지로 표시)
        public DiceFaceReader.DiceFaceType faceType;
        public int value;
    }

    public string diceId;
    public string displayName;
    public Sprite icon;
    public GameObject worldPrefab;   // PlayScene에서 실제로 굴릴 프리팹 (AttackOnlyDice 등)
    public GameObject previewPrefab; // 정보 패널에서 돌려보는 경량 모델 (물리/게임 로직 컴포넌트 없이 메쉬만)
    [TextArea] public string description;
    public FaceInfo[] faces = new FaceInfo[6];
}
