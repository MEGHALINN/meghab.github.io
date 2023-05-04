using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadpiecespawner : MonoBehaviour

{
    [SerializeField]private GameObject[] _availableRoadPieces;
    [SerializeField] private roadpiece _currentRoadPieces;
    private List<GameObject> _spawnedRoadPieces= new List<GameObject>();
    [SerializeField] private int _numberOfDefaultRoadpieces=3;
    private int _numberOfRoadPiecesSpawned;
    

    void Start()
    {
        for(int i=0;i<_numberOfDefaultRoadpieces;i++)
           SpawnRandomRoadPiece();

    }
    void Update()
    {

    }
     public void SpawnRandomRoadPiece()
    {
        if(_numberOfRoadPiecesSpawned-_numberOfDefaultRoadpieces>_numberOfDefaultRoadpieces)
             DeleteOldestpiece();
        _numberOfRoadPiecesSpawned++;

        int x= Random.Range(0,_availableRoadPieces.Length);
        
        GameObject g = Instantiate(_availableRoadPieces[x], _currentRoadPieces.SpawnPoint.position,_currentRoadPieces.SpawnPoint.rotation);
        _currentRoadPieces=g.GetComponent<roadpiece>();
        _spawnedRoadPieces.Add(g);
        
    }
    void DeleteOldestpiece()
    {
        if(_spawnedRoadPieces.Count>0)
        {
            Destroy(_spawnedRoadPieces[0]);
            _spawnedRoadPieces.RemoveAt(0);
        }
    }
}