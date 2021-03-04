using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float startGameTimer;

    public GameObject mainCamera;
    private Vector3 characterPosition;
    private Vector3 cameraFollowVector;

    private bool startGameBool;
    private bool endGameBool;
    public float runSpeed;
    public float jumpSpeed;
    public float gravity;

    private bool movingLeft;
    private bool movingRight;
    private bool jumping;
    private float horizontalMoveTimer;

    public GameObject[] pathArea;
    public GameObject endArea;
    private bool playerEnterPathBool;
    private int pathIndex;
    private int pathMax;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        GetComponent<PlayerAnimationController>().isRunning = false;
        GetComponent<PlayerAnimationController>().isRunningLeft = false;
        GetComponent<PlayerAnimationController>().isRunningRight = false;
        GetComponent<PlayerAnimationController>().isJumping = false;
        GetComponent<PlayerAnimationController>().won = false;
        GetComponent<PlayerAnimationController>().lost = false;

        mainCamera.GetComponent<Transform>().position = new Vector3(0.6f, 1.6f, -3.6f);
        mainCamera.GetComponent<Transform>().rotation = Quaternion.Euler(20.0f, 0.0f, 0.0f);
        
        startGameBool = false;
        endGameBool = false;
        startGameTimer = 0.0f;

        movingLeft = false;
        movingRight = false;
        jumping = false;
        horizontalMoveTimer = 0.0f;

        int randomPathInit_1 = Random.Range(0, 3);
        Instantiate(pathArea[randomPathInit_1], new Vector3(0.62f, 0, 8.0f), Quaternion.identity);
        int randomPathInit_2 = Random.Range(0, 3);
        Instantiate(pathArea[randomPathInit_2], new Vector3(0.62f, 0, 16.0f), Quaternion.identity);

        playerEnterPathBool = false;
        pathIndex = 2;
        pathMax = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameBool)
        {
            //if (characterController.isGrounded)
            if (transform.position.y <= 0.06)
            {
                moveDirection = new Vector3 (0, 0, 1);
                moveDirection = transform.TransformDirection(moveDirection);
                
                if (movingLeft)
                {
                    moveDirection = new Vector3 (-2, 0, 0);
                    moveDirection = transform.TransformDirection(moveDirection);
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
                    moveDirection = new Vector3 (2, 0, 0);
                    moveDirection = transform.TransformDirection(moveDirection);
                    horizontalMoveTimer += Time.deltaTime;
                    if (horizontalMoveTimer >= 0.3f)
                    {
                        horizontalMoveTimer = 0.0f;
                        movingRight = false;
                        GetComponent<PlayerAnimationController>().isRunningRight = false;
                    }
                }

                //if (Input.GetKeyDown(KeyCode.Space)) 
                if (jumping) 
                {
                    moveDirection = new Vector3(0, jumpSpeed, 1);
                    moveDirection = transform.TransformDirection(moveDirection);
                    //GetComponent<PlayerAnimationController>().isJumping = true;
                }
                moveDirection *= runSpeed;
            }
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
        }

        if (startGameBool)
        {
            characterPosition = GetComponent<Transform>().position;
            cameraFollowVector = new Vector3(characterPosition.x, 1.6f, characterPosition.z - 1.0f);
            mainCamera.GetComponent<Transform>().position = cameraFollowVector;
        }

        if (!endGameBool)
        {
            startGameTimer += Time.deltaTime;
            if (startGameTimer >= 1.0f)
            {
                startGameBool = true;
                startGameTimer = 0.0f;
                GetComponent<PlayerAnimationController>().isRunning = true;
            }
        }

        if (playerEnterPathBool)
        {
            int randomPathIndex = Random.Range(0, 3);
            Instantiate(pathArea[randomPathIndex], new Vector3(0.62f, 0, 8.0f * pathIndex), Quaternion.identity);
            
            playerEnterPathBool = false;
        }
    }

    public void moveLeftButton()
    {
        if (characterPosition.x > 0.1f && movingRight == false)
        {
            movingLeft = true;
            GetComponent<PlayerAnimationController>().isRunningLeft = true;
        }
    }

    public void moveRightButton()
    {
        if (characterPosition.x < 1.1f && movingLeft == false)
        {
            movingRight = true;
            GetComponent<PlayerAnimationController>().isRunningRight = true;
        }
    }

    public void JumpButton()
    {
        jumping = true;
        GetComponent<PlayerAnimationController>().isJumping = true;
    }

    void OnControllerColliderHit(ControllerColliderHit playerHit)
    {
        switch (playerHit.gameObject.tag)
        {
            case "GoalHouse":
                Debug.Log("You win");
                startGameBool = false;
                endGameBool = true;
                GetComponent<Transform>().position = new Vector3(0.604f, 0.0f, -2.6f); // Unsolved
                GetComponent<PlayerAnimationController>().won = true;
                mainCamera.GetComponent<Transform>().position = new Vector3(0.6f, 0.4f, -1.5f);
                mainCamera.GetComponent<Transform>().rotation = Quaternion.Euler(-15.0f, 180.0f, 0.0f);
                break;
            case "Sea":
                Debug.Log("You lose");
                startGameBool = false;
                endGameBool = true;
                GetComponent<Transform>().position = new Vector3(0.604f, 0.0f, -2.6f);
                GetComponent<PlayerAnimationController>().lost = true;
                mainCamera.GetComponent<Transform>().position = new Vector3(0.6f, 0.4f, -1.5f);
                mainCamera.GetComponent<Transform>().rotation = Quaternion.Euler(-15.0f, 180.0f, 0.0f);
                break;
        }
    }

    private void OnTriggerEnter(Collider playerTrigger)
    {
        switch (playerTrigger.tag)
        {
            case "NormalGround":
                jumping = false;
                GetComponent<PlayerAnimationController>().isJumping = false;
                break;

            case "PathTrigger":
                if (pathIndex < pathMax)
                {
                    pathIndex += 1;
                    playerEnterPathBool = true;
                }
                else if (pathIndex == pathMax)
                {
                    Quaternion endAreaRotation = Quaternion.Euler(0, 180, 0);
                    Instantiate(endArea, new Vector3(0.62f, 0.0f, 8.0f * pathIndex), endAreaRotation);
                    pathIndex += 1;
                }
                break;
        }
    }
}
