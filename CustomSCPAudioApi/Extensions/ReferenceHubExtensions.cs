using Exiled.API.Features;

namespace CustomSCPAudioApi.Extensions
{
    public static class ReferenceHubExtensions
    {
        public static AudioPlayerBase GetAudioPlayer(this ReferenceHub hub)
        {
            return AudioPlayerBase.Get(hub);
        }
    }
}