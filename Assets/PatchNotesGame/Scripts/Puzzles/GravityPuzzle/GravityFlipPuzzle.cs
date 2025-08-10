using UnityEngine;
using System.Collections.Generic;

public class GravityFlipPuzzle : PuzzleBase
{
    [SerializeField] private List<Transform> orbSpawns = new List<Transform>();
    [SerializeField] private List<Transform> orbDestinations = new List<Transform>();
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private GameObject destinationPrefab;

    private GravityNodule spawnedOrb;
    private int numOrbs = 0;
    private int completedOrbs = 0;
    
    private List<Transform> usedLocations = new List<Transform>();
    private List<Transform> usedDestinations = new List<Transform>();

    private void FixedUpdate()
    {
        if (spawnedOrb != null && spawnedOrb.complete)
        {
            OrbComplete();
        }
    }

    protected override void OnActivate()
    {
        if (orbSpawns.Count > 0 && orbPrefab != null)
        { 
            numOrbs = orbSpawns.Count;
            SpawnOrb();
        }
        // Possibly handle text to explain the puzzle
    }

    protected override void OnComplete()
    {
        if (spawnedOrb) spawnedOrb.OnComplete();
        bugToFix.Fix();
    }

    protected override void OnFailed()
    {
        //
    }

    private void SpawnOrb()
    {
        
        int index = Random.Range(0, orbSpawns.Count);
        Transform transform = orbSpawns[index];

        usedLocations.Add(transform);
        orbSpawns.RemoveAt(index);

        GameObject go = Instantiate(orbPrefab, transform.position, Quaternion.identity);
        spawnedOrb = go.GetComponent<GravityNodule>();

        Transform destination = GetRandomDestination();
        spawnedOrb.destination = destination;

        GameObject dgo = Instantiate(destinationPrefab, destination.position, Quaternion.identity);
        dgo.GetComponent<GravityNodeDestination>().orb = go;
    }

    private void OrbComplete()
    {
        completedOrbs++;
        if (completedOrbs == numOrbs) OnComplete();
        else
        {
            spawnedOrb.OnComplete();
            SpawnOrb();
        }
    }

    private Transform GetRandomDestination()
    {
        int index = Random.Range(0, orbDestinations.Count);
        Transform destination = orbDestinations[index];
        usedDestinations.Add(destination);
        orbDestinations.RemoveAt(index);
        return destination;
    }
}
