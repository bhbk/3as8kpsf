# WAF (Web Application Firewall / Filtering)
Lightweight C# ASP.NET MVC & Web API library to filter traffic that modern firewalls have trouble with. Entities have different types of firewalls that offer different feature
sets, but not many entities have true WAF's (web application firewalls). This library can help bridge the gap between what infrastructure geeks think code geeks should do inside 
code... versus what code geeks think that infrastructure should "handle for them".

Modern firewalls are able to easily identify applications, protocols & parse the DNS segments of URL structure. Like below...
* http://defcon.org
* https://google.com
* ftp://freebsd.org

Firewalls are designed to protect networks, but they do not have the feature set to conveniently manage aspects that a WAF can handle. There is often so much confusion on the topic 
that some vendors publish briefs to provide clarity, like https://www.paloaltonetworks.com/resources/techbriefs/palo-alto-networks-vs-web-application-firewalls.

Modern WAF's are able to filter traffic specific to the non-DNS segments of the URL structure, identify specific verbs within the HTTP protocol & handle other nuances
that can NOT easily or conveniently be managed by a firewall. Like below...
* http://defcon.org/app1
* https://defcon.org/app1/ref1
* https://defcon.org/app1/ref2

This library isn't attempting to implement the full feature set that a modern WAF could provide, but instead save entities the time & expense of a WAF implementation... because
basic functionality is all that is needed. This library is meant to make it VERY easy for developers on a .Net stack to apply WAF like features so the burden doesn't fall on
infrastructure to assemble complicated & brittle web.configs, which saves on finger pointing & wasted troubleshooting cycles that can ensue before, during & after a deployment.

## Http Option(s)

* HTTPS Required, Not Allowed & Optional

```cs

	//SSL (Required)
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterHttpOption(HttpFilteringAction.SslRequired)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

```

## Schedule(s) (NOT FULLY IMPLIMENTED YET...)

* Single schedule
* Multiple schedule(s)

```cs

	//Schedule (Dynamic)
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterSchedule(ScheduleFilteringAction.Deny, ScheduleFilteringOccur.Daily)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

	//Schedule (Static)
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterSchedule("2017:01:01T17:00:00-2018:01:01T08:00:00", ScheduleFilteringAction.Deny, ScheduleFilteringOccur.Daily)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

```

## IPv4/IPv6 address(es) & IPv4/IPv6 range(s)

* Single IP or CIDR address
* Multiple IP(s) or CIDR address(es)

```cs

	//IPv4 & IPv6 (Dynamic)
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterIpAddress(IpAddressFilteringAction.Deny)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

	//IPv4 (Static)
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterIpAddress("10.0.0.0/8", IpAddressFilteringAction.Deny)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

	//IPv6 (Static)
	public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterIpAddress("2001:db8:a::123/64", IpAddressFilteringAction.Deny)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

```

## DNS address(es)

* Single DNS address, contains & regex
* Multiple DNS addresses(es), contains & regex(es)

```cs

	//DNS (Dynamic)
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterDnsAddress(DnsAddressFilteringAction.Deny)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

	public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterDnsAddress(DnsAddressFilteringAction.DenyContains)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }
	
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.DenyRegEx)]
        public IActionResult DoSomething(int id)
        {
			//response logic...
        }
	}

	//DNS (Static)
    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterDnsAddress("defcon.org", DnsAddressFilteringAction.Deny)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

	public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterDnsAddress(".org", DnsAddressFilteringAction.DenyContains)]
        public HttpResponseMessage DoSomething(int id)
        {
			//response logic...
        }
    }

    public class SampleController : ApiController
    {
        [HttpGet]
        [Route("api/{id}")]
        [ActionFilterDnsAddress(@"[a-zA-Z0-9]*\.org$", DnsAddressFilterAction.DenyRegEx)]
        public IActionResult DoSomething(int id)
        {
			//response logic...
        }
	}

```

# Notes

## Action filters. 
These implement IActionFilter and wrap the action method execution. The IActionFilter interface declares two methods: OnActionExecuting and OnActionExecuted. OnActionExecuting runs before the action method. OnActionExecuted runs after the action method and can perform additional processing, such as providing extra data to the action method, inspecting the return value, or canceling execution of the action method.

## Authorization filters. 
These implement IAuthorizationFilter and make security decisions about whether to execute an action method, such as performing authentication or validating properties of the request. The AuthorizeAttribute class is an example of an authorization filter. Authorization filters run before any other filter.

## HTTPS Development Tidbits.
For Chrome you need to enter URL chrome://flags/#allow-insecure-localhost into the address bar & then enable setting.

# 
