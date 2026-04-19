using System;
using System.Linq;
using Data;
using Managers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RewardManager))]
class RewardHelper : Editor {
    public override void OnInspectorGUI()
    {
        
        RewardManager rewardManager = (RewardManager)target;
        
        DrawDefaultInspector();

        if (!Application.isPlaying)
        {
            return;
        }

        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Reward Action", EditorStyles.boldLabel); 
        EditorGUILayout.Separator();

        if (GUILayout.Button("Generate Reward"))
            rewardManager.GenerateRewards();


        if (rewardManager.HasRewards)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reward", EditorStyles.boldLabel); 
            EditorGUILayout.Separator();
        
            EditorGUILayout.BeginHorizontal();
            
        
            
            EditorGUILayout.EndHorizontal();
        
            foreach (PlayCardData reward in RewardManager.Instance.CurrentRewards)
            {
            
                EditorGUILayout.BeginHorizontal();
            
                GUILayout.Label(reward.GetCardName());
                if (GUILayout.Button("Take"))
                    rewardManager.SelectReward(reward);
            
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Separator();
        
            if (GUILayout.Button("Remove Card"))
                rewardManager.ShowRemoveCard();
        
            if (GUILayout.Button("Skip Reward"))
                rewardManager.SkipReward();
        }

        if (rewardManager.IsRemoveView)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Remove Card", EditorStyles.boldLabel); 
            EditorGUILayout.Separator();
        
            EditorGUILayout.BeginHorizontal();
            
            
            EditorGUILayout.EndHorizontal();
            PlayerCards playerCards = PlayerManager.Instance.Player.PlayerCards;
            
            foreach (PlayCardData card in PlayerManager.Instance.Player.PlayerCards.GetDeckList.cards.ToList())
            { 
                EditorGUILayout.BeginHorizontal();
            
                GUILayout.Label(card.GetCardName());
                if (GUILayout.Button("Take"))
                    playerCards.RemoveFromDeck(card);
                EditorGUILayout.EndHorizontal();
            }
        }
        
    }
    
}