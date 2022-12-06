using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]List<CreditsObject> creditObjects;
    int currIdx = 0;
    void Start()
    {
        StartCoroutine(DoIt());
    }

    IEnumerator DoIt()
    {
        while (currIdx <= creditObjects.Count)
        {
            var currCreditObj = creditObjects[currIdx];
            currCreditObj.Spawn();
            if (currIdx + 1 <= creditObjects.Count)
                yield return new WaitWhile(() => currCreditObj.hasFinished == false);
            else
                break;
            currIdx++;

        }
    }
}
