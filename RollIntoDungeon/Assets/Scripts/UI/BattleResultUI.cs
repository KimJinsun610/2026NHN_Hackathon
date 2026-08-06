using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class BattleResultUI : MonoBehaviour
{
    public static BattleResultUI Instance;

    [Header("결과창 패널")]
    [SerializeField] private GameObject clearPopup; // 승리 시 띄울 이미지(패널)
    [SerializeField] private GameObject overPopup;  // 패배 시 띄울 이미지(패널)

    [Header("이동할 씬 이름")]
    [SerializeField] private string mainSceneName = "MainScene";

    [Header("애니메이션 설정")]
    [SerializeField] private float slideDuration = 0.5f; // 올라오는 데 걸리는 시간 (0.5초)
    [SerializeField] private Vector2 startPosition = new Vector2(0, -1500f); // 시작 위치 (화면 완전 아래쪽)
    [SerializeField] private Vector2 endPosition = Vector2.zero; // 도착 위치 (화면 정중앙)

    private bool isResultActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (clearPopup != null) clearPopup.SetActive(false);
        if (overPopup != null) overPopup.SetActive(false);
    }

    // 승리 팝업 띄우기
    public void ShowClearPopup()
    {
        if (clearPopup != null)
        {
            clearPopup.SetActive(true);
            StartCoroutine(SlideUpRoutine(clearPopup.GetComponent<RectTransform>()));
        }
    }

    // 패배 팝업 띄우기
    public void ShowOverPopup()
    {
        if (overPopup != null)
        {
            overPopup.SetActive(true);
            StartCoroutine(SlideUpRoutine(overPopup.GetComponent<RectTransform>()));
        }
    }

    private IEnumerator SlideUpRoutine(RectTransform targetRect)
    {
        if (targetRect == null) yield break;

        float elapsedTime = 0f;
        targetRect.anchoredPosition = startPosition; 

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / slideDuration;

            // Ease-Out 효과
            float smoothT = 1f - Mathf.Pow(1f - t, 3f);

            // 시작점과 끝점 사이를 부드럽게 이동
            targetRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, smoothT);

            yield return null; // 다음 프레임까지 대기
        }

        // 오차 방지를 위해 최종 위치에 정확하게 고정
        targetRect.anchoredPosition = endPosition;

        // 애니메이션이 완전히 끝난 후에야 아무 키나 눌러서 씬 이동이 가능하도록 활성화!
        isResultActive = true;
    }

    private void Update()
    {
        if (!isResultActive) return;

        bool anyKeyPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
        bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        // 아무 키나 마우스 클릭이 감지되면 메인 씬으로 이동
        if (anyKeyPressed || mouseClicked)
        {
            ReturnToMainScene();
        }
    }

    private void ReturnToMainScene()
    {
        // 씬을 로드
        SceneManager.LoadScene(mainSceneName);
    }
}