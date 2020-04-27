using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using static Gameiki.GameikiUtils;

namespace Gameiki {
    internal sealed class SongPlayer {
        private static readonly string SongDirectory = Path.Combine("Content\\Sounds\\Gameiki");
        private static SongPlayer _instance;
        private readonly IList<Song> _queue = new List<Song>();
        private Song _currentSong;

        private SongPlayer() {
            MediaPlayer.MediaStateChanged += MediaPlayerOnMediaStateChanged;
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

        public static SongPlayer Instance => _instance ?? (_instance = new SongPlayer());

        public bool IsPlaying => MediaPlayer.State == MediaState.Playing;
        
        public void PlayRandom() {
            new TaskFactory().StartNew(() => {
                if (IsPlaying) {
                    _queue.Clear();
                    _currentSong.Dispose();
                }
            
                var songs = Directory.GetFiles(SongDirectory).Where(f => f.EndsWith(".mp3")).ToArray();
                _currentSong = Song.FromUri("Hello", new Uri(songs[GetRandom(songs.Length)], UriKind.RelativeOrAbsolute));
                MediaPlayer.Play(_currentSong);
            });
        }

        public void Pause() {
            MediaPlayer.Pause();
        }

        public void PlayShuffled() {
            
        }
    }
}