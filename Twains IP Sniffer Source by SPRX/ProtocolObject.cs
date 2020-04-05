// Decompiled with JetBrains decompiler
// Type: Twain_s_IP_Sniffer.ProtocolObject
// Assembly: Twains IP Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E72FCB45-4D6A-40D7-9358-127F19A5779E
// Assembly location: C:\Users\xCreations\Desktop\IPSniffer\Twains IP Sniffer\bin\Debug\Twains IP Sniffer.exe

namespace Twain_s_IP_Sniffer
{
  internal class ProtocolObject
  {
    public string description;
    public int port;
    public int type;

    public ProtocolObject(int port, int type, string description)
    {
      this.port = port;
      this.type = type;
      this.description = description;
    }
  }
}
