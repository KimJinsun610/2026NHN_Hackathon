// DiceManager가 계산한 공격/방어 합산값. UI와 전투 시스템이 공유하는 데이터 계약.
public readonly struct DiceTotals
{
    public readonly int Attack;
    public readonly int Defense;

    public DiceTotals(int attack, int defense)
    {
        Attack = attack;
        Defense = defense;
    }
}
