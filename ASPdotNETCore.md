# ASP.NET Core

## Links & Docs

* https://www.microsoft.com/net/core#centos
* https://blog.kloud.com.au/2016/05/31/building-dotnet-core-application-on-amazon-linux/
* http://www.hanselman.com/blog/PublishingAnASPNETCoreWebsiteToACheapLinuxVMHost.aspx
* https://docs.asp.net/en/latest/publishing/linuxproduction.html
* http://blog.earth-works.com/2013/04/12/how-to-get-networking-working-in-centos-under-hyper-v/
* https://technet.microsoft.com/en-us/windows-server-docs/compute/hyper-v/best-practices-for-running-linux-on-hyper-v

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
    server_name  172.22.199.136;
#    root         /root/cwa1/wwwroot;

# Load configuration files for the default server block.
#    include /etc/nginx/default.d/*.conf;

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