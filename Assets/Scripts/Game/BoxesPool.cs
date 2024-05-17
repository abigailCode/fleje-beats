using System.Collections;
using UnityEngine;

public class BoxesPool : MonoBehaviour {
    [SerializeField] GameObject[] _boxPrefabs;
    [SerializeField] float _boxSpeed = 10f;
    float _rotationSpeed = 100f;
    float[] _spawnRates = new float[] { 2.413991f, 3.927574f, 5.78364f, 7.459965f, 8.944175f, 11.044384f, 12.673392f, 14.048015f, 15.617548f, 17.159244f, 18.665021f, 20.231694f, 21.754917f, 22.848171f, 24.028485f, 25.135854f, 26.444975f, 27.708892f, 29.036123f, 30.306572f, 31.524819f, 32.759073f, 34.048685f, 35.293819f, 36.509439f, 37.88261f, 39.028527f, 40.360402f, 41.353651f, 47.141476f, 48.157173f, 49.16034f, 50.484003f, 51.780309f, 52.984155f, 54.354312f, 55.655803f, 56.881069f, 59.27638f, 60.17908f, 60.766956f, 61.377817f, 62.368863f, 63.258374f, 63.963179f, 64.909171f, 65.762778f, 66.374359f, 67.473037f, 70.926328f, 71.54649f, 72.072702f, 72.438759f, 72.794682f, 73.069839f, 73.36752f, 73.640669f, 73.928931f, 74.207231f, 74.541392f, 74.866101f, 75.17505f, 77.136985f, 79.629215f, 81.041684f, 82.059643f, 83.290818f, 84.551336f, 85.685979f, 87.13003f, 88.232238f, 89.801709f, 91.83137f, 93.027327f, 94.207617f, 95.592809f, 96.753826f, 97.976924f, 99.27788f, 100.632043f, 102.044155f, 103.596268f, 104.923788f, 106.244572f };
    //float[] _spawnRates = new float[] {2.262787f, 5.566769f, 5.886875f, 9.102852f, 12.526729f, 12.678871f, 16.214861f, 16.599069f, 16.982874f, 18.814715f, 19.10312f, 19.350938f, 19.615079f, 20.079811f, 20.359136f, 22.342949f, 22.686941f, 23.038823f, 23.207063f, 23.614973f, 23.766986f, 26.310789f, 26.471017f, 26.64684f, 26.790796f, 27.126957f, 27.28691f, 29.646897f, 29.79895f, 30.103275f, 30.310964f, 30.694515f, 30.902907f, 31.382511f, 31.790773f, 32.158896f, 32.591069f, 32.87106f, 33.166998f, 33.511096f, 33.751082f, 34.207102f, 34.463031f, 34.83101f, 35.206858f, 35.615068f, 36.07928f, 36.35899f, 36.679206f, 36.991046f, 37.23091f, 37.646958f, 37.959103f, 38.359323f, 38.719084f, 39.111268f, 39.574939f, 39.79105f, 40.119286f, 40.399163f, 40.654834f, 41.055287f, 41.303071f, 41.654894f, 42.095014f, 42.487225f, 43.222837f, 43.454543f, 43.719256f, 43.943317f, 44.215065f, 44.67105f, 45.014942f, 45.3594f, 45.767227f, 46.151444f, 46.607079f, 46.975543f, 47.287147f, 47.590953f, 47.846481f, 48.21466f, 48.598761f, 48.919524f, 49.303246f, 49.719436f, 50.095533f, 50.303178f, 50.639192f, 50.991053f, 51.255158f, 51.647107f, 51.86295f, 52.167247f, 52.535113f, 52.935272f, 53.303234f, 53.67148f, 53.847002f, 54.159095f, 54.527063f, 54.751019f, 55.102941f, 55.375384f, 55.663166f, 56.087347f, 56.431228f, 56.799462f, 57.103366f, 57.278773f, 57.575221f, 57.847192f, 58.039101f, 58.295243f, 58.679287f, 58.943204f, 59.295264f, 59.727424f, 60.087396f, 60.503511f, 60.767598f, 60.951111f, 61.247361f, 61.487195f, 61.75931f, 62.167327f, 62.343305f, 62.702655f, 63.119333f, 63.559341f, 63.911044f, 64.151374f, 64.327397f, 64.695428f, 64.96734f, 65.159595f, 65.615427f, 65.791426f, 66.151473f, 66.415338f, 66.543374f, 67.015398f, 67.399151f, 67.655057f, 67.830893f, 68.17486f, 68.350819f, 68.727348f, 69.119588f, 69.342945f, 69.710648f, 69.911259f, 70.327442f, 70.583253f, 70.775342f, 71.111558f, 71.454865f, 71.647236f, 71.911296f, 72.295446f, 72.695323f, 72.935654f, 73.199346f, 73.590722f, 73.983273f, 74.447094f, 74.887567f, 75.335566f, 75.743554f, 76.183268f, 76.599182f, 77.039282f, 77.479299f, 77.871442f, 78.263039f, 78.703102f, 79.143442f, 79.57556f, 80.023398f, 80.479296f, 80.959319f, 81.375329f, 81.807283f, 82.27146f, 82.735632f, 83.167355f, 83.599426f, 84.023754f, 84.471443f, 84.935474f, 85.391575f, 85.807782f, 86.239418f, 86.65523f, 87.05602f, 87.543572f, 87.887417f, 88.047856f, 88.479095f, 88.903646f, 89.351628f, 89.799263f, 90.255336f, 90.695442f, 91.11937f, 91.647578f, 92.063376f, 93.263724f, 95.591295f, 95.967891f, 96.375375f, 96.823562f, 100.423507f, 102.110933f, 104.047656f, 106.671862f, 108.751607f, 110.67985f, 114.656047f, 115.071462f, 115.495869f, 115.9192f, 116.288112f, 116.839836f, 117.327999f, 117.808189f, 118.295815f, 118.615836f, 119.112098f, 119.599943f, 119.951701f, 120.44802f, 120.872193f, 121.359859f, 121.751556f, 122.079768f, 122.568136f, 122.967825f, 123.383918f, 123.775678f, 124.264101f, 124.744159f, 125.22363f, 125.639806f, 126.087802f, 126.520109f, 126.967906f, 128.359971f, 128.759947f, 129.288102f, 129.65629f, 130.056134f, 130.431978f, 130.919512f, 131.335979f, 131.79139f, 132.287868f, 132.687922f, 133.128308f, 133.487637f, 133.911874f, 134.407982f, 134.832071f, 135.280092f, 135.744032f, 136.112337f, 136.615718f, 137.031974f, 137.423955f, 137.871894f, 138.327926f, 138.759937f, 139.168105f, 139.592139f, 139.999848f, 140.455953f, 140.864489f, 141.279395f, 141.447873f, 141.79975f, 142.064306f, 142.239855f, 142.759362f, 143.487882f, 144.271835f, 144.720041f, 144.959899f, 145.312274f, 145.6401f, 145.799842f, 146.28814f, 146.568014f, 146.999896f, 147.408053f, 147.824016f, 148.216055f, 148.383879f, 148.760001f, 149.112179f, 149.368444f, 149.760213f, 149.975995f, 150.352078f, 150.800351f, 151.232176f, 151.680261f, 151.888152f, 152.055799f, 152.45614f, 152.920348f, 153.311875f, 153.48815f, 154.000371f, 154.408577f, 154.800257f, 155.320161f, 155.727415f, 156.2964f, 156.792075f, 157.152195f, 157.631853f, 158.04006f, 158.359673f, 158.783611f, 158.96001f, 159.29616f, 159.584023f, 159.944418f, 160.863853f, 161.208217f, 161.600207f, 161.960537f, 162.368391f, 162.528354f, 162.919993f, 163.360283f, 164.247842f, 164.607921f, 165.031946f, 165.408254f, 165.655467f, 166.050208f, 166.440486f, 166.832092f, 167.720176f, 167.960112f, 168.472387f, 168.8487f, 169.488474f, 169.728036f, 170.072418f, 170.352152f, 170.840288f};

    float spawnDelay = 4.5f;
    //TODO: FIX LAST BOXES (LAST 4.5 SECONDS)

    void Start() { StartCoroutine(SpawnBoxes()); }

    IEnumerator SpawnBoxes() {
        float timeTotal = -spawnDelay;
        for (int i = 0; i < _spawnRates.Length; i++) {
            timeTotal -= _spawnRates[i];
            Debug.Log($"Time total: {timeTotal}");
            Debug.Log($"Spawn rate: {_spawnRates[i]}");
            if (i > 0) timeTotal -= _spawnRates[i - 1];
            yield return new WaitForSeconds(timeTotal);
            Invoke("InstantiateBoxP1", _spawnRates[i]);
            Invoke("InstantiateBoxP2", _spawnRates[i]);
        }
    }

    void InstantiateBox() {
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

    void InstantiateBoxP1()
    {
        float randomX = Random.Range(-9.5f, -12.5f);

        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        //! The box model needs to be rotated 180 degrees on the Y axis
        Quaternion rotation = Quaternion.Euler(0, 180, 0);

        int boxIndex = Random.Range(0, _boxPrefabs.Length);

        GameObject box = Instantiate(_boxPrefabs[boxIndex], spawnPosition, rotation);

        StartCoroutine(Rotate(box));

        Rigidbody rb = box.GetComponent<Rigidbody>();
        rb.velocity = -Vector3.forward * _boxSpeed;
    }

    void InstantiateBoxP2()
    {
        float randomX = Random.Range(9.5f, 12.5f);

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
