using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration;

public class RedisOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Password { get; set; } = null!;
    public int DefaultDatabase { get; set; } = 0;
    public int SyncTimeout { get; set; } = 5000;
}