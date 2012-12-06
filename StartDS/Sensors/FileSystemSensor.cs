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
        private FileSystemWatcher _fileSystemWatcher;
        private static readonly string _messageName = typeof(FileSystemSensor).Name + "Message";
        private static readonly string _channelName = typeof(FileSystemSensor).Name + "Channel";
        private readonly object _thisLock = new object();

        public void Start()
        {
            StartWatching();
        }

        public void Stop()
        {
            StopWatching();
        }

        public ITracked Channel()
        {
            return Tracked.WithString(_channelName);
        }

        public ITracked Message()
        {
            return Tracked.WithString(_messageName);
        }

        public static FileSystemSensor CreateWithPath(string path)
        {
            return new FileSystemSensor(path);
        }

        private FileSystemSensor(string path)
        {
            _path = path;
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
            var text = new Field<string>("text");
            var hash = new Field<string>("hash");
            text[message] = messageText;
            hash[message] = Tracked.WithString(_channelName).Hash();
            return message;
        }
    }
}
