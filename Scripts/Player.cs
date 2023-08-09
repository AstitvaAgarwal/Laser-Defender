using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f ;
    [SerializeField] float projectileFiringPeriod=0.1f;

    Coroutine firingCoroutine;
    
    void Start()
    {
        SetUpMoveBoundaries();
    }

    float xMin , yMin;
    float xMax , yMax;
  

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire(){
        if(Input.GetButtonDown("Fire1"))
        {
            firingCoroutine= StartCoroutine(FireContinuously()); //Calling coroutine // A method which can suspend its execution until thr yield instructions you gave it are met
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()//return type is IEnumerator for Coroutine
    {  
        while(true)
        {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject; // Quaternion.identity ensures that the new object will be in its "natural" orientation
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0,projectileSpeed);
        
        yield return new WaitForSeconds(projectileFiringPeriod); // will wait for projectileFiringPeriod seconds to execute the upcoming command
        } 
    }
    

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal")*Time.deltaTime * moveSpeed; //Time.deltatime will make framerate independent --> means the speed will be same on every computer
        var deltaY = Input.GetAxis("Vertical")*Time.deltaTime*moveSpeed;
        
        var newXPos = Mathf.Clamp(transform.position.x + deltaX,xMin,xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY,yMin,yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

      private void SetUpMoveBoundaries(){
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding; //ViewportToWorldPoint converts the position of something as it relates to the camera view , into a world space value --> bottom left to 0,0 no matter what the actual length is and top right to 1,1
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;

        yMin =gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax =gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }
}
