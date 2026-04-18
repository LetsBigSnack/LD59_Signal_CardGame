using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSpawnManager : MonoBehaviour
{
    public static PopupSpawnManager Instance;

    public List<RectTransform> spawnAreas;

    [Header("Canvas")]
    public Canvas targetCanvas;

    [Header("PopupPrefab")]
    public GameObject spawnPrefab;

    public void Awake()
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

    public void Start()
    {
        SpawnPopupOnScreen();
    }

    public Vector2 RandomPointInRect(RectTransform rec)
    {
        return new Vector2(
            Random.Range(rec.rect.xMin, rec.rect.xMax),
            Random.Range(rec.rect.yMin, rec.rect.yMax)
        );
    }

    public void SpawnPopupOnScreen()
    {
        RectTransform randomRect = GetRandomSpawnArea();
        Vector2 randomPoint = RandomPointInRect(randomRect);

        bool isOverlapping = IsOverLapping(randomRect, randomPoint, spawnPrefab);

        while (!isOverlapping)
        {
            randomPoint = RandomPointInRect(randomRect);
            isOverlapping = IsOverLapping(randomRect, randomPoint, spawnPrefab);
        }

        if (isOverlapping)
        {
            Vector3 worldPoint = randomRect.TransformPoint(randomPoint);
            Instantiate(spawnPrefab, worldPoint, Quaternion.identity, targetCanvas.transform);
        }
    }


    public RectTransform GetRandomSpawnArea()
    {
        int randomInt = Random.Range(0, spawnAreas.Count);
        return spawnAreas[randomInt];
    }

    public bool IsOverLapping(RectTransform randomSpawnArea, Vector2 randomPoint, GameObject gameObject)
    {
        RectTransform prefabRect = gameObject.GetComponentInChildren<RectTransform>();

        float halfW = prefabRect.rect.width / 2f;
        float halfH = prefabRect.rect.height / 2f;

        Rect area = randomSpawnArea.rect;

        return randomPoint.x - halfW > area.xMin &&
               randomPoint.x + halfW < area.xMax &&
               randomPoint.y - halfH > area.yMin &&
               randomPoint.y + halfH < area.yMax;
    }


}
