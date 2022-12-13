using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreditsObject : MonoBehaviour
{
    public float spawnDelay = 1.0f;
    public float lifetime = 1.0f;
    public bool hasFinished;

    Rigidbody body;
    [SerializeField] float _Time;
    [SerializeField] Vector3 _StartPos;
    [SerializeField] Vector3 _Scale;
    [SerializeField] TextExplosion _Explosion;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
    }
    public void Spawn()
    {
        StartCoroutine(SpawnSpulHier());
        _Explosion = FindObjectOfType<TextExplosion>();
    }
    IEnumerator SpawnSpulHier()
    {
        body.isKinematic = true;
        body.velocity = Vector3.zero;
        this.gameObject.transform.position = _StartPos;
        this.gameObject.transform.localScale = _Scale;
        yield return new WaitForSeconds(spawnDelay);
        body.isKinematic = false;
        LeanTween.scale(this.gameObject, Vector3.one, _Time);
        yield return new WaitForSeconds(lifetime);

        hasFinished = true;
        _Explosion.SimpleExplosion();
    }
}
