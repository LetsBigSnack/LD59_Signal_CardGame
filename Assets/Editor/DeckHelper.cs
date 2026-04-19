using System;
using System.Linq;
using Data;
using Managers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
class DecalMeshHelperEditor : Editor {
    public override void OnInspectorGUI()
    {
        
        GameManager gameManager = (GameManager)target;
        
        DrawDefaultInspector();

        if (!Application.isPlaying)
        {
            return;
        }

        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Game Action", EditorStyles.boldLabel); 
        EditorGUILayout.Separator();
        
        if(GUILayout.Button("Start Game"))
            gameManager.StartGame();

        if (GUILayout.Button("Handle Current State"))
            gameManager.HandleCurrentState();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Player Set Cards", EditorStyles.boldLabel); 
        EditorGUILayout.Separator();
        
        foreach (GameSlot slot in gameManager.GameSlots.ToList())
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(slot.PlayerSide.ToString());
            GUILayout.Label(slot.SlotPosition.ToString());
            GUILayout.Label(slot.PlayCardData?.GetCardName());
            
            if (GUILayout.Button("Unset"))
                gameManager.UnsetCardToSlot(slot.PlayCardData, slot.PlayerSide, slot.SlotPosition);
            
            EditorGUILayout.EndHorizontal();
        }
        
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Player Hand", EditorStyles.boldLabel); 
        EditorGUILayout.Separator();
        foreach (PlayCardData handCard in PlayerManager.Instance.GetPlayer().PlayerCards.Hand.ToList())
        {
            
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label(handCard.GetCardName());
            
            if (GUILayout.Button("Slot 1"))
                gameManager.SetCardToSlot(handCard, PlayerSide.Player, SlotPosition.Position1);
            if (GUILayout.Button("Slot 2"))
                gameManager.SetCardToSlot(handCard, PlayerSide.Player, SlotPosition.Position2);
            if (GUILayout.Button("Slot 3"))
                gameManager.SetCardToSlot(handCard, PlayerSide.Player, SlotPosition.Position3);
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Enemy Hand", EditorStyles.boldLabel); 
        EditorGUILayout.Separator();
        foreach (PlayCardData handCard in EnemyManager.Instance.GetCurrentEnemy().PlayerCards.Hand.ToList())
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(handCard.GetCardName());
            
            if (GUILayout.Button("Slot 1"))
                gameManager.SetCardToSlot(handCard, PlayerSide.Enemy, SlotPosition.Position1);
            if (GUILayout.Button("Slot 2"))
                gameManager.SetCardToSlot(handCard, PlayerSide.Enemy, SlotPosition.Position2);
            if (GUILayout.Button("Slot 3"))
                gameManager.SetCardToSlot(handCard, PlayerSide.Enemy, SlotPosition.Position3);
            EditorGUILayout.EndHorizontal();
        }
        
    }
    
}