using Integreat.Data.Loader;
using System.Linq;

namespace Integreat.Data.Utilities
{
    public class TopicListItem
    {
        private readonly DataLoaderProvider _dataLoaderProvider;

        public TopicListItem(string topicString, DataLoaderProvider dataLoaderProvider)
        {
            _dataLoaderProvider = dataLoaderProvider;
            TopicString = topicString;
            DisplayName = GenerateDisplayName(topicString);
        }

        public string DisplayName { get; }

        public string TopicString { get; }

        private string GenerateDisplayName(string topicString)
        {
            var temp = topicString.Split('-');

            var location = _dataLoaderProvider.LocationsDataLoader.Load(false).Result.First(l => l.Id == int.Parse(temp[0]));

            var displayName = temp[2] == "news" ? $"{location.Name}({temp[1]})" : $"{location.Name}({temp[1]}) - {temp[2]}";

            return displayName;
        }
    }
}
