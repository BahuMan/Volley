using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VolleybalControl : MonoBehaviour
{

    [SerializeField] Transform _startPositieWillekeurig;
    [SerializeField] Transform _startPositieSpeler1;
    [SerializeField] Transform _startPositieComputer;
    [SerializeField] Collider2D speler1Grond;
    [SerializeField] Collider2D computerGrond;
    [SerializeField] Text speler1ScoreText;
    [SerializeField] Text computerScoreText;
    [SerializeField] Rigidbody2D _rigidBall;
    [SerializeField] Rigidbody2D _rigidSpeler1;
    [SerializeField] Rigidbody2D _rigidComputer;
    [SerializeField] float maxSpeed;

    int speler1Score = 0;
    int computerScore = 0;

    public enum VolleyStatusEnum { BEGIN, SPEL, OPSLAG}
    public VolleyStatusEnum VolleyStatus = VolleyStatusEnum.BEGIN;

    public VolleyStatusEnum Status { get => VolleyStatus; private set => VolleyStatus = value; }

    public UnityEvent ReadySpeler1 = new UnityEvent();
    public UnityEvent ReadyComputer = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        StartWillekeurig();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x < -10 || transform.position.x > 10 || transform.position.y < -10 || transform.position.y > 10)
        {
            Debug.Log("Bal buiten spel... reset");
            StartWillekeurig();
        }

        if (_rigidBall.velocity.magnitude > maxSpeed)
        {
            _rigidBall.velocity = maxSpeed * _rigidBall.velocity.normalized;
        }
    }

    public void StartWillekeurig()
    {
        StartCoroutine(StartWillekeurigIntern());
    }

    public void OpslagSpeler1()
    {
        StartCoroutine(StartOpslagIntern(_startPositieSpeler1.position));
    }

    public void OpslagSpeler2()
    {
        StartCoroutine(StartOpslagIntern(_startPositieComputer.position));
    }

    private IEnumerator StartOpslagIntern(Vector2 startpos)
    {
        float zwaartekracht = _rigidBall.gravityScale;
        HangStil(startpos);
        this.VolleyStatus = VolleyStatusEnum.OPSLAG;
        while (this.VolleyStatus == VolleyStatusEnum.OPSLAG)
        {
            yield return null; //wait every frame until player has hit ball
        }
        //now that the player has hit the ball, it can interact with gravity again:
        _rigidBall.gravityScale = zwaartekracht;
        this.VolleyStatus = VolleyStatusEnum.SPEL;
    }

    private void HangStil(Vector2 positie)
    {
        transform.position = positie;
        _rigidBall.angularVelocity = 0;
        _rigidBall.velocity = Vector2.zero;
        _rigidBall.gravityScale = 0;
    }

    private IEnumerator StartWillekeurigIntern()
    {
        VolleyStatus = VolleyStatusEnum.BEGIN;
        float zwaartekracht = _rigidBall.gravityScale;
        HangStil(_startPositieWillekeurig.position);

        yield return new WaitForSeconds(2);

        VolleyStatus = VolleyStatusEnum.SPEL;
        _rigidBall.gravityScale = zwaartekracht;
        _rigidBall.velocity = Random.insideUnitCircle.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == speler1Grond)
        {
            computerScore += 1;
            computerScoreText.text = computerScore.ToString();
            OpslagSpeler1();
            if (ReadySpeler1 != null) ReadySpeler1.Invoke();
        }
        else if (collision.collider == computerGrond)
        {
            speler1Score += 1;
            speler1ScoreText.text = speler1Score.ToString();
            OpslagSpeler2();
            if (ReadyComputer != null) ReadyComputer.Invoke();
        }
        else if (VolleyStatus == VolleyStatusEnum.OPSLAG
            && (collision.rigidbody == _rigidSpeler1
            || collision.rigidbody == _rigidComputer)) {

            VolleyStatus = VolleyStatusEnum.SPEL;
        }
    }
}
