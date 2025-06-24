using Exiled.API.Interfaces;

namespace ExampleAudioPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
        public string AudioDirectory { get; set; } = ".Config/audio";
    }
}