using Exiled.API.Features;
using NAudio.Wave;
using System;
using System.IO;
using UnityEngine;
using Mirror;

namespace CustomSCPAudioApi
{
    public class AudioPlayerBase
    {
        protected ReferenceHub TargetHub { get; }
        protected WaveOutEvent OutputDevice { get; private set; }
        protected AudioFileReader AudioFile { get; private set; }

        public AudioPlayerBase(ReferenceHub hub)
        {
            TargetHub = hub;
        }

        public virtual void PlayAudio(string filePath, float volume = 1.0f)
        {
            if (!File.Exists(filePath) || !filePath.EndsWith(".ogg"))
            {
                Log.Error($"Invalid audio file: {filePath}");
                return;
            }

            try
            {
                AudioFile = new AudioFileReader(filePath);
                OutputDevice = new WaveOutEvent();
                OutputDevice.Init(AudioFile);
                OutputDevice.Volume = Mathf.Clamp01(volume);
                OutputDevice.Play();

                // الحصول على Player من ReferenceHub
                Player player = Player.Get(TargetHub);
                if (player == null)
                {
                    Log.Error("Player not found for ReferenceHub.");
                    return;
                }

                // إرسال RPC إلى اللاعبين القريبين
                foreach (Player nearbyPlayer in Player.List)
                {
                    if (Vector3.Distance(nearbyPlayer.Position, player.Position) < 30f)
                    {
                        nearbyPlayer.Connection.Send(new AudioRpcMessage(filePath, volume));
                    }
                }

                Log.Info($"Playing audio {filePath} for {player.Nickname}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error playing audio: {ex.Message}");
            }
        }

        public virtual void StopAudio()
        {
            try
            {
                OutputDevice?.Stop();
                OutputDevice?.Dispose();
                AudioFile?.Dispose();

                // إرسال إشارة إيقاف الصوت إلى اللاعبين القريبين
                Player player = Player.Get(TargetHub);
                if (player == null)
                {
                    Log.Error("Player not found for ReferenceHub.");
                    return;
                }

                foreach (Player nearbyPlayer in Player.List)
                {
                    if (Vector3.Distance(nearbyPlayer.Position, player.Position) < 30f)
                    {
                        nearbyPlayer.Connection.Send(new AudioRpcMessage(null, 0f));
                    }
                }

                Log.Info($"Stopped audio for {player.Nickname}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error stopping audio: {ex.Message}");
            }
        }

        public static AudioPlayerBase Get(ReferenceHub hub)
        {
            return new AudioPlayerBase(hub);
        }
    }

    public struct AudioRpcMessage :  NetworkMessage
    {
        public string FilePath { get; set; }
        public float Volume { get; set; }

        public AudioRpcMessage(string filePath, float volume)
        {
            FilePath = filePath;
            Volume = volume;
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.WriteString(FilePath);
            writer.WriteFloat(Volume);
        }

        public void Deserialize(NetworkReader reader)
        {
            FilePath = reader.ReadString();
            Volume = reader.ReadFloat();
        }
    }
}