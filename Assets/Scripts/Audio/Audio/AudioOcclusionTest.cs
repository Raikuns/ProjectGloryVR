using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioOcclusionTest : MonoBehaviour
{
    public float rate=1;
    public bool burstFire;
    private float timer=1;

    public GameObject flashObject;
    public Light flashLight;

    public UnityEvent audioTrigger;
    public AudioEvent audioEvent;
    void Start()
    {
        timer = 1;
    }

    // Update is called once per frame
    void Update()
    {        
        timer -= Time.deltaTime * rate;
        flashLight.intensity = Mathf.Lerp(flashLight.intensity, 0, Time.deltaTime*15);
        if (timer < 0)
        {
            timer = 1;
            if(burstFire)
            {
                StartCoroutine(BurstFire());
            }
            else
            {
                audioTrigger?.Invoke();
                StartCoroutine(MuzFlash());
            }
            
        }
    }

    

    private IEnumerator MuzFlash()
    {
        flashObject.SetActive(true);
        flashLight.intensity = 17;
        yield return new WaitForSeconds(.05f);
        flashObject.SetActive(false);
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < 5; i++)
        {
            audioTrigger?.Invoke();
            audioEvent.Invoke();
            StartCoroutine(MuzFlash());
            yield return new WaitForSeconds(.06f);
        }
    }
}
