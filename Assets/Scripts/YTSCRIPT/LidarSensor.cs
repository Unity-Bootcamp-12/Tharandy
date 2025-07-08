using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Lidar센서를 통한 입력 감지기
/// Using Rayrtraced Lidar
/// </summary>
public class LidarSensor : MonoBehaviour
{
    [SerializeField] private float width;
    [SerializeField] private float maxWidth;
    [SerializeField] private float height;
    [SerializeField] private float maxheight;

    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Vector3 detectionPosition;

    [SerializeField] private Sensor sensor;

    public Action <Vector3> OnInputDetected;


    private void Awake()
    {
        maxWidth = Screen.width;
        maxheight = Screen.height;
    }

    private void Start()
    {
        if (sensor == null)
        {
            sensor = GetComponentInChildren<Sensor>();
        }

        StartCoroutine(C_Initialize());     
    }

    /// <summary>
    /// 입력 감지
    /// 외부장치에서 포지션 값이 들어온다고 가정함
    /// 테스트는 자식 오브젝트 Sensor
    /// </summary>
    private void Update()
    {
        if (sensor != null)
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
    IEnumerator C_Initialize()
    {
        yield return null;
    }
}
