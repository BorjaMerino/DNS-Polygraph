# DNS-Polygraph
Tool designed to study the answers of your DNS resolver and make easier the identification of techniques such as DNS Hijacking/Poisoning

![alt text](https://3.bp.blogspot.com/-Ja1ChvCeslE/XCOir4leM6I/AAAAAAAAN0Y/iyCfnmaXHWI22EOtZSQZxgx0s1XQVn-ggCLcBGAs/s1600/DNS%2BPolygraph.gif "DNs Polygraph How-to gif")

DNS Polygraph is developed in C# and relies on both: the nice [SharpPcap](https://github.com/chmorgan/sharppcap) library of Chris Morgan and a cute [DNS library](https://github.com/kapetan/dns) developed by Mirza Kapetanovic.

The idea of DNS Polygraph is to show you in a datagrid each DNS response that your host receives (called by the tool as *“untrusted response”*) and compare this with a response from a trusted source made over HTTPS. So for every DNS response that your host receives a DNS request will be done over HTTPS. Currently you can choose between the [Google DoH service](https://developers.google.com/speed/public-dns/docs/dns-over-https) or the [Cloudflare](https://developers.cloudflare.com/1.1.1.1/dns-over-https/) one.

Both responses (*trusted* and *unstrusted*) will be compared and, if they do not match, different colors will indicate the level of relationship that exist between both responses. For now, the criteria I have used is the following:

* Check if both responses, trusted and untrusted, belong to the same /24 network.
* If not, check if both responses, trusted and untrusted, belong to the same /16 network.
* If not, It makes a reverse DNS lookup of both responses and check if they have a second domain level in common.

Organizing the answers in this way saves a lot of work, allowing you to focus only on the apparently unrelated responses.

The graphical interface of the application is shown below.  You just have to select the network interface and click on the Capture button. After clearing the cache DNS it will start getting the responses from your resolver.  The datagrid columns are self-explanatory except perhaps the one called **"R"**. This indicates the DoH resolver selected (*G* for Google and *C* for CloudFlare). Here an example:

![alt text](https://2.bp.blogspot.com/-nIslM3PdD-E/XCNnU7tiYZI/AAAAAAAANyY/QW6__a9m80MsDooUmV3h6DplCdaBEmD7gCLcBGAs/s1600/DSN%2BPolygraph%2BMain.png "DNS Polygraph")

Note that the tool will also highlight, with a dark blue color, when a new resolver is detected. Those IP addresses that do not fit with any of the previously described criteria will be marked as *"Unrelated"* (gray color). If the option *"Automatic Whois for Unrelated"* is checked a whois query will be done to retrieve some data regarding the untrusted IP (organization, ISP, etc.). The information gathered will be pasted into the "Info" field. This is useful because sometimes the organization name will be match the name of the requested domain which allows you to quickly discard a possible DNS spoofing attempt.

![alt text](https://2.bp.blogspot.com/-sBwc2JVDTnI/XCN94TOhwwI/AAAAAAAANy8/WU81mNpv7hAyZvJk2EWC_PDNKySFbFQkwCLcBGAs/s1600/DNS%2BPolygraph%2BWhois.png "DNS Polygraph Whois")

Apart from the previous criteria there are configured some rules to identify certain types of DNS poisoning related attacks. For example, if an unstrusted response corresponds to a private IP and the domain resolves, via DoH, to a public IP, this could mean a potential Local DNS Spoof attack. If this happens, this entry will be shown in red. Lets force this case with [DNSChef](https://github.com/iphelix/dnschef) to see how DNS Polygraph would show it.

![alt text](https://2.bp.blogspot.com/-t8iHByaWzZQ/XCODz2CbzMI/AAAAAAAAN0M/6-XUaLxTRg40sM5sAzhtPdBYhGKuMYpxACLcBGAs/s1600/dnschef_local_spoof.PNG "DNSChef Local DNS Spoof")
![alt text](https://2.bp.blogspot.com/-ybO14103Jvo/XCODBKRxInI/AAAAAAAANz0/DXGtDRsBmxkGl1YitrOtPVovjksb0oYMQCLcBGAs/s1600/local_dns_spoof.PNG "DNS Polygraph Local DNS Spoof")

An unethical technique used by certain ISP is to redirect DNS requests for non-existent domains. In this case, if an untrusted response receives a public IP and the requested domain can not be resolved via DoH (which is indicated as *"NXDOMAIN"*), it will be marked in pink with the corresponding alert in the "Info" field.

![alt text](https://1.bp.blogspot.com/-eN4ip_V0AGU/XCOBsGZYh_I/AAAAAAAANzY/uPePfFr5-ooVegzdtZcKxKKS1_UHT7B_QCLcBGAs/s1600/dnschef_nxdomain.PNG "DNSChef NXdomain")
![alt text](https://4.bp.blogspot.com/-U1urj9k0Bvk/XCOBsA9yOlI/AAAAAAAANzc/y-dp0mqlDAI9EzQptvY1oLw3cNxVeySxgCEwYBhgL/s1600/nxdomain.PNG "DNS Polygraph NXdomain")

**Some considerations:**

* Currently the tool is not very stable and the code is quite ugly; my initial intention was to create a funtional tool for personal use without looking at the performance. If I have time I will try to improve it.
* By selecting the "Passive" option no DoH requests will be done, so you just get the untrusted responses of your resolver. This is useful if you just want to monitor the DNS your host makes in a passive way (for example, for malware purposes).
* An IP marked as "Unrelated" (grey color) does not necessarily mean that there is a DNS-related attack but that the response should be investigated more thoroughly. In fact, you will receive many responses of this type due to things like CDN, companies with multiple IP ranges, balancers, etc.
* For now the tool only considers A Records with just one IP. If there are multiple IPs, it will only check if they match with those recovered via DoH. So more work needs to be done in this aspect.
* The Whois service used is http://ip-api.com. The limit of this service is 150 requests per minute. Be careful if you have selected the checkbox "Automatic Whois for Unrelated" to not go over this limit or your IP will be blackholed.
* This tool is oriented to study DNS responses from your resolver in search of anomalies/attacks, not  to prevent techniques like the ones described above. To prevent this attacks use DNSSEC or configure a client to route all your queries via HTTPS.

**Future ideas:**
* Read an input pcap file.
* Tagging of malicious domains.
* Consider other DNS type records like: MX, AAAA, etc.
* Add more intelligence for *unrelated* responses.
