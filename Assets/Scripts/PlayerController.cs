using UnityEngine;
using UnityEngine.SceneManagement;
using cyclone;
using Vector3 = cyclone.Vector3;

public class PlayerController : MonoBehaviour
{
    public Particle particle;
    public BoundingRectangle boundingRectangle;
    public ParticleContact particleContact;
    public GameManager gameManager;
    public bool usableSuit = false;

    [SerializeField]
    private bool jump = false;
    [SerializeField]
    private bool right = false;
    [SerializeField]
    private bool left = false;
    [SerializeField]
    private bool grounded = false;
    [SerializeField]
    private bool jetpacking = false;

    private float previousY;
    private Vector3 eventPoint;

    void Start()
    {
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        eventPoint = new Vector3(100f, -6f, 0);
        particle = new Particle(new Vector3(startPos.x, startPos.y, startPos.z), 0.8f, 1f);
        boundingRectangle = new BoundingRectangle(new Vector3(startPos.x, startPos.y, startPos.z), 1, 1);
        particleContact = new ParticleContact(particle, 0f, new Vector3());

        Scene scene = SceneManager.GetActiveScene();
        string[] LevelsWithSuit = System.Enum.GetNames(typeof(GameManager.LevelsWithSuit));
        foreach (string level in LevelsWithSuit){
            if(scene.name == level)
            {
                usableSuit = true;
            }
        }
    }

    private void Update()
    {
        if(gameManager.controlMode == GameManager.ControlMode.normalControl)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                jump = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                left = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                right = true;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                jetpacking = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(previousY == particle.GetPosition().y)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        Vector3 accVector = new Vector3();
        if (jump == true)
        {
            jump = false;
            if(grounded == true)
            {
                grounded = false;
                if (gameManager.downwardsGravity)
                {
                    accVector += new Vector3(0, gameManager.jumpAcc, 0);
                }
                else
                {
                    accVector += new Vector3(0, -gameManager.jumpAcc, 0);
                }
            }
        }
        if (right == true)
        {
            accVector += new Vector3(gameManager.runAcc, 0, 0);
            right = false;
        }
        if (left == true)
        {
            accVector += new Vector3(-gameManager.runAcc, 0, 0);
            left = false;
        }
        if (jetpacking == true && gameManager.jetpackFuel > 0 && usableSuit)
        {
            grounded = false;
            if (gameManager.downwardsGravity)
            {
                accVector += new Vector3(0, gameManager.jetpackAcc, 0);
            }
            else
            {
                accVector += new Vector3(0, -gameManager.jetpackAcc, 0);

            }
            gameManager.jetpackFuel -= gameManager.jetpackDrainCoeff * Time.fixedDeltaTime;
            jetpacking = false;
        }
        if (grounded == true && gameManager.jetpackFuel < 100f)
        {
            gameManager.jetpackFuel += gameManager.jetpackFillCoeff * Time.fixedDeltaTime;
        }
        gameManager.jetpackFill.fillAmount = gameManager.jetpackFuel / 100;
        if (gameManager.downwardsGravity)
        {
            accVector += new Vector3(0, gameManager.gravity, 0);
        }
        else
        {
            accVector += new Vector3(0, -gameManager.gravity, 0);
        }

        previousY = particle.GetPosition().y;

        particle.SetForceAccum(accVector.x, accVector.y, accVector.z);
        particle.Integrate(Time.fixedDeltaTime);
        boundingRectangle.center = particle.GetPosition();
        transform.position = particle.GetPosition().CycloneToUnity();        
    }

    public void SpringPopup()
    {
        if (!gameManager.isTriggeredOnce)
        {
            particle.SetAcceleration(0, 0, 0);
            particle.SetVelocity(0, 0, 0);
            particle.ClearAccumulator();
            particle.SetPosition(eventPoint.x, eventPoint.y, eventPoint.z);
            gameManager.jetpackFuel = 100f;
            gameManager.downwardsGravity = true;
            gameManager.controlMode = GameManager.ControlMode.noControl;
            gameManager.textPanel.SetActive(true);
            gameManager.text.text = "There is an obstacle Bob can't pass without his jetpack. He needs to hit the trigger on the other side to pass.";
        }
    }

    public void ChangeToSpringControl()
    {
        gameManager.isTriggeredOnce = true;
        gameManager.textPanel.SetActive(false);
        gameManager.controlMode = GameManager.ControlMode.springControl;
    }

    public void ChangeToPlayerControl()
    {
        gameManager.controlMode = GameManager.ControlMode.normalControl;
    }
}
