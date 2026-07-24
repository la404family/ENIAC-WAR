using Microsoft.Xna.Framework.Audio;
using System;

namespace EniacWar;

public class AudioEngine
{
    private SoundEffect _typingSound;

    public AudioEngine()
    {
        int sampleRate = 44100;
        double duration = 0.05;
        int numSamples = (int)(sampleRate * duration);
        byte[] buffer = new byte[numSamples * 2];
        double frequency = 800.0;
        
        for (int i = 0; i < numSamples; i++)
        {
            double time = i / (double)sampleRate;
            double envelope = 1.0 - (time / duration);
            short sample = (short)(Math.Sin(2 * Math.PI * frequency * time) * short.MaxValue * 0.2 * envelope);
            buffer[i * 2] = (byte)(sample & 0xFF);
            buffer[i * 2 + 1] = (byte)(sample >> 8);
        }
        
        _typingSound = new SoundEffect(buffer, sampleRate, AudioChannels.Mono);
    }

    public void PlayTypingSound()
    {
        _typingSound.Play();
    }
}
