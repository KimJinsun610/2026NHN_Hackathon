using UnityEngine;
using TMPro;

// DiceRoundController.OnRerollsRemainingChanged를 구독해 남은 리롤 횟수를 표시한다.
public class RerollCountView : MonoBehaviour
{
    [SerializeField] private DiceRoundController roundController;
    [SerializeField] private TMP_Text rerollCountText;

    void OnEnable()
    {
        roundController.OnRerollsRemainingChanged += HandleRerollsRemainingChanged;
    }

    void OnDisable()
    {
        roundController.OnRerollsRemainingChanged -= HandleRerollsRemainingChanged;
    }

    // OnEnable 시점엔 다른 오브젝트의 DiceRoundController.Awake()가 아직 안 돌았을 수 있어
    // (Awake→OnEnable 순서는 오브젝트 간 보장되지 않음) 씬 전체 Awake가 끝난 뒤 실행되는 Start에서 최초 동기화한다.
    void Start()
    {
        HandleRerollsRemainingChanged(roundController.RerollsRemaining);
    }

    void HandleRerollsRemainingChanged(int remaining)
    {
        rerollCountText.text = $"{remaining}/{roundController.MaxRerollCount}";
    }
}
