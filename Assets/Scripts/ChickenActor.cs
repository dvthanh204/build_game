using UnityEngine;

public sealed class ChickenActor : MonoBehaviour
{
    [SerializeField] private Transform body;
    private Vector3 destination;
    private float idleOffset;
    private float bobPhase;
    private bool isWalking;

    public string DisplayName { get; set; }
    public float Happiness { get; private set; } = 50f;
    public bool IsPlayer { get; set; }

    public void Initialize(string displayName, bool isPlayer)
    {
        DisplayName = displayName;
        IsPlayer = isPlayer;
        destination = transform.position;
        idleOffset = Random.Range(0f, 6.28f);
    }

    public void SetBody(Transform bodyTransform)
    {
        body = bodyTransform;
    }

    public void MoveTo(Vector3 position)
    {
        destination = new Vector3(position.x, transform.position.y, position.z);
        isWalking = true;
    }

    public void Pet()
    {
        Happiness = Mathf.Min(100f, Happiness + 8f);
    }

    public void Feed()
    {
        Happiness = Mathf.Min(100f, Happiness + 22f);
    }

    private void Update()
    {
        bobPhase += Time.deltaTime * (isWalking ? 13f : 4f);
        Vector3 flatDelta = destination - transform.position;
        flatDelta.y = 0f;

        if (flatDelta.sqrMagnitude > 0.04f)
        {
            Vector3 direction = flatDelta.normalized;
            transform.position += direction * (isWalking ? 2.4f : 1.1f) * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 8f);
        }
        else
        {
            isWalking = false;
        }

        if (body != null)
        {
            Vector3 local = body.localPosition;
            local.y = 0.32f + Mathf.Sin(bobPhase + idleOffset) * (isWalking ? 0.07f : 0.025f);
            body.localPosition = local;
            body.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(bobPhase * 0.5f) * 3f);
        }
    }
}
