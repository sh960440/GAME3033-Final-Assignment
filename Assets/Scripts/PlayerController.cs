using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public GameObject mainCamera;
    public GameController gameController;
    
    [Header("Movement")]
    public float runSpeed;
    public float jumpSpeed;
    public float gravity;
    private bool movingLeft = false;
    private bool movingRight = false;
    private float horizontalMoveTimer = 0.0f;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Sound effects")]
    [SerializeField] private GameObject coinSound;
    [SerializeField] private GameObject foodSound;
    [SerializeField] private GameObject hitSound;
    [SerializeField] private GameObject shiftSound;
    [SerializeField] private GameObject jumpSound;
    [SerializeField] private GameObject drowningSound;

    private bool startGameBool = false;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameBool)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                MoveLeft();

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                MoveRight();

            //if (characterController.isGrounded)
            if (transform.position.y <= 0.06f)
            {
                moveDirection = new Vector3 (0, 0, runSpeed);
                
                if (movingLeft)
                {
                    moveDirection += new Vector3 (-2, 0, 0);
                    horizontalMoveTimer += Time.deltaTime;
                    if (horizontalMoveTimer >= 0.3f)
                    {
                        horizontalMoveTimer = 0.0f;
                        movingLeft = false;
                        GetComponent<PlayerAnimationController>().isRunningLeft = false;
                    }
                }

                if (movingRight)
                {
                    moveDirection += new Vector3 (2, 0, 0);
                    horizontalMoveTimer += Time.deltaTime;
                    if (horizontalMoveTimer >= 0.3f)
                    {
                        horizontalMoveTimer = 0.0f;
                        movingRight = false;
                        GetComponent<PlayerAnimationController>().isRunningRight = false;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space)) 
                {
                    moveDirection += new Vector3(0, jumpSpeed, 0);
                    GetComponent<PlayerAnimationController>().isJumping = true;
                    Instantiate(jumpSound, transform.position, Quaternion.identity);
                }
            }
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
            GameController.distance += moveDirection.z * Time.deltaTime;

            // Camera follows the player
            Vector3 cameraFollowVector = new Vector3(transform.position.x, 1.6f, transform.position.z - 1.0f);
            mainCamera.transform.position = cameraFollowVector;
        }
    }

    IEnumerator StartGame()
    {
        gameController.GenerateInitialPath();
        yield return new WaitForSeconds(2.0f);
        startGameBool = true;
        GetComponent<PlayerAnimationController>().isRunning = true;
    }

    public void MoveLeft()
    {
        if (transform.position.x > 0.5f && movingRight == false)
        {
            movingLeft = true;
            GetComponent<PlayerAnimationController>().isRunningLeft = true;
            Instantiate(shiftSound, transform.position, Quaternion.identity);
        }
    }

    public void MoveRight()
    {
        if (transform.position.x < 0.7f && movingLeft == false)
        {
            movingRight = true;
            GetComponent<PlayerAnimationController>().isRunningRight = true;
            Instantiate(shiftSound, transform.position, Quaternion.identity);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit playerHit)
    {
        switch (playerHit.gameObject.tag)
        {
            case "Hurdle":
                if (startGameBool)
                {
                    Instantiate(hitSound, transform.position, Quaternion.identity);
                    Lose();
                } 
                break;
            case "Sea":
                if (startGameBool)
                {
                    Instantiate(drowningSound, transform.position, Quaternion.identity);
                    Lose();
                }
                break;
        }
    }

    void Lose()
    {
        startGameBool = false;
        transform.position = new Vector3(0.604f, 0.0f, -2.6f);
        GetComponent<PlayerAnimationController>().lost = true;
        mainCamera.transform.position = new Vector3(0.6f, 0.4f, -1.5f);
        mainCamera.transform.rotation = Quaternion.Euler(-15.0f, 180.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "NormalGround":
                GetComponent<PlayerAnimationController>().isJumping = false;
                break;
            case "PathTrigger":
                gameController.GenerateNewPath();
                break;
            case "GoldCoin":
                Destroy(other.gameObject);
                Instantiate(coinSound, transform.position, Quaternion.identity);
                GameController.score++;
                break;
            case "TreasureBox":
                Destroy(other.gameObject);
                Instantiate(coinSound, transform.position, Quaternion.identity);
                CheckTresureType(other.gameObject.name);
                break;
            case "Food":
                Destroy(other.gameObject);
                Instantiate(foodSound, transform.position, Quaternion.identity);
                CheckFoodType(other.gameObject.name);
                break;
        }
    }

    void CheckTresureType(string name)
    {
        string subString = name.Substring(0, 6);
        switch (subString)
        {
            case "Copper":
                GameController.score += 3;
                break;
            case "Silver":
                GameController.score += 6;
                break;
            case "Golden":
                GameController.score += 10;
                break;
        }
    }

    void CheckFoodType(string name)
    {
        switch (name)
        {
            case "Donuts":
                GameController.foodValue += 0.05f;
                break;
            case "Cake":
                GameController.foodValue += 0.1f;
                break;
            case "Waffle":
                GameController.foodValue += 0.15f;
                break;
            case "Hamburger":
                GameController.foodValue += 0.2f;
                break;
        }
    }
}
