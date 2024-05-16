using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesPool : MonoBehaviour {
    [SerializeField] GameObject[] _boxPrefabs;
    [SerializeField] float _boxSpeed = 10f;
    float _rotationSpeed = 100f;
    float _spawnDelay = 4.5f;
    //float[] _spawnRates;
    int _songDuration;

    //TODO: FIX LAST BOXES (LAST 4.5 SECONDS)
    void Start() {
        _songDuration = PlayerPrefs.GetInt("song.duration");
        SongLevelConfiguration _levelConf = new LevelConfReader().ReadConf();
        AudioManager.instance.PlaySong();
        StartCoroutine(SpawnBoxes2(_levelConf.beatData));
    }
    IEnumerator SpawnBoxes2(List<SongBeat> _beatData) {
        float oldTime = 0;
        foreach (SongBeat beat in _beatData) {
            if (beat.time + _spawnDelay > _songDuration) break;
            yield return new WaitForSeconds(beat.time + _spawnDelay - oldTime);
            oldTime = beat.time;
            InstantiateBox2(beat);
        }
        //SCManager.instance.LoadScene("Ranking");
        AudioManager.instance.UnloadSong();
    }

    float GetLocation(string location, string type) {
        // X Direction
        float LEFT_LOCATION = -0.39f;
        float RIGHT_LOCATION = 0.456112f;
        float CENTER_LOCATION = (-0.39f + 0.456112f) / 2;

        // Y Direction
        float TOP_LOCATION = 1.49f;
        float BOTTOM_LOCATION = 0.70f;

        switch (type) {
            case "locX":
                if (location == "Left") return LEFT_LOCATION;
                if (location == "Right") return RIGHT_LOCATION;
                if (location == "Middle") return CENTER_LOCATION;
                break;
            case "locY":
                if (location == "Top") return TOP_LOCATION;
                if (location == "Bottom") return BOTTOM_LOCATION;
                break;
        }
        return -1;
    }

    int GetHitArea(string location) {
        int rotation = 0;

        switch(location) {
            case "Left":
                rotation = 1;
                break;
            case "Bottom":
                rotation = 2;
                break;
            case "Right":
                rotation = 3;
                break;
        }
        return rotation;
    }

    void InstantiateBox2(SongBeat beat) {
        float locationX = GetLocation(beat.locationX, "locX");
        float locationY = GetLocation(beat.locationY, "locY");
        int hitArea = GetHitArea(beat.hit);

        Vector3 spawnPosition = new Vector3(locationX, locationY, transform.position.z);

        //! The box model needs to be rotated 180 degrees on the Y axis
        Quaternion rotation = Quaternion.Euler(0, 180, 0);

        int boxIndex = Random.Range(0, _boxPrefabs.Length);

        GameObject box = Instantiate(_boxPrefabs[boxIndex], spawnPosition, rotation);

        StartCoroutine(Rotate(box, hitArea));

        Rigidbody rb = box.GetComponent<Rigidbody>();
        rb.velocity = -Vector3.forward * _boxSpeed;
    }

    IEnumerator Rotate(GameObject box, int direction) {
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
