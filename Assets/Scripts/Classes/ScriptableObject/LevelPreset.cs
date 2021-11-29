using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Level Preset")]
public class LevelPreset : ScriptableObject
{
    [Header("Main")]
    public float platformDistance;
    public Platform platformPrefab;
    [Header("Stages")]
    public int platformPerStage;
    public float stageChangeAnimDuration;
    public List<LevelStage> levelStages;
}
