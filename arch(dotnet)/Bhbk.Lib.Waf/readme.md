# Bhbk.Lib.Waf

A Web Application Firewall library providing ASP.NET Core action filters for controlling access based on IP addresses, DNS hostnames, HTTP options, and time schedules.

## Installation

Add a project reference to `Bhbk.Lib.Waf` in your ASP.NET Core application.

## Features

- **IP Address Filtering** - Allow/deny requests by IPv4/IPv6 addresses and CIDR ranges
- **DNS Address Filtering** - Allow/deny requests by hostname (exact match, contains, or regex)
- **HTTP Option Filtering** - Enforce or restrict SSL/HTTPS connections
- **Schedule Filtering** - Allow/deny requests based on time windows

## Usage

All filters are applied as attributes on controllers or actions. Filters can use **static** values (hardcoded in the attribute) or **dynamic** values (loaded from `appsettings.json` at runtime).

### IP Address Filtering

Filter requests based on client IP address using CIDR notation.

```csharp
using Bhbk.Lib.Waf.IpAddress;

// Static: Allow only localhost (IPv6)
[IpAddress("::1", IpAddressFilterAction.Allow)]
public IActionResult SecureEndpoint() { ... }

// Static: Allow a subnet
[IpAddress("192.168.1.0/24", IpAddressFilterAction.Allow)]
public IActionResult InternalOnly() { ... }

// Static: Deny specific IPs (comma-separated)
[IpAddress("10.0.0.1, 10.0.0.2", IpAddressFilterAction.Deny)]
public IActionResult BlockBadActors() { ... }

// Dynamic: Load from appsettings.json
[IpAddress(IpAddressFilterAction.Allow)]
public IActionResult ConfiguredEndpoint() { ... }
```

**Dynamic configuration** (`appsettings.json`):
```json
{
  "FirewallRules": {
    "IpDynamicAllow": [ "192.168.1.0/24", "10.0.0.0/8" ],
    "IpDynamicDeny": [ "0.0.0.0/0" ]
  }
}
```

### DNS Address Filtering

Filter requests based on the client's reverse DNS hostname.

```csharp
using Bhbk.Lib.Waf.DnsAddress;

// Static: Allow exact hostname match
[DnsAddress("trusted.example.com", DnsAddressFilterAction.Allow)]
public IActionResult TrustedOnly() { ... }

// Static: Allow hostnames containing a substring
[DnsAddress(".internal", DnsAddressFilterAction.AllowContains)]
public IActionResult InternalHosts() { ... }

// Static: Allow hostnames matching a regex pattern
[DnsAddress(@"[a-zA-Z0-9]*\.corp\.local$", DnsAddressFilterAction.AllowRegEx)]
public IActionResult CorpNetwork() { ... }

// Static: Deny specific hostnames
[DnsAddress("malicious.example.com", DnsAddressFilterAction.Deny)]
public IActionResult BlockHost() { ... }

// Dynamic: Load from appsettings.json
[DnsAddress(DnsAddressFilterAction.Allow)]
public IActionResult ConfiguredDns() { ... }
```

**Filter actions:**
| Action | Description |
|--------|-------------|
| `Allow` | Only allow exact hostname matches |
| `AllowContains` | Allow if hostname contains the substring |
| `AllowRegEx` | Allow if hostname matches the regex pattern |
| `Deny` | Deny exact hostname matches |
| `DenyContains` | Deny if hostname contains the substring |
| `DenyRegEx` | Deny if hostname matches the regex pattern |

**Dynamic configuration** (`appsettings.json`):
```json
{
  "FirewallRules": {
    "DnsDynamicAllow": [ "trusted.example.com" ],
    "DnsDynamicDeny": [ "blocked.example.com" ],
    "DnsDynamicAllowContains": [ ".internal" ],
    "DnsDynamicDenyContains": [ ".external" ],
    "DnsDynamicAllowRegEx": [ ".*\\.corp\\.local$" ],
    "DnsDynamicDenyRegEx": [ ".*\\.untrusted\\.net$" ]
  }
}
```

### HTTP Option Filtering

Enforce SSL/HTTPS requirements on endpoints.

```csharp
using Bhbk.Lib.Waf.HttpOption;

// Require HTTPS - reject HTTP requests
[HttpOption(HttpFilterAction.SslRequired)]
public IActionResult SecureOnly() { ... }

// Require HTTP - reject HTTPS requests (rare use case)
[HttpOption(HttpFilterAction.SslNotAllowed)]
public IActionResult PlaintextOnly() { ... }

// Allow both HTTP and HTTPS
[HttpOption(HttpFilterAction.SslOptional)]
public IActionResult EitherProtocol() { ... }
```

### Schedule Filtering

Restrict access based on time windows. Useful for maintenance windows or business hours enforcement.

```csharp
using Bhbk.Lib.Waf.Schedule;

// Static: Allow access only during business hours (8 AM - 5 PM daily)
[Schedule("8-17", ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
public IActionResult BusinessHours() { ... }

// Static: Deny access during maintenance window
[Schedule("2-4", ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily)]
public IActionResult NoMaintenance() { ... }

// Static: Allow only on weekdays
[Schedule("Monday-Friday", ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly)]
public IActionResult WeekdaysOnly() { ... }

// Dynamic: Load schedule from appsettings.json
[Schedule(ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
public IActionResult ConfiguredSchedule() { ... }
```

**Occurrence types:**
| Occur | Format | Description |
|-------|--------|-------------|
| `Daily` | `H-H` (e.g., `8-17`) | Hour range (0-23) |
| `Weekly` | `Day-Day` (e.g., `Monday-Friday`) | Day of week range |
| `Hourly` | `M-M` (e.g., `0-30`) | Minute range (0-59) |
| `Once` | `yyyy:M:dTH:m:s-yyyy:M:dTH:m:s` | Specific datetime range |

**Dynamic configuration** (`appsettings.json`):
```json
{
  "FirewallRules": {
    "ScheduleDynamicAllow": [ "8-17" ],
    "ScheduleDynamicDeny": [ "0-6" ]
  }
}
```

## Applying Filters to Controllers

Filters can be applied at the controller level to affect all actions:

```csharp
[IpAddress("192.168.0.0/16", IpAddressFilterAction.Allow)]
[HttpOption(HttpFilterAction.SslRequired)]
public class SecureController : Controller
{
    // All actions inherit the IP and SSL filters

    [HttpGet]
    public IActionResult Index() { ... }

    // Override or add additional filters per action
    [Schedule("9-17", ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
    public IActionResult BusinessHoursOnly() { ... }
}
```

## Response Behavior

When a request is denied, the filter returns:
- **HTTP 401 Unauthorized**
- **Content-Type:** `application/json`
- **Body:** Error message with the client identifier (IP, hostname, etc.)

Example response:
```
(192.168.1.100) Your IP address is denied.
```

## Dependencies

- `Microsoft.AspNetCore.Mvc` (2.3.9+)
- `IPNetwork2` (2.5.240+) - For CIDR parsing
- `Bhbk.Lib.Common` - For regex validation utilities

## Notes

- IP address filtering supports both IPv4 and IPv6 with CIDR notation
- DNS filtering performs reverse DNS lookups, which may add latency
- Schedule filtering uses server local time (`DateTime.Now`)
- Filters return 401 Unauthorized (not 403 Forbidden) for denied requests
