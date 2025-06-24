using Exiled.API.Features;
using CustomSCPAudioApi;
using CustomSCPAudioApi.Extensions;

namespace ExampleAudioPlugin
{
    public class AudioExample
    {
        public void PlayScpSound(ReferenceHub referenceHub, string audioName)
        {
            // التأكد من تحميل ملفات الصوت
            AudioManager.LoadAudioFiles("path/to/audio/folder");

            // الحصول على مسار الصوت
            string audioPath = AudioManager.GetAudioPath(audioName);
            if (audioPath == null)
            {
                Log.Error($"Audio file not found: {audioName}");
                return;
            }

            // الحصول على مشغل الصوت
            AudioPlayerBase audioPlayer = referenceHub.GetAudioPlayer();

            // تشغيل الصوت بمستوى صوت 80%
            audioPlayer.PlayAudio(audioPath, 0.8f);

            Log.Info($"Playing audio: {audioName} for player: {referenceHub.nicknameSync.Network_myNickSync}");
        }

        public void StopScpSound(ReferenceHub referenceHub)
        {
            // إيقاف الصوت
            AudioPlayerBase audioPlayer = referenceHub.GetAudioPlayer();
            audioPlayer.StopAudio();
            Log.Info($"Stopped audio for player: {referenceHub.nicknameSync.Network_myNickSync}");
        }
    }
}