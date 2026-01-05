using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Resources;

namespace AzureConsoleApplication.Factories
{
    public class CreateVirtualMachineUsingGalleyFactory
    {
        public static string subscription = "";
        public static AzureLocation location = AzureLocation.WestUS2;
        public static string rgName = "kam-dev-rg-wus2";

        public static readonly string EbdSku = "Standard_E16bds_v5";
        public static readonly string Eadsv6 = "Standard_E4ads_v6";

        public static readonly int DataDisk = 500;

        public static readonly string GalleryName = "kam.dev.acg.wus2";
        public static readonly string ImageDefinitionName = "TrustedLaunchSupportedWithAnyControllerTypeImageName";

        public static ResourceIdentifier ImageDefResourceId = GalleryImageResource.CreateResourceIdentifier(
            subscription,
            rgName,
            GalleryName,
            ImageDefinitionName);

        /// <summary>
        /// 1. you can create Vnet/NIC/VM in this method
        /// 2. Create VM and OS managed disk from VM in this method. use the OS managed disk to create image version
        /// 3. create gallery/image definition/image version in this method
        /// 4. create managed disk from azure image version in this method
        /// 5. create VM from image gallery version
        /// </summary>
        /// <returns></returns>
        public static async Task AzureOperationInWestUS2Async()
        {
            var vmName = "kam-dev-vm-wus2";
            ArmClient client = GetAzureClient(subscription);

            // Create Resource Group
            SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();
            //Console.WriteLine("--------Start create group--------");
            //var rgResource = await CreateResourceGroup(subscriptionResource, rgName, location);
            //Console.WriteLine("--------Finish create group--------");

            // Create Vnet and Nic
            var vnetName = "VnetSampleName"; //"VnetSampleName01";
            var networkInterfaceName = "VnetSampleNameInterfaceName1";
            var networkInterfaceIpConfigName = "VnetSampleNameInterfaceIpConfigName1";
            //await CreateVirtualNetworkAsync(subscriptionResource, rgName, location, vnetName);
            //await CreateVirtualNetworkInterfaceAsync(
            //    subscriptionResource,
            //    rgName,
            //    location,
            //    vnetName,
            //    networkInterfaceName,
            //    networkInterfaceIpConfigName);

            // Get all vms
            Console.WriteLine("--------Get all vms--------");
            //await GetAllVmsAsync(subscriptionResource, rgName);
            Console.WriteLine("--------Finish Get all vms--------");

            Console.WriteLine("--------Create gallery and imgae definition--------");
            //await CreateImageGalleryAndImageDefinitionAsync(subscriptionResource, rgName, location);
            Console.WriteLine("--------Create image version--------");
            //var standardDisk = "";
            //await CreateImageVersionFromOsDiskResourceIdAsync(
            //    location, 
            //    ImageDefResourceId, 
            //    standardDisk,
            //    "1.0.0");
            //var standardNvmeDisk = "";
            //await CreateImageVersionFromOsDiskResourceIdAsync(
            //    location, 
            //    ImageDefResourceId,
            //    standardNvmeDisk,
            //    "2.0.0");
            //var trustedScsiDisk = "";
            //await CreateImageVersionFromOsDiskResourceIdAsync(
            //    location, 
            //    ImageDefResourceId,
            //    trustedScsiDisk,
            //    "3.0.0");

            // Create a Virtual Machine
            Console.WriteLine("--------Create vm--------");

            // TODO: Provide arm resource id. we have 2 Nics
            var networkInterfaceArmId = "";
            var networkInterfaceArmId1 = "";
            //var networkInterfaceArmId = "";

            //await CreateVirtualMachineWithNicToCreateOsManagedDiskAsync(
            //    subscriptionResource,
            //    EbdSku,
            //    rgName,
            //    location,
            //    vmName,
            //    networkInterfaceArmId,
            //    true,
            //    DiskControllerType.Scsi);

            //TODO: Provide arm resource id
            var galleryRef = "";

            var diskName = $"ManagedOsDiskFromVhd_{vmName}";
            var diskSize = 256;
            //var diskResourceId = await CreateManagedDiskFromGalleyImagerVersion(
            //    subscriptionResource,
            //    rgName,
            //    diskName,
            //    location,
            //    galleryRef,
            //    diskSize);

            var diskResourceId = "";
            var diskResourceIdentifier = new ResourceIdentifier(diskResourceId);
            //await CreateVMWithManagedDiskAndNicResourceProvidedAsync(
            //    subscriptionResource,
            //    Eadsv6,
            //    rgName,
            //    location,
            //    vmName,
            //    networkInterfaceArmId,
            //    diskResourceIdentifier);

            //var galleryImageResrc = new ResourceIdentifier("");
            var csGalleryImageResrc = new ResourceIdentifier("");
            await CreateVMWithImageGalleryVersionAndNicResourceProvidedAsync(
                subscriptionResource,
                Eadsv6,
                rgName,
                location,
                $"{vmName}-scsi-{Eadsv6}",
                networkInterfaceArmId1,
                csGalleryImageResrc,
                DiskControllerType.Scsi);

            //await CreateManagedDiskFromVhdInEastAsiaAsync();

            Console.WriteLine("--------Finish Create vm--------");

            //Delete resource group if necessary
            //Console.WriteLine("--------Start delete vm--------");
            //await DeleteVirtualMachineAsync(subscriptionResource, rgName, vmName);
            //Console.WriteLine("--------Finish delete vm--------");
        }

        private static ArmClient GetAzureClient(String subId)
        {
            VisualStudioCredential credential = new VisualStudioCredential();
            ArmClient client = new ArmClient(credential, subId);
            return client;
        }

        public static async Task CreateVMWithManagedDiskAndNicResourceProvidedAsync(
            SubscriptionResource subscription,
            string skuName,
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
            Console.WriteLine("--------Start create VM with networkInterfaceId and managed disk-------- ");

            Console.WriteLine($"{networkInterfaceId}");
            var networkInterfaceReference = ArmModelCreator.CreateVirtualMachineNetworkInterfaceReference(networkInterfaceId);

            var virtualMachineData = new VirtualMachineData(location)
            {
                HardwareProfile = ArmModelCreator.CreateVirtualMachineHardwareProfile(skuName),
                NetworkProfile = ArmModelCreator.CreateVirtualMachineNetworkProfile(networkInterfaceReference),
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.Attach)
                    {
                        ManagedDisk = new VirtualMachineManagedDisk()
                        {
                            Id = managedDiskId,
                        },
                        OSType = SupportedOperatingSystemType.Linux,
                    },
                    DataDisks =
                    {
                        ArmModelCreator.CreateVirtualMachineDataDisk(
                            $"SampleDataDisk_1_{vmName}",
                            DataDisk,
                            0,
                            DiskCreateOptionType.Empty,
                            ArmModelCreator.CreateVirtualMachineManagedDisk(StorageAccountType.PremiumLrs),
                            CachingType.None),
                    },
                    DiskControllerType = DiskControllerType.Scsi,
                },
                SecurityProfile = new SecurityProfile()
                {
                    SecurityType = SecurityType.TrustedLaunch,
                    UefiSettings = new UefiSettings()
                    {
                        IsSecureBootEnabled = true,
                        IsVirtualTpmEnabled = true,
                    }
                },
            };

            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();
            ArmOperation<VirtualMachineResource> virtualMachineOperation = await vmCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                vmName,
                virtualMachineData);
            var virtualMachine = virtualMachineOperation.Value;

            Console.WriteLine("VM ID: " + virtualMachine.Id);
            Console.WriteLine("--------Done create VM with networkInterfaceId and managed disk--------");
        }


        public static async Task<ResourceIdentifier> CreateManagedDiskFromGalleyImagerVersion(
            SubscriptionResource subscriptionResource,
            string rgName,
            string diskName,
            AzureLocation location,
            string galleryRef,
            int diskSize)
        {
            var rgCollections = subscriptionResource.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            Console.WriteLine("-------- start managed disk creation from VHD --------");
            ManagedDiskData managedDiskData = new ManagedDiskData(location)
            {
                Sku = new DiskSku()
                {
                    Name = DiskStorageAccountType.PremiumLrs
                },
                CreationData = new DiskCreationData(DiskCreateOption.FromImage)
                {
                    GalleryImageReference = new ImageDiskReference()
                    {
                        Id = new ResourceIdentifier(galleryRef)
                    }
,
                },
                DiskSizeGB = diskSize,
                HyperVGeneration = HyperVGeneration.V2,
                OSType = SupportedOperatingSystemType.Linux,
                SecurityProfile = new DiskSecurityProfile()   // Added this
                {
                    SecurityType = DiskSecurityType.TrustedLaunch
                }
                //SupportedCapabilities = new SupportedCapabilities()
                //{
                //    DiskControllerTypes = "NVME",
                //},
            };

            ManagedDiskCollection diskCollection = resourceGroup.GetManagedDisks();
            ArmOperation<ManagedDiskResource> managedDisOperation = await diskCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                diskName,
                managedDiskData);

            ManagedDiskResource disk = managedDisOperation.Value;

            Console.WriteLine($"-------- Done: created managed disk from VHD disk.Id:{disk.Id}--------");
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
        public static async Task CreateVirtualMachineWithNicToCreateOsManagedDiskAsync(
            SubscriptionResource subscription,
            string skuName,
            string rgName,
            string location,
            string vmName,
            string networkInterfaceId,
            bool isTrustedLaunch,
            DiskControllerType diskControllerType)
        {
            var rgCollections = subscription.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            var namePrefix = isTrustedLaunch ? "trusted" : string.Empty;
            var securityProfile = new SecurityProfile();
            if (isTrustedLaunch)
            {
                securityProfile = new SecurityProfile()
                {
                    SecurityType = SecurityType.TrustedLaunch,
                    UefiSettings = new UefiSettings()
                    {
                        IsVirtualTpmEnabled = true,
                        IsSecureBootEnabled = true,
                    },
                };
            }
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
                    ComputerName = "computerName",
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
                SecurityProfile = securityProfile,
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.FromImage)
                    {
                        Name = string.Join("_", new[] { namePrefix, diskControllerType.ToString(), vmName }),
                        DiskSizeGB = 256,
                        OSType = SupportedOperatingSystemType.Linux,
                        Caching = CachingType.None,
                        ManagedDisk = new VirtualMachineManagedDisk()
                        {
                            StorageAccountType = StorageAccountType.PremiumLrs
                        }
                    },
                    ImageReference = ArmModelCreator.CreateImageReference(
                        "Canonical",
                        "0001-com-ubuntu-server-jammy",
                        "22_04-lts-gen2"),
                    DiskControllerType = diskControllerType,
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

        private static async Task CreateVMWithImageGalleryVersionAndNicResourceProvidedAsync(
            SubscriptionResource subscriptionResource,
            string skuName,
            string rgName,
            AzureLocation location,
            string vmName,
            string networkInterfaceArmId,
            ResourceIdentifier imageVersionResource,
            DiskControllerType diskType)
        {
            var rgCollections = subscriptionResource.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            var networkInterfaceReference = ArmModelCreator.CreateVirtualMachineNetworkInterfaceReference(networkInterfaceArmId);

            var virtualMachineData = new VirtualMachineData(location)
            {
                HardwareProfile = ArmModelCreator.CreateVirtualMachineHardwareProfile(skuName),
                NetworkProfile = ArmModelCreator.CreateVirtualMachineNetworkProfile(networkInterfaceReference),
                StorageProfile = ArmModelCreator.CreateVirtualMachineStorageProfileForImageVersion(imageVersionResource, diskType),
                SecurityProfile = new SecurityProfile()
                {
                    SecurityType = SecurityType.TrustedLaunch,
                    UefiSettings = new UefiSettings()
                    {
                        IsSecureBootEnabled = true,
                        IsVirtualTpmEnabled = true,
                    }
                },
            };

            VirtualMachineCollection vmCollection = resourceGroup.GetVirtualMachines();
            ArmOperation<VirtualMachineResource> virtualMachineOperation = await vmCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                vmName,
                virtualMachineData);
            var virtualMachine = virtualMachineOperation.Value;

            Console.WriteLine("VM ID: " + virtualMachine.Id);
            Console.WriteLine("--------Done create VM with image version and disk controller type--------");
        }

        public static async Task CreateImageVersionFromOsDiskResourceIdAsync(
            AzureLocation location,
            ResourceIdentifier imageDefResourceId,
            string managedDiskResourceId,
            string imageVersionName)
        {
            ArmClient client = GetAzureClient(subscription);
            // 1. Setup the client and resource identifier for the Image Definition
            GalleryImageResource imageDefinition = client.GetGalleryImageResource(imageDefResourceId);
            GalleryImageVersionCollection versionCollection = imageDefinition.GetGalleryImageVersions();

            // 2. Configure the Image Version Data
            var versionData = new GalleryImageVersionData(location)
            {
                StorageProfile = new GalleryImageVersionStorageProfile()
                {
                    OSDiskImage = new GalleryOSDiskImage()
                    {
                        // Point directly to the Managed Disk ID
                        Source = new GalleryDiskImageSource()
                        {
                            Id = new ResourceIdentifier(managedDiskResourceId)
                        }
                    }
                },
                PublishingProfile = new GalleryImageVersionPublishingProfile()
                {
                    // Define where this image should be replicated
                    TargetRegions =
                    {
                        new TargetRegion("East US") { RegionalReplicaCount = 1 },
                        new TargetRegion("West US 2") { RegionalReplicaCount = 1 },
                    }
                }
            };

            // 3. Create the version (use semantic versioning: e.g., "1.0.0")
            ArmOperation<GalleryImageVersionResource> lro = await versionCollection.CreateOrUpdateAsync(
                WaitUntil.Started,
                imageVersionName,
                versionData);

            Console.WriteLine($"Succeeded: Created Version {lro.Value.Data.Name}, id:{lro.Id}");
        }

        public static async Task CreateImageGalleryAndImageDefinitionAsync(
            SubscriptionResource subscriptionResource,
            string rgName,
            AzureLocation location)
        {
            //string imageVersionName = "1.0.0";
            var rgCollections = subscriptionResource.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            /// Create Gallery
            var galleryCollection = resourceGroup.GetGalleries();

            ResourceIdentifier galleryResourceId = GalleryResource.CreateResourceIdentifier(
                subscriptionResource.Id,
                rgName,
                GalleryName);
            GalleryResource gallery;
            bool exists = await galleryCollection.ExistsAsync(GalleryName);
            if (!exists)
            {
                var galleryData = new GalleryData(location)
                {
                    Description = "kartik mahaley gallery"
                };
                ArmOperation<GalleryResource> galleryUpsertOp =
                await galleryCollection.CreateOrUpdateAsync(WaitUntil.Completed, GalleryName, galleryData);
                gallery = galleryUpsertOp.Value;
                //var galleryResourceId = gallery.Id;
                Console.WriteLine("Created Gallery ID: " + galleryResourceId);
            }
            else
            {
                gallery = await galleryCollection.GetAsync(GalleryName);
                Console.WriteLine("Gallery already exists. ID: " + gallery.Id);
            }

            /// Create Image Definition
            var imageDefinitionData = new GalleryImageData(location)
            {
                OSType = SupportedOperatingSystemType.Linux,          // Linux or Windows
                OSState = OperatingSystemStateType.Specialized,                      // Generalized or Specialized
                HyperVGeneration = HyperVGeneration.V2,    // V1 (Gen1) or V2 (Gen2). TL/CVM => V2.
                Description = "kamahale trsuted launch supported and any disk controller type",
                Identifier = new GalleryImageIdentifier("MyPublisher", "MyOffer", "MySku"),
            };
            var trustedLaunchFeature = new GalleryImageFeature()
            {
                Name = "SecurityType",
                Value = "TrustedLaunchSupported"
            };
            var diskControllerFeature = new GalleryImageFeature()
            {
                Name = "DiskControllerTypes",
                Value = "SCSI,NVMe"
            };
            imageDefinitionData.Features.Add(trustedLaunchFeature);
            imageDefinitionData.Features.Add(diskControllerFeature);

            GalleryImageCollection imageCollection = gallery.GetGalleryImages();
            if (await imageCollection.ExistsAsync(ImageDefinitionName))
            {
                Console.WriteLine("Image definition already exists. Skipping creation.");
                return;
            }

            ArmOperation<GalleryImageResource> imageDefCreateOp =
                await imageCollection.CreateOrUpdateAsync(WaitUntil.Completed, ImageDefinitionName, imageDefinitionData);
            GalleryImageResource imageDefinition = imageDefCreateOp.Value;
            var imageDefinitionId = imageDefinition.Id;

            Console.WriteLine("Created image definition ID: " + imageDefinitionId);
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

        public static async Task CreateManagedDiskFromVhdInEastAsiaAsync()
        {
            var rgName = "kam-dev-rg-ea";
            ArmClient client = GetAzureClient("9d08c327-957c-4c61-97ce-21341902341c");

            // Create Resource Group
            SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();
            var rgCollections = subscriptionResource.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;
            var storageAccountArmId = "/subscriptions/9d08c327-957c-4c61-97ce-21341902341c/resourceGroups/bbc-vhd-creation-ppe-rg/providers/Microsoft.Storage/storageAccounts/vhdcreationarteappesa";
            var managedDiskData = new ManagedDiskData(new AzureLocation("east asia"))
            {
                Sku = new DiskSku()
                {
                    Name = DiskStorageAccountType.PremiumLrs
                },
                CreationData = new DiskCreationData(DiskCreateOption.Import)
                {
                    SourceUri = new Uri("https://vhdcreationarteappesa.blob.core.windows.net/vhds/06c347530e9a1b.vhd"),
                    StorageAccountId = new ResourceIdentifier(storageAccountArmId),
                },
                DiskSizeGB = 30,
                HyperVGeneration = HyperVGeneration.V2,
                OSType = SupportedOperatingSystemType.Linux,
                SupportedCapabilities = new SupportedCapabilities()
                {
                    DiskControllerTypes = "SCSI,NVMe",
                },
            };

            var diskName = "sampleDiskToCreateImageGalleryVersion1";
            ManagedDiskCollection diskCollection = resourceGroup.GetManagedDisks();
            ArmOperation<ManagedDiskResource> managedDisOperation = await diskCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                diskName,
                managedDiskData);
            var managedDiskId = managedDisOperation.Value.Id;

            Console.WriteLine($"--------completed create Managed Disk from vhd-------- id:{managedDiskId}");
        }
    }
}
