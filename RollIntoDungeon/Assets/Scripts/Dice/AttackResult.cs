// "이대로 공격하기" 확정 시점의 최종 공격/방어값. 전투 시스템(타 팀원)에 전달하는 데이터 계약.
public readonly struct AttackResult
{
    public readonly int Attack;
    public readonly int Defense;
    public readonly bool IsCritical;

    public AttackResult(int attack, int defense, bool isCritical)
    {
        Attack = attack;
        Defense = defense;
        IsCritical = isCritical;
    }
}
