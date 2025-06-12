using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.CSharpUtilities
{
    public static class SubnetUtility
    {
        /// <summary>
        /// Generate subnet IP address from 10.90.0.0
        /// 153.43.0.0
        /// 209.44.33
        /// </summary>
        /// <param name="subnetIndex"></param>
        public static void GetSubnetForContainers(int subnetIndex)
        {
            var addressSpaceTemplate = "10.{0}.{1}.{2}/{3}";
            var blockSize = 32 * 256;//4096
            var cidr = 32 - (int)Math.Log(blockSize, 2);

            var startingAddress = subnetIndex * blockSize;
            // 2nd byte position
            var p0 = startingAddress / (256 * 256) + 90;

            // 3rd and 4th bytes
            var addressSpaceForLower2Bytes = startingAddress % (256 * 256);
            var p1 = addressSpaceForLower2Bytes / 256;
            var p2 = addressSpaceForLower2Bytes % 256;

            var s = string.Format(addressSpaceTemplate, p0, p1, p2, cidr);
            Console.WriteLine($"cidr:{cidr}, subnetIndex:{subnetIndex} => p0:{p0}, p1:{p1}, p2:{p2}, ===> s:{s}");
            Console.WriteLine();
        }

        public static void GetClusterSubnetAddressSpace(int index)
        {
            if (index < 0 || index > 2047)
            {
                throw new Exception($"Index {index} is out of allowed range.");
            }

            var addressSpaceTemplate = "10.{0}.{1}.{2}/{3}";
            var blockSize = 32 * 256;
            var cidr = 32 - (int)Math.Log(blockSize, 2);

            var startingAddress = index * blockSize;

            // 2nd byte position
            var p0 = startingAddress / (256 * 256);

            // 3rd and 4th bytes
            var addressSpaceForLower2Bytes = startingAddress % (256 * 256);
            var p1 = addressSpaceForLower2Bytes / 256;
            var p2 = addressSpaceForLower2Bytes % 256;

            var s = string.Format(addressSpaceTemplate, p0, p1, p2, cidr);
            Console.WriteLine($"cidr:{cidr} => p0:{p0}, p1:{p1}, p2:{p2}, ===> s:{s}");
            Console.WriteLine();
        }

        public static void GetClusterPeerSubnetAddressSpace(int index)
        {
            if (index < 0 || index > 127)
            {
                throw new Exception($"Index {index} is out of allowed range.");
            }

            index += 128;

            var addressSpaceTemplate = "172.{0}.{1}.{2}/{3}";
            var blockSize = 32 * 256;
            var cidr = 32 - (int)Math.Log(blockSize, 2);

            var startingAddress = index * blockSize;

            // 2nd byte position
            var p0 = startingAddress / (256 * 256);

            // 3rd and 4th bytes
            var addressSpaceForLower2Bytes = startingAddress % (256 * 256);
            var p1 = addressSpaceForLower2Bytes / 256;
            var p2 = addressSpaceForLower2Bytes % 256;

            var s = string.Format(addressSpaceTemplate, p0, p1, p2, cidr);
            Console.WriteLine($"cidr:{cidr} => p0:{p0}, p1:{p1}, p2:{p2}, ===> s:{s}");
            Console.WriteLine();
        }
    }
}
