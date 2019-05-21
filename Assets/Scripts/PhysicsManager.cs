using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = cyclone.Vector3;

public class PhysicsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platforms;
    [SerializeField]
    private GameObject endPoint;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private GameManager gameManager;

    private List<BoundingRectangle> rectangles;
    public BoundingRectangle endPointRectangle;
    public BoundingRectangle throwableEndPointRectangle;
    public BoundingRectangle springBlockRectangle;
    public GameObject springBlock;
    public GameObject throwableEndPoint;
    public ThrowableController throwable;

    public UnityEvent throwableCollidedWithEndPoint;

    void Start()
    {
        rectangles = new List<BoundingRectangle>();
        foreach (GameObject gobj in platforms)
        {
            Vector3 center = new Vector3
            {
                x = gobj.transform.position.x,
                y = gobj.transform.position.y,
                z = gobj.transform.position.z
            };
            BoundingRectangle newbr = new BoundingRectangle(center, gobj.transform.lossyScale.x, gobj.transform.lossyScale.y);
            rectangles.Add(newbr);
        }
        Vector3 endPointCenter = new Vector3
        {
            x = endPoint.transform.position.x,
            y = endPoint.transform.position.y,
            z = endPoint.transform.position.z
        };
        endPointRectangle = new BoundingRectangle(endPointCenter, endPoint.transform.lossyScale.x, endPoint.transform.lossyScale.y);

        Vector3 throwableEndPointCenter = new Vector3
        {
            x = throwableEndPoint.transform.position.x,
            y = throwableEndPoint.transform.position.y,
            z = throwableEndPoint.transform.position.z
        };
        throwableEndPointRectangle = new BoundingRectangle(throwableEndPointCenter, throwableEndPoint.transform.lossyScale.x, throwableEndPoint.transform.lossyScale.y);

        Vector3 springBlockCenter = new Vector3
        {
            x = springBlock.transform.position.x,
            y = springBlock.transform.position.y,
            z = springBlock.transform.position.z
        };
        springBlockRectangle = new BoundingRectangle(springBlockCenter, springBlock.transform.lossyScale.x, springBlock.transform.lossyScale.y);
    }
    
    void FixedUpdate()
    {
        foreach (BoundingRectangle rec in rectangles)
        {
            if (player.boundingRectangle.Overlaps(rec))
            {
                player.particleContact.penetration = player.boundingRectangle.penetration;
                player.particleContact.contactNormal = player.boundingRectangle.contactNormal;
                player.particleContact.Resolve(Time.fixedDeltaTime);
                break;
            }
        }

        if (gameManager.isSpringLevel)
        {
            foreach (BoundingRectangle rect in rectangles)
            {
                if (throwable.boundingRectangle.Overlaps(rect))
                {
                    throwable.particleContact.penetration = throwable.boundingRectangle.penetration;
                    throwable.particleContact.contactNormal = throwable.boundingRectangle.contactNormal;
                    throwable.particleContact.Resolve(Time.fixedDeltaTime);
                    break;
                }
            }

            if (throwable.boundingRectangle.Overlaps(springBlockRectangle))
            {
                throwable.particleContact.penetration = throwable.boundingRectangle.penetration;
                throwable.particleContact.contactNormal = throwable.boundingRectangle.contactNormal;
                throwable.particleContact.Resolve(Time.fixedDeltaTime);
            }

            if (throwable.boundingRectangle.Overlaps(throwableEndPointRectangle))
            {
                throwable.gameObject.SetActive(false);
                springBlock.SetActive(false);
                throwableEndPoint.SetActive(false);
                if (throwableCollidedWithEndPoint != null)
                {
                    throwableCollidedWithEndPoint.Invoke();
                }
            }
        }
    }
}
