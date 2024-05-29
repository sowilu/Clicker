using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 100f;
    public Vector3 direction;
    public bool randomise = true;

    void Start()
    {
        if (randomise)
        {
            direction.x = Random.Range(-1f, 1f);
            direction.y = Random.Range(-1f, 1f);
            direction.z = Random.Range(-1f, 1f);
        }

    }


    void Update()
    {
        transform.Rotate(direction * speed * Time.deltaTime);
    }
}
