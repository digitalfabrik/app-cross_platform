﻿using System;
using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;

namespace Integreat.Shared.Utilities
{
    public class TopicListItem
    {
        private DataLoaderProvider _dataLoaderProvider;

        public TopicListItem(string topicString, DataLoaderProvider dataLoaderProvider)
        {
            _dataLoaderProvider = dataLoaderProvider;
            TopicString = topicString;
            DisplayName = GenerateDisplayName(topicString);
        }

        public string DisplayName;

        public string TopicString;

        private string GenerateDisplayName(string topicString)
        {
            string[] temp = topicString.Split('-');

            Location location = _dataLoaderProvider.LocationsDataLoader.Load(false).Result.First(l => l.Id == Int32.Parse(temp[0]));

            string displayName;

            if(temp[2] == "news")
            {
                //default topic
                displayName = location.Name + "(" + temp[1] + ")";
            }
            else
            {
                displayName = location.Name + "(" + temp[1] + ") - " + temp[2];
            }

            return displayName;
        }
    }
}