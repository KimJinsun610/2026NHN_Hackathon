using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [Header("공통 이펙트 모음")]
    public GameObject defaultSpawnEffect; 
    // public GameObject playerHitEffect; // 나중에 타격 이펙트 등도 여기에 추가

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 외부에서 프리팹과 위치를 넘겨주면 대신 생성해주는 범용 함수
    /// </summary>
    public void PlayEffect(GameObject effectPrefab, Vector3 position)
    {
        if (effectPrefab != null)
        {
            Quaternion cameraRot = Camera.main.transform.rotation;
            Instantiate(effectPrefab, position, cameraRot);
        }
    }

    /// <summary>
    /// 기본 소환 이펙트를 발생시키는 전용 함수
    /// </summary>
    public void PlayDefaultSpawnEffect(Vector3 position)
    {
        if (defaultSpawnEffect != null)
        {
            Quaternion cameraRot = Camera.main.transform.rotation;
            Instantiate(defaultSpawnEffect, position, cameraRot);
        }
    }
}