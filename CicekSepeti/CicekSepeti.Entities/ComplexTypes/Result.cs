using CicekSepeti.Entities.ComplexTypes.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CicekSepeti.Entities.ComplexTypes
{
    public class Result<T>
    {
        public Result()
        {
            EntityList = new List<T>();
            ErrorMessages = new List<ErrorModel>();
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ErrorModel> ErrorMessages { get; set; }

        public List<T> EntityList { get; set; }

        public int EntityCount { get { return EntityList.Count(); } set { } }

        public bool IsValid
        {
            get{ return string.IsNullOrEmpty(ErrorMessages.FirstOrDefault()?.Text ?? null); }
            set { }
        }

        public bool IsSuccessful { get; set; }
    }
}
