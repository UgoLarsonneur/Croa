using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor {

    private int showcaseQuality = 32;
    private static float showcaseJumpCharge = 1f;
    private static bool showShowCase = false;
    private Vector3[] showcasePoints;

    SerializedProperty jumpChargeDuration;
    SerializedProperty minJumpDuration;
    SerializedProperty maxJumpDuration;
    SerializedProperty minJumpDist;
    SerializedProperty maxJumpDist;
    SerializedProperty maxJumpHeight;
    SerializedProperty jumpShape;
    SerializedProperty chargeMaxHeight;

    private void OnEnable() {
        generateJumpShowcase();

        jumpChargeDuration = serializedObject.FindProperty("jumpChargeDuration");
        minJumpDuration = serializedObject.FindProperty("minJumpDuration");
        maxJumpDuration = serializedObject.FindProperty("maxJumpDuration");
        minJumpDist = serializedObject.FindProperty("minJumpDist");
        maxJumpDist = serializedObject.FindProperty("maxJumpDist");
        maxJumpHeight = serializedObject.FindProperty("maxJumpHeight");
        jumpShape = serializedObject.FindProperty("jumpShape");
        chargeMaxHeight = serializedObject.FindProperty("chargeMaxHeight");
    }

    private void OnSceneGUI() {
        Player p = (Player)target;

        if(showShowCase)
        {
            Handles.color = Color.yellow;
            Vector3[] playerPositioned = showcasePoints;
            for (int i = 0; i < playerPositioned.Length; i++)
            {
                playerPositioned[i] += p.transform.position;
            }

            Handles.DrawPolyLine(playerPositioned);
        }
        
    }

    public override void OnInspectorGUI() {
        Player p = (Player)target;

        //DrawDefaultInspector();

        EditorGUILayout.PropertyField(jumpChargeDuration);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(minJumpDuration);
        EditorGUILayout.PropertyField(maxJumpDuration);
        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(minJumpDist);
        EditorGUILayout.PropertyField(maxJumpDist);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(maxJumpHeight);
        EditorGUILayout.PropertyField(jumpShape);
        EditorGUILayout.PropertyField(chargeMaxHeight);

        EditorGUILayout.Space();
        if(showShowCase = EditorGUILayout.Toggle("Show jump arc", showShowCase))
        {
            showcaseJumpCharge = EditorGUILayout.Slider("Jump charge", showcaseJumpCharge, 0f, 1f);
        }
        EditorGUILayout.Space();

        if(EditorGUI.EndChangeCheck())
        {
            generateJumpShowcase();
            SceneView.RepaintAll();
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    private void generateJumpShowcase()
    {
        Player p = (Player)target;

        showcasePoints = new Vector3[showcaseQuality];
        for (int i = 0; i < showcaseQuality; i++)
        {
            float time = (float)i/(showcaseQuality-1);
            showcasePoints[i] = new Vector3(
                0f,
                p.JumpShape.Evaluate(time) * p.ChargeMaxHeight.Evaluate(showcaseJumpCharge) * p.MaxJumpHeight,
                (p.MinJumpDist + (p.MaxJumpDist - p.MinJumpDist) * showcaseJumpCharge) * time);
        }
    }

    
}
