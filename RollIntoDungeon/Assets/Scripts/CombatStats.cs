using UnityEngine;
using System;

public class CombatStats : MonoBehaviour
{
    [SerializeField] private int maxHP = 30;
    [SerializeField] private int defense = 0;
    [SerializeField] private int minDamage = 1; // 방어력이 데미지보다 커도 최소 1은 들어가게

    public int CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    public event Action<int> OnDamaged; // 인자: 방어력 적용 후 실제 데미지
    public event Action OnDeath;

    void Awake()
    {
        CurrentHP = maxHP;
    }

    public void TakeDamage(int rawDamage)
    {
        if (IsDead) return;

        int finalDamage = Mathf.Max(rawDamage - defense, minDamage);
        CurrentHP = Mathf.Max(CurrentHP - finalDamage, 0);

        OnDamaged?.Invoke(finalDamage);

        if (IsDead)
            OnDeath?.Invoke();
    }
}
