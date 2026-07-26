using UnityEngine;
using UnityEngine.InputSystem;

// 마우스 클릭으로 3D 씬의 주사위를 선택(고정 토글)한다.
public class DiceSelector : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private LayerMask diceLayer;
    [SerializeField] private float maxRayDistance = 100f;
    [SerializeField] private DiceManager diceManager;

    void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Ray ray = targetCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, diceLayer))
        {
            if (hit.collider.TryGetComponent<Dice>(out var dice))
                diceManager.TryToggleFix(dice);
        }
    }
}
