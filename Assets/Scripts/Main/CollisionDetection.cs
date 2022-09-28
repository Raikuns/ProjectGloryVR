using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FTail;
using Oculus.Interaction.Input;

public class CollisionDetection : MonoBehaviour
{
    TailAnimator2[] tails;
    public HandPhysicsCapsules[] handPhysics;
    public List<Collider> colliders = new List<Collider>();

    private IEnumerator Start()
    {
        tails = FindObjectsOfType<TailAnimator2>();
        handPhysics = FindObjectsOfType<HandPhysicsCapsules>();
        for (int i = 0; i < handPhysics.Length; i++)
        {
            print("this one started");
            yield return new WaitUntil(() => { return handPhysics[i]._capsulesAreActive; });
            var capsules = handPhysics[i]._capsules;
            for (int y = 0; y < capsules.Count; y++)
            {
                colliders.Add(capsules[y].CapsuleCollider);
            }
        }

        for (int i = 0; i < tails.Length; i++)
        {
            for (int j = 0; j < colliders.Count; j++)
            {
                tails[i].AddCollider(colliders[j]);
            }
        }
    }


}
