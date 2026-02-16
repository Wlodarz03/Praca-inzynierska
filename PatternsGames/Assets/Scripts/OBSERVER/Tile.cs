using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;

public class Tile : MonoBehaviour
{
    public int x,y;
    public bool isOn = false;
    public event Action<Tile> ThingToggled;
    private List<Tile> subscribers = new List<Tile>();
    private List<Tile> subscribedTo = new List<Tile>();
    private SpriteRenderer sr;
    public GameObject earPrefab;

    [Header("Highlighting")]
    public Color highlightColor = Color.cyan;
    public float flashDuration = 2f;
    public float flashScale = 1.2f;
    private Vector3 deafultScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        deafultScale = transform.localScale;
        RefreshVisual();
    }

    public void Toggle(bool notify = true)
    {
        isOn = !isOn;
        RefreshVisual();

        if (notify)
            ThingToggled?.Invoke(this);
    }

    void OnMouseUpAsButton()
    {
        if (!GameManagerO.Instance.isFinished)
        {
            Toggle(true);
            foreach (var sub in subscribers)
            {
                if (sub != null)
                    sub.FlashAsSubscriber(flashDuration);
            }

            GameManagerO.Instance?.RegisterMove();
            GameManagerO.Instance?.CheckForWin();
        }
    }

    public void RefreshVisual()
    {
        sr.color = isOn ? Color.yellow : Color.gray;
    }

    public void SubscribeTo(Tile other)
    {
        if (other == null || subscribedTo.Contains(other)) return;
        other.ThingToggled += OnNeighborToggled;
        subscribedTo.Add(other);
        other.AddSubscriber(this);
    }

    public void UnsubscribeFrom(Tile other)
    {
        if (other == null || !subscribedTo.Contains(other)) return;
        other.ThingToggled -= OnNeighborToggled;
        subscribedTo.Remove(other);
        other.RemoveSubscriber(this);
    }

    public void UnsubscribeAll()
    {
        var copy = new List<Tile>(subscribedTo);
        foreach (var other in copy)
        {
            if (other != null)
                UnsubscribeFrom(other);
        }

        subscribedTo.Clear();
    }

    public void AddSubscriber(Tile t)
    {
        if (t == null || subscribers.Contains(t)) return;
        subscribers.Add(t);
    }

    public void RemoveSubscriber(Tile t)
    {
        if (t == null || !subscribers.Contains(t)) return;
        subscribers.Remove(t);
    }

    public void RemoveAllSubscribers()
    {
        subscribers.Clear();
    }
    private void OnNeighborToggled(Tile source)
    {
        Toggle(false);
    }

    private void OnDestroy()
    {
        UnsubscribeAll();
        foreach (var s in subscribers)
            if (s != null) s.RemoveSubscriber(this);
        RemoveAllSubscribers();
    }

    public void FlashAsSubscriber(float duration)
    {

        StartCoroutine(FlashAsSubscriberCoroutine(duration));
    }

    public IEnumerator FlashAsSubscriberCoroutine(float duration)
    {
        if (sr == null) yield break;

        //Color original = sr.color;

        GameObject ear = null;
        if (earPrefab != null)
        {
            ear = Instantiate(earPrefab, transform.position, Quaternion.identity, transform);
        }

        //sr.color = highlightColor;
        transform.localScale = deafultScale * flashScale;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        //sr.color = original;
        transform.localScale = deafultScale;

        if (ear != null) Destroy(ear);
    }
}
