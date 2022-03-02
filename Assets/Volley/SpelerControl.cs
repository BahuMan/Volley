using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelerControl : MonoBehaviour
{

    [SerializeField]
    Rigidbody2D _rigid;
    [SerializeField]
    GameObject _grond;

    [SerializeField]
    float jump;
    [SerializeField]
    float speed;

    bool onGround = false;
    bool wantToJump = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            wantToJump = true;
        }

        float vertical = _rigid.velocity.y;
        if (onGround && wantToJump) //this way, if we pressed the button while not on the ground, our man will jump as soon as he lands
        {
            vertical = jump;
            wantToJump = false;
        }
        _rigid.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), vertical);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == _grond)
        {
            onGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _grond)
        {
            onGround = true;
        }
    }
}
