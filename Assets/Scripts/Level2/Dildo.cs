using FIMSpace.FTail;
using UnityEngine;

public class Dildo : MonoBehaviour
{
    RopeGrabber dick;

    public Dimension dimension = Dimension.z;

    [SerializeField] public GameObject moveableDick;

    public float availablePointDecreaseMultiplier;

    TailAnimator2 tailAnim;
    Transform startedOnBone;

    public float availablePoints = 10;
    float basePoints;
    bool decreasePoints = false;

    public Transform dickBone;
    Quaternion startRot;

    [HideInInspector] public DildoHandler handler;

    [SerializeField] Transform penisLook;

    Vector3 startPos;

    [HideInInspector] public Vector3 grabbedPos;

    private void Awake()
    {
        basePoints = availablePoints;
        dick = GetComponentInChildren<RopeGrabber>();
        dick.dildo = this;

        tailAnim = GetComponentInChildren<TailAnimator2>();
        startedOnBone = tailAnim.StartBone;
        startRot = dickBone.rotation;

        moveableDick = dick.transform.parent.gameObject;
    }

    private void Start()
    {
        startPos = moveableDick.transform.position;
        moveableDick.SetActive(false);
    }

    private void Update()
    {
        if (decreasePoints)
        {
            availablePoints -= Time.deltaTime * availablePointDecreaseMultiplier;
            if (availablePoints < 1)
            {
                BackToOrigin();
            }
        }

        MoveDickAccordingly();
    }

    void MoveDickAccordingly()
    {
        Vector3 desiredPos = penisLook.position;

        if (dimension == Dimension.z)
        {
            desiredPos = new(penisLook.position.x, penisLook.position.y, dick.transform.position.z + 0.18f);
        }
        else if (dimension == Dimension.x)
        {
            desiredPos = new(penisLook.position.x, penisLook.position.y, dick.transform.position.z);
        }

        penisLook.position = desiredPos;
    }

    public void BackToOrigin()
    {
        tailAnim.StartBone = startedOnBone;
        dickBone.rotation = startRot;

        LeanTween.move(moveableDick, startPos, 0.2f).setOnComplete(GetNewDildo);
        decreasePoints = false;
    }

    void GetNewDildo()
    {
        LeanTween.delayedCall(1f, DisableDildo);
        availablePoints = basePoints;
        handler.GetNewDildo(false);
    }

    void DisableDildo()
    {
        moveableDick.SetActive(false);
    }

    public void IsChosenDildo(DildoHandler dildoHandler)
    {
        handler = dildoHandler;
        decreasePoints = true;
    }

    public void PausePointsDecrease()
    {
        decreasePoints = false;
    }

    public void Pulled()
    {
        handler.OnDildoPulled(availablePoints);
        BackToOrigin();
    }

    public void OnGrab()
    {
        tailAnim.StartBone = dickBone;
    }
}
