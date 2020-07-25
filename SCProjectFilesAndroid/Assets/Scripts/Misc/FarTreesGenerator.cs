﻿using UnityEngine;

public class FarTreesGenerator : MonoBehaviour
{
    private const float PLAYER_DISTANCE_SPAWN_FAR_TREES_PART = 40.0f;

    [SerializeField] private Transform _farTreesEndPosition;
    [SerializeField] private Transform _farTreesPartToSpawn;
    [SerializeField] private LandPuss _landPuss;
    [SerializeField] private Vector3 _lastEndPosition;

    private void Awake() 
    {
        _lastEndPosition = _farTreesEndPosition.transform.position;
    }

    private void Update() 
    {
        if(Vector3.Distance(_landPuss.GetPosition() , _lastEndPosition) < PLAYER_DISTANCE_SPAWN_FAR_TREES_PART) 
        {
            // Spawn another cloud part
            SpawnFarTreesPart();
        }
    }

    private void SpawnFarTreesPart() 
    {
        Transform chosenFarTreesPart = _farTreesPartToSpawn;
        Transform lastFarTreesPartTransform = SpawnFarTreesPart(chosenFarTreesPart , _lastEndPosition);
        _lastEndPosition = lastFarTreesPartTransform.Find("EndPosition").position;
    }

    private Transform SpawnFarTreesPart(Transform farTreesPart , Vector3 spawnPosition) 
    {
        Transform farTreesPartTransform = Instantiate(farTreesPart , new Vector3(spawnPosition.x , -0.66f , spawnPosition.z) , Quaternion.identity);
        return farTreesPartTransform;
    }
}
