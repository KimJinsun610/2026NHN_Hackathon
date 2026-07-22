using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHitTester : MonoBehaviour
{
    [SerializeField] private CombatStats target;
    [SerializeField] private int testDamage = 10;

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            target.TakeDamage(testDamage);
        }
    }
}
