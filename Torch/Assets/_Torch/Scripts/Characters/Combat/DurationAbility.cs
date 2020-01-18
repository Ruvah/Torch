using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DurationAbility : Ability
{
    [Serializable]
    public class DurationAbilityEvent : IComparable<DurationAbilityEvent>
    {
        public float Time;
        public UnityEvent Event;

        public int CompareTo(DurationAbilityEvent other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Time.CompareTo(other.Time);
        }
    }

    // -- PROPERTIES
    public float CurrentDuration
    {
        get;
        private set;
    }

    // -- FIELDS

    public float Duration;
    public List<DurationAbilityEvent> TimeEventsQueue;

    private int TimeEventIndex;

    // -- METHODS

    protected void OnValidate()
    {
        TimeEventsQueue.Sort();
    }

    public override void UpdateAbility(float delta_time)
    {
        base.UpdateAbility(delta_time);
        if (IsActive)
        {
            CurrentDuration += delta_time;
            CheckTimeEvents();
            if (CurrentDuration >= Duration)
            {
                CurrentDuration = 0;
                IsActive = false;
            }
        }
    }

    private void CheckTimeEvents()
    {
        while (TimeEventsQueue[TimeEventIndex].Time >= CurrentDuration)
        {
            var duration_event = TimeEventsQueue[TimeEventIndex].Event;
            duration_event?.Invoke();
            ++TimeEventIndex;
        }
    }
}
