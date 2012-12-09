using System.IO;
using HWdTech;
using HWdTech.DS.v30;
using HWdTech.DS.v30.Messages;
using HWdTech.DS.v30.PropertyObjects;
using HWdTech.Factories;
using StartDS.EventTracking.EventTrackers;
using StartDS.EventTracking.Interfaces;
using StartDS.Sensors.Interfaces;

namespace StartDS.Sensors
{
    public class FileSystemSensor : ISensor
    {
        private readonly string _path;
        private readonly string _type;
        private FileSystemWatcher _fileSystemWatcher;
        private static string _channelName;
        private readonly object _thisLock = new object();

        public void Start()
        {
            StartWatching();
        }

        public void Stop()
        {
            StopWatching();
        }

        public IToken Token()
        {
            return new Token(null, _type);
        }

        public static FileSystemSensor CreateWithPathAndType(string path, string type)
        {
            return new FileSystemSensor(path, type);
        }

        private FileSystemSensor(string path, string type)
        {
            _path = path;
            _type = type;
            _channelName = "TrackersChannel";
        }

        private void StartWatching()
        {
            _fileSystemWatcher = new FileSystemWatcher
                {
                    Path = _path,
                    Filter = "*.*",
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName
                };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Created += OnCreated;
            _fileSystemWatcher.Deleted += OnDeleted;
            _fileSystemWatcher.Renamed += OnRenamed;

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void StopWatching()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Dispose();
            _fileSystemWatcher = null;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            lock (_thisLock)
            {
                try
                {
                    _fileSystemWatcher.EnableRaisingEvents = false;

                    SendMessage("File changed.");
                }
                finally
                {
                    _fileSystemWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            SendMessage("File created.");
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            SendMessage("File deleted.");
        }

        private void OnRenamed(object sender, FileSystemEventArgs e)
        {
            SendMessage("File renamed.");
        }

        private void SendMessage(string messageText)
        {
            var message = PrepareMessageWithText(messageText);
            var channel = Singleton<DIFactory>.Instance.Create<IChannel>(_channelName);
            var builder = Singleton<DIFactory>.Instance.Create<ObjectBuilder>();
            builder.Message().To(channel).Make(message);
            MessageBus.Send(message);
        }

        private IMessage PrepareMessageWithText(string messageText)
        {
            var message = Singleton<DIFactory>.Instance.Create<IMessage>();
            IToken token = new Token(messageText, _type);
            return token.WrapMessage(message);
        }
    }
}
