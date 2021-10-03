using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Leg : MonoBehaviour
{
    public InputActionMap actions;
    public Vector3 anchor;
    public float maxDistance = 2.0f;
    public Leg nextLeg;
    public AudioSource stepSound;

    private bool isUp;

    public bool IsUp
    {
        get => isUp;
        set
        {
            isUp = value;

            if (value)
            {
                anchor = nextLeg.transform.position + anchorOffset;
                IndicateUp();
            }
            else
            {
                IndicateDown();
            }
        }
    }

    public SpriteRenderer footprint;
    public Vector3 anchorOffset;

    public IceHex selectedIceHex;
    private float _previousSteppingDown;

    // Start is called before the first frame update
    void Start()
    {
        if (!nextLeg.IsUp) IsUp = true;

        anchor = transform.position;
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUp && !GameManager.Instance.isPause)
        {
            if (selectedIceHex != null && selectedIceHex.HasDrown)
                GameManager.Instance.GameOver("You drowned in the river!");
        }

        if (!isUp || GameManager.Instance.isPause)
        {
            if (actions["Positioning"].ReadValue<Vector2>() != Vector2.zero)
            {
                GameManager.Instance.StartTheGame();
            }

            return;
        }

        var steppingDown = actions["Stepping down"].ReadValue<float>();

        if (steppingDown > 0)
        {
            SteppingDown(steppingDown);
        }
        else
        {
            var direction = actions["Positioning"].ReadValue<Vector2>();

            var targetPosition = anchor + new Vector3(direction.x, direction.y, 0) * maxDistance;

            if (Physics.Raycast(targetPosition, Vector3.forward, out var hit, Mathf.Infinity))
            {
                var hitObject = hit.collider.transform.gameObject;

                if (hitObject.GetComponent<IceHex>() != null)
                {
                    selectedIceHex = hitObject.GetComponent<IceHex>();

                    var position = hitObject.transform.position;

                    if (position.x != nextLeg.transform.position.x || position.y != nextLeg.transform.position.y)
                    {
                        transform.DOMove(new Vector3(position.x, position.y, transform.position.z), 0.2f);
                    }
                }
            }

            IndicateUp();
        }
    }

    private void SteppingDown(float steppingDown)
    {
        selectedIceHex.ReceivePressure(steppingDown - _previousSteppingDown);

        _previousSteppingDown = steppingDown;

        footprint.color = Color.Lerp(new Color(1, 1, 1, 0.5f), new Color(1, 1, 1, 1), steppingDown);
        transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, steppingDown);

        if (steppingDown == 1.0f && !selectedIceHex.HasDrown)
        {
            IsUp = false;
            nextLeg.IsUp = true;

            stepSound.pitch = Random.Range(0.8f, 1.2f);
            stepSound.Play();

            GameManager.Instance.playerDistance = selectedIceHex.y;

            if (selectedIceHex.y == Map.Instance.height - 1)
            {
                GameManager.Instance.Win();
            }
        }
    }

    void IndicateDown()
    {
        footprint.color = new Color(1, 1, 1, 1);
        transform.localScale = Vector3.one;
    }

    void IndicateUp()
    {
        footprint.color = new Color(1, 1, 1, 0.5f);
        transform.localScale = Vector3.one * 0.5f;
    }
}