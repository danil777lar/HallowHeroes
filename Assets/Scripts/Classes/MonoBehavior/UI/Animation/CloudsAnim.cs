using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CloudsAnim : MonoBehaviour
{
    [SerializeField] private float _noiseScale;
    [SerializeField] private float _noiseFrequency;
    [SerializeField] private List<Image> _clouds;
    private Dictionary<Image, float> _seeds;
    private Dictionary<Image, Vector3> _defaultPositions;


    private void Start()
    {
        _seeds = new Dictionary<Image, float>();
        _defaultPositions = new Dictionary<Image, Vector3>();
        foreach (Image cloud in _clouds)
        {
            _seeds[cloud] = Random.Range(0, 100000);
            _defaultPositions[cloud] = cloud.transform.localPosition;
        }
    }

    private void Update()
    {
        foreach (Image cloud in _clouds)
        {
            Vector3 targetPoition = _defaultPositions[cloud];
            targetPoition.y += (Mathf.PerlinNoise(_seeds[cloud], Time.time * _noiseFrequency) - 0.5f) * _noiseScale;
            cloud.transform.localPosition = targetPoition;
        }
    }
}
