using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2rayReverseProxy.Model
{
    public class ConfigModel
    {
        public Log log;
        public Reverse reverse;
        public List<Outbounds> outbounds;
        public List<Inbounds> inbounds;
        public Routing routing;

        public void SetSimpleBridges()
        {
            reverse = new Reverse();
            reverse.bridges = new List<Bridges>()
            {
                new Bridges()
                {
                    tag="bridge",
                    domain="private.cloud.com"
                }
            };
            outbounds = new List<Outbounds>()
            {
                new Outbounds(){
                    tag = "tunnel",
                    protocol = "vmess",
                    settings=new Settings()
                    {
                        vnext=new List<Vnext>()
                        {
                            new Vnext()
                            {
                                users=new List<Users>()
                                {
                                    new Users()
                                    {
                                        alterId=64
                                    }
                                }
                            }
                        }
                    }
                },
                new Outbounds(){
                    tag = "out",
                    protocol = "freedom"
                }
            };
            routing = new Routing()
            {
                rules = new List<Rules>()
                {
                    new Rules()
                    {
                        type="field",
                        inboundTag=new List<string>{ "bridge" },
                        domain=new List<string> { "full:private.cloud.com" },
                        outboundTag="tunnel"
                    },
                    new Rules()
                    {
                        type="field",
                        inboundTag=new List<string>{ "bridge" },
                        outboundTag="out"
                    },
                }
            };
        }

        public void SetSimplePortals()
        {
            reverse = new Reverse();
            reverse.portals = new List<Bridges>()
            {
                new Bridges()
                {
                    tag="portal",
                    domain="private.cloud.com"
                }
            };
            inbounds = new List<Inbounds>()
            {
                new Inbounds(){
                    protocol = "vmess",
                    settings=new Settings()
                    {
                        clients=new List<Clients>()
                        {
                            new Clients()
                            {
                                alterId=64
                            }
                        }
                    }
                }
            };
            routing = new Routing()
            {
                rules = new List<Rules>()
                {
                    new Rules()
                    {
                        type="field",
                        domain=new List<string> { "full:private.cloud.com" },
                        outboundTag="portal"
                    },
                }
            };
        }
    }

    public class Log
    {
        public string loglevel;
    }

    public class Reverse
    {
        public List<Bridges> bridges;
        public List<Bridges> portals;
    }

    public class Bridges
    {
        public string tag;
        public string domain;
    }

    public class Portals
    {
        public string tag;
        public string domain;
    }

    public class Outbounds
    {
        public string tag;
        public string protocol;
        public Settings settings;
    }

    public class Inbounds
    {
        public string tag;
        public int port;
        public string protocol;
        public Settings settings;
    }

    public class Settings
    {
        public List<Vnext> vnext;
        public List<Clients> clients;
    }

    public class Vnext
    {
        public string address;
        public int port;
        public List<Users> users;
    }

    public class Clients
    {
        public string id;
        public int alterId;
    }

    public class Users
    {
        public string id;
        public int alterId;
    }

    public class Routing
    {
        public List<Rules> rules;
    }

    public class Rules
    {
        public string type;
        public List<string> inboundTag;
        public List<string> domain;
        public string outboundTag;
    }
}
