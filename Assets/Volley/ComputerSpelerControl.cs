using System;
using System.Collections;
using UnityEngine;

public class ComputerSpelerControl : MonoBehaviour
{

    [SerializeField]
    VolleybalControl _spel;
    [SerializeField]
    Rigidbody2D _ball;
    [SerializeField]
    Rigidbody2D _rigid;
    [SerializeField]
    GameObject _grond;
    [SerializeField]
    GameObject _startPunt;

    [SerializeField]
    float _jump;
    [SerializeField]
    float _speed;

    bool onGround = false;
    float beweging;
    bool wantToJump = false;
    float startx;

    void Start()
    {
        beweging = -_speed;
        _rigid.velocity = new Vector2(beweging, 0);
        startx = transform.position.x;
    }


    void FixedUpdate()
    {
        if (_spel.Status == VolleybalControl.VolleyStatusEnum.SPEL)
        {
            float vertical = 0f;

            if (onGround && wantToJump)
            {
                vertical = _jump;
                wantToJump = false;
            }
            else
            {
                beweging = PredictTrajectory();
                vertical = _rigid.velocity.y;
            }
            _rigid.velocity = new Vector2(beweging, vertical);
        }

    }

    private float PredictTrajectory()
    {
        RaycastHit2D hit = Physics2D.Raycast(_ball.transform.position, _ball.velocity);
        if (hit.collider == null)
        {
            return beweging;
        }
        if (hit.collider.gameObject != _grond) return beweging;

        float destination = hit.point.x;
        float me = transform.position.x;
        if (destination < me) beweging = -_speed;
        else if (destination > me) beweging = _speed;

        return beweging;
    }

    private IEnumerator Opslag()
    {
        Debug.Log("return to start pos");
        _rigid.velocity = Vector2.zero;
        transform.position = new Vector2(startx, transform.position.y);
        yield return new WaitForSeconds(1f);

        Debug.Log("BOOM");
        _rigid.velocity = new Vector2(-_speed, _jump);

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
        else if (collision.gameObject == _ball.gameObject)
        {
            //ignore
        }
        else
        {
            if (transform.position.x < startx)
            {
                beweging = _speed;
            }
            else
            {
                beweging = -_speed;
            }
            _rigid.velocity = new Vector2(beweging, _rigid.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _ball.gameObject)
        {
            //jump in the right direction:
            if (transform.position.x < _ball.transform.position.x
                && beweging < 0)
            {
                beweging = _speed;
            }
            else if (transform.position.x > _ball.transform.position.x
                && beweging > 0)
            {
                beweging = -_speed;
            }
            wantToJump = true;
        }
    }

    //dit linken we in Unity als een EventListener:
    public void VolleyBall_ReadyComputer()
    {
        StartCoroutine(Opslag());
    }
}
