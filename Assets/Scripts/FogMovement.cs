using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMovement : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] Fog Left;
    [SerializeField] Fog Right;
    [SerializeField] Fog Up;
    [SerializeField] Fog Bottom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Left.mustMove)
        {
            transform.Translate(new Vector3(-27.9375f, 0, 0));
            Left.mustMove = false;
        }
        if (Right.mustMove)
        {
            transform.Translate(new Vector3(27.9375f, 0, 0));
            Right.mustMove = false;
        }
        if (Up.mustMove)
        {
            transform.Translate(new Vector3(0,27.9375f, 0));
            Up.mustMove = false;
        }
        if (Bottom.mustMove)
        {
            transform.Translate(new Vector3(0,(-27.9375f), 0));
            Bottom.mustMove = false;
        }

    }
}
