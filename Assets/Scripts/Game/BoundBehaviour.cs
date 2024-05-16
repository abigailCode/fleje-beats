using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
