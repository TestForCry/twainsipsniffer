// Decompiled with JetBrains decompiler
// Type: Twain_s_IP_Sniffer.Program
// Assembly: Twains IP Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E72FCB45-4D6A-40D7-9358-127F19A5779E
// Assembly location: C:\Users\xCreations\Desktop\IPSniffer\Twains IP Sniffer\bin\Debug\Twains IP Sniffer.exe

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Twain_s_IP_Sniffer
{
  internal static class Program
  {
    public static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Twain's IP Sniffer\\";
    public static bool justUpdated = true;
    public static string oldVersion = "2";
    public static string newVersion = "";
    public static string downloadLink = "";
    public static string whatsNew = "- Source LEAKED by SPRX \n- Instagram: @sprx.sh\n- Discord: SQLFail#6868\n- CCM: sprx";
    private static string[] buffer = Process.GetCurrentProcess().MainModule.FileName.Split('\\');
    private static string Name = Program.buffer[Program.buffer.Length - 1];
    private static string[] Folders = Path.GetDirectoryName(Application.ExecutablePath).Split('\\');
    public static double oldV = double.Parse(Program.oldVersion);
    public static double newV = 0.0;

    public static string[] readFromPaste(string string_3)
    {
      StreamReader streamReader = new StreamReader(WebRequest.Create(string_3).GetResponse().GetResponseStream());
      streamReader.ReadLine();
      string[] strArray1 = new string[(int) byte.MaxValue];
      int index = 0;
      string str;
      while ((str = streamReader.ReadLine()) != null)
      {
        string[] strArray2 = str.Split(Environment.NewLine.ToCharArray());
        strArray1[index] = strArray2[0];
        ++index;
      }
      return strArray1;
    }

    [STAThread]
    private static void Main()
    {
      string[] strArray1 = Program.readFromPaste("http://pastebin.com/raw/1Svv1av7");
      Program.newVersion = strArray1[0];
      Program.newV = double.Parse(Program.newVersion);
      Program.downloadLink = strArray1[1];
      if (Program.newV > Program.oldV)
      {
        if (MessageBox.Show("There is an Update available, would you like to Download it?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          Process.Start(Program.downloadLink);
          Application.Exit();
        }
        else
        {
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);
          Application.Run((Form) new Form1());
        }
      }
      else
      {
        if (!Directory.Exists(Program.path))
          Directory.CreateDirectory(Program.path);
        if (!System.IO.File.Exists(Program.path + "Version.txt"))
        {
          Program.justUpdated = true;
          System.IO.File.AppendAllText(Program.path + "Version.txt", Program.oldVersion);
        }
        else
        {
          string str = System.IO.File.ReadAllText(Program.path + "Version.txt");
          if (Program.oldVersion != str)
          {
            Program.justUpdated = true;
            System.IO.File.Delete(Program.path + "Version.txt");
            System.IO.File.AppendAllText(Program.path + "Version.txt", Program.oldVersion);
          }
        }
        string[] strArray2 = Process.GetCurrentProcess().MainModule.FileName.Split('\\');
        string str1 = strArray2[strArray2.Length - 1];
        string[] strArray3 = Path.GetDirectoryName(Application.ExecutablePath).Split('\\');
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < strArray3.Length; ++index)
          stringBuilder.Append(strArray3[index] + "\\");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run((Form) new Form1());
      }
    }
  }
}
