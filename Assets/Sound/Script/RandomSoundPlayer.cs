using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{

    public List<AudioClip> sounds;
    //how often to play a sound (Seconds)
    public float playbackCooldown;

    private Timer timer;
    private AudioSource _audio;

    public void Awake()
    {
        timer = new Timer();
        _audio = GetComponent<AudioSource>();
    }

    public void Update()
    {
        timer.TickTimer(Time.deltaTime);
        if (timer.IsFinished)
        {
            timer.StartTimer(playbackCooldown);
            int randomIndex = Random.Range(0, sounds.Count);
            _audio.clip = sounds[randomIndex];
            _audio.Play(); 
        }
    }


    

    private class Timer
    {
        private float initTime = 1; //initialized as 1 to prevent div by 0
        private float timer = 0;
        bool finishedLastCheck;

        public bool IsFinished
        {
            get { return timer <= 0; }
        }

        public float AsFraction()
        {
            if (timer < 0) return 0;

            return 1 - timer / initTime;
        }

        public bool HasJustFinished()
        {
            bool result = finishedLastCheck == IsFinished;
            finishedLastCheck = IsFinished;

            return result;
        }

        public void StartTimer(float startTime) { timer = this.initTime = startTime; }
        public void TickTimer(float amount) { timer -= amount; }
        public void EndTimer() { timer = 0; }
    }

}
