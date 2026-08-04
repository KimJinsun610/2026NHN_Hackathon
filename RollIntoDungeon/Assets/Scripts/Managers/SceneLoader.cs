using UnityEngine.SceneManagement;

// LoadingScene을 거쳐 목표 씬으로 이동할 때 쓰는 정적 헬퍼.
// "어디서 로딩 화면으로 진입했는지"와 "로딩 화면이 실제로 무엇을 로드하는지"를 분리해서,
// 나중에 Lobby -> Play 전환에도 동일한 로딩 흐름을 재사용할 수 있게 한다.
public static class SceneLoader
{
    private const string LoadingSceneName = "LoadingScene";

    public static string TargetScene { get; private set; }

    public static void LoadWithLoading(string targetSceneName)
    {
        TargetScene = targetSceneName;
        SceneManager.LoadScene(LoadingSceneName);
    }
}
