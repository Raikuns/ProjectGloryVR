using UnityEngine;

public class Dildo : MonoBehaviour
{
    RopeGrabber dick;

    [SerializeField] public GameObject moveableDick;

    public float availablePoints = 10;
    float basePoints;
    bool decreasePoints = false;

    [HideInInspector] public DildoHandler handler;

    [SerializeField] Transform penisLook;

    Vector3 startPos;

    [HideInInspector] public Vector3 grabbedPos;

    private void Awake()
    {
        basePoints = availablePoints;
        dick = GetComponentInChildren<RopeGrabber>();
        dick.dildo = this;

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
            availablePoints -= Time.deltaTime;
            if (availablePoints < 1)
            {
                BackToOrigin();
            }
        }

        MoveDickAccordingly();
    }

    void MoveDickAccordingly()
    {
        Vector3 desiredPos = new(penisLook.position.x, penisLook.position.y, dick.transform.position.z + 0.18f);

        penisLook.position = desiredPos;
    }

    public void BackToOrigin()
    {
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
}
