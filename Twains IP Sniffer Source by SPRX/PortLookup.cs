// Decompiled with JetBrains decompiler
// Type: Twain_s_IP_Sniffer.PortLookup
// Assembly: Twains IP Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E72FCB45-4D6A-40D7-9358-127F19A5779E
// Assembly location: C:\Users\xCreations\Desktop\IPSniffer\Twains IP Sniffer\bin\Debug\Twains IP Sniffer.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace Twain_s_IP_Sniffer
{
  internal class PortLookup
  {
    private Dictionary<int, ProtocolObject> db;
    private string DBLocation;

    public PortLookup(string DBLocation)
    {
      this.DBLocation = DBLocation;
      this.db = new Dictionary<int, ProtocolObject>();
    }

    public void createDBFromText(string location)
    {
      if (!File.Exists(location))
        return;
      StreamReader streamReader = new StreamReader(location);
      bool flag = false;
      int num1 = 0;
      int type = 0;
      string description = "";
      int num2 = 0;
      string str;
      while ((str = streamReader.ReadLine()) != null)
      {
        if (flag)
        {
          if (str == "</tr>")
          {
            if (!this.db.ContainsKey(num1))
            {
              ProtocolObject protocolObject = new ProtocolObject(num1, type, description);
              this.db.Add(num1, protocolObject);
            }
            flag = false;
            num2 = 0;
            num1 = 0;
            type = 0;
            description = "";
          }
          else if (num2 == 0)
          {
            try
            {
              num1 = Convert.ToInt32(str.Substring(4, str.IndexOf("</td>") - 4));
            }
            catch (Exception ex)
            {
            }
            ++num2;
          }
        }
        else if (str.Contains("<tr"))
          flag = true;
      }
      streamReader.Close();
      this.writeToFile(this.DBLocation);
    }

    public void loadFromFile()
    {
      if (!File.Exists(this.DBLocation))
        return;
      StreamReader streamReader = new StreamReader(this.DBLocation);
      string str;
      while ((str = streamReader.ReadLine()) != null)
      {
        char[] charArray = " ".ToCharArray();
        string[] strArray = str.Split(charArray, 3);
        int key = 0;
        int port = 0;
        int type = 0;
        string description = "";
        for (int index = 0; index < strArray.Length; ++index)
        {
          switch (index)
          {
            case 0:
              key = port = Convert.ToInt32(strArray[0]);
              break;
            case 1:
              type = Convert.ToInt32(strArray[1]);
              break;
            case 2:
              description = strArray[2];
              break;
          }
        }
        if (!this.db.ContainsKey(key))
        {
          ProtocolObject protocolObject = new ProtocolObject(port, type, description);
          this.db.Add(key, protocolObject);
        }
      }
      streamReader.Close();
    }

    public string lookup(string port)
    {
      try
      {
        return this.db[Convert.ToInt32(port)].description;
      }
      catch (Exception ex)
      {
      }
      return "";
    }

    public void writeToFile(string fileLocation)
    {
      if (!File.Exists(fileLocation))
        File.Create(fileLocation);
      StreamWriter streamWriter = new StreamWriter(fileLocation);
      foreach (int key in this.db.Keys)
        streamWriter.WriteLine(key.ToString() + " " + (object) this.db[key].type + " " + this.db[key].description);
      streamWriter.Close();
    }
  }
}
