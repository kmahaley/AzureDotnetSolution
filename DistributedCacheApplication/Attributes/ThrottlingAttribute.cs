namespace DistributedCacheApplication.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ThrottlingAttribute : Attribute
    {
        public string ApiName { get; set; }

        public string ApiAlias { get; set; }

        public string RouteParameter { get; set; }

        public IList<string> ApiMetadata { get; set; } = new List<string>();

        public ThrottlingAttribute(
            string apiName,
            string apiAlias) 
        {
            this.ApiName = apiName;
            this.ApiAlias = apiAlias;
        }
    }
}
