using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LobbyBoxPool : MonoBehaviour
{


    [SerializeField] GameObject[] _boxPrefabs;
    [SerializeField] float _spawnInterval = 1f; // Intervalo de generación en segundos
    [SerializeField] float _minSeparation = 3f; // Separación mínima entre prefabs
    [SerializeField] float _timer = 10f;
   // [SerializeField] bool _countdown = false;
    float _nextSpawnTime;

    void Start()
    {
        _nextSpawnTime = Time.time;
        StartCoroutine(StartCountdown());
        StartCoroutine(StartSpawning());
    }

    void Update()
    {
        /*if (Time.time >= _nextSpawnTime)
           
        {
            InstantiateBox();
            _nextSpawnTime += _spawnInterval;

        }
        */
        
    }

    public void InstantiateBox()
    {
        //float randomX = Random.Range(-0.39f, 0.4560112f);
        //float randomz = Random.Range(-0.39f, 0.4560112f);
        float[] randoms = {-0.5f, -0.4f, 0.4f, 0.5f};
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        Vector3 spawnPosition = new Vector3(randoms[Random.Range(0,randoms.Length)], 5, randoms[Random.Range(0, randoms.Length)]);

        /*if (!CheckSeparation(spawnPosition))
            return;
        */

        int boxIndex = Random.Range(0, _boxPrefabs.Length);
        Instantiate(_boxPrefabs[boxIndex], spawnPosition, rotation);


       
    }

    /*bool CheckSeparation(Vector3 position)
    {
        foreach (var box in GameObject.FindGameObjectsWithTag("Box"))
        {
            if (Vector3.Distance(position, box.transform.position) < _minSeparation)
                return false;
        }
        return true;
    }*/

    IEnumerator StartSpawning()
    {

        yield return null;
        if (_timer > 0)
        {

            InstantiateBox();
            yield return new WaitForSeconds(_spawnInterval);
            StartCoroutine(StartSpawning());
        }

        else StopAllCoroutines();
    }

    IEnumerator StartCountdown()
    {

        while (_timer > 0)
        {

            _timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }



    }
}
