using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;

namespace CustomSCPAudioApi
{
    public static class AudioManager
    {
        private static readonly Dictionary<string, string> AudioCache = new Dictionary<string, string>();

        public static void LoadAudioFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Log.Error($"Audio directory not found: {directory}");
                return;
            }

            foreach (var file in Directory.GetFiles(directory, "*.ogg"))
            {
                AudioCache[Path.GetFileNameWithoutExtension(file)] = file;
                Log.Info($"Loaded audio file: {file}");
            }
        }

        public static string GetAudioPath(string audioName)
        {
            return AudioCache.TryGetValue(audioName, out var path) ? path : null;
        }
    }
}