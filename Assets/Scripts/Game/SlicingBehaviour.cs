using UnityEngine;
using EzySlice;
using Unity.VisualScripting;
using System.Collections;
using System;

public class SlicingBehaviour : MonoBehaviour {
    [SerializeField] Material _sliceMaterial;
    [SerializeField] float _cutForce = 2500;
    [SerializeField] VelocityEstimator _estimator;
    [SerializeField] Transform _startSlicePoint, _endSlicePoint;
    GameObject _scoreCanvas;
    AudioSource[] audioSources;

    //event for hand vibration
    public delegate void HandsVibrating();
    public static event HandsVibrating OnHandsVibrating;

    void Start() {
        _scoreCanvas = GameObject.Find("ScoreCanvas");
        audioSources = GetComponents<AudioSource>();
    }

    void Slice(GameObject target) {
        Vector3 velocity = _estimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(_endSlicePoint.position - _startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(target.transform.position, planeNormal);
        if (hull == null) return;

        var slicedHulls = CreateSlicedHulls(target, hull);

        Destroy(target.transform.parent.gameObject);
        StartCoroutine(DestroySlicedObjects(slicedHulls.Item1, slicedHulls.Item2));
    }

    Tuple<GameObject, GameObject> CreateSlicedHulls(GameObject target, SlicedHull hull) {
        GameObject upperHull = hull.CreateUpperHull(target, _sliceMaterial);
        GameObject lowerHull = hull.CreateLowerHull(target, _sliceMaterial);

        upperHull.transform.position = _endSlicePoint.position;
        lowerHull.transform.position = _endSlicePoint.position;

        SetupSliceComponent(upperHull);
        SetupSliceComponent(lowerHull);
        return new Tuple<GameObject, GameObject>(upperHull, lowerHull);
    }

    void SetupSliceComponent(GameObject slicedObject) {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = rb.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(_cutForce, slicedObject.transform.position, 1);
        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other) {
        if (other != null) {
            if (other.CompareTag("CorrectHitbox")) {
                _scoreCanvas.SendMessage("IncreaseScore");
                Slice(other.transform.parent.Find("Body").gameObject);
                audioSources[0].Play();
            
            }
            if (other.CompareTag("Box")) { 
                _scoreCanvas.SendMessage("ResetCombo");
                Slice(other.gameObject);
                audioSources[1].Play();
            }
        }
        OnHandsVibrating?.Invoke();
    }

    void OnCollisionEnter(Collision collision) {
        OnHandsVibrating?.Invoke();
    }

    IEnumerator DestroySlicedObjects(GameObject upperHull, GameObject lowerHull) {
        yield return new WaitForSeconds(2);
        Destroy(upperHull);
        Destroy(lowerHull);
    }
}