using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Status")]
    // 상속받을 Boss 클래스에서도 접근할 수 있도록 private 대신 protected를 사용합니다.
    [SerializeField] protected int maxHp = 50;
    [SerializeField] protected int currentHp;

    // 적은 당장 주사위가 없으므로 일단 고정 스탯으로 둡니다.
    [SerializeField] protected int attackPower = 8;
    [SerializeField] protected int defensePower = 2;

    [Header("Components")]
    [SerializeField] protected Animator animator;

    public bool IsDead => currentHp <= 0;

    // virtual 키워드를 붙여두면 자식 클래스(Boss)에서 이 함수를 덮어쓰기(Override) 할 수 있습니다.
    protected virtual void Start()
    {
        currentHp = maxHp;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    /// <summary>
    /// 플레이어를 공격할 때 호출하는 함수
    /// </summary>
    public virtual void Attack(Player target)
    {
        Debug.Log($"[Enemy] 플레이어에게 {attackPower}의 데미지로 기본 공격!");

        if (animator != null) animator.SetTrigger("Attack");

        // 플레이어의 피격 함수 호출
        target.TakeDamage(attackPower);
    }

    /// <summary>
    /// 플레이어에게 공격받을 때 호출되는 함수
    /// </summary>
    public virtual void TakeDamage(int incomingDamage)
    {
        // 방어력을 차감한 실제 데미지 계산 (최소 데미지는 0)
        int actualDamage = Mathf.Max(0, incomingDamage - defensePower);
        currentHp -= actualDamage;

        Debug.Log($"[Enemy] {actualDamage}의 피해를 입음. (남은 체력: {currentHp})");

        if (animator != null) animator.SetTrigger("Hit");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 사망 처리 로직
    /// </summary>
    protected virtual void Die()
    {
        currentHp = 0;
        Debug.Log("[Enemy] 쓰러졌습니다.");

        if (animator != null) animator.SetBool("IsDead", true);

        // TODO: BattleManager에 적 사망 이벤트를 전달하여 턴 종료 및 보상 처리
    }
}