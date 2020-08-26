using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject medsPrefab = null;
    [SerializeField] GameObject bigMedsPrefab = null;
    [SerializeField] GameObject planePrefab = null; //for firingrate
    [SerializeField] GameObject pointA = null;
    [SerializeField] GameObject pointB = null;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(transform.position.x, pointA.transform.position.y);
    }

    public void Meds()
    {
        float pointAPosX = pointA.transform.position.x;
        float pointBPosX = pointB.transform.position.x;
        Vector3 powerUpPos = new Vector3(Random.Range(pointAPosX, pointBPosX), transform.position.y, transform.position.z);
        GameObject meds = Instantiate(medsPrefab, powerUpPos, transform.rotation) as GameObject;
        meds.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x, -5f);
    }
    
    public void BigMeds()
    {
        float pointAPosX = pointA.transform.position.x;
        float pointBPosX = pointB.transform.position.x;
        Vector3 powerUpPos = new Vector3(Random.Range(pointAPosX, pointBPosX), transform.position.y, transform.position.z);
        GameObject bigMeds = Instantiate(bigMedsPrefab, powerUpPos, transform.rotation) as GameObject;
        bigMeds.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x, -3.5f);
    }

    public void Plane()
    {
        float pointAPosX = pointA.transform.position.x;
        float pointBPosX = pointB.transform.position.x;
        Vector3 powerUpPos = new Vector3(Random.Range(pointAPosX, pointBPosX), transform.position.y, transform.position.z);
        GameObject plane = Instantiate(planePrefab, powerUpPos, transform.rotation) as GameObject;
        plane.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x, -7f);
    }
}
