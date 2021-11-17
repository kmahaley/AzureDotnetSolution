using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApplication.Configurations
{
    public class MongoDbConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ConnectionString {
            get {
                return $"mongodb://{Host}:{Port}";
                //return $"mongodb://{Username}:{Password}@{Host}:{Port}";
            }
        }
    }
}
