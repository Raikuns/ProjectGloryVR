
using UnityEngine;

public class CloudsLeanTween : MonoBehaviour
{
    [SerializeField] LeanTweenType type;
    [SerializeField] float _duration;
    [SerializeField] float _moveTo;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveLocalY(this.gameObject, _moveTo, _duration).setLoopPingPong();
    }

}
