using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;

namespace AzureConsoleApplication.Factories
{
    public static class CreateArmVirtualMachineUsingGalleyFactory
    {
        public static string subscription = "";
        public static AzureLocation westus2Location = AzureLocation.WestUS2;
        public static string rgName = "kam-dev-rg-wus2";

        public static readonly string Dpsv6 = "Standard_D4ps_v6";
        public static readonly string DpdsV6 = "Standard_D4pds_v6";

        public static readonly int DataDisk = 500;
        public static readonly int DiskSize = 128;

        public static readonly string GalleryName = "kam.dev.acg.wus2";
        public static readonly string ImageDefinitionName = "Arm_TrustedLaunchSupported";

        public static async Task AzureOperationInWestUS2Async()
        {
            var vmName = "kam-dev-arm-vm-wus2";

            //ArmClient client = GetAzureClient(subscription);
            //SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();


            //if (true)
            //{
            //    var res = "/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Compute/galleries/kam.dev.acg.wus2";
            //    var galleryResource = client.GetGalleryResource(new ResourceIdentifier(res));
            //    var galleryResourceAzure = await galleryResource.GetAsync();
            //    var data1 = galleryResourceAzure.Value.Data;

            //    var rg = client.GetResourceGroupResource(ResourceGroupResource.CreateResourceIdentifier(subscription.ToString(), rgName));
            //    var galleries = rg.GetGalleries();
            //    var galleyRes = await galleries.GetAsync("kam.dev.acg.wus2");
            //    var data2 = galleyRes.Value.Data;
            //}
            //else
            //{
            //    return;
            //}

            // Create Resource Group
            Console.WriteLine("--------Create gallery and imgae definition--------");
            //await CreateImageGalleryAndImageDefinitionAsync(subscriptionResource, rgName, location);

            Console.WriteLine("--------Create managed disk from vhd --------");
            //var diskId = await CreateManagedDiskFromVhdAsync(
            //    "/subscriptions//resourceGroups/bbc-vhd-creation-ppe-rg/providers/Microsoft.Storage/storageAccounts/vhdcreationartppeneug1",
            //    subscription,//"",
            //    rgName,
            //    "north europe",
            //    "https://vhdcreationartppeneug1.blob.core.windows.net/vhds/5fe81555120511.g2.vhd",
            //    "armDisk-supported-windows-5fe81555120511.g2.vhd",
            //    SupportedOperatingSystemType.Windows);

            //var imageDefResourceId = new ResourceIdentifier("/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Compute/galleries/kam.dev.acg.wus2/images/Arm_TrustedLaunchSupported");
            //var diskId = "/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Compute/disks/armDisk-supported-windows-5fe81555120511.g2.vhd";
            //var imageDefResourceId = new ResourceIdentifier("/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Compute/galleries/kam.dev.acg.wus2/images/TrustedLaunchSupportedWithAnyControllerTypeImageName");
            //var diskId = "/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Compute/disks/armDisk-windows-5fe81555120511.g2.vhd";
            Console.WriteLine("--------Create sig image version--------");
            //await CreateImageVersionFromOsDiskResourceIdAsync(
            //    "north europe",
            //    imageDefResourceId,
            //    diskId,
            //    "1.0.2");

            // Create a Managed disk from SIG
            Console.WriteLine("--------Create managed disk from SIG--------");
            //var diskName = "armDiskFromSig_bd339923a2a526";
            //var galleryRef = "/subscriptions//resourceGroups/Cluster-AzureComputeGallery-cosmosOnAzureNonProd-rg/providers/Microsoft.Compute/galleries/Cluster_AzureComputeGallery_cosmosOnAzureNonProd_Primary/images/Scope_2.0.0_Windows_G2_Arm64/versions/1.20260506.19573147";
            //var managedDiskFromSig = await CreateManagedDiskFromGalleyImagerVersion(
            //    subscriptionResource,
            //    rgName,
            //    diskName,
            //    westus2Location,
            //    galleryRef,
            //    DiskSize);
            // Create a Virtual Machine
            Console.WriteLine("--------Create vm--------");

            // TODO: Provide arm resource id. we have 2 Nics
            var networkInterfaceArmId = "/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Network/networkInterfaces/VnetSampleNameInterfaceName";
            var networkInterfaceArmId1 = "/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Network/networkInterfaces/VnetSampleNameInterfaceName1";
            //var managedDiskFromSig = new ResourceIdentifier("/subscriptions//resourceGroups/kam-dev-rg-wus2/providers/Microsoft.Compute/disks/armDiskFromSig1");

            //await CreateVMWithManagedDiskAndNicResourceProvidedAsync(
            //    subscriptionResource,
            //    DpdsV6,
            //    rgName,
            //    westus2Location,
            //    vmName,
            //    networkInterfaceArmId,
            //    managedDiskFromSig);
            //await CreateVirtualMachineWithNicToCreateOsManagedDiskAsync(
            //    subscriptionResource,
            //    Dpsv6, 
            //    rgName,
            //    location,
            //    vmName,
            //    networkInterfaceArmId);

            Console.WriteLine("--------Finish Create vm--------");

        }

        private static ArmClient GetAzureClient(String subId)
        {
            VisualStudioCredential credential = new VisualStudioCredential();
            ArmClient client = new ArmClient(credential, subId);
            return client;
        }

        public static async Task CreateVirtualMachineWithNicToCreateOsManagedDiskAsync(
            SubscriptionResource subscription,
            string skuName,
            string rgName,
            AzureLocation location,
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
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.FromImage)
                    {
                        Name = string.Join("_", new[] { "ManagedArmDisk", vmName }),
                        DiskSizeGB = 256,
                        OSType = SupportedOperatingSystemType.Windows,
                        Caching = CachingType.None,
                        ManagedDisk = new VirtualMachineManagedDisk()
                        {
                            StorageAccountType = StorageAccountType.PremiumLrs
                        }
                    },
                    ImageReference = ArmModelCreator.CreateImageReference(
                        "microsoftwindowsdesktop",
                        "windows11preview-arm64",
                        "win11-25h2-ent"),
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
                OSType = SupportedOperatingSystemType.Windows,          // Linux or Windows
                OSState = OperatingSystemStateType.Specialized,                      // Generalized or Specialized
                HyperVGeneration = HyperVGeneration.V2,    // V1 (Gen1) or V2 (Gen2). TL/CVM => V2.
                Description = "kamahale trsuted launch supported with arm",
                Identifier = new GalleryImageIdentifier("ArmMyPublisher", "ArmMyOffer", "ArmMySku"),
                Architecture = ArchitectureType.Arm64, // x64 or Arm64. TL/CVM => Arm64
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

        public static async Task<string> CreateManagedDiskFromVhdAsync(
            string storageAccountArmId,
            string vhdSubId,
            string rgName,
            string sameRegionAsVhdBlobLocation,
            string vhdUri,
            string diskName,
            SupportedOperatingSystemType operatingSystemType)
        {
            ArmClient client = GetAzureClient(vhdSubId);

            // Create Resource Group
            SubscriptionResource subscriptionResource = await client.GetDefaultSubscriptionAsync();
            var rgCollections = subscriptionResource.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;
            var managedDiskData = new ManagedDiskData(new AzureLocation(sameRegionAsVhdBlobLocation))
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
                HyperVGeneration = HyperVGeneration.V2,
                OSType = operatingSystemType,
                SupportedCapabilities = new SupportedCapabilities()
                {
                    DiskControllerTypes = "SCSI,NVMe",
                    Architecture = "Arm64",
                },
            };

            ManagedDiskCollection diskCollection = resourceGroup.GetManagedDisks();
            ArmOperation<ManagedDiskResource> managedDisOperation = await diskCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                diskName,
                managedDiskData);
            var managedDiskId = managedDisOperation.Value.Id;
            Console.WriteLine($"--------completed create Managed Disk from vhd-------- id:{managedDiskId}");

            return managedDiskId;
        }

        public static async Task CreateImageVersionFromOsDiskResourceIdAsync(
            AzureLocation managedDiskLocation,
            ResourceIdentifier imageDefResourceId,
            string managedDiskResourceId,
            string imageVersionName)
        {
            ArmClient client = GetAzureClient(subscription);
            // 1. Setup the client and resource identifier for the Image Definition
            GalleryImageResource imageDefinition = client.GetGalleryImageResource(imageDefResourceId);
            GalleryImageVersionCollection versionCollection = imageDefinition.GetGalleryImageVersions();

            // 2. Configure the Image Version Data
            var versionData = new GalleryImageVersionData(managedDiskLocation)
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
                        new TargetRegion("north europe") { RegionalReplicaCount = 1 },
                    }
                }
            };

            // 3. Create the version (use semantic versioning: e.g., "1.0.0")
            ArmOperation<GalleryImageVersionResource> lro = await versionCollection.CreateOrUpdateAsync(
                WaitUntil.Started,
                imageVersionName,
                versionData);

            Console.WriteLine($"Succeeded: Called SIG Version not waiting, id:{lro.Id}");
        }

        public static async Task CreateVMWithManagedDiskAndNicResourceProvidedAsync(
            SubscriptionResource subscription,
            string skuName,
            string rgName,
            string location,
            string vmName,
            string networkInterfaceId,
            ResourceIdentifier managedDiskId,
            bool isTrustedLaunch = false)
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
                        OSType = SupportedOperatingSystemType.Windows,
                    },
                    //DiskControllerType = "SCSI,NVMe",
                },

            };

            //if (isTrustedLaunch)
            //{
            //    virtualMachineData.SecurityProfile = new SecurityProfile()
            //    {
            //        SecurityType = SecurityType.TrustedLaunch,
            //        UefiSettings = new UefiSettings()
            //        {
            //            IsSecureBootEnabled = true,
            //            IsVirtualTpmEnabled = true,
            //        }
            //    };
            //}

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
            int diskSize,
            bool isTrustedLaunch = false)
        {
            var rgCollections = subscriptionResource.GetResourceGroups();
            var rgResourceResponse = await rgCollections.GetAsync(rgName);
            var resourceGroup = rgResourceResponse.Value;

            Console.WriteLine("-------- start managed disk creation from SIG --------");
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
                //DiskSizeGB = diskSize,
                //HyperVGeneration = HyperVGeneration.V2,
                //OSType = SupportedOperatingSystemType.Linux,

            };
            //if (isTrustedLaunch)
            //{
            //    managedDiskData.SecurityProfile = new DiskSecurityProfile()
            //    {
            //        SecurityType = DiskSecurityType.TrustedLaunch
            //    };
            //}

            ManagedDiskCollection diskCollection = resourceGroup.GetManagedDisks();
            ArmOperation<ManagedDiskResource> managedDisOperation = await diskCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                diskName,
                managedDiskData);

            ManagedDiskResource disk = managedDisOperation.Value;

            Console.WriteLine($"-------- Done: created managed disk from SIG.Id:{disk.Id}--------");
            return disk.Id;
        }
    }
}
