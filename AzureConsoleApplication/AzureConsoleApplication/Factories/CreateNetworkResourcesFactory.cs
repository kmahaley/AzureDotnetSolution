using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureConsoleApplication.Factories
{
    public class CreateNetworkResourcesFactory
    {
        private const string TransformationNsgRgName = "vnet-91cd0ea0-westeurope-transformation-nsg-rg";

        private const string TransformationNsgName = "vnet-91cd0ea0-subnet-vm-secure-nsg";
        //private const string TransformationNsgName = "vnet-91cd0ea0-subnet-vm-dep-nsg";

        private const string OriginalNsgRgName = "vnet-91cd0ea0-westeurope-dep-nsg-rg";

        private const string VnetName = "vnet-91cd0ea0-westeurope-33";

        private const string VnetRg = "vnet-91cd0ea0-westeurope-33-rg";

        public static async Task UpdateNetworkSecurityGroupInSubnetAsync()
        {
            var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("TenantId");
            var subscription = Environment.GetEnvironmentVariable("SubscriptionId");
            //ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            VisualStudioCredential credential = new VisualStudioCredential();
            ArmClient client = new ArmClient(credential, subscription);
            SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();


            var nsgData = await GetNetworkSecurityGroupData(subscriptionResource, TransformationNsgRgName, TransformationNsgName);
            //Console.WriteLine($"recieved NSG. nsgArmId:{nsgData.Name}");

            //var vnet = (await subscriptionResource.GetResourceGroupAsync(vnetRg)).Value;

            var shouldUpdateSubnetNsg = false;
            if (shouldUpdateSubnetNsg)
            {
                await UpdateNetworkSecurityGroupOfSubnets(subscriptionResource, nsgData, VnetRg, VnetName);
            }
            else
            { 
                //await ListNetworkSecurityGroupOfSubnets(subscriptionResource, VnetRg, VnetName);
                await ResetNetworkSecurityGroupOfSubnets(subscriptionResource, VnetRg, VnetName);
            }
        }

        private static async Task UpdateNetworkSecurityGroupOfSubnets(
            SubscriptionResource subscriptionResource,
            NetworkSecurityGroupData nsgData,
            string vnetRg,
            string vnetName)
        {
            var vnetRgResource = (await subscriptionResource.GetResourceGroupAsync(vnetRg)).Value;
            var vnetResource = (await vnetRgResource.GetVirtualNetworkAsync(vnetName)).Value;
            var vnetData = vnetResource.Data;

            var subnetCollection = vnetData.Subnets;//vnetResource.GetSubnets();
            int i = 0;
            var sw = new Stopwatch();
            foreach (var subnet in subnetCollection)
            {
                var subnetData = subnet;
                var subnetName = subnetData.Name;
                if (string.Equals("PrivateEndpointsSubnet", subnetName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                sw.Start();
                var current = subnetData.NetworkSecurityGroup.Id.Name;
                if (!string.Equals(subnetData.NetworkSecurityGroup.Id.Name, nsgData.Name, StringComparison.OrdinalIgnoreCase))
                {
                    subnetData.NetworkSecurityGroup.Id = nsgData.Id;
                }
            }

            try
            {
                await vnetRgResource.GetVirtualNetworks().CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    vnetName,
                    vnetData);
                sw.Stop();
                Console.WriteLine($"completed update:{sw.Elapsed.Milliseconds}ms, totalSubnets:{i}, vnet:{vnetName} ---> nsg:{nsgData.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update subnets. time:{sw.ElapsedMilliseconds}ms, i:{i}, ex:{ex.Message}");
                Console.WriteLine();
            }

        }

        private static async Task ResetNetworkSecurityGroupOfSubnets(
            SubscriptionResource subscriptionResource,
            string vnetRg,
            string vnetName)
        {
            var vnetRgResource = (await subscriptionResource.GetResourceGroupAsync(vnetRg)).Value;
            var vnetResource = (await vnetRgResource.GetVirtualNetworkAsync(vnetName)).Value;
            var vnetData = vnetResource.Data;
            var subnetCollection = vnetData.Subnets;
            var filteredSubnetCollection = subnetCollection
                .Where(subnetData => 
                {
                    return (!string.Equals("PrivateEndpointsSubnet", subnetData.Name, StringComparison.OrdinalIgnoreCase)
                    && subnetData.NetworkSecurityGroup.Id.Name.Equals(TransformationNsgName, StringComparison.OrdinalIgnoreCase));
                })
                .ToList();

            var subnetNames = filteredSubnetCollection
                .Select(x => x.Name)
                .ToList();
            var names = string.Join(", ", subnetNames);
            Console.WriteLine($"count of subnet to be reset. totalCount:{subnetCollection.Count()}, filterCount:{filteredSubnetCollection.Count}, subnetNames:{names}");
                
            var sw = new Stopwatch();
            foreach (var subnetData in filteredSubnetCollection)
            {
                var subnetName = subnetData.Name;
                try
                {
                    var nsgNameTobeAttachedToSubnet = CreateNsgNameFromSubnet(subnetName);
                    var nsgData = await GetNetworkSecurityGroupData(
                        subscriptionResource,
                        OriginalNsgRgName,
                        nsgNameTobeAttachedToSubnet);
                    subnetData.NetworkSecurityGroup.Id = nsgData.Id;
                    sw.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exceptionduring subnet reset. subnetName:{subnetName}, ex:{ex.Message}");
                    continue;
                }
            }

            try
            {
                await vnetRgResource.GetVirtualNetworks().CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    vnetName,
                    vnetData);
                sw.Stop();
                Console.WriteLine($"Reset vnet completed:{sw.Elapsed.Milliseconds}ms, totalSubnets:{filteredSubnetCollection.Count}, vnet:{vnetName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to reset subnets from vnet. time:{sw.ElapsedMilliseconds}ms, ex:{ex.Message}");
                Console.WriteLine();
            }
        }

        private static async Task ListNetworkSecurityGroupOfSubnets(
            SubscriptionResource subscriptionResource,
            string vnetRg,
            string vnetName)
        {
            var vnetRgResource = (await subscriptionResource.GetResourceGroupAsync(vnetRg)).Value;
            var vnetResource = (await vnetRgResource.GetVirtualNetworkAsync(vnetName)).Value;
            var subnetCollection = vnetResource.GetSubnets();
            var filteredSubnetCollection = subnetCollection
                .Where(subnet =>
                {
                    var subnetData = subnet.Data;
                    return !string.Equals("PrivateEndpointsSubnet", subnetData.Name, StringComparison.OrdinalIgnoreCase)
                    && subnetData.NetworkSecurityGroup.Id.Name.Equals(TransformationNsgName, StringComparison.OrdinalIgnoreCase);
                })
                .ToList();

            var subnetNames = filteredSubnetCollection
                .Select(x => x.Data.Name)
                .ToList();
            var names = string.Join(", ", subnetNames);
            Console.WriteLine($"count of subnet to be reset. totalCount:{subnetCollection.Count()}, filterCount:{filteredSubnetCollection.Count}, subnetNames:{names}");

        }
        public static async Task WestpacVnetTransformationCheck()
        {
            var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("TenantId");
            var subscription = Environment.GetEnvironmentVariable("SubscriptionId");

            VisualStudioCredential credential = new VisualStudioCredential();
            ArmClient client = new ArmClient(credential, subscription);
            SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();

            var vnetRgResource = (await subscriptionResource.GetResourceGroupAsync(VnetRg)).Value;
            var vnetResource = (await vnetRgResource.GetVirtualNetworkAsync(VnetName)).Value;
            var subnetCollection = vnetResource.GetSubnets();
            bool isUpdate = true;
            if (isUpdate)
            {
                var nsgRgName = "vnet-91cd0ea0-westeurope-transformation-nsg-rg";
                var nsgName = "vnet-91cd0ea0-subnet-vm-dep-nsg";
                //var nsgRgName = "vnet-91cd0ea0-westeurope-dep-nsg-rg";
                //var nsgName = "vnet-91cd0ea0-subnet-0-dep-nsg";
                var nsgData = await GetNetworkSecurityGroupData(subscriptionResource, nsgRgName, nsgName);
                var sw = new Stopwatch();
                foreach (var subnet in subnetCollection)
                {
                    var subnetData = subnet.Data;
                    var subnetName = subnetData.Name;
                    if (subnetName.Contains("-subnet-0"))
                    {
                        subnetData.NetworkSecurityGroup = nsgData;
                        await subnet.UpdateAsync(WaitUntil.Completed, subnetData);
                        Console.WriteLine($"Updated NSG name.subnetName:{subnetName}, nsgName:{nsgData.Name}");
                    }
                }
            }
            else
            {
                foreach (var subnet in subnetCollection)
                {
                    var subnetData = subnet.Data;
                    var subnetName = subnetData.Name;
                    if (subnetName.Contains("subnet-0"))
                    {
                        var nsg = subnetData.NetworkSecurityGroup.Id;
                        Console.WriteLine($"Name of the subnet. subnetName:{subnetName}, nsg:{nsg}");
                    }
                }
            }
        }

        public static async Task<NetworkSecurityGroupData> GetNetworkSecurityGroupData(
            SubscriptionResource subscriptionResource,
            string nsgRgName,
            string nsgName)
        {
            var azureNsgRgResource = (await subscriptionResource.GetResourceGroupAsync(nsgRgName)).Value;
            var nsgResource = (await azureNsgRgResource.GetNetworkSecurityGroupAsync(nsgName)).Value;
            return nsgResource.Data;
        }

        public static string CreateNsgNameFromSubnet(string subnetName)
        {
            var arrayOfSubnet = subnetName.Split('-', StringSplitOptions.TrimEntries);
            var nsgName = string.Concat(arrayOfSubnet[0], '-', arrayOfSubnet[1], "-subnet-", arrayOfSubnet[5], "-dep-nsg");
            return nsgName;
        }
    }
}
