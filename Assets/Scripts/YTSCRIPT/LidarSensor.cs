using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Lidar센서를 통한 입력 감지기
/// Using Rayrtraced Lidar
/// </summary>
public class LidarSensor : MonoBehaviour
{
    public GameObject ScanAreaPrefab;

    [Header("ScreenResolution")]
    [SerializeField] private float maxWidth;
    [SerializeField] private float maxHeight;

    [Header("ScanArea")]
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Vector3 detectionPosition;

    [SerializeField] private IDetectable sensor;

    public Action <Vector3> OnInputDetected;

    private bool isSetuped;

    private void Awake()
    {
        maxWidth = Screen.width;
        maxHeight = Screen.height;
        minX = float.PositiveInfinity;
        maxX = float.NegativeInfinity;
        minZ = float.PositiveInfinity;
        maxZ = float.NegativeInfinity;
        isSetuped = false;
    }

    private void Start()
    {
        if (sensor == null)
        {
            sensor = GetComponentInChildren<IDetectable>();
        }

        StartCoroutine(C_Initialize(5f));     
    }

    /// <summary>
    /// 입력 감지
    /// 외부장치에서 포지션 값이 들어온다고 가정함
    /// 테스트는 자식 오브젝트 Sensor
    /// </summary>
    private void Update()
    {
        if (isSetuped)
        {
            Vector3 detected = sensor.Scan();
            if (detected != Vector3.zero)
            {
                OnInputDetected?.Invoke(detected);
                Debug.Log(detected.ToString()); 
            }
        }
    }

    /// <summary>
    /// 시작세팅
    /// 5초간 주변을 스캔
    /// 센서의 원점 위치 보정
    /// </summary>
    /// <returns></returns>
    IEnumerator C_Initialize(float TimeSet)
    {
        float elaplsedTime = 0f;
        while (elaplsedTime < TimeSet)
        {
            elaplsedTime += Time.deltaTime;
            Vector3 temp = sensor.Scan();
            if (temp != Vector3.zero)
            {
                minX = Mathf.Min(temp.x, minX);
                maxX = Mathf.Max(temp.x, maxX);
                minZ = Mathf.Min(temp.z, minZ);
                maxZ = Mathf.Max(temp.z, maxZ);
            }
            yield return null;
        }

        width = Mathf.Clamp(maxX - minX, 1, maxWidth);
        height = Mathf.Clamp(maxZ - minZ, 1, maxHeight);

        centerPosition = new Vector3((minX + maxX) / 2f, 0, (minZ + maxZ) / 2f);
        GameObject go = Instantiate(ScanAreaPrefab, centerPosition, Quaternion.identity);
        go.transform.localScale = new Vector3(width, 1, height);
        isSetuped = true;
    }
}
