using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Resources;

namespace AzureConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var location = AzureLocation.EastAsia;
            var rgName = "kam-arm-dev-rg-ea";
            var vmName = "dev-022701-vm-ea";
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("TenantId");
            var subscription = Environment.GetEnvironmentVariable("SubscriptionId");
            //ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            VisualStudioCredential credential = new VisualStudioCredential();
            ArmClient client = new ArmClient(credential, subscription);

            // Create Resource Group
            SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();
            Console.WriteLine("--------Start create group--------");
            var rgResource = await CreateResourceGroup(subscriptionResource, rgName, location);
            Console.WriteLine("--------Finish create group--------");

            // Get all vms

            Console.WriteLine("--------Get all vms--------");
            await GetAllVmsAsync(subscriptionResource, rgName);
            Console.WriteLine("--------Finish Get all vms--------");
            
            // Create a Virtual Machine
            Console.WriteLine("--------Create vm--------");
            //await CreateVirtualMachineAsync(subscriptionResource, rgName, location, vmName);
            var networkInterface = "/subscriptions/bf0cfa3e-3884-49df-b8e8-9715efcc9c00/resourceGroups/kam-arm-dev-rg-ea/providers/Microsoft.Network/networkInterfaces/SampleNicName";
            await CreateVirtualMachineAsync(subscriptionResource, rgName, location, vmName, networkInterface);
            Console.WriteLine("--------Finish Create vm--------");

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
                        }
                    },
                    DataDisks =
                    {
                        new VirtualMachineDataDisk(1, DiskCreateOptionType.Empty)
                        {
                            Name = "SampleDataDisk_1",
                            DiskSizeGB = 100,
                            ManagedDisk = new VirtualMachineManagedDisk()
                            {
                                StorageAccountType = StorageAccountType.StandardLrs,
                            }
                        },
                        /*
                        new VirtualMachineDataDisk(2, DiskCreateOptionType.Empty)
                        {
                            DiskSizeGB = 10,
                            Caching = CachingType.ReadWrite,
                            ManagedDisk = new VirtualMachineManagedDisk()
                            {
                                StorageAccountType = StorageAccountType.StandardLrs
                            }
                        },*/
                    },
                    ImageReference = new ImageReference()
                    {
                        Publisher = "MicrosoftWindowsServer",
                        Offer = "WindowsServer",
                        Sku = "2022-Datacenter-g2",
                        Version = "latest",
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
        public static async Task CreateVirtualMachineAsync(
            SubscriptionResource subscription,
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
                        new VirtualMachineDataDisk(1, DiskCreateOptionType.Empty)
                        {
                            Name = "SampleDataDisk_1",
                            DiskSizeGB = 100,
                            ManagedDisk = new VirtualMachineManagedDisk()
                            {
                                StorageAccountType = StorageAccountType.StandardLrs,
                            }
                        },
                    },
                    ImageReference = new ImageReference()
                    {
                        Publisher = "MicrosoftWindowsServer",
                        Offer = "WindowsServer",
                        Sku = "2022-Datacenter-g2",
                        Version = "latest",
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
    }
}
