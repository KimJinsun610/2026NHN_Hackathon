using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Status")]
    // 기획(인스펙터)에서 조절할 최대 체력. 안전하게 private으로 보호합니다.
    [SerializeField] private int maxHp = 100;

    // 게임 중 체력이 깎이는 것을 에디터에서 눈으로 확인(디버깅)하기 위해 열어둡니다.
    [SerializeField] private int currentHp;

    public bool IsDead => currentHp <= 0;

    // 플레이어의 전투 스탯 변수
    private int currentAttack;
    private int currentDefense;
    private bool IsCriticalAttack = false;

    [Header("Components")]
    // 애니메이터를 인스펙터에서 직접 끌어다(드래그 앤 드롭) 연결할 수 있게 열어둡니다.
    [SerializeField] private Animator animator;


    private Enemy currentTarget;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private DiceRoundController diceController;

    [Header("Sounds")]
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip hitSound;

    // 1. 컴포넌트가 켜질 때 이벤트를 구독(연결)합니다.
    private void OnEnable()
    {
        if (diceController != null)
        {
            diceController.OnAttackConfirmed += ApplyDiceResult;
        }
    }

    // 2. 컴포넌트가 꺼질 때 이벤트를 해제합니다. (메모리 누수 방지용)
    private void OnDisable()
    {
        if (diceController != null)
        {
            diceController.OnAttackConfirmed -= ApplyDiceResult;
        }
    }
    
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
    /// 적을 공격할 때 호출하는 함수
    /// </summary>
    public void Attack(Enemy target)
    {
        Debug.Log($"[Player] 적에게 {currentAttack}의 데미지로 공격!");

        currentTarget = target;
        if (animator != null)
        {
            if(IsCriticalAttack)
                animator.SetTrigger("CriticalAttack");
            else
                animator.SetTrigger("Attack");
            if (SfxManager.Instance != null && attackSound != null)
            {
                SfxManager.Instance.PlaySFX(attackSound);
            }
        }

    }

    public void OnAttackImpact()
    {
        if (currentTarget != null && !currentTarget.IsDead)
        {
            currentTarget.TakeDamage(currentAttack); 
        }
    }

    /// <summary>
    /// 적에게 공격받을 때 호출되는 함수
    /// </summary>
    public void TakeDamage(int incomingDamage)
    {
        // 방어력을 차감한 실제 데미지 계산 (최소 데미지는 0)
        int actualDamage = Mathf.Max(0, incomingDamage - currentDefense);
        currentHp -= actualDamage;
        if (currentHp < 0) currentHp = 0;


        Debug.Log($"[Player] {actualDamage}의 실제 피해를 입음. (남은 체력: {currentHp})");

        // 데미지를 입을 때마다 체력바 업데이트!
        if (healthBar != null) 
        {
            healthBar.UpdateHP(currentHp, maxHp);
        }

        if (animator != null)
        {
            animator.SetTrigger("Hit");
            if (SfxManager.Instance != null && hitSound != null)
            {
                SfxManager.Instance.PlaySFX(hitSound);
            }
        }

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

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
            if (SfxManager.Instance != null && deathSound != null)
            {
                SfxManager.Instance.PlaySFX(deathSound);
            }
        }
    }


    private void ApplyDiceResult(AttackResult result)
    {
        // 넘겨받은 result 값으로 플레이어의 스탯을 갱신합니다.
        currentAttack = result.Attack;
        currentDefense = result.Defense;
        IsCriticalAttack = result.IsCritical;

        Debug.Log($"[Player] 주사위 스탯 장전 완료! 공격력: {currentAttack}, 방어력: {currentDefense}");

        if (result.IsCritical)
        {
            Debug.Log("[Player] 크리티컬 공격 준비!");
        }
        
    }
}