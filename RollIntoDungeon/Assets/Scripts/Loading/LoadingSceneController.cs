using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// LoadingScene 진행. SceneLoader.TargetScene을 비동기로 로드한다.
public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private string fallbackScene = "LobbyScene"; // LoadingScene에 직접 진입한 경우(테스트 등) 대비
    [SerializeField] private float minShowTime = 0.5f; // 로딩이 즉시 끝나 화면이 깜빡이듯 지나가는 것 방지

    void Start() => StartCoroutine(LoadRoutine());

    IEnumerator LoadRoutine()
    {
        string target = string.IsNullOrEmpty(SceneLoader.TargetScene) ? fallbackScene : SceneLoader.TargetScene;
        float startTime = Time.time;

        AsyncOperation op = SceneManager.LoadSceneAsync(target);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) yield return null;

        float elapsed = Time.time - startTime;
        if (elapsed < minShowTime) yield return new WaitForSeconds(minShowTime - elapsed);

        op.allowSceneActivation = true;
    }
}
