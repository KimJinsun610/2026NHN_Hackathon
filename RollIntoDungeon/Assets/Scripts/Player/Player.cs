using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Status")]
    // 기획(인스펙터)에서 조절할 최대 체력. 안전하게 private으로 보호합니다.
    [SerializeField] private int maxHp = 100;

    // 게임 중 체력이 깎이는 것을 에디터에서 눈으로 확인(디버깅)하기 위해 열어둡니다.
    [SerializeField] private int currentHp;

    public bool IsDead => currentHp <= 0;

    // 턴마다 갱신되는 스탯. (프로퍼티는 기본적으로 인스펙터에 노출되지 않으며, 외부에서는 읽기만 가능합니다.)
    public int currentAtk { get; private set; }
    public int currentDef { get; private set; }

    [Header("Components")]
    // 애니메이터를 인스펙터에서 직접 끌어다(드래그 앤 드롭) 연결할 수 있게 열어둡니다.
    [SerializeField] private Animator animator;


    private Enemy currentTarget;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        // 게임 시작 시 체력 초기화
        currentHp = maxHp;

        // 만약 인스펙터에서 컴포넌트를 실수로 안 넣었다면 코드로 알아서 찾아오도록 안전장치를 둡니다.
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // 체력바 초기화
        if (healthBar != null) 
        {
            healthBar.UpdateHP(currentHp, maxHp);
        }
    }

    /// <summary>
    /// 턴 시작 시 호출되어 이번 턴의 공격력과 방어력을 세팅합니다.
    /// </summary>
    public void CalculateTurnStats()
    {
        // 🚧 [더미 데이터 구간] 주사위 로직 완성 전까지 고정값 사용
        currentAtk = 10;
        currentDef = 5;

        Debug.Log($"[Player] 턴 스탯 갱신 - 공격력: {currentAtk}, 방어력: {currentDef}");
    }

    /// <summary>
    /// 적을 공격할 때 호출하는 함수
    /// </summary>
    public void Attack(Enemy target)
    {
        Debug.Log($"[Player] 적에게 {currentAtk}의 데미지로 공격!");

        currentTarget = target;
        if (animator != null) animator.SetTrigger("Attack");

    }

    public void OnAttackImpact()
    {
        if (currentTarget != null && !currentTarget.IsDead)
        {
            currentTarget.TakeDamage(currentAtk); 
        }
    }

    /// <summary>
    /// 적에게 공격받을 때 호출되는 함수
    /// </summary>
    public void TakeDamage(int incomingDamage)
    {
        // 방어력을 차감한 실제 데미지 계산 (최소 데미지는 0)
        int actualDamage = Mathf.Max(0, incomingDamage - currentDef);
        currentHp -= actualDamage;
        if (currentHp < 0) currentHp = 0;


        Debug.Log($"[Player] {actualDamage}의 실제 피해를 입음. (남은 체력: {currentHp})");

        // 데미지를 입을 때마다 체력바 업데이트!
        if (healthBar != null) 
        {
            healthBar.UpdateHP(currentHp, maxHp);
        }

        if (animator != null) animator.SetTrigger("Hit");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 사망 처리 로직
    /// </summary>
    private void Die()
    {
        Debug.Log("[Player] 사망했습니다.");

        if (animator != null) animator.SetBool("IsDead", true);
    }
}