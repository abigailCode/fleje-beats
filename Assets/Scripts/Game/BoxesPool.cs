using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesPool : MonoBehaviour {
    [SerializeField] GameObject[] _boxPrefabs;
    [SerializeField] float _boxSpeed = 10f;
    float _rotationSpeed = 100f;
    float _spawnDelay = 4.5f;
    float[] _spawnRates;
    int _songDuration;

    //TODO: FIX LAST BOXES (LAST 4.5 SECONDS)
    void Start() {
        _songDuration = PlayerPrefs.GetInt("song.duration");
        SongLevelConfiguration _levelConf = new LevelConfReader().ReadConf();
        StartCoroutine(SpawnBoxes2(_levelConf.beatData));
        //StartCoroutine(SpawnBoxes());
    }
    IEnumerator SpawnBoxes2(List<SongBeat> _beatData) {
        foreach (SongBeat beat in _beatData) {
            yield return new WaitForSeconds(beat.time + _spawnDelay);
            InstantiateBox2(beat);
        }
    }

    IEnumerator SpawnBoxes() {
        float timeTotal = -_spawnDelay;
        for (int i = 0; i < _spawnRates.Length; i++) {
            timeTotal -= _spawnRates[i];
            Debug.Log($"Time total: {timeTotal}");
            Debug.Log($"Spawn rate: {_spawnRates[i]}");
            if (i > 0) timeTotal -= _spawnRates[i - 1];
            yield return new WaitForSeconds(timeTotal);
            Invoke("InstantiateBox", _spawnRates[i]);
        }
    }

    void InstantiateBox2(SongBeat beat) {

        float randomX = Random.Range(-0.39f, 0.4560112f);

        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        //! The box model needs to be rotated 180 degrees on the Y axis
        Quaternion rotation = Quaternion.Euler(0, 180, 0);

        int boxIndex = Random.Range(0, _boxPrefabs.Length);

        GameObject box = Instantiate(_boxPrefabs[boxIndex], spawnPosition, rotation);
      
        StartCoroutine(Rotate(box));

        Rigidbody rb = box.GetComponent<Rigidbody>();
        rb.velocity = -Vector3.forward * _boxSpeed;
    }

    IEnumerator Rotate(GameObject box) {
        // Create a target rotation that is 90 degrees turned from the original in the y-axis direction
        int direction = Random.Range(0, 4);

        RotateHitbox(box, direction);
        GameObject arrow = box.transform.Find("Arrow").gameObject;

        Quaternion targetRotation = arrow.transform.rotation * Quaternion.Euler(0, 0, 90 * direction);

        // As long as the box's rotation is not close enough to the target rotation
        while (Quaternion.Angle(arrow.transform.rotation, targetRotation) > 0.1f) {
            // Rotate the box towards the target rotation at the _rotationSpeed
            arrow.transform.rotation = Quaternion.RotateTowards(arrow.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void RotateHitbox(GameObject box, int direction) {
        Transform hitbox = box.transform.Find("CorrectHitbox");
        if (hitbox == null) return;

        switch (direction) {
            case 1:
                hitbox.localPosition = new Vector3(0.001f, 0.002f, -0.004f);
                hitbox.rotation = Quaternion.Euler(0f, 0f, 90f);
                Debug.Log("Izquierda (hitbox a la derecha)");
                break;
            case 2:
                hitbox.localPosition = new Vector3(0f, 0.004f, -0.121f);
                Debug.Log("Arriba (hitbox abajo)");
                break;
            case 3:
                hitbox.localPosition = new Vector3(0.689f, 0.002f, -0.004f);
                hitbox.rotation = Quaternion.Euler(0f, 0f, 90f);
                Debug.Log("Derecha (hitbox a la izquierda)");
                break;
        }
    }
}
