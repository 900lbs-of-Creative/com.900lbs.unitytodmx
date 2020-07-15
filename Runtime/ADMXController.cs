using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX
{
    #region Interfaces
    /// <summary>
    /// Interface for controllers of DMX devices with properties of type <typeparamref name="TDMXProperties"/>.
    /// </summary>
    /// <typeparam name="TDMXProperties">Type of data for this controller.</typeparam>
    public interface IDMXController<TDMXProperties>
    {
        /// <summary>
        /// Properties of this DMX controller.
        /// </summary>
        TDMXProperties Properties { get; }

        /// <summary>
        /// Initialize with the given <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">Properties to initialize with.</param>
        void SetProperties(TDMXProperties properties);

        /// <summary>
        /// Gets and returns an array of bytes for this DMX controller.
        /// <para>
        /// By default, all DMX controllers receive ArtNet calls through a 530 <see cref="byte"/>
        /// array, with the first 18 indices holding ArtNet information and universe identification 
        /// and up to the final 512 indices holding the data to be processed by the DMX hardware 
        /// controller.
        /// </para>
        /// </summary>
        /// <returns>A byte array of up to 512 elements.</returns>
        byte[] GetDMXData();

        /// <summary>
        /// Sends a command to the target DMX device by packing the <paramref name="dmxData"/>
        /// into ArtNet protocol and dispatching it.
        /// </summary>
        /// <param name="dmxData">The command to be sent.</param>
        void SendCommand(byte[] dmxData);

        /// <summary>
        /// Sends a command to the target DMX device by packing the <paramref name="dmxData"/>
        /// into ArtNet protocol and dispatching it.
        /// </summary>
        /// <param name="dmxData">The command to be sent.</param>
        void SendCommand(ArraySegment<byte> dmxData);
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of a DMX device with the given properties of type <typeparamref name="TDMXProperties"/>.
    /// </summary>
    /// <typeparam name="TDMXProperties">Type of properties for this controller.</typeparam>
    public abstract class ADMXController<TDMXProperties> : MonoBehaviour, IDMXController<TDMXProperties>
    {
        #region Constants
        /// <summary>
        /// The number of bytes per DMX universe.
        /// </summary>
        private const int DMX_UNIVERSE_LENGTH = 512;

        /// <summary>
        /// The number of bytes occupied by ArtNet protocol.
        /// </summary>
        private const int ARTNET_LENGTH = 18;
        #endregion

        #region Events
        /// <summary>
        /// Dispatched when a command is sent.
        /// </summary>
        public Action<byte[]> CommandSent { get; set; }
        #endregion

        #region Properties
        public TDMXProperties Properties => properties;
        public int Universe => universe;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Properties of this DMX controller.")]
        [SerializeField] private TDMXProperties properties;

        [Tooltip("IP address of the target DMX device.")]
        [SerializeField] private string ip = "10.55.4.96";

        [Tooltip("Target universe on the target DMX device.")]
        [SerializeField] private int universe = 0x0;

        [Tooltip("Target port on the target DMX device.")]
        [SerializeField] private int port = 6454;
        #endregion

        #region Private Variables
        /// <summary>
        /// The ArtNet packet to be dispatched.
        /// </summary>
        private byte[] artNetPacket = new byte[ARTNET_LENGTH + DMX_UNIVERSE_LENGTH];

        /// <summary>
        /// The <see cref="UdpClient"/> data will be sent to.
        /// </summary>
        private UdpClient udpClient;
        #endregion

        #region Unity Methods
        private void OnApplicationQuit()
        {
            udpClient.Close();    
        }

        protected virtual void Start()
        {
            udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Connect(IPAddress.Parse(ip), port);

            artNetPacket[0] = (byte)'A';
            artNetPacket[1] = (byte)'r';
            artNetPacket[2] = (byte)'t';
            artNetPacket[3] = (byte)'-';
            artNetPacket[4] = (byte)'N';
            artNetPacket[5] = (byte)'e';
            artNetPacket[6] = (byte)'t';
            artNetPacket[7] = 0x0;
            artNetPacket[8] = 0x0;
            artNetPacket[9] = 0x50;
            artNetPacket[10] = 0x0;
            artNetPacket[11] = 0x14;
            artNetPacket[12] = 0x0;
            artNetPacket[13] = 0;
            artNetPacket[14] = (byte)(universe & 0xff);
            artNetPacket[15] = (byte)((universe >> 8) & 0x7f);
            artNetPacket[16] = ((512 >> 8) & 0xFF);
            artNetPacket[17] = (512 & 0xFF);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">Properties to initialize with.</param>
        public void SetProperties(TDMXProperties properties)
        {
            this.properties = properties;
            OnPropertiesSet();
        }

        public abstract byte[] GetDMXData();

        public virtual void SendCommand(byte[] dmxData)
        {
            Buffer.BlockCopy(dmxData, 0, artNetPacket, ARTNET_LENGTH, dmxData.Length - 1);
            udpClient.Send(artNetPacket, artNetPacket.Length);
            CommandSent?.Invoke(artNetPacket);
        }

        public virtual void SendCommand(ArraySegment<byte> dmxData)
        {
            Buffer.BlockCopy(dmxData.Array, dmxData.Offset, artNetPacket, ARTNET_LENGTH, dmxData.Count);
            udpClient.Send(artNetPacket, artNetPacket.Length);
            CommandSent?.Invoke(artNetPacket);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handler method for when <see cref="properties"/> has been set.
        /// At this point, <see cref="Properties"/> can now be safely accessed.
        /// </summary>
        protected virtual void OnPropertiesSet() { }
        #endregion
    }
    #endregion
}