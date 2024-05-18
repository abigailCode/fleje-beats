using System.Collections;
using UnityEngine;

public class BoundBehaviour : MonoBehaviour {
    [SerializeField] GameObject _scoreCanvas;
    int boxesLost = 0;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Box")) {
            if (++boxesLost == 5) StartCoroutine(Lose());
            _scoreCanvas.SendMessage("ResetCombo");
            if (_scoreCanvas.GetComponent<Score>().GetCombo() != 0) boxesLost = 0;

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
