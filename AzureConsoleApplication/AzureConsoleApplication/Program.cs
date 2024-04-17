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
using System.ComponentModel;

namespace AzureConsoleApplication
{
    public static class Program
    {
        public static Dictionary<string, StorageAccountType> StorageAccountTypeDict =
            new Dictionary<string, StorageAccountType>(StringComparer.OrdinalIgnoreCase)
            {
                { "StandardLRS", StorageAccountType.StandardLrs },
                { "PremiumLRS", StorageAccountType.PremiumLrs },
                { "SStandardSSDLRS", StorageAccountType.StandardSsdLrs },
                { "UltraSSDLRS", StorageAccountType.UltraSsdLrs },
            };

        

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
            //var val = "UltraSSDLRS";
            //var requestedDataDiskSku = GetArmSdkBasedStorageAccountType(val, "vmName123", "clusterName123");
            //await Console.Out.WriteLineAsync($"converted:{requestedDataDiskSku}, type:{requestedDataDiskSku.GetType().FullName} ");

            //CreateVirtualMachineFactory.CreateVirtualMachineInEastAsiaAsync();
            await CreateVirtualMachineFactory.CreateVirtualMachineInNorthEuropeAsync();

            
        }


        public static StorageAccountType GetArmSdkBasedStorageAccountType(
            string storageAccountTypeInString,
            string vmName,
            string clusterName)
        {
            if (!StorageAccountTypeDict.TryGetValue(storageAccountTypeInString, out var requestedDataDiskSku))
            {
                string errorMsg = $"Incorrect StorageAccountType mentioned for data disk. reqDataDiskSku={storageAccountTypeInString}, " +
                    $"vm:{vmName}, clusterName:{clusterName}.";
                throw new Exception(errorMsg);
            }

            return requestedDataDiskSku;
        }

    }
}
