using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform _platformHolder;
    [SerializeField] private CameraMovement _camera;
    [SerializeField] private LevelPreset _levelPreset;

    private int _platformCount;
    private int _platformsPassed;
    private int _currentStage;
    private List<Platform> _platforms;


    private void Start()
    {
        float screenSizeToworld = Camera.main.ViewportToWorldPoint(Vector3.up).y - Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        _platformCount = Mathf.CeilToInt(screenSizeToworld / _levelPreset.platformDistance) + 1;
        _platforms = new List<Platform>();
        for (int i = 0; i < _platformCount; i++) 
        {
            SpawnPlatform(i == 0);
        }
        SetCameraColors(0f);
    }

    private void OnEnable()
    {
        LevelManager.Default.OnRestart += HandleOnLevelRestart;
    }

    private void OnDisable()
    {
        LevelManager.Default.OnRestart -= HandleOnLevelRestart;
    }

    private void SpawnPlatform(bool isStartPlatform) 
    {
        float platformHeight = 0f;
        if (_platforms.Count > 0)
            platformHeight = _platforms[_platforms.Count - 1].transform.localPosition.y + _levelPreset.platformDistance;

        Platform instance = Instantiate(_levelPreset.platformPrefab);
        instance.transform.SetParent(_platformHolder);
        instance.transform.localPosition = Vector3.up * platformHeight;
        instance.Init(_levelPreset.levelStages[_currentStage], isStartPlatform);
        instance.SetColor(_levelPreset.levelStages[_currentStage].platformColors, 0f);
        instance.OnLanded += HandleOnPlatformLanded;
        _platforms.Add(instance);
    }

    private void ChangeStage(int newStage) 
    {
        _currentStage = Mathf.Min(newStage, _levelPreset.levelStages.Count - 1);
        foreach (Platform platform in _platforms) 
        {
            platform.SetColor(_levelPreset.levelStages[_currentStage].platformColors, _levelPreset.stageChangeAnimDuration);
        }
        SetCameraColors(_levelPreset.stageChangeAnimDuration);
    }

    private void HandleOnPlatformLanded(Platform platform) 
    {
        _platformsPassed++;
        platform.SetNumText(_platformsPassed);
        ProcessPanel.Default.UpdateScore(_platformsPassed);
        int newStage = _platformsPassed / _levelPreset.platformPerStage;
        if (newStage != _currentStage && _levelPreset.levelStages.Count > 1) 
        {
            ChangeStage(newStage);
        }
        platform.OnLanded -= HandleOnPlatformLanded;

        SpawnPlatform(false);
        if (_platforms.Count > _platformCount + 1)
        {
            Destroy(_platforms[0].gameObject);
            _platforms.RemoveAt(0);
        }
    }

    private void SetCameraColors(float duration) 
    {
        BoxCollider2D back = _levelPreset.levelStages[_currentStage].backgroundPrefab;
        Color stripColor = _levelPreset.levelStages[_currentStage].paralaxStripColor;
        _camera.SetColors(back, stripColor, duration);
    }

    private void HandleOnLevelRestart()
    {
        if (_platformsPassed > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", _platformsPassed);
        }
    }
}
