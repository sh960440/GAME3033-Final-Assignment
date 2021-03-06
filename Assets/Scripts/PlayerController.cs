﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject hungerSound;

    [Header("Countdown")]
    [SerializeField] private Image countdown;
    [SerializeField] private Sprite[] numbers;

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
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                MoveLeft();

            if (Input.GetKeyDown(KeyCode.RightArrow))
                MoveRight();

            //if (characterController.isGrounded)
            if (transform.position.y <= 0.081f)
            {
                moveDirection = new Vector3 (0, 0, runSpeed);
                
                if (Input.GetKeyDown(KeyCode.Space)) 
                {
                    moveDirection += new Vector3(0, jumpSpeed, 0);
                    GetComponent<PlayerAnimationController>().isJumping = true;
                    Instantiate(jumpSound, transform.position, Quaternion.identity);
                }
                else
                {
                    if (movingLeft)
                    {
                        moveDirection += new Vector3 (-2.5f, 0, 0);
                        horizontalMoveTimer += Time.deltaTime;
                        if (horizontalMoveTimer >= 0.3f)
                        {
                            horizontalMoveTimer = 0.0f;
                            movingLeft = false;
                        }
                    }
                    else if (movingRight)
                    {
                        moveDirection += new Vector3 (2.5f, 0, 0);
                        horizontalMoveTimer += Time.deltaTime;
                        if (horizontalMoveTimer >= 0.3f)
                        {
                            horizontalMoveTimer = 0.0f;
                            movingRight = false;
                        }
                    }
                }

                
            }
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
            GameController.distance += moveDirection.z * Time.deltaTime;

            // Camera follows the player
            Vector3 cameraFollowVector = new Vector3(transform.position.x, 1.6f, transform.position.z - 1.0f);
            mainCamera.transform.position = cameraFollowVector;

            // Food & Drink
            GameController.foodValue -= Time.deltaTime / 200.0f;
            GameController.drinkValue -= Time.deltaTime / 200.0f;
            if (GameController.foodValue <= 0 || GameController.drinkValue <= 0)
            {   
                startGameBool = false;
                Instantiate(hungerSound, transform.position, Quaternion.identity);
                GameOver();
            }
        }
    }

    IEnumerator StartGame()
    {
        gameController.GenerateInitialPath();
        
        yield return new WaitForSeconds(1.0f);
        countdown.sprite = numbers[1];
        yield return new WaitForSeconds(1.0f);
        countdown.sprite = numbers[2];
        yield return new WaitForSeconds(1.0f);
        countdown.gameObject.SetActive(false);

        startGameBool = true;
        GetComponent<PlayerAnimationController>().isRunning = true;
    }

    public void MoveLeft()
    {
        if (transform.position.x > 0.5f && movingRight == false)
        {
            movingLeft = true;
            Instantiate(shiftSound, transform.position, Quaternion.identity);
        }
    }

    public void MoveRight()
    {
        if (transform.position.x < 0.7f && movingLeft == false)
        {
            movingRight = true;
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
                    GameOver();
                } 
                break;
            case "Sea":
                if (startGameBool)
                {
                    Instantiate(drowningSound, transform.position, Quaternion.identity);
                    GameOver();
                }
                break;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameController.DisplayGameoverScreen();
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
            case "Drink":
                Destroy(other.gameObject);
                Instantiate(foodSound, transform.position, Quaternion.identity);
                CheckDrinkType(other.gameObject.name);
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
            case "Cake":
                FindObjectOfType<Inventory>().GetItem(Item.CAKE);
                break;
            case "Donuts":
                FindObjectOfType<Inventory>().GetItem(Item.DONUT);
                break;
            case "Hamburger":
                FindObjectOfType<Inventory>().GetItem(Item.BURGER);
                break;
        }
    }

    void CheckDrinkType(string name)
    {
        switch (name)
        {
            case "Drink_Blue":
                FindObjectOfType<Inventory>().GetItem(Item.BLUE);
                break;
            case "Drink_Green":
                FindObjectOfType<Inventory>().GetItem(Item.GREEN);
                break;
            case "Drink_Red":
                FindObjectOfType<Inventory>().GetItem(Item.RED);
                break;
        }
    }
}
