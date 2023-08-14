using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.IO;

[System.Serializable]
public class MyCollection
{
    public List<Vector2> positions = new List<Vector2>();
    public List<bool> click = new List<bool>();
}

public class Pointer : MonoBehaviour
{
    [SerializeField] private float _distance = 10;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _clickDelay = 0.1f;
    [SerializeField] private float _hitDelay = 0.1f;
    [SerializeField] private bool _isRecording = true;
    [SerializeField] private bool _isReplaying = false;

    public event UnityAction<Vector2> Clicked;
    private float _lastX;
    private float _lastRight = 0;
    private List<Vector2> _cursorPositions = new List<Vector2>();
    private List<bool> _mouseClick = new List<bool>();
    private int _currentFrame = 0;
    private Vector2 _lastCursorPosition;
    private bool _clicked = false;
    private int _stateIndex = 1;

    public void Play(string animation) => _animator.SetTrigger(animation);

    public void ResetAngry() => _stateIndex = 1;

    public void AddAngry() => _stateIndex = _stateIndex == 3 ? 1 : _stateIndex + 1;

    private void Awake()
    {
        Cursor.visible = false;
        if (_isReplaying)
        {
            MyCollection collection = JsonUtility.FromJson<MyCollection>(ReadString());
            _cursorPositions = collection.positions;
            _mouseClick = collection.click;
        }
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isRecording)
        {
            Save();
            _isRecording = false;
            Debug.Log("SAVED!");
        }

        if (!_isReplaying)
        {
            HandleMousePosition(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                _clicked = true;
                Cursor.visible = false;
                string animation = "click";
                switch (_stateIndex)
                {
                    case 2:
                        animation = "angryClick";
                        StartCoroutine(ClickAfter(_clickDelay));
                        break;
                    case 3:
                        animation = "hit";
                        StartCoroutine(ClickAfter(_hitDelay));
                        break;
                    default:
                        StartCoroutine(ClickAfter(_clickDelay));
                        break;
                }
                _animator.SetTrigger(animation);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _stateIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _stateIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            _stateIndex = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            _animator.SetTrigger("angry");
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            _animator.SetTrigger("ok");
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            _animator.SetTrigger("thumbUp");
    }

    private IEnumerator ClickAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Clicked?.Invoke(Input.mousePosition);
    }

    private IEnumerator ClickAfter(Vector2 mousePosition)
    {
        yield return new WaitForSeconds(_clickDelay);
        Clicked?.Invoke(mousePosition);
    }

    private void Save()
    {
        MyCollection collection = new MyCollection();
        collection.positions = _cursorPositions;
        collection.click = _mouseClick;

        WriteString(JsonUtility.ToJson(collection));
        _currentFrame = 0;
        _isRecording = false;
    }

    private void HandleMousePosition(Vector2 mousePosition)
    {
        Vector2 cameraSize = new Vector2(Camera.main.orthographicSize * (float)((float)Screen.width / (float)Screen.height) * 2, Camera.main.orthographicSize * 2);
        float x = cameraSize.x * (mousePosition.x / Screen.width) - cameraSize.x / 2;
        float y = cameraSize.y * (mousePosition.y / Screen.height) - cameraSize.y / 2;
        transform.localPosition = new Vector3(x, y, transform.localPosition.z);

        //Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        //transform.position = Camera.main.transform.position + ray.direction * _distance;

        float speed = transform.position.x - _lastX;
        float right = 0.5f + speed * 1.5f;

        if (right - _lastRight > 0.02f)
            right = _lastRight + 0.02f;
        else if (_lastRight - right > 0.02f)
            right = _lastRight - 0.02f;

        _animator.SetFloat("right", right);
        _lastX = transform.position.x;
        _lastRight = right;
    }

    private IEnumerator Move(Vector2 from, Vector2 to)
    {
        _lastCursorPosition = to;
        float duration = Time.fixedDeltaTime;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            HandleMousePosition(Vector2.Lerp(from, to, time / duration));
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (_isRecording)
        {
            _cursorPositions.Add(Input.mousePosition);
            _mouseClick.Add(_clicked);

            _clicked = false;
        }
        else if (_isReplaying)
        {
            StartCoroutine(Move(_lastCursorPosition, _cursorPositions[_currentFrame]));
            if (_mouseClick[_currentFrame])
            {
                _animator.SetTrigger("click");
                StartCoroutine(ClickAfter(_lastCursorPosition));
            }

            _currentFrame++;
            if (_currentFrame > _cursorPositions.Count - 1)
                _currentFrame = 0;
        }

        
    }

    private void WriteString(string data)
    {
        string path = "Assets/test.json";

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(data);
        writer.Close();
    }

    private string ReadString()
    {
        string path = "Assets/test.json";

        StreamReader reader = new StreamReader(path);
        string result = reader.ReadToEnd();
        reader.Close();

        return result;
    }
}
