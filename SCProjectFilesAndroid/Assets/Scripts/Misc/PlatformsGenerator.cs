﻿using System.Collections.Generic;
using UnityEngine;

public class PlatformsGenerator : MonoBehaviour 
{
    private const float PLAYER_DISTANCE_SPAWN_PLATFORMS_PART = 20.0f;
    private Vector3 _lastEndPosition;

    [SerializeField] private LandPuss _landPuss;
    [SerializeField] private List<Transform> _platformTransformsList;
    [SerializeField] private Transform _platformEndPositionTransform;

    private void Awake() 
    {
        _lastEndPosition = _platformEndPositionTransform.transform.position;
    }

    private void Update() 
    { 
        if(Time.timeScale == 0)
        {
            return;
        }

        if(Vector3.Distance(_landPuss.GetPosition() , _lastEndPosition) < PLAYER_DISTANCE_SPAWN_PLATFORMS_PART) 
        {
            // Spawn another level part
            SpawnLandPart();
        }
    }

    private void SpawnLandPart() 
    {
        Transform chosenPlatformToSpawn = _platformTransformsList[Random.Range(0 , _platformTransformsList.Count)];
        Transform lastLandPartTransform = SpawnLandPart(chosenPlatformToSpawn , _lastEndPosition);
        _lastEndPosition = lastLandPartTransform.Find("EndPosition").position;
    }

    private Transform SpawnLandPart(Transform objectToSpawn , Vector3 spawnPosition) 
    {
        float randomYPosition = Random.Range(spawnPosition.y - 0.5f , spawnPosition.y + 0.5f);
        //_platformEndPositionTransform = Instantiate(objectToSpawn , new Vector3(spawnPosition.x , Mathf.Clamp(randomYPosition , -4.50f , 0.70f) , spawnPosition.z) , Quaternion.identity);
         
        //TODO Remove the line below after testing and use the one above instead
        _platformEndPositionTransform = Instantiate(objectToSpawn , new Vector3(spawnPosition.x , -4.50f , spawnPosition.z) , Quaternion.identity); //TODO Remove this after testing
        return _platformEndPositionTransform;
    }
}

