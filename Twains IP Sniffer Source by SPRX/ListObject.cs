// Decompiled with JetBrains decompiler
// Type: Twain_s_IP_Sniffer.ListObject
// Assembly: Twains IP Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E72FCB45-4D6A-40D7-9358-127F19A5779E
// Assembly location: C:\Users\xCreations\Desktop\IPSniffer\Twains IP Sniffer\bin\Debug\Twains IP Sniffer.exe

namespace Twain_s_IP_Sniffer
{
  internal class ListObject
  {
    public int packetDelt;

    public string label { get; set; }

    public string ipDisplay { get; set; }

    public string ipDest { get; set; }

    public string ipSource { get; set; }

    public string macAddress { get; set; }

    public string macVendor { get; set; }

    public int packetCount { get; set; }

    public string portDest { get; set; }

    public string portSource { get; set; }

    public string protocol { get; set; }
  }
}
