using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
class DecalMeshHelperEditor : Editor {
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        
        EditorGUILayout.Space();
        
        if(GUILayout.Button("Draw a card"))
            target.GameObject().GetComponent<GameManager>().DrawCard();
            

        if (GUILayout.Button("Play a card"))
            target.GameObject().GetComponent<GameManager>().PlayCard();
        
        
    }
}