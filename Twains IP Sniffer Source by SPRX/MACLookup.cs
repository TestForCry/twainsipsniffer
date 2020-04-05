// Decompiled with JetBrains decompiler
// Type: Twain_s_IP_Sniffer.MACLookup
// Assembly: Twains IP Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E72FCB45-4D6A-40D7-9358-127F19A5779E
// Assembly location: C:\Users\xCreations\Desktop\IPSniffer\Twains IP Sniffer\bin\Debug\Twains IP Sniffer.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Twain_s_IP_Sniffer
{
  internal class MACLookup
  {
    public Dictionary<string, string> db;
    private string DBLocation;

    public MACLookup(string DBLocation)
    {
      this.DBLocation = DBLocation;
      this.db = new Dictionary<string, string>();
    }

    public string createDBFromText(string location)
    {
      if (File.Exists(location))
      {
        StreamReader streamReader = new StreamReader(location);
        string input;
        while ((input = streamReader.ReadLine()) != null)
        {
          if (new Regex("^(\\w\\w-\\w\\w-\\w\\w)").Match(input).Success)
          {
            List<string> list = ((IEnumerable<string>) input.Replace('\t', ' ').Split(' ')).Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))).ToList<string>();
            if (!this.db.ContainsKey(list[0]))
            {
              string str = "";
              for (int index = 2; index < list.Count; ++index)
                str = str + list[index] + " ";
              this.db.Add(list[0], str);
            }
          }
        }
        streamReader.Close();
      }
      this.writeToFile(this.DBLocation);
      return "";
    }

    public void loadFromFile()
    {
      if (!File.Exists(this.DBLocation))
        return;
      StreamReader streamReader = new StreamReader(this.DBLocation);
      string str1;
      while ((str1 = streamReader.ReadLine()) != null)
      {
        char[] charArray = " ".ToCharArray();
        string[] strArray = str1.Split(charArray, 2);
        string key = "";
        string str2 = "";
        for (int index = 0; index < strArray.Length; ++index)
        {
          switch (index)
          {
            case 0:
              key = strArray[0];
              break;
            case 1:
              str2 = strArray[1];
              break;
          }
        }
        if (!this.db.ContainsKey(key))
          this.db.Add(key, str2);
      }
      streamReader.Close();
    }

    public string lookup(string mac)
    {
      string key = mac.Substring(0, 8).Replace(':', '-');
      return this.db.ContainsKey(key) ? this.db[key] : "N/A";
    }

    public void writeToFile(string fileLocation)
    {
      if (!File.Exists(fileLocation))
        File.Create(fileLocation);
      StreamWriter streamWriter = new StreamWriter(fileLocation);
      foreach (string key in this.db.Keys)
        streamWriter.WriteLine(key + " " + this.db[key]);
      streamWriter.Close();
    }
  }
}
