# ASP.NET Core

## Docs & Guidelines

* Deploy
  * https://www.microsoft.com/net/core#centos
  * https://docs.microsoft.com/en-us/aspnet/core/publishing/linuxproduction
  * https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run
* Nginx
  * https://www.nginx.com/resources/wiki/start/topics/tutorials/install/
  * https://www.nginx.com/blog/tutorial-proxy-net-core-kestrel-nginx-plus/
* Firewall
  * https://www.linode.com/docs/security/firewalls/introduction-to-firewalld-on-centos
  * https://www.digitalocean.com/community/tutorials/iptables-essentials-common-firewall-rules-and-commands
* SSL
  * https://www.digitalocean.com/community/tutorials/how-to-create-a-self-signed-ssl-certificate-for-nginx-on-centos-7
  * http://nginx.org/en/docs/http/configuring_https_servers.html
  * https://www.blinkingcaret.com/2017/02/01/using-openssl-to-create-certificates/
* Logging
  * https://www.loggly.com/ultimate-guide/using-journalctl/

### Possible issues
* Upstream sent too big header
  * https://andrewlock.net/fixing-nginx-upstream-sent-too-big-header-error-when-running-an-ingress-controller-in-kubernetes/
* Too many open files
  * https://medium.com/@mshanak/soved-dotnet-core-too-many-open-files-in-system-when-using-postgress-with-entity-framework-c6e30eeff6d1
  * https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/

### Best practices
* Razor Pages
  * https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/?tabs=visual-studio
* DI
  * https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
  * https://joonasw.net/view/aspnet-core-di-deep-dive
  
### Samples
* https://github.com/dodyg/practical-aspnetcore/

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
	# proxy_buffering off;		# Uncomment to fix CLOSE_WAIT problem "Too many open files in system"
        # proxy_read_timeout 7200;	# Uncomment to fix CLOSE_WAIT problem "Too many open files in system"
	
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
	
        # proxy_buffers 8 16k;		# Uncomment to fix error "Upstream sent too big header"
        # proxy_buffer_size 16k;	# Uncomment to fix error "Upstream sent too big header"
    }
}
```
* restart nginx
  * `nginx -s stop` / `nginx`
* autostart nginx
  * `systemctl enable nginx` (must allow port permission in SELinux next)
  * `yum install policycoreutils-python` (install SELinux management tool)
  * `semanage port --add --type http_port_t --proto tcp 5000` (add a record of the port to SELinux)
  * `systemctl status nginx`

## Autostart web application

`useradd -M --system dotnetwww`

`nano /etc/systemd/system/kestrel-rmp.service`

```
[Unit]
Description=RMP Web App

[Service]
WorkingDirectory=/var/RMP
ExecStart=/usr/bin/dotnet /var/RMP/RMP.WebUI.dll
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

## Tips

`find / -name dotnet -type f` find "donet" location

`/usr/bin/dotnet --version` dotnet version

`dotnet --info` runtime information

`which dotnet` full path of executable

`journalctl -f`  like the Linux tail command so it continuously prints log messages as they are added

`cat /var/log/nginx/error.log` nginx error log

`sudo /opt/mssql/bin/mssql-conf set-sa-password` reset SQL SA password

### Clear audit logs

`sudo du -sch * --exclude=home`
`sudo du -Sh | sort -rh | head -5`

```
sudo su
rm /var/log/audit/*
exit
```


