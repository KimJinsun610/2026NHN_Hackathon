using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// MainScene(시작 화면) 진행. 화면 아무 곳 클릭 시 클릭 사운드 재생 + 화면/BGM 페이드아웃 후 로딩 화면으로 이동.
[RequireComponent(typeof(AudioSource))]
public class MainSceneController : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup; // 화면 전체를 덮는 검은 Image에 부착, 시작 alpha = 0
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string nextSceneName = "LobbyScene";

    private AudioSource audioSource;
    private bool isTransitioning;

    void Awake() => audioSource = GetComponent<AudioSource>();

    void Update()
    {
        if (isTransitioning) return;

        bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool keyPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;

        if (mouseClicked || keyPressed)
            StartTransition();
    }

    void StartTransition()
    {
        isTransitioning = true;
        if (clickSound != null) audioSource.PlayOneShot(clickSound);
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        float startVolume = BgmManager.Instance != null ? BgmManager.Instance.Volume : 1f;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float ratio = t / fadeDuration;
            fadeCanvasGroup.alpha = ratio;
            BgmManager.Instance?.SetVolume(startVolume * (1f - ratio));
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
        SceneLoader.LoadWithLoading(nextSceneName);
    }
}
