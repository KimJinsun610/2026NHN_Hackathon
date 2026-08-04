using UnityEngine;

// 씬마다 하나씩 배치. 이 씬에 진입하면 지정된 곡으로 BgmManager를 전환시킨다.
public class SceneBgmTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;

    void Start()
    {
        BgmManager.Instance.SetVolume(1f); // 이전 씬(MainScene 등)에서 페이드아웃한 볼륨이 남아있지 않도록 새 씬 진입 시 항상 복귀
        BgmManager.Instance.PlayBgm(bgmClip);
    }
}
