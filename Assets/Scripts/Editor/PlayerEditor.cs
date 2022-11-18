using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor {

    private const int showcaseQuality = 32;
    [SerializeField] private static float showcaseJumpCharge = 1f;
    [SerializeField] private bool showShowCase = false;
    private Vector3[] shapePoints = new Vector3[showcaseQuality];
    private Vector3[] turnPoints = new Vector3[showcaseQuality];

    SerializedProperty turnSpeed;
    SerializedProperty maxTurnAngle;
    SerializedProperty chargeDuration;
    SerializedProperty minJumpDuration;
    SerializedProperty maxJumpDuration;
    SerializedProperty minJumpDist;
    SerializedProperty maxJumpDist;
    SerializedProperty maxJumpHeight;
    SerializedProperty jumpShape;
    SerializedProperty chargeMaxHeight;

    private void OnEnable() {
        generateJumpShowcase();

        turnSpeed = serializedObject.FindProperty("turnSpeed");
        maxTurnAngle = serializedObject.FindProperty("maxTurnAngle");
        chargeDuration = serializedObject.FindProperty("chargeDuration");
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
            //Shape
            Handles.color = Color.yellow;
            Handles.DrawPolyLine(toPlayer(shapePoints));

            //Turning
            Handles.color = Color.green;
            Handles.DrawPolyLine(toPlayer(turnPoints));
        }
    }

    private Vector3[] toPlayer(Vector3[] pos)
    {
        Player p = (Player)target;

        Vector3[] playerPositioned = pos;
        for (int i = 0; i < playerPositioned.Length; i++)
        {
            playerPositioned[i] += p.transform.position;
        }
        return playerPositioned;
    }

    public override void OnInspectorGUI() {
        Player p = (Player)target;

        //DrawDefaultInspector();

        EditorGUILayout.LabelField("Jumping", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(chargeDuration);
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(minJumpDuration);
        EditorGUILayout.PropertyField(maxJumpDuration);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(minJumpDist);
        EditorGUILayout.PropertyField(maxJumpDist);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Jump shape", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(maxJumpHeight);
        EditorGUILayout.PropertyField(jumpShape);
        EditorGUILayout.PropertyField(chargeMaxHeight);

        EditorGUILayout.LabelField("Turning", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(turnSpeed);
        EditorGUILayout.PropertyField(maxTurnAngle);

        EditorGUILayout.Space();
        if(showShowCase = EditorGUILayout.Toggle("Show jump gizmos", showShowCase))
        {
            showcaseJumpCharge = EditorGUILayout.Slider("Jump charge", showcaseJumpCharge, 0f, 1f);
        }
        if(EditorGUI.EndChangeCheck())
        {
            generateJumpShowcase();
            generateTurnShowcase();
            SceneView.RepaintAll();
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    private void generateJumpShowcase()
    {
        Player p = (Player)target;

        shapePoints = new Vector3[showcaseQuality];
        for (int i = 0; i < showcaseQuality; i++)
        {
            float time = (float)i/(showcaseQuality-1);
            shapePoints[i] = new Vector3(
                0f,
                p.JumpShape.Evaluate(time) * p.ChargeMaxHeight.Evaluate(showcaseJumpCharge) * p.MaxJumpHeight,
                (p.getJumpDistance(showcaseJumpCharge)) * time);
        }
    }

    private void generateTurnShowcase()
    {
        Player p = (Player)target;

        turnPoints = new Vector3[showcaseQuality];
        turnPoints[0] = Vector3.zero;
        turnPoints[showcaseQuality-1] = Vector3.zero;

        Vector3 v = Vector3.forward * p.MaxJumpDist;
        for (int i = 1; i < showcaseQuality-2; i++)
        {
            float time = (float)i/(showcaseQuality-3);
            float angle = Mathf.Lerp(-p.MaxTurnAngle, p.MaxTurnAngle, time);
            turnPoints[i] = Quaternion.AngleAxis(angle, Vector3.up) * v;
        }
    }
}
