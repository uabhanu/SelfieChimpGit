﻿using System.Collections.Generic;
using UnityEngine;

public class PlatformsGenerator : MonoBehaviour 
{
    private const float PLAYER_DISTANCE_SPAWN_LAND_PART = 20.0f;
    private Vector3 _lastEndPosition;

    [SerializeField] private Transform _platformEndPosition;
    [SerializeField] private Transform _platformStart;
    [SerializeField] private List<Transform> _platformsList;
    [SerializeField] private LandPuss _landPuss;

    private void Awake() 
    {
        _lastEndPosition = _platformEndPosition.transform.position;
    }

    private void Update() 
    { 
        if(Vector3.Distance(_landPuss.GetPosition() , _lastEndPosition) < PLAYER_DISTANCE_SPAWN_LAND_PART) 
        {
            // Spawn another level part
            SpawnLandPart();
        }
    }

    private void SpawnLandPart() 
    {
        Transform chosenLandPart = _platformsList[Random.Range(0 , _platformsList.Count)];
        Transform lastLandPartTransform = SpawnLandPart(chosenLandPart , _lastEndPosition);
        _lastEndPosition = lastLandPartTransform.Find("EndPosition").position;
    }

    private Transform SpawnLandPart(Transform landPart , Vector3 spawnPosition) 
    {
        //TODO Do the following if not in Super mode, else just spawn at the lowest possible y position
        float randomYPosition = Random.Range(spawnPosition.y - 0.5f , spawnPosition.y + 0.5f); //TODO Ideal values yet to be decided
        Transform landPartTransform = Instantiate(landPart , new Vector3(spawnPosition.x , Mathf.Clamp(randomYPosition , -4.50f , 0.70f) , spawnPosition.z) , Quaternion.identity);
        return landPartTransform;
    }

}

