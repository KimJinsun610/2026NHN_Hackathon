using UnityEngine;

public class EnemyVisualEventBridge : MonoBehaviour
{
    [Header("본체 연결")]
    [SerializeField] private Enemy enemyBody; 

    public void TriggerAttackImpact()
    {
        if (enemyBody != null)
        {
            enemyBody.OnAttackImpact();
        }
    }

    public void TriggerDeathComplete()
    {
        if (enemyBody != null)
        {
            enemyBody.OnDeathAnimationComplete();
        }
    }
}
