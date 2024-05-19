using System.Collections;
using UnityEngine;

public class BoundBehaviour : MonoBehaviour {
    [SerializeField] GameObject _scoreCanvas;
    int _boxesLost = 0;

    void FixedUpdate() {
        if (_scoreCanvas.GetComponent<Score>().GetCombo() != 0) _boxesLost = 0;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Box")) {
            if (++_boxesLost == 5) StartCoroutine(Lose());
            _scoreCanvas.SendMessage("ResetCombo");
            

            other.transform.parent.SendMessage("DestroyBox");
        }
    }

    IEnumerator Lose() {
        GameObject boxesSpawn = GameObject.Find("BoxesSpawn");
        boxesSpawn.SendMessage("StopSpawn");
        boxesSpawn.BroadcastMessage("DestroyBox");
        //Lose sound
        yield return new WaitForSeconds(2);
        Score.FinishGame();
    }
}
