using System.Net;
public static class MyHardwareHelper
{
    public static string GetMyComputerName()
    {
        return Dns.GetHostName(); // Retrive the Name of HOST
    }

    public static string GetMyIpAddress()
    {
        string hostName = GetMyComputerName(); 
        var myIP = Dns.GetHostEntry(hostName);
        foreach (IPAddress ip in Dns.GetHostEntry(hostName).AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                var my = ip.MapToIPv4().ToString();
                return my;
            }
        }
        return "No IP Address Found";
    }
}