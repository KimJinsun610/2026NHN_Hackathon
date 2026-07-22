using UnityEngine;

public class PlayerVisualEventBridge : MonoBehaviour
{
    [Header("본체 연결")]
    [SerializeField] private Player playerBody; // 부모에 있는 Player 스크립트

    // 유니티 애니메이션 창에서 이 함수를 선택하게 될 겁니다.
    public void TriggerAttackImpact()
    {
        if (playerBody != null)
        {
            // 이 함수가 실행되면, 부모(Player)에게 "지금 때려!" 라고 전달합니다.
            playerBody.OnAttackImpact();
        }
    }
}
