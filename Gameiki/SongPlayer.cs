using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameiki.Extensions;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

namespace Gameiki {
    internal sealed class SongPlayer {
        private static readonly string SongDirectory = Path.Combine("Content\\Sounds\\Gameiki");
        private static SongPlayer _instance;
        private readonly IList<Song> _queue = new List<Song>();
        private Song _currentSong;

        private SongPlayer() {
            MediaPlayer.MediaStateChanged += MediaPlayerOnMediaStateChanged;
            Hooks.PreCursorDraw += OnPreCursorDraw;
        }

        public static SongPlayer Instance => _instance ?? (_instance = new SongPlayer());

        public bool IsPlaying => MediaPlayer.State == MediaState.Playing;

        public void Pause() {
            MediaPlayer.Pause();
        }

        public void PlayRandom() {
            new TaskFactory().StartNew(() => {
                if (IsPlaying) {
                    _queue.Clear();
                    _currentSong.Dispose();
                }

                var songs = Directory.GetFiles(SongDirectory).Where(f => f.EndsWith(".mp3")).ToArray();
                var randomSongPath = songs[GameikiUtils.GetRandom(songs.Length)];
                _currentSong = Song.FromUri(Path.GetFileNameWithoutExtension(randomSongPath),
                    new Uri(randomSongPath, UriKind.RelativeOrAbsolute));
                MediaPlayer.Play(_currentSong);
            });
        }

        public void PlayShuffled() {
            var songs = Directory.GetFiles(SongDirectory).Where(f => f.EndsWith(".mp3")).ToList();
            songs.Shuffle();
            foreach (var songPath in songs) {
                _queue.Add(Song.FromUri(Path.GetFileNameWithoutExtension(songPath), new Uri(songPath, UriKind.Relative)));
            }

            _currentSong = _queue[0];
            MediaPlayer.Play(_currentSong);
        }

        private void MediaPlayerOnMediaStateChanged(object sender, EventArgs e) {
            if (MediaPlayer.State != MediaState.Stopped) {
                return;
            }

            _queue.Remove(_currentSong);
            _currentSong.Dispose();
            if (_queue.Count == 0) {
                return;
            }

            MediaPlayer.Play(_currentSong = _queue[0]);
        }

        private void OnPreCursorDraw(object sender, HandledEventArgs e) {
            if (!IsPlaying) {
                return;
            }

            var displayString = $"Now playing: '{_currentSong.Name}'";
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, displayString,
                new Vector2((float) (628 - FontAssets.MouseText.Value.MeasureString(displayString).X * 0.5) + (Main.screenWidth - 800),
                    Main.screenHeight - 84), Color.White);
        }
    }
}