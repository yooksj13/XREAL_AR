using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackedMultiImageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] trackedPrefabs; // �����ϰ��� �ϴ� �̹����鿡 ���� ���� ������Ʈ �����յ��� ��� �迭
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>(); // �����ϰ� �ִ� �̹����� ����� ���� ������Ʈ���� ��� ��ųʸ�
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>(); // ARTrackedImageManager ������Ʈ�� �����ͼ� �Ҵ�

        // �����ϰ��� �ϴ� �̹����鿡 ���� ���� ������Ʈ�� �����ϰ�, ��Ȱ��ȭ�� �� spawnedObjects ��ųʸ��� �߰�
        foreach (GameObject prefab in trackedPrefabs)
        {
            GameObject clone = Instantiate(prefab);
            clone.name = prefab.name;
            clone.SetActive(false);
            spawnedObjects.Add(clone.name, clone);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged; // ARTrackedImageManager�� ���� �̹��� ���� �̺�Ʈ�� ó���ϴ� �Լ��� �Ҵ�
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged; // ARTrackedImageManager�� ���� �̹��� ���� �̺�Ʈ ó�� �Լ��� ����
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // �̹����� �߰��ǰų� ������Ʈ�� ������ UpdateImage �Լ� ȣ��
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        // �̹����� ������ �� �ش� �̹����� ����� ���� ������Ʈ�� ��Ȱ��ȭ
        foreach (var trackedImage in eventArgs.removed)
        {
            spawnedObjects[trackedImage.name].SetActive(false);
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name; // ���� ���� �̹����� �̸��� ������
        GameObject trackedObject = spawnedObjects[name]; // ���� ���� �̹����� ����� ���� ������Ʈ�� ������

        // ���� ���� �����̸� ���� ������Ʈ�� ��ġ�� ȸ�� ���� ������Ʈ�ϰ� Ȱ��ȭ
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            trackedObject.transform.position = trackedImage.transform.position;
            trackedObject.transform.rotation = trackedImage.transform.rotation;
            trackedObject.SetActive(true);
        }
        // ���� ���� �ƴϸ� ���� ������Ʈ�� ��Ȱ��ȭ
        else
        {
            trackedObject.SetActive(false);
        }
    }
}