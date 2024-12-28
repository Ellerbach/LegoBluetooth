namespace LegoBluetooth
{
    /// <summary>
    /// Represents the various Message Types for the LEGO Hub Characteristic (0x1624).
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Set or retrieve standard Hub Property information.
        /// </summary>
        HubProperties = 0x01,

        /// <summary>
        /// Perform actions on connected hub.
        /// </summary>
        HubActions = 0x02,

        /// <summary>
        /// Subscribe or retrieve Hub alerts.
        /// </summary>
        HubAlerts = 0x03,

        /// <summary>
        /// Transmitted upon Hub detection of attached I/O.
        /// </summary>
        HubAttachedIO = 0x04,

        /// <summary>
        /// Generic Error Messages from the Hub.
        /// </summary>
        GenericErrorMessages = 0x05,

        /// <summary>
        /// Commands used for H/W Networks.
        /// </summary>
        HWNetworkCommands = 0x08,

        /// <summary>
        /// Set the Hub in a special Boot Loader mode.
        /// </summary>
        FWUpdateGoIntoBootMode = 0x10,

        /// <summary>
        /// Locks the memory.
        /// </summary>
        FWUpdateLockMemory = 0x11,

        /// <summary>
        /// Request the Memory Locking State.
        /// </summary>
        FWUpdateLockStatusRequest = 0x12,

        /// <summary>
        /// Answer to the F/W Lock Status Request.
        /// </summary>
        FWLockStatus = 0x13,

        /// <summary>
        /// Request Port information.
        /// </summary>
        PortInformationRequest = 0x21,

        /// <summary>
        /// Request Port Mode information.
        /// </summary>
        PortModeInformationRequest = 0x22,

        /// <summary>
        /// Setup input format for single mode.
        /// </summary>
        PortInputFormatSetupSingle = 0x41,

        /// <summary>
        /// Setup input format for multiple modes (CombinedMode).
        /// </summary>
        PortInputFormatSetupCombinedMode = 0x42,

        /// <summary>
        /// Port Information.
        /// </summary>
        PortInformation = 0x43,

        /// <summary>
        /// Port Mode Information.
        /// </summary>
        PortModeInformation = 0x44,

        /// <summary>
        /// Value update related to single Port Mode.
        /// </summary>
        PortValueSingle = 0x45,

        /// <summary>
        /// Value update related to multiple Port Modes in combination (CombinedMode).
        /// </summary>
        PortValueCombinedMode = 0x46,

        /// <summary>
        /// Port Input Format (Single).
        /// </summary>
        PortInputFormatSingle = 0x47,

        /// <summary>
        /// Port Input Format (CombinedMode).
        /// </summary>
        PortInputFormatCombinedMode = 0x48,

        /// <summary>
        /// Used to control synchronization between synchronizable ports.
        /// </summary>
        VirtualPortSetup = 0x61,

        /// <summary>
        /// Used to execute Port Output commands.
        /// </summary>
        PortOutputCommand = 0x81,

        /// <summary>
        /// Provides feedback on completed Port Output commands.
        /// </summary>
        PortOutputCommandFeedback = 0x82
    }
}


