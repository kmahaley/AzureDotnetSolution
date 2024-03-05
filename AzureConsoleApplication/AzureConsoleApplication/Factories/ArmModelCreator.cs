using Azure.Core;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Compute;

namespace AzureConsoleApplication.Factories
{
    /// <summary>
    /// factory to create ARM SDK models
    /// </summary>
    public static class ArmModelCreator
    {
        public static VirtualMachineOSProfile CreateVirtualMachineOSProfile(
            string adminUsername,
            string adminPassword,
            string computerName)
        {
            return new VirtualMachineOSProfile()
            {
                AdminUsername = adminUsername,
                AdminPassword = adminPassword,
                ComputerName = computerName,
                WindowsConfiguration = new WindowsConfiguration()
                {
                    ProvisionVmAgent = true,
                },
            };
        }

        public static VirtualMachineHardwareProfile CreateVirtualMachineHardwareProfile(
            VirtualMachineSizeType vmSkuType)
        {
            return new VirtualMachineHardwareProfile()
            {
                VmSize = vmSkuType,
            };
        }

        public static VirtualMachineNetworkProfile CreateVirtualMachineNetworkProfile(
            VirtualMachineNetworkInterfaceReference networkInterfaceReferences)
        {
            return new VirtualMachineNetworkProfile()
            {
                NetworkInterfaces =
                    {
                        networkInterfaceReferences,
                    },
            };
        }

        public static VirtualMachineNetworkInterfaceReference CreateVirtualMachineNetworkInterfaceReference(string networkInterfaceId)
        {
            return new VirtualMachineNetworkInterfaceReference()
            {
                Id = new ResourceIdentifier(networkInterfaceId),
                Primary = true,
            };
        }

        public static VirtualMachineStorageProfile CreateVirtualMachineStorageProfile(
            VirtualMachineOSDisk virtualMachineOSDisk,
            ImageReference imageReference,
            DiskControllerType diskControllerType)
        {
            return new VirtualMachineStorageProfile()
            {
                OSDisk = virtualMachineOSDisk,
                ImageReference = imageReference,
                DiskControllerType = diskControllerType,
            };
        }

        public static VirtualMachineOSDisk CreateVirtualMachineOSDisk(
            string diskName,
            int diskSize,
            SupportedOperatingSystemType supportedOperatingSystemType,
            CachingType cachingType,
            VirtualMachineManagedDisk virtualMachineManagedDisk)
        {
            return new VirtualMachineOSDisk(DiskCreateOptionType.FromImage)
            {
                Name = diskName,
                DiskSizeGB = diskSize,
                OSType = supportedOperatingSystemType,
                Caching = cachingType,
                ManagedDisk = virtualMachineManagedDisk,
            };
        }

        // diskCreateOptionType = DiskCreateOptionType.Empty
        // logicalDataDiskNumber = 1
        public static VirtualMachineDataDisk CreateVirtualMachineDataDisk(
            string diskName,
            int diskSize,
            int logicalDataDiskNumber,
            DiskCreateOptionType diskCreateOptionType,
            VirtualMachineManagedDisk virtualMachineManagedDisk,
            CachingType cachingType)
        {
            return new VirtualMachineDataDisk(logicalDataDiskNumber, diskCreateOptionType)
            {
                Name = diskName,
                DiskSizeGB = diskSize,
                ManagedDisk = virtualMachineManagedDisk,
                Caching = cachingType,
            };
        }

        // StorageAccountType.StandardLrs
        public static VirtualMachineManagedDisk CreateVirtualMachineManagedDisk(StorageAccountType storageAccountType)
        {
            return new VirtualMachineManagedDisk()
            {
                StorageAccountType = storageAccountType,
            };
        }

        public static ImageReference CreateImageReference(
            string imagePublisher,
            string imageOffer,
            string imageSku)
        {
            return new ImageReference()
            {
                Publisher = imagePublisher,
                Offer = imageOffer,
                Sku = imageSku,
                Version = "latest",
            };
        }

        public static VirtualMachineData CreateVirtualMachineData(
            AzureLocation location,
            VirtualMachineHardwareProfile hardwareProfile,
            VirtualMachineOSProfile oSProfile,
            VirtualMachineNetworkProfile networkProfile,
            VirtualMachineStorageProfile storageProfile)
        {
            return new VirtualMachineData(location)
            {
                HardwareProfile = hardwareProfile,
                OSProfile = oSProfile,
                NetworkProfile = networkProfile,
                StorageProfile = storageProfile,
            };
        }
    }
}
