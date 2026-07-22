using UnityEngine;

[RequireComponent(typeof(CombatStats))]
[RequireComponent(typeof(Animator))]
public class EnemyView : MonoBehaviour
{
    [SerializeField] private float deathDestroyDelay = 1.5f; // Death 애니메이션 재생 시간에 맞춰 조절

    private CombatStats stats;
    private Animator animator;

    static readonly int HitTrigger = Animator.StringToHash("Hit");
    static readonly int DeathTrigger = Animator.StringToHash("Death");

    void Awake()
    {
        stats = GetComponent<CombatStats>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        stats.OnDamaged += HandleDamaged;
        stats.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        stats.OnDamaged -= HandleDamaged;
        stats.OnDeath -= HandleDeath;
    }

    void HandleDamaged(int finalDamage)
    {
        Debug.Log($"{name} took {finalDamage} damage. HP: {stats.CurrentHP}");

        if (stats.IsDead) return; // 죽는 타격이면 Hit 대신 Death만 재생

        animator.SetTrigger(HitTrigger);
    }

    void HandleDeath()
    {
        animator.SetTrigger(DeathTrigger);

        if (TryGetComponent<Collider2D>(out var col))
            col.enabled = false;

        Destroy(gameObject, deathDestroyDelay);
    }
}
