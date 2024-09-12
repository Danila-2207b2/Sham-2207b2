using System.ComponentModel.Design;

public class Servers
{
    private static Servers instance;
    private static readonly object padlock = new object();
    private List<string> serverList;

    private Servers()
    {
        serverList = new List<string>();
    }

    public static Servers Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Servers();
                }
                return instance;
            }    
        }
    }

    public bool AddServer(string serverAddress)
    {
        if ((!serverAddress.StartsWith("http://") && !serverAddress.StartsWith("https://")))
        {
            return false;
        }

        lock (padlock)
        {
            if (serverList.Contains(serverAddress))
            {
                return false;
            }
        }

        serverList.Add(serverAddress);
        return true;
    }

    public List<string> GetHttpServers()
    {
        lock (padlock)
        {
            return serverList.FindAll(server => server.StartsWith("http://"));
        }
    }

    public List<string> GetHttpsServers()
    {
        lock (padlock)
        {
            return serverList.FindAll(server => server.StartsWith("https://"));
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Servers servers = Servers.Instance;

        Console.WriteLine(servers.AddServer("http://ex.com")); 
        Console.WriteLine(servers.AddServer("https://2207b2.com")); 
        Console.WriteLine(servers.AddServer("ftp://ton.com"));
        Console.WriteLine(servers.AddServer("http://sham.com"));

        var httpServers = servers.GetHttpServers();
        var httpsServers = servers.GetHttpsServers();

        Console.WriteLine("https");
        foreach (var server in httpsServers)
        {
            Console.WriteLine(server);
        }

        Console.WriteLine("http");
        foreach (var server in httpServers)
        {
            Console.WriteLine(server);
        }
    }
}




