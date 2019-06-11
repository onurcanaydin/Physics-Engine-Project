using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using Vector3 = cyclone.Vector3;

public class GameManager : MonoBehaviour
{
    public Image jetpackFill;
    public bool downwardsGravity = true;
    public float jumpAcc = 1000f;
    public float runAcc = 5f;
    public float gravity = -5f;
    public float jetpackFuel = 100f;
    public float jetpackAcc = 20f;
    public float jetpackDrainCoeff = 40f;
    public float jetpackFillCoeff = 20f;
    public string nextLevel;
    public string friendsLevel;
    public string shuttleLevel;
    public ControlMode controlMode = ControlMode.normalControl;
    public bool isSpringLevel = false;
    public GameObject textPanel;
    public Text text;
    public Text gravityChangeCountText;
    public GameObject choicePanel;
    public GameObject endingPanel;
    public Text endingText;
    public bool isTriggeredOnce = false;
    public int gravityChangeCount;

    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PhysicsManager physicsManager;
    private Vector3 startPos;
    private string levelName;

    public enum LevelsWithSuit { Level_3, Level_4_Friends, Level_4_Shuttle, Level_5_Friends, Level_5_Shuttle};
    public enum ControlMode { noControl, normalControl, springControl };

    public UnityEvent springPointReached;

    void Start()
    {
        startPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Scene scene = SceneManager.GetActiveScene();
        levelName = scene.name;
        if(levelName == "Level_1")
        {
            controlMode = ControlMode.noControl;
            textPanel.SetActive(true);
            text.text = "Bob's communication with the base is cut off. He needs to go there to check the situation and recharge his suit";
        }
        else if(levelName == "Level_2")
        {
            isSpringLevel = true;
        }
        else if(levelName == "Level_3")
        {
            gravityChangeCount = 3;
            controlMode = ControlMode.noControl;
            textPanel.SetActive(true);
            text.text = "Bob reached the base. All he can see is mostly ashes, the base is exploded and it's the reason he couldn't communicate with them. Bob recharges his suit and goes after another expedition";
        }
        else if (levelName == "Level_4_Friends" || levelName == "Level_4_Shuttle" || levelName == "Level_5_Friends" || levelName == "Level_5_Shuttle")
        {
            gravityChangeCount = 3;
        }
        gravityChangeCountText.text = gravityChangeCount.ToString();
    }

    void Update()
    {
        if (player.particle.GetPosition().y > 13 || player.particle.GetPosition().y < -12)
        {
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.G) && player.usableSuit && gravityChangeCount > 0)
        {
            gravityChangeCount--;
            gravityChangeCountText.text = gravityChangeCount.ToString();
            downwardsGravity = !downwardsGravity;
        }
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }
        if(isSpringLevel && player.particle.GetPosition().x >= 100)
        {
            if (springPointReached != null)
            {
                springPointReached.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        if (player.boundingRectangle.Overlaps(physicsManager.endPointRectangle))
        {
            if(levelName == "Level_3")
            {
                ChoicePanel();
            }
            else if(levelName == "Level_5_Friends")
            {
                controlMode = ControlMode.noControl;
                endingPanel.SetActive(true);
                endingText.text = "Bob ran out of resources while searching for his friends. He couldn't return to the shuttle and he died";
            }
            else if(levelName == "Level_5_Shuttle")
            {
                controlMode = ControlMode.noControl;
                endingPanel.SetActive(true);
                endingText.text = "Bob made it to shuttle and returned to the Earth but he will never know whether he left his friends there or not";
            }
            else
            {
                NextLevel();
            }
        }
    }

    public void Reset()
    {
        if(controlMode == ControlMode.normalControl)
        {
            player.particle.SetAcceleration(0, 0, 0);
            player.particle.SetVelocity(0, 0, 0);
            player.particle.ClearAccumulator();
            player.particle.SetPosition(startPos.x, startPos.y, startPos.z);
            jetpackFuel = 100f;
            downwardsGravity = true;
            if (levelName == "Level_2")
            {
                physicsManager.throwable.tr.Clear();
                physicsManager.throwable.gameObject.SetActive(true);
                physicsManager.throwable.Reset();
                physicsManager.springBlock.SetActive(true);
                physicsManager.throwableEndPoint.SetActive(true);
                isTriggeredOnce = false;
            }
            else if (levelName == "Level_3" || levelName == "Level_4_Friends" || levelName == "Level_4_Shuttle" || levelName == "Level_5_Friends" || levelName == "Level_5_Shuttle")
            {
                gravityChangeCount = 3;
            }
            gravityChangeCountText.text = gravityChangeCount.ToString();
        }
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void ContinueGame()
    {
        controlMode = ControlMode.normalControl;
        textPanel.SetActive(false);
    }

    public void ChoicePanel()
    {
        if(isTriggeredOnce == false)
        {
            isTriggeredOnce = true;
            controlMode = ControlMode.noControl;
            player.particle.SetAcceleration(0, 0, 0);
            player.particle.SetVelocity(0, 0, 0);
            player.particle.ClearAccumulator();
            choicePanel.SetActive(true);
        }
    }

    public void Friends()
    {
        SceneManager.LoadScene(friendsLevel);
    }

    public void Shuttle()
    {
        SceneManager.LoadScene(shuttleLevel);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
