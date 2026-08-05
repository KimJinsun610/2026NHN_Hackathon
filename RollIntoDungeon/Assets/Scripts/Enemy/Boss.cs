using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class Boss : Enemy
{
    [Header("Boss Pattern Settings")]
    [SerializeField] private Enemy minionPrefab;
    [SerializeField] private Transform[] minionSpawnPoints;

    private bool hasSummonedMinions = false;
    private BattleManager battleManager;

    protected override void Start()
    {
        base.Start();

        battleManager = FindAnyObjectByType<BattleManager>();
    }

    public override void Attack(Player target)
    {
        if (!hasSummonedMinions && (currentHp <= maxHp / 2f))
        {
            SummonMinions();
            return; 
        }

        base.Attack(target);
    }

    private void SummonMinions()
    {
        hasSummonedMinions = true;
        Debug.Log($"[Boss] 체력이 절반 이하로 떨어져 분노합니다! 부하 소환!");

        // 보스의 소환 애니메이션 트리거가 있다면 여기서 실행
        if (animator != null) animator.SetTrigger("Summon");


        foreach (Transform spawnPoint in minionSpawnPoints)
        {

            if (EffectManager.Instance != null)
            {
                EffectManager.Instance.PlayDefaultSpawnEffect(spawnPoint.position);
            }

            Enemy minion = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);

            if (battleManager != null)
            {
                battleManager.AddSpawnedEnemy(minion);
            }
        }
    }
}