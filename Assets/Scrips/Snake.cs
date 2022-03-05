using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
     //Create and declared a Array like List type where add the body for the snake 
    //when eat the apple
    public List<Transform> _body = new List<Transform>();
    //Prefab is a Object thas save in Unity like a model or objectecycle
    //create in Unity
    //_bodyPrefab and use for specify what is the component to add for the List
    public Transform _bodyPrefab;

    public Transform _tailPrefab;
    enum Movements { left, right, up, down }

    Movements _movement;
    public float _frameRate = 0.02f;
    public float _nextStep = 0.16f;
    Vector3 _lastPos;

    public Vector2 verticalLimits;
    public Vector2 horizontalLimits;

    public BoxCollider2D _gridArea;
    void Start()
    {
        RandomizePosition();
        transform.localRotation = Quaternion.Euler(0, 0, -90);
        _body[0].rotation = Quaternion.Euler(0, 0, 90);
        InvokeRepeating("Move", _frameRate, _frameRate);
    }
    void Move()
    {

        _lastPos = transform.position;
        Vector3 nextPos = Vector3.zero;
        if (_movement == Movements.up)
        {
            nextPos = Vector3.up;
        }
        else if (_movement == Movements.down)
        {
            nextPos = Vector3.down;
        }
        else if (_movement == Movements.left)
        {
            nextPos = Vector3.left;
        }
        else if (_movement == Movements.right)
        {
            nextPos = Vector3.right;
        }
        nextPos *= _nextStep;
        transform.position += nextPos;
        MoveBody();
    }
    public void MoveBody()
    {
        for (int i = 0; i < _body.Count; i++)
        {
            Vector3 temp = _body[i].position;
            _body[i].position = _lastPos;
            _lastPos = temp;
        }
    }

    public void RandomizePosition()
    {
        Bounds bounds = this._gridArea.bounds;

        // Pick a random position inside the bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        // Round the values to ensure it aligns with the grid
        x = Mathf.Round(x);
        y = Mathf.Round(y);

        transform.position = new Vector2(x, y);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _movement = Movements.up;
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            for (int i = 0; i < _body.Count; i++)
            {
                _body[i].rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _movement = Movements.down;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            for (int i = 0; i < _body.Count; i++)
            {
                _body[i].rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _movement = Movements.left;
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            for (int i = 0; i < _body.Count; i++)
            {
                _body[i].rotation = Quaternion.Euler(0, 0, -90);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _movement = Movements.right;
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            for (int i = 0; i < _body.Count; i++)
            {
                _body[i].rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Borders"))
        {
            print("Game Over");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else if (collider.CompareTag("Apple"))
        {
            print("Eat the apple");
            _body.Add(Instantiate(_bodyPrefab, _body[_body.Count - 1]
            .position, Quaternion.identity).transform);
            collider.transform.position = new Vector2(Random.Range(horizontalLimits.x, horizontalLimits.y), Random.Range(verticalLimits.x, verticalLimits.y));
        }
    }
}
