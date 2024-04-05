using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        // set's camera's position at the start of game
        offset = transform.position - player.transform.position;
    }

    // Late Update is called once per frame after all other updates are done
    void LateUpdate()
    {
        // camera aligns with player position
        transform.position = player.transform.position + offset;
    }
}
