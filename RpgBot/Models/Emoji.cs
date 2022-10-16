using System;
using System.Collections.Generic;
using System.Text;

namespace RpgBot.Models
{
    public class Emoji
    {
        public Emoji(long id, string name, string url)
        {
            Id = id;
            Name = name;
            Url = url;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
