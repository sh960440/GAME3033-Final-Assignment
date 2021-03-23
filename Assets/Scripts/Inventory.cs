using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Item
{
    CAKE,
    DONUT,
    BURGER,
    BLUE,
    GREEN,
    RED
}
public class Inventory : MonoBehaviour
{
    public static int cakeAmount;
    public static int donutAmount;
    public static int burgerAmount;
    public static int blueAmount;
    public static int greenAmount;
    public static int redAmount;

    [SerializeField] private Text[] amountText;

    [SerializeField] private GameObject eatSound;
    [SerializeField] private GameObject drinkSound;

    // Start is called before the first frame update
    void Start()
    {
        cakeAmount = 0;
        donutAmount = 0;
        burgerAmount = 0;
        blueAmount = 0;
        greenAmount = 0;
        redAmount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (cakeAmount > 0)
            {
                GameController.foodValue += 0.1f;
                amountText[0].text = (--cakeAmount).ToString();
                Instantiate(eatSound, transform.position, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (donutAmount > 0)
            {
                GameController.foodValue += 0.15f;
                amountText[1].text = (--donutAmount).ToString();
                Instantiate(eatSound, transform.position, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (burgerAmount > 0)
            {
                GameController.foodValue += 0.2f;
                amountText[2].text = (--burgerAmount).ToString();
                Instantiate(eatSound, transform.position, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (blueAmount > 0)
            {
                GameController.drinkValue += 0.1f;
                amountText[3].text = (--blueAmount).ToString();
                Instantiate(drinkSound, transform.position, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (greenAmount > 0)
            {
                GameController.drinkValue += 0.15f;
                amountText[4].text = (--greenAmount).ToString();
                Instantiate(drinkSound, transform.position, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (redAmount > 0)
            {
                GameController.drinkValue += 0.2f;
                amountText[5].text = (--redAmount).ToString();
                Instantiate(drinkSound, transform.position, Quaternion.identity);
            }
        }

    }

    public void GetItem(Item itemType)
    {
        switch (itemType)
        {
            case Item.CAKE:
                cakeAmount++;
                amountText[(int)itemType].text = cakeAmount.ToString();
                break;
            case Item.DONUT:
                donutAmount++;
                amountText[(int)itemType].text = donutAmount.ToString();
                break;
            case Item.BURGER:
                burgerAmount++;
                amountText[(int)itemType].text = burgerAmount.ToString();
                break;
            case Item.BLUE:
                blueAmount++;
                amountText[(int)itemType].text = blueAmount.ToString();
                break;
            case Item.GREEN:
                greenAmount++;
                amountText[(int)itemType].text = greenAmount.ToString();
                break;
            case Item.RED:
                redAmount++;
                amountText[(int)itemType].text = redAmount.ToString();
                break;
        }
    }
}
