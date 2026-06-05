
using System.Collections.Generic;
using Godot;

namespace Phrolova.PhrolovaCode.Extensions
{
    public static class PhrolovaAudio
    {
        private static readonly Dictionary<string, AudioStream> _cache = new();

        public static void Play(string pathWithoutExtension, float volume = 0.8f)
        {
            // 自动按优先级尝试加载 wav 或 mp3
            string actualPath = null;
            foreach (var ext in new[] { ".wav", ".mp3", ".ogg" })
            {
                string candidate = pathWithoutExtension + ext;
                if (ResourceLoader.Exists(candidate))
                {
                    actualPath = candidate;
                    break;
                }
            }

            if (actualPath == null)
            {
                GD.PrintErr($"Audio file not found: {pathWithoutExtension} (tried .wav, .mp3, .ogg)");
                return;
            }

            if (!_cache.TryGetValue(actualPath, out AudioStream value))
            {
                value = GD.Load<AudioStream>(actualPath);
                if (value != null) _cache[actualPath] = value;
            }

            if (value == null)
            {
                GD.PrintErr($"Failed to load audio stream: {actualPath}");
                return;
            }

            var player = new AudioStreamPlayer
            {
                Stream = value,
                VolumeDb = ToDb(volume),
                Autoplay = true
            };
            ((SceneTree)Engine.GetMainLoop()).Root.AddChild(player);
            player.Finished += () => player.QueueFree();
        }

        private static float ToDb(float volume) => volume <= 0f ? -80f : Mathf.LinearToDb(volume);
    }
}