using UnityEngine;
using cyclone;
using Vector3 = cyclone.Vector3;

public class ThrowableController : MonoBehaviour
{
    public Vector3 startPos;
    public Particle throwableParticle;
    public BoundingRectangle boundingRectangle;
    public ParticleContact particleContact;
    public AnchoredSpring anchoredSpring;
    public GameManager gameManager;

    private void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        throwableParticle = new Particle(new Vector3(startPos.x, startPos.y, startPos.z), 0.9f, 1f);
        boundingRectangle = new BoundingRectangle(new Vector3(startPos.x, startPos.y, startPos.z), 1, 1);
        particleContact = new ParticleContact(throwableParticle, 0f, new Vector3());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void FixedUpdate()
    {
        if(gameManager.controlMode == GameManager.ControlMode.springControl)
        {
            Vector3 accVector = new Vector3();
            accVector += new Vector3(0, gameManager.gravity, 0);
            throwableParticle.AddForce(accVector.x, accVector.y, accVector.z);
            throwableParticle.Integrate(Time.fixedDeltaTime);
            boundingRectangle.center = throwableParticle.GetPosition();
            transform.position = throwableParticle.GetPosition().CycloneToUnity();
        }
    }

    private void OnMouseUp()
    {
        if (gameManager.controlMode == GameManager.ControlMode.springControl)
        {
            UnityEngine.Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mp.z = 0;
            Vector3 mousePos = new Vector3(mp.x, mp.y, mp.z);
            anchoredSpring.ApplySpringForce(throwableParticle, mousePos);
        }
    }

    public void Reset()
    {
        throwableParticle.SetAcceleration(0, 0, 0);
        throwableParticle.SetVelocity(0, 0, 0);
        throwableParticle.ClearAccumulator();
        throwableParticle.SetPosition(startPos.x, startPos.y, startPos.z);
        boundingRectangle.center = throwableParticle.GetPosition();
    }
}
