using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IceHex : MonoBehaviour
{
    public GameObject crackPrefab;
    public List<IceHex> connections;
    public float pressureThreshold = 0.1f;
    public int x;
    public int y;

    public bool HasDrown => connections.Count == 0;

    public void ReceivePressure(float pressure, IceHex source = null)
    {
        if (HasDrown) return;
        if (pressure < pressureThreshold) return;

        var possibleConnectionsToCrack = Math.Min(6, connections.Count);

        var crackedConnections = Math.Min(possibleConnectionsToCrack,
            Math.Ceiling(pressure / pressureThreshold));

        for (var i = 0; i < crackedConnections; i++)
        {
            if (connections.Count > 0)
            {
                var otherIceHex = connections[Random.Range(0, connections.Count)];

                if (otherIceHex != source)
                {
                    connections.Remove(otherIceHex);
                    otherIceHex.connections.Remove(this);

                    var crackSpawnPosition = Vector3.Lerp(transform.position, otherIceHex.transform.position, 0.5f);
                    var diff = transform.position - otherIceHex.transform.position;
                    var angle = Vector3.Angle(diff.y < 0 ? -diff : diff, Vector3.right);
                    var crackSpawnRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    Instantiate(crackPrefab, crackSpawnPosition, crackSpawnRotation);

                    GameManager.Instance.PlayRandomIceCrackSound();

                    otherIceHex.ReceivePressure(0.5f * pressure, this);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (HasDrown)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            enabled = false;
            GameManager.Instance.iceDrowningSound.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var iceHex = other.gameObject.GetComponent<IceHex>();

        if (iceHex != null)
        {
            connections.Add(iceHex);
        }
    }
}