using System.Collections;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour {
    private string _direction = "";

    void DestroyBox() => StartCoroutine(DestroyBoxCoroutine());

    IEnumerator DestroyBoxCoroutine() {
        GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void SetDirection(string newDirection){
    //Debug.Log("SetDirection: " + newDirection);
        _direction = newDirection;
    } 
    public string GetDirection() {
        //Debug.Log("GetDirection: " + _direction);
    return _direction;
    } 
}
