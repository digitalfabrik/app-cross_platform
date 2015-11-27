using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Integreat
{
	[Table("Language")]
	public class Language
	{
		[PrimaryKey, Column("_id")]
        [JsonProperty("id")]
        public int Id{get;set;}

        [JsonProperty("code")]
        public string ShortName{get;set;}

        [JsonProperty("native_name")]
        public string Name{get;set;}

        [JsonProperty("country_flag_url")]
        public string IconPath{get;set;}
        
        public Location Location{get;set;}

        public Language() { }
		public Language(int id, string shortName, string name, string iconPath) {
			Id = id;
			ShortName = shortName;
			Name = name;
			IconPath = iconPath;
		}

	    public override string ToString()
	    {
	        return ShortName + "";
	    }
	}
}

