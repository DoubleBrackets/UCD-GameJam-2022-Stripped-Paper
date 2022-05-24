using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SpriteLayerObject))]
public class SpriteLayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();       
        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        var spriteLayer = (SpriteLayerObject)target;

        Vector3 upperPos = Handles.PositionHandle(spriteLayer.transform.position + spriteLayer.upper, Quaternion.identity);
        Handles.Label(upperPos, "Upper");
        //Vector3 lowerPos = Handles.PositionHandle(spriteLayer.transform.position + spriteLayer.lower, Quaternion.identity);
        //Handles.Label(lowerPos, "Lower");
        Vector3 dissolvePos = Handles.PositionHandle(spriteLayer.transform.position + spriteLayer.dissolve, Quaternion.identity);
        Handles.Label(dissolvePos, "Dissolve");
        if (EditorGUI.EndChangeCheck())
        {
            spriteLayer.upper = upperPos - spriteLayer.transform.position;
            //spriteLayer.lower = lowerPos - spriteLayer.transform.position;
            spriteLayer.dissolve = dissolvePos - spriteLayer.transform.position;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
    }
}
