# ASP.NET Core

## Links & Docs

* https://www.microsoft.com/net/core#centos
* https://www.nginx.com/resources/wiki/start/topics/tutorials/install/
* http://www.hanselman.com/blog/PublishingAnASPNETCoreWebsiteToACheapLinuxVMHost.aspx
* https://www.nginx.com/blog/tutorial-proxy-net-core-kestrel-nginx-plus/
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run
* https://docs.microsoft.com/en-us/aspnet/core/publishing/linuxproduction
* https://www.linode.com/docs/security/firewalls/introduction-to-firewalld-on-centos
* https://blog.kloud.com.au/2016/05/31/building-dotnet-core-application-on-amazon-linux/
* https://docs.asp.net/en/latest/publishing/linuxproduction.html
* http://blog.earth-works.com/2013/04/12/how-to-get-networking-working-in-centos-under-hyper-v/
* https://technet.microsoft.com/en-us/windows-server-docs/compute/hyper-v/best-practices-for-running-linux-on-hyper-v

### DI

* https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
* https://joonasw.net/view/aspnet-core-di-deep-dive

## How to Install ASP.NET core on Linux

* install SmartTTY or Windows bash shell
* setup CentOS 7
  * enable networking
* install .NET Core
* configure firewall
  * add rules for HTTP, HTTPS, port 80
* install nginx
* update `/etc/nginx/conf.d/default.conf`
```
# For more information on configuration, see:
#   * Official English Documentation: http://nginx.org/en/docs/
#   * Official Russian Documentation: http://nginx.org/ru/docs/

server {
    listen       80;
    server_name  localhost; # or 172.22.199.136;

    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```
* restart nginx (nginx -s stop / nginx)


## Autostart web application

useradd -M --system dotnetwww
nano /etc/systemd/system/kestrel-rmp.service
```
[Unit]
Description=RMP Web App

[Service]
WorkingDirectory=/var/RMP
ExecStart=/usr/local/bin/dotnet /var/RMP/RMP.WebUI.dll
Restart=always
RestartSec=10 # Restart service after 10 seconds if dotnet service crashes
SyslogIdentifier=dotnet-rmp
User=dotnetwww
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

systemctl daemon-reload
systemctl start kestrel-rmp.service
systemctl status kestrel-rmp.service
