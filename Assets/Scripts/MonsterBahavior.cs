using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBahavior : MonoBehaviour
{
    [SerializeField] private bool isMovingHorizontally;
    [SerializeField] private float speed;
    [SerializeField] private float center;
    [SerializeField] private float boundary;


    // Start is called before the first frame update
    void Start()
    {
        if (isMovingHorizontally)
        {
            transform.position = new Vector3(Random.Range(center - boundary, center + boundary), transform.position.y, transform.position.z);
            speed = Random.Range(0, 2) == 0 ? speed : -speed;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, Random.Range(center - boundary, center + boundary), transform.position.z);
            speed = Random.Range(0, 2) == 0 ? speed : -speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingHorizontally)
        {
            if (transform.position.x >= center + boundary)
                speed = -speed;
            else if (transform.position.x <= center - boundary)
                speed = -speed;
            
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            if (transform.position.y >= center + boundary)
                speed = -speed;
            else if (transform.position.y <= center - boundary)
                speed = -speed;
            
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
        }
    }
}
