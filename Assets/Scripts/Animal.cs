using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RSG;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Outline))]
public class Animal : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Color _countColor;

    private Animator _animator;
    private Outline _outline;
    private Damping _damp = new Damping(0.5f, 2, 0, 1);
    private Damping _errorDamp = new Damping(0.1f, 5, 0, 3);
    private Vector3 _aviaryDoorPosition;
    private Vector3 _startShakePosition;
    private IPromiseTimer _timer = new PromiseTimer();
    private Vector3 _baseScale;

    private Coroutine _moveTask;
    private Coroutine _rotateTask;
    private Coroutine _pressTask;
    private Coroutine _shakeTask;

    public int ID => _id;
    public Color CountColor => _countColor;
    public Vector3 TargetPosition { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _outline = GetComponent<Outline>();
        _baseScale = transform.localScale;
    }

    private void Start()
    {
        StartCoroutine(RandomIdle());
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private IEnumerator RandomIdle()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            string[] triggers = new string[] { "lookLeft", "lookRight", "munch", "munch", "munch", "munch", "munch", "munch" };
            _animator.SetTrigger(triggers[Random.Range(0, triggers.Length)]);

        }
    }

    public void SetID(int ID)
    {
        _id = ID;
    }

    public void Select()
    {
        _outline.enabled = true;
        if (_pressTask != null)
            StopCoroutine(_pressTask);

        _pressTask = StartCoroutine(ShowPress());
    }

    public void Unselect()
    {
        _outline.enabled = false;
    }

    public void Shake()
    {
        PlayAnimation("fear");
        if (_shakeTask != null)
        {
            StopCoroutine(_shakeTask);
            transform.position = _startShakePosition;
        }

        _shakeTask = StartCoroutine(ShowShake());
    }

    private IPromise _movePromise;

    public void Go(Vector3 targetPosition, float duration)
    {
        TargetPosition = targetPosition;
        _timer.Cancel(_movePromise);

        _movePromise = Move(targetPosition, duration).Then(() =>
        {
            _movePromise = RotateBack(0.25f);
        });
    }

    private IPromise Move(Vector3 targetPosition, float duration)
    {
        _animator.SetBool("isMoving", true);

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Vector3 delta = targetPosition - position;
        Quaternion targetRotation = delta.magnitude > 0.1f ? Quaternion.LookRotation(targetPosition - position, Vector3.up) : rotation;
        CubicBezier easing = new CubicBezier(Easing.EaseInOut);

        var promise = new Promise();
        _timer.WaitWhile(time =>
        {
            float value = easing.GetValue(time.elapsedTime / duration);
            transform.position = Vector3.Lerp(position, targetPosition, value);
            transform.rotation = Quaternion.Lerp(rotation, targetRotation, value * 2);
            return time.elapsedTime < duration;
        }).Then(() =>
        {
            promise.Resolve();
            _animator.SetBool("isMoving", false);
        });

        return promise;
    }

    private void Stretch(float progress)
    {
        Vector3 scale = _baseScale;
        Vector3 targetScale = _baseScale;
        targetScale.x *= 0.75f;
        targetScale.y *= 0.8f;
        targetScale.z *= 1.5f;
        transform.localScale = Vector3.Lerp(scale, targetScale, GetStretchValue(progress));
    }

    private float GetStretchValue(float x)
    {
        return 1 - Mathf.Pow(2 * x - 1, 4);
    }

    private IPromise RotateBack(float duration)
    {
        Quaternion rotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        CubicBezier easing = new CubicBezier(Easing.EaseInOut);
        return _timer.WaitWhile(timeData =>
        {
            transform.rotation = Quaternion.Lerp(rotation, targetRotation, easing.GetValue(timeData.elapsedTime / duration));
            return timeData.elapsedTime < duration;
        });
    }

    public IPromise MoveFromAviary(Vector3 target)
    {
        SmoothPath path = new SmoothPath(transform.position, target, new Vector3[] { _aviaryDoorPosition }, Vector3.forward, target - _aviaryDoorPosition, 2f);
        var promise = new Promise();
        _animator.SetBool("isMoving", true);
        MoveAlongPath(path, 0.5f).Then(() =>
        {
            transform.position = target;
            _animator.SetBool("isMoving", false);
            RotateBack(0.2f);
            promise.Resolve();
        });
        return promise;
    }

    public IPromise MoveToAviary(Aviary aviary, float duration, Vector3 targetPosition)
    {
        _aviaryDoorPosition = aviary.DoorPosition;
        SmoothPath path = new SmoothPath(transform.position, targetPosition, new Vector3[] { aviary.DoorPosition + aviary.transform.forward * 5 }, transform.forward, transform.forward, 3f);
        var promise = new Promise();
        _animator.SetBool("isMoving", true);
        MoveAlongPath(path, duration).Then(() =>
        {
            transform.position = targetPosition;
            _animator.SetBool("isMoving", false);
            RotateBack(0.2f);
            promise.Resolve();
        });
        return promise;
    }

    private IPromise MoveAlongPath(SmoothPath path, float duration)
    {
        CubicBezier easing = new CubicBezier(Easing.EaseInOut);
        float t = 0;
        return _timer.WaitWhile(timeData =>
        {
            float value = easing.GetValue(t);
            Vector3 newPosition = path.GetPosition(value);
            Vector3 direction = newPosition - transform.position;
            if (direction.magnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(direction);

            transform.position = newPosition;
            Stretch(t);

            t = timeData.elapsedTime / duration;
            return t < 1;
        });
    }

    public void PlayAnimation(string name, float delay = 0)
    {
        StartCoroutine(PlayAnimationAfter(name, delay));
    }

    private IEnumerator PlayAnimationAfter(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetTrigger(name);
    }

    private IEnumerator ShowPress()
    {
        float duration = 0.3f;
        float time = 0;
        Vector3 scale = transform.localScale;
        Vector3 targetScale = _baseScale;
        targetScale.x *= 1.5f;
        targetScale.y *= 0.6f;
        targetScale.z *= 1.3f;

        while (time < duration)
        {
            float value = _damp.GetValue(time / duration);
            float x = scale.x + (targetScale.x - scale.x) * value;
            float y = scale.y + (targetScale.y - scale.y) * value;
            float z = scale.z + (targetScale.z - scale.z) * value;
            transform.localScale = new Vector3(x, y, z);
            yield return null;
            time += Time.deltaTime;
        }

        transform.localScale = _baseScale;
    }

    private IEnumerator ShowShake()
    {
        _startShakePosition = transform.position;
        float duration = 0.5f;
        float time = 0;
        Vector3 position = transform.position;
        Vector3 targetPosition = position + Vector3.right * 0.5f;

        while (time < duration)
        {
            float value = _errorDamp.GetValue(time / duration);
            float x = position.x + (targetPosition.x - position.x) * value;
            float y = position.y + (targetPosition.y - position.y) * value;
            float z = position.z + (targetPosition.z - position.z) * value;
            transform.position = new Vector3(x, y, z);
            yield return null;
            time += Time.deltaTime;
        }

        transform.position = position;
        _shakeTask = null;
    }
}
