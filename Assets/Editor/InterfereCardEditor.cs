using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Data;
using ScriptableObjects.BasicCards;
using ScriptableObjects.InterfereEffects;

[CustomEditor(typeof(InterfereCardCreator))]
public class InterfereCardCreatorEditor : Editor
{
    public Sprite prio1;
    public Sprite prio2;
    public Sprite prio3;
    public Sprite prio4;

    private Dictionary<int, Sprite> prioSprites = new Dictionary<int, Sprite>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Create Interfere Card Assets"))
        {
            CreateAssets((InterfereCardCreator)target);
        }
    }

    private void CreateAssets(InterfereCardCreator creator)
    {
        prioSprites.Add(1, prio1);
        prioSprites.Add(2, prio2);
        prioSprites.Add(3, prio3);
        prioSprites.Add(4, prio4);

        string folderPath = "Assets/ScriptableObjects/Cards/Interfere";
        EnsureFolderExists("Assets", "ScriptableObjects");
        EnsureFolderExists("Assets/ScriptableObjects", "Cards");
        EnsureFolderExists("Assets/ScriptableObjects/Cards", "Interfere");

        if (creator.setEffects.Count == 0 || creator.respondEffects.Count == 0)
        {
            Debug.LogWarning("Both lists need at least one effect.");
            return;
        }

        int createdCount = 0;

        foreach (InterfereEffect setEffect in creator.setEffects)
        {
            foreach (InterfereEffect respondEffect in creator.respondEffects)
            {
                Debug.Log("TEST");
                
                if (setEffect == null || respondEffect == null)
                    continue;
                
                InterfereCard asset = ScriptableObject.CreateInstance<InterfereCard>();

                 asset.SetEffect = setEffect;
                asset.RespondEffect = respondEffect;
                asset.cardType = CardType.Interfere;
                
                asset.cardDescription = "Interfere Card in the Opponents slot";
                asset.CardRarity = CardRarity.Legendary;
                //TODO: add Sprite
                
                int comboScore = setEffect.tier * respondEffect.tier;
                Debug.Log(comboScore);
                
                switch (comboScore)
                {
                    case >= 1 and <= 2:
                        asset.priority = 4;
                       
                        break;
                    case >= 3 and <= 4:
                        asset.priority = 3;
                        
                        break;
                    case >= 5 and <= 6:
                        asset.priority = 2;
                    
                        break;
                    case >= 7:
                        asset.priority = 1;
                       
                        break;
                }
                asset.cardName = "Interfere " + asset.priority;
                asset.SetCardSprite(prioSprites[asset.priority]);
                string fileName = $"Interfere_{setEffect.name}_{respondEffect.name}.asset";
                string fullPath = Path.Combine(folderPath, fileName);
                string uniquePath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

                AssetDatabase.CreateAsset(asset, uniquePath);
                createdCount++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created {createdCount} InterfereCard assets in: {folderPath}");
    }

    private void EnsureFolderExists(string parent, string folderName)
    {
        string fullPath = $"{parent}/{folderName}";
        if (!AssetDatabase.IsValidFolder(fullPath))
        {
            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}