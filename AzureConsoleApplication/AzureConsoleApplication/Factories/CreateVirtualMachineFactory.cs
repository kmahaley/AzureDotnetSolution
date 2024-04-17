﻿using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Network;
using Azure;
using System.Net.NetworkInformation;

namespace AzureConsoleApplication.Factories
{
    public static class CreateVirtualMachineFactory
    {
        public static readonly string EbdSku = "Standard_E16bds_v5";

        public static readonly string DaoSku = "Standard_D32aods_v6";

        public static async Task CreateVirtualMachineInEastAsiaAsync()
        {
            var location = AzureLocation.EastAsia;
            var rgName = "";
            var vmName = "";
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("TenantId");
            var subscription = Environment.GetEnvironmentVariable("SubscriptionId");
            //ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            VisualStudioCredential credential = new VisualStudioCredential();
            ArmClient client = new ArmClient(credential, subscription);

            // Create Resource Group
            SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();
            //Console.WriteLine("--------Start create group--------");
            //var rgResource = await CreateResourceGroup(subscriptionResource, rgName, location);
            //Console.WriteLine("--------Finish create group--------");

            // Create Vnet and Nic
            var vnetName = "VnetSampleName"; //"VnetSampleName01";
            var networkInterfaceName = "VnetSampleNameInterfaceName01";
            var networkInterfaceIpConfigName = "VnetSampleNameInterfaceIpConfigName01";
            //await CreateVirtualNetworkAsync(subscriptionResource, rgName, location, vnetName);
            //await CreateVirtualNetworkInterfaceAsync(
            //    subscriptionResource,
            //    rgName,
            //    location,
            //    vnetName,
            //    networkInterfaceName,
            //    networkInterfaceIpConfigName);

            // Get all vms
            //Console.WriteLine("--------Get all vms--------");
            //await GetAllVmsAsync(subscriptionResource, rgName);
            //Console.WriteLine("--------Finish Get all vms--------");

            // Create a Virtual Machine
            Console.WriteLine("--------Create vm--------");

            // TODO: Provide arm resource id. we have 2 Nics
            var networkInterfaceArmId = "";
            //var networkInterfaceArmId = "";

            // TODO: Provide arm resource id. of existing disk
            //var existingManagedDiskArmId = "";

            //await CreateVirtualMachineWithNicAsync(subscriptionResource, rgName, location, vmName);
            //await CreateVirtualMachineWithNicAsync(subscriptionResource, EbdSku, rgName, location, vmName, networkInterfaceArmId);

            //await CreateVMWithManagedDiskAndNicResourceProvidedAsync(
            //    subscriptionResource,
            //    rgName,
            //    location,
            //    vmName,
            //    networkInterfaceArmId,
            //    existingManagedDiskArmId);

            /*
             * Below is single group activity VHD - managed disk - VM
             */


            //TODO: Provide arm resource id
            //var vhdUri = "";
            //var storageArmId = "";

            //var diskName = $"ManagedOsDiskFromVhd_{vmName}";
            //var diskSize = 256;
            //var diskResourceId = await CreateManagedDiskFromVhd(
            //    subscriptionResource,
            //    rgName,
            //    diskName,
            //    location,
            //    vhdUri,
            //    storageArmId,
            //    diskSize);

            //await CreateVMWithManagedDiskFromVhdAndNicResourceProvidedAsync(
            //    subscriptionResource,
            //    rgName,
            //    location,
            //    vmName,
            //    networkInterfaceArmId,
            //    diskResourceId);

            Console.WriteLine("--------Finish Create vm--------");

            //Delete resource group if necessary
            //Console.WriteLine("--------Start delete vm--------");
            //await DeleteVirtualMachineAsync(subscriptionResource, rgName, vmName);
            //Console.WriteLine("--------Finish delete vm--------");
        }

        public static async Task CreateVirtualMachineInNorthEuropeAsync()
        {
            var location = AzureLocation.NorthEurope;
            var rgName = "";
            var vmName = "";
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("TenantId");
            var subscription = "";
            //ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            VisualStudioCredential credential = new VisualStudioCredential();
            ArmClient client = new ArmClient(credential, subscription);

            // Create Resource Group
            try
            {
                SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();
                Console.WriteLine("--------Start create group--------");
                //var rgResource = await CreateResourceGroup(subscriptionResource, rgName, location);
                Console.WriteLine("--------Finish create group--------");

                // Create Vnet and Nic
                var vnetName = "VnetSampleName"; //"VnetSampleName01";
                var networkInterfaceName = "VnetSampleNameInterfaceName01";
                var networkInterfaceIpConfigName = "VnetSampleNameInterfaceIpConfigName01";
                //await CreateVirtualNetworkAsync(subscriptionResource, rgName, location, vnetName);
                //await CreateVirtualNetworkInterfaceAsync(
                //    subscriptionResource,
                //    rgName,
                //    location,
                //    vnetName,
                //    networkInterfaceName,
                //    networkInterfaceIpConfigName);


                // TODO: Provide arm resource id. we have 2 Nics
                var networkInterfaceArmId = "";
                await CreateVirtualMachineWithNicAsync(subscriptionResource, DaoSku, rgName, location, vmName, networkInterfaceArmId);

                // TODO: Provide arm resource id. of existing disk
                //var existingManagedDiskArmId = "";
                //await CreateVMWithManagedDiskAndNicResourceProvidedAsync(
                //    subscriptionResource,
                //    rgName,
                //    location,
                //    vmName,
                //    networkInterfaceArmId,
                //    existingManagedDiskArmId);

                Console.WriteLine("--------Finish Create vm--------");
            } 
            catch (Exception ex) 
            {
                await Console.Out.WriteLineAsync($"Failed creation of VM. region:{location}, errorMsg:{ex.Message}");
            }
            

            //Delete resource group if necessary
            //Console.WriteLine("--------Start delete vm--------");
            //await DeleteVirtualMachineAsync(subscriptionResource, rgName, vmName);
            //Console.WriteLine("--------Finish delete vm--------");
        }

        private async static Task<ResourceGroupResource> CreateResourceGroup(
            SubscriptionResource subscription,
            string rgName,
            string location)
        {
            var resourceGroups = subscription.GetResourceGroups();
            ResourceGroupData resourceGroupData = new ResourceGroupData(location);
            ArmOperation<ResourceGroupResource> resourceGroupOperation = await resourceGroups.CreateOrUpdateAsync(
                WaitUntil.Completed,
                rgName,
                resourceGroupData);
            return resourceGroupOperation.Value;
        }

        public static async Task GetAllVmsAsync(
            SubscriptionResource subscription,
            string rgName)
        {
            ResourceGroupCollection rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();

            AsyncPageable<VirtualMachineResource> response = vmCollection.GetAllAsync();
            await foreach (VirtualMachineResource vm in response)
            {
                Console.WriteLine(vm.Data.Name);
            }
        }

        private static async Task DeleteVirtualMachineAsync(
            SubscriptionResource subscription,
            string rgName,
            string vmName)
        {
            ResourceGroupResource resourceGroup = await subscription.GetResourceGroups().GetAsync(rgName);
            // Now we get the virtual machine collection from the resource group
            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();
            VirtualMachineResource vm = await vmCollection.GetAsync(vmName);
            await vm.DeleteAsync(WaitUntil.Completed);
        }

        private async static Task<VirtualNetworkResource> CreateVirtualNetworkAsync(
            SubscriptionResource subscription,
            string rgName,
            string location,
            string virtualNetworkName)
        {
            var rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            // Create VNet
            Console.WriteLine("--------Start create VNet--------");
            var virtualNetworkData = new VirtualNetworkData()
            {

                Location = location,
                AddressPrefixes = { "10.0.0.0/16" },
                Subnets = { new SubnetData() { Name = "SubnetSampleName", AddressPrefix = "10.0.0.0/28" } }
            };

            VirtualNetworkCollection virtualNetworks = resourceGroup.GetVirtualNetworks();
            ArmOperation<VirtualNetworkResource> virtualNetworkOperation = await virtualNetworks.CreateOrUpdateAsync(
                WaitUntil.Completed,
                virtualNetworkName,
                virtualNetworkData);
            VirtualNetworkResource virtualNetwork = virtualNetworkOperation.Value;

            return virtualNetwork;
        }

        private async static Task<NetworkInterfaceResource> CreateVirtualNetworkInterfaceAsync(
            SubscriptionResource subscription,
            string rgName,
            string location,
            string vnetName,
            string networkInterfaceName,
            string networkInterfaceIpConfigName)
        {
            var rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            var vnetResourceResponse = await resourceGroup.GetVirtualNetworkAsync(vnetName);
            var virtualNetwork = vnetResourceResponse.Value;

            // Create Network Interface
            Console.WriteLine("--------Start create Network Interface--------");
            var networkInterfaceIPConfiguration = new NetworkInterfaceIPConfigurationData()
            {
                Name = networkInterfaceIpConfigName,
                Primary = true,
                PrivateIPAllocationMethod = NetworkIPAllocationMethod.Dynamic,
                Subnet = new SubnetData() { Id = virtualNetwork.Data.Subnets.First().Id }
            };

            var networkInterfaceData = new NetworkInterfaceData() { Location = location };
            networkInterfaceData.IPConfigurations.Add(networkInterfaceIPConfiguration);
            var networkInterfaceCollection = resourceGroup.GetNetworkInterfaces();
            ArmOperation<NetworkInterfaceResource> networkInterfaceOperation = await networkInterfaceCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                networkInterfaceName,
                networkInterfaceData);

            NetworkInterfaceResource networkInterface = networkInterfaceOperation.Value;
            Console.WriteLine($"--------completed create Network Interface-------- id:{networkInterface.Id}");
            return networkInterface;
        }

        public static async Task CreateVirtualMachineAsync(
            SubscriptionResource subscription,
            string rgName,
            string location,
            string vmName)
        {
            var rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            // Create VNet
            Console.WriteLine("--------Start create VNet--------");
            string virtualNetworkName = "VnetSampleName";
            var virtualNetworkData = new VirtualNetworkData()
            {

                Location = location,
                AddressPrefixes = { "10.0.0.0/16" },
                Subnets = { new SubnetData() { Name = "SubnetSampleName", AddressPrefix = "10.0.0.0/28" } }
            };

            VirtualNetworkCollection virtualNetworks = resourceGroup.GetVirtualNetworks();
            ArmOperation<VirtualNetworkResource> virtualNetworkOperation = await virtualNetworks.CreateOrUpdateAsync(
                WaitUntil.Completed,
                virtualNetworkName,
                virtualNetworkData);
            VirtualNetworkResource virtualNetwork = virtualNetworkOperation.Value;

            // Create Network Interface
            Console.WriteLine("--------Start create Network Interface--------");
            string networkInterfaceName = "SampleNicName";
            var networkInterfaceIPConfiguration = new NetworkInterfaceIPConfigurationData()
            {
                Name = "SampleIpConfigName",
                Primary = true,
                PrivateIPAllocationMethod = NetworkIPAllocationMethod.Dynamic,
                Subnet = new SubnetData() { Id = virtualNetwork.Data.Subnets.First().Id }
            };

            var networkInterfaceData = new NetworkInterfaceData() { Location = location };
            networkInterfaceData.IPConfigurations.Add(networkInterfaceIPConfiguration);
            var networkInterfaceCollection = resourceGroup.GetNetworkInterfaces();
            ArmOperation<NetworkInterfaceResource> networkInterfaceOperation = await networkInterfaceCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                networkInterfaceName,
                networkInterfaceData);
            NetworkInterfaceResource networkInterface = networkInterfaceOperation.Value;

            // Create VM
            Console.WriteLine("--------Start create VM--------");
            var virtualMachineData = new VirtualMachineData(location)
            {
                HardwareProfile = new VirtualMachineHardwareProfile()
                {
                    VmSize = "Standard_E16bds_v5"
                },
                OSProfile = new VirtualMachineOSProfile()
                {
                    AdminUsername = Environment.GetEnvironmentVariable("AzureAdminUsername"),
                    AdminPassword = Environment.GetEnvironmentVariable("AzureAdminPassword"),
                    ComputerName = "windowsComputer",
                    WindowsConfiguration = new WindowsConfiguration()
                    {
                        ProvisionVmAgent = true
                    },
                },
                NetworkProfile = new VirtualMachineNetworkProfile()
                {
                    NetworkInterfaces =
                    {
                        new VirtualMachineNetworkInterfaceReference()
                        {
                            Id = networkInterface.Id,
                            Primary = true,
                        }
                    }
                },
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.FromImage)
                    {
                        Name = "SampleOsDisk",
                        DiskSizeGB = 128,
                        OSType = SupportedOperatingSystemType.Windows,
                        Caching = CachingType.None,
                        ManagedDisk = new VirtualMachineManagedDisk()
                        {
                            StorageAccountType = StorageAccountType.StandardLrs
                        },
                    },
                    DataDisks =
                    {
                        ArmModelCreator.CreateVirtualMachineDataDisk(
                            $"DataDisk_1",
                            100,
                            1,
                            DiskCreateOptionType.Empty,
                            ArmModelCreator.CreateVirtualMachineManagedDisk(StorageAccountType.StandardLrs),
                            CachingType.None),
                        /*
                        new VirtualMachineDataDisk(2, DiskCreateOptionType.Empty)
                        {
                            Name = "DataDisk_2",
                            DiskSizeGB = 10,
                            Caching = CachingType.None,
                            ManagedDisk = new VirtualMachineManagedDisk()
                            {
                                StorageAccountType = StorageAccountType.StandardLrs
                            }
                        },
                        */
                    },
                    ImageReference = ArmModelCreator.CreateImageReference(
                        "MicrosoftWindowsServer",
                        "WindowsServer",
                        "2022-Datacenter-g2"),
                    DiskControllerType = "NVME",
                }
            };

            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();
            ArmOperation<VirtualMachineResource> virtualMachineOperation = await vmCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                vmName,
                virtualMachineData);
            var virtualMachine = virtualMachineOperation.Value;

            Console.WriteLine("VM ID: " + virtualMachine.Id);
            Console.WriteLine("--------Done create VM--------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="rgName"></param>
        /// <param name="location"></param>
        /// <param name="vmName"></param>
        /// <param name="networkInterfaceId"></param>
        /// <returns></returns>
        public static async Task CreateVirtualMachineWithNicAsync(
            SubscriptionResource subscription,
            string skuName,
            string rgName,
            string location,
            string vmName,
            string networkInterfaceId)
        {
            var rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            // Create VM
            Console.WriteLine("--------Start create VM with networkInterfaceId-------- ");
            Console.WriteLine($"{networkInterfaceId}");
            var virtualMachineData = new VirtualMachineData(location)
            {
                HardwareProfile = new VirtualMachineHardwareProfile()
                {
                    VmSize = skuName
                },
                OSProfile = new VirtualMachineOSProfile()
                {
                    AdminUsername = Environment.GetEnvironmentVariable("AzureAdminUsername"),
                    AdminPassword = Environment.GetEnvironmentVariable("AzureAdminPassword"),
                    ComputerName = "windowsComputer",
                    WindowsConfiguration = new WindowsConfiguration()
                    {
                        ProvisionVmAgent = true
                    },
                },
                NetworkProfile = new VirtualMachineNetworkProfile()
                {
                    NetworkInterfaces =
                    {
                        new VirtualMachineNetworkInterfaceReference()
                        {
                            Id = new ResourceIdentifier(networkInterfaceId),
                            Primary = true,
                        }
                    }
                },
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.FromImage)
                    {
                        Name = "SampleOsDisk",
                        DiskSizeGB = 128,
                        OSType = SupportedOperatingSystemType.Windows,
                        Caching = CachingType.None,
                        ManagedDisk = new VirtualMachineManagedDisk()
                        {
                            StorageAccountType = StorageAccountType.StandardLrs
                        }
                    },
                    DataDisks =
                    {
                        ArmModelCreator.CreateVirtualMachineDataDisk(
                            $"DataDisk_1",
                            100,
                            1,
                            DiskCreateOptionType.Empty,
                            ArmModelCreator.CreateVirtualMachineManagedDisk(StorageAccountType.PremiumLrs),
                            CachingType.None),
                    },
                    ImageReference = ArmModelCreator.CreateImageReference(
                        "MicrosoftWindowsServer",
                        "WindowsServer",
                        "2022-Datacenter-g2"),
                    DiskControllerType = "NVME",
                }
            };

            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();
            ArmOperation<VirtualMachineResource> virtualMachineOperation = await vmCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                vmName,
                virtualMachineData);
            var virtualMachine = virtualMachineOperation.Value;

            Console.WriteLine("VM ID: " + virtualMachine.Id);
            Console.WriteLine("--------Done create VM with networkInterfaceId--------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="rgName"></param>
        /// <param name="location"></param>
        /// <param name="vmName"></param>
        /// <param name="networkInterfaceId"></param>
        /// <returns></returns>
        public static async Task CreateVMWithManagedDiskAndNicResourceProvidedAsync(
            SubscriptionResource subscription,
            string rgName,
            string location,
            string vmName,
            string networkInterfaceId,
            string managedDiskId)
        {
            var rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            // Create VM
            Console.WriteLine("--------Start create VM with networkInterfaceId-------- ");

            Console.WriteLine($"{networkInterfaceId}");
            var networkInterfaceReference = ArmModelCreator.CreateVirtualMachineNetworkInterfaceReference(networkInterfaceId);

            var virtualMachineData = new VirtualMachineData(location)
            {
                HardwareProfile = ArmModelCreator.CreateVirtualMachineHardwareProfile("Standard_E16bds_v5"),
                NetworkProfile = ArmModelCreator.CreateVirtualMachineNetworkProfile(networkInterfaceReference),
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.Attach)
                    {
                        ManagedDisk = new VirtualMachineManagedDisk()
                        {
                            Id = new ResourceIdentifier(managedDiskId),
                        },
                        OSType = SupportedOperatingSystemType.Windows,
                    },
                    DataDisks =
                    {
                        ArmModelCreator.CreateVirtualMachineDataDisk(
                            $"SampleDataDisk_1_{vmName}",
                            1000,
                            1,
                            DiskCreateOptionType.Empty,
                            ArmModelCreator.CreateVirtualMachineManagedDisk(StorageAccountType.PremiumLrs),
                            CachingType.None),
                    },
                    DiskControllerType = "NVME",
                }
            };

            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();
            ArmOperation<VirtualMachineResource> virtualMachineOperation = await vmCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                vmName,
                virtualMachineData);
            var virtualMachine = virtualMachineOperation.Value;

            Console.WriteLine("VM ID: " + virtualMachine.Id);
            Console.WriteLine("--------Done create VM with networkInterfaceId--------");
        }

        private static async Task<ResourceIdentifier> CreateManagedDiskFromVhd(
            SubscriptionResource subscriptionResource,
            string rgName,
            string diskName,
            AzureLocation location,
            string vhdUri,
            string storageAccountArmId,
            int diskSize)
        {
            var rgCollections = subscriptionResource.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            ManagedDiskData managedDiskData = new ManagedDiskData(location)
            {
                Sku = new DiskSku()
                {
                    Name = DiskStorageAccountType.PremiumLrs
                },
                CreationData = new DiskCreationData(DiskCreateOption.Import)
                {
                    SourceUri = new Uri(vhdUri),
                    StorageAccountId = new ResourceIdentifier(storageAccountArmId),
                },
                DiskSizeGB = diskSize,
                HyperVGeneration = HyperVGeneration.V2,
                OSType = SupportedOperatingSystemType.Windows,
                SupportedCapabilities = new SupportedCapabilities()
                {
                    DiskControllerTypes = "NVME",
                },
            };

            ManagedDiskCollection diskCollection = resourceGroup.GetManagedDisks();
            ArmOperation<ManagedDiskResource> managedDisOperation = await diskCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                diskName,
                managedDiskData);

            ManagedDiskResource disk = managedDisOperation.Value;

            return disk.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="rgName"></param>
        /// <param name="location"></param>
        /// <param name="vmName"></param>
        /// <param name="networkInterfaceId"></param>
        /// <returns></returns>
        public static async Task CreateVMWithManagedDiskFromVhdAndNicResourceProvidedAsync(
            SubscriptionResource subscription,
            string rgName,
            string location,
            string vmName,
            string networkInterfaceId,
            ResourceIdentifier managedDiskId)
        {
            var rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            // Create VM
            Console.WriteLine("--------Start create VM with networkInterfaceId-------- ");

            Console.WriteLine($"{networkInterfaceId}");
            var networkInterfaceReference = ArmModelCreator.CreateVirtualMachineNetworkInterfaceReference(networkInterfaceId);

            var virtualMachineData = new VirtualMachineData(location)
            {
                HardwareProfile = ArmModelCreator.CreateVirtualMachineHardwareProfile("Standard_E16bds_v5"),
                NetworkProfile = ArmModelCreator.CreateVirtualMachineNetworkProfile(networkInterfaceReference),
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.Attach)
                    {
                        ManagedDisk = new VirtualMachineManagedDisk()
                        {
                            Id = managedDiskId,
                        },
                        OSType = SupportedOperatingSystemType.Windows,
                    },
                    DataDisks =
                    {
                        ArmModelCreator.CreateVirtualMachineDataDisk(
                            $"SampleDataDisk_1_{vmName}",
                            2048,
                            0,
                            DiskCreateOptionType.Empty,
                            ArmModelCreator.CreateVirtualMachineManagedDisk(StorageAccountType.PremiumLrs),
                            CachingType.None),
                    },
                    DiskControllerType = DiskControllerType.NVMe,
                },
            };

            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();
            ArmOperation<VirtualMachineResource> virtualMachineOperation = await vmCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                vmName,
                virtualMachineData);
            var virtualMachine = virtualMachineOperation.Value;

            Console.WriteLine("VM ID: " + virtualMachine.Id);
            Console.WriteLine("--------Done create VM with networkInterfaceId--------");
        }


        
    }
}
