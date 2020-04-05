// Decompiled with JetBrains decompiler
// Type: Twain_s_IP_Sniffer.DataGridViewExtensioncs
// Assembly: Twains IP Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E72FCB45-4D6A-40D7-9358-127F19A5779E
// Assembly location: C:\Users\xCreations\Desktop\IPSniffer\Twains IP Sniffer\bin\Debug\Twains IP Sniffer.exe

using System.Reflection;
using System.Windows.Forms;

namespace Twain_s_IP_Sniffer
{
  public static class DataGridViewExtensioncs
  {
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
      dgv.GetType().GetProperty(nameof (DoubleBuffered), BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) dgv, (object) setting, (object[]) null);
    }
  }
}
