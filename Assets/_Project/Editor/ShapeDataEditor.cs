using UnityEditor;
using UnityEngine;
using BlockBlast.Data;
[CustomEditor(typeof(ShapeData))]
public sealed class ShapeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();var shape=(ShapeData)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("blockColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationPolicy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("difficulty"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("allowCornerConnections"));
        serializedObject.ApplyModifiedProperties();
        int size=EditorGUILayout.IntSlider("Design grid size",shape.DesignSize,3,5);
        if(size!=shape.DesignSize)
        {
            Undo.RecordObject(shape,"Resize shape design");
            try{shape.ResizeDesign(size);EditorUtility.SetDirty(shape);}
            catch(System.InvalidOperationException error){Debug.LogWarning(error.Message,shape);}
            serializedObject.Update();
        }
        size=shape.DesignSize;
        var m=shape.DesignMatrix();EditorGUILayout.LabelField(size+" x "+size+" — Row 0 at top",EditorStyles.boldLabel);
        for(int r=0;r<size;r++){EditorGUILayout.BeginHorizontal();GUILayout.Label("Row "+r,GUILayout.Width(55));for(int c=0;c<size;c++){
            bool value=GUILayout.Toggle(m[r,c],"",GUILayout.Width(35),GUILayout.Height(30));
            if(value!=m[r,c]){Undo.RecordObject(shape,"Edit shape cell");shape.Migrate();serializedObject.Update();serializedObject.FindProperty("cells").GetArrayElementAtIndex(r*size+c).boolValue=value;serializedObject.ApplyModifiedProperties();}
        }EditorGUILayout.EndHorizontal();}
        serializedObject.ApplyModifiedProperties();
        if(!shape.IsValid)EditorGUILayout.HelpBox("Select connected cells. Edge connections are required unless Allow Corner Connections is enabled. Invalid shapes are excluded from spawning.",MessageType.Warning);
        else EditorGUILayout.LabelField("Allowed orientations: "+shape.Variants().Count);
    }
}
