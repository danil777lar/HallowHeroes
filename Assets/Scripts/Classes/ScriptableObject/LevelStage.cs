using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Level Stage")]
public class LevelStage : ScriptableObject
{
    [Header("Colors")]
    public Color paralaxStripColor;
    public Color backroundColor;
    public PlatformColors platformColors;

    [Header("Enemies")]
    public float enemySpawnChance;
    public List<PlatformAttachment> enemyPrefabs;

    [Header("Environments")]
    public List<PlatformAttachment> environmentPrefabs;

    [Header("Money")]
    public float moneySpawnChance;
    public PlatformAttachment moneyPrefab;
}
