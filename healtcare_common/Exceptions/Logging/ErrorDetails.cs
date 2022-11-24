

using Newtonsoft.Json;

namespace HealthCare_Common.Logging
{
    public class ErrorDetails
    {
        [JsonProperty("status")]
        public int StatusCode { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}