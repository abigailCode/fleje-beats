using UnityEngine;

public class BoundBehaviour : MonoBehaviour {
    [SerializeField] GameObject _scoreCanvas;
    GameObject _currentBox;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Box")) {
            _scoreCanvas.SendMessage("ResetCombo");
            _currentBox = other.gameObject;
            Invoke(nameof(ActivateGravity), 1);
            Invoke(nameof(DestroyBox), 2);
        }
    }

    void ActivateGravity() => _currentBox.GetComponent<Rigidbody>().useGravity = true;

    void DestroyBox() => Destroy(_currentBox.transform.parent.gameObject);
}
