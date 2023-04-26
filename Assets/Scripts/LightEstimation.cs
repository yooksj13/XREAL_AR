using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;

    private new Light light; // component Ŭ������ �̹� light�� ����. new �� ���Ӱ� ����

    private void Awake()
    {
        if (arCameraManager == null)
        {
            Debug.LogError("ARCameraManager not found.");
            Destroy(this);

        }

        light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        arCameraManager.frameReceived += FrameChanged; // ���ο� ī�޶��� ������ ���ŵ� ������ ȣ��Ǵ� �̺�Ʈ �޼ҵ�
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameChanged; // �޼ҵ� ��� ����
    }

    private void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue) // ������ ��� ���
        {
            light.intensity = args.lightEstimation.averageBrightness.Value;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue) // ��� �� �µ� 
        {
            light.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }

        if (args.lightEstimation.colorCorrection.HasValue) // ���� ����(RGBA)
        {
            light.color = args.lightEstimation.colorCorrection.Value;
        }

        if (args.lightEstimation.mainLightDirection.HasValue) // ����� �ֿ� ���� ����
        {
            light.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
        }

        if (args.lightEstimation.mainLightIntensityLumens.HasValue) // ��� ������ �� ���� ����ġ
        {
            light.intensity = args.lightEstimation.averageMainLightBrightness.Value;
        }

        if (args.lightEstimation.ambientSphericalHarmonics.HasValue) // ���� 2���� ���� �����ĸ� ����� �ֺ� ��� ���� ����
        {
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = args.lightEstimation.ambientSphericalHarmonics.Value;
        }
    }
}