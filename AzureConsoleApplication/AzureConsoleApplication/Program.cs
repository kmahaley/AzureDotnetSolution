using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Resources;
using AzureConsoleApplication.Factories;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using static Azure.Core.HttpHeader;

namespace AzureConsoleApplication
{
    public static class Program
    {
        /// <summary>
        /// FluenSDk to Arm Sdk migration: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/resourcemanager/Azure.ResourceManager/docs/MigrationGuide.md#create-a-network-interface
        /// Arm sdk compute: https://github.com/Azure/azure-sdk-for-net/blob/Azure.ResourceManager.Compute_1.1.0/sdk/compute/Azure.ResourceManager.Compute/samples/Sample2_ManagingVirtualMachines.md
        /// Arm sdk managed disk: https://github.com/Azure/azure-sdk-for-net/blob/Azure.ResourceManager.Compute_1.1.0/sdk/compute/Azure.ResourceManager.Compute/samples/Sample1_ManagingDisks.md
        /// Azure models: https://azuresdkdocs.blob.core.windows.net/$web/dotnet/Azure.ResourceManager.Compute/1.4.0/api/Azure.ResourceManager.Compute/Azure.ResourceManager.Compute.ManagedDiskData.html
        /// 
        /// Disk overview: https://learn.microsoft.com/en-us/azure/virtual-machines/managed-disks-overview
        /// Disk encryption: https://www.alifconsulting.com/post/azure-managed-disk-encryption
        /// 
        /// github examples: https://github.com/Azure-Samples/azure-samples-net-management/blob/master/samples/compute/create-virtual-machine/Program.cs
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            //await CreateVirtualMachineFactory.CreateVirtualMachineInEastAsiaAsync();
            //await CreateVirtualMachineFactory.CreateVirtualMachineInNorthEuropeAsync();
            await CreateNetworkResourcesFactory.UpdateNetworkSecurityGroupInSubnetAsync();
            //await CreateNetworkResourcesFactory.WestpacVnetTransformationCheck();

            

            sw.Stop();
            Console.WriteLine($"--- end of progrem ---time:{sw.ElapsedMilliseconds}");
        }


        public static string GetSubnetAddressSpace(int subnetIndex)
        {
            int IPv4BitLength = 32;
            int NumberOfHosts = 4096;

            var zeroBasedSubnetIndex = subnetIndex;
            var addressSpaceTemplate = "10.{0}.{1}.{2}/{3}";
            // 2 ^ (32-subnetMask) = numberOfHosts
            var cidr = IPv4BitLength - (int)Math.Log(NumberOfHosts, 2);

            //10.90.32.0 ie. {p0}:{p1}:{p2}:{p3}
            int octect2 = 90, octect3 = 32, octect4 = 0;
            int numberOfHostInEachOctect = 256;

            int numberOfHostIn3rdOctect = octect3 + (subnetIndex * 16);
            int incrementOctect2By = numberOfHostIn3rdOctect / numberOfHostInEachOctect;
            octect3 = numberOfHostIn3rdOctect % numberOfHostInEachOctect;
            if (incrementOctect2By > 0)
            {
                octect2 = octect2 + incrementOctect2By;
                if (octect2 > 255)
                {
                    throw new Exception("AKS based clusters range is not supported. Please Check subnetIndex");
                }
            }

            return string.Format(addressSpaceTemplate, octect2, octect3, octect4, cidr);
        }

        
    }

    enum ErrorCode 
    {
        InvalidRequest = 1
    }
}
