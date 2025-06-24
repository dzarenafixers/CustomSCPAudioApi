using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using CustomSCPAudioApi;
using CustomSCPAudioApi.Extensions;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace ExampleAudioPlugin
{
    public class ExampleAudioPlugin : Plugin<Config>
    {
        public override string Name => "ExampleAudioPlugin";
        public override string Author => "MONCEF50G";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 6, 1);

        public override void OnEnabled()
        {
            base.OnEnabled();
            Startup.SetupDependencies();
            AudioManager.LoadAudioFiles(Config.AudioDirectory);
            Exiled.Events.Handlers.Player.Spawned += OnPlayerSpawned;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Player.Spawned -= OnPlayerSpawned;
        }

        private void OnPlayerSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp173)
            {
                string audioPath = AudioManager.GetAudioPath("scp173_sound");
                if (audioPath != null)
                {
                    var audioPlayer = ev.Player.ReferenceHub.GetAudioPlayer();
                    audioPlayer.PlayAudio(audioPath, 0.8f);
                }
            }
        }
    }
}