<!-- 
Copyright (c) 2026 Robert A. Howell
Author: Robert A. Howell
Description: This project was created as an addition/wrapper/showcase for my portfolio projects and is written in C#. The repository code is live deployed at www.roberthowell.dev.
Created_Date: May 2026
Edited: 2026-07-07
-->


# Description  
This project was created as an addition/wrapper/showcase for my portfolio projects and is written in C#. It is live deployed at [www.roberthowell.dev](https://www.roberthowell.dev).  
Restrictions: You may not use this code in commercial applications, production environments, or for unauthorized purposes without explicit permission from the author.  
Author: *see document metadata in code or raw view*    


## Feature List  
- <u>React‑style framework</u> — Component‑driven UI architecture modeled with declarative rendering patterns and modular front‑end logic.
- <u>Reusable component design</u> — Encapsulated UI elements with isolated behavior, argument‑based configuration, and shared utility bindings.
- <u>Database‑first topology</u> — Schema‑driven data modeling where the structure defines the ORM layer, a strongly typed source base, and stable query surfaces.
- <u>Scalable, maintainable, tailored design</u> — Architecture optimized for scalability: modular services, cloud‑ready endpoints, async pipelines, and separation of concerns across layers.
- <u>Custom components</u>
    - *Search feature* — Query‑driven filtering with keyword inputs; try terms like Frontend, .NET, database, cloud, or Azure.
    - *Details panel* — High‑level project insights rendered dynamically from structured metadata.
- <u>Event‑driven UI behavior</u> — DOM event hooks, async handlers, and dynamic element generation.
- <u>Node + npm pipeline</u> — Build automation, bundling, minification, and standards‑based ES compilation.
- <u>Cloud‑ready hosting</u> — Windows hosting, Kestrel web server, and optional cloud deployment.
- <u>Security‑tested architecture</u> — Validated using industry tools.
- <u>Open‑source samples</u> — Public GitHub repsitory.


### Project Security  
- HTTP security headers and middleware extension design  
- ASP.NET, ASP.NET Core secure cookies, session, HTTP, API configuration  
- Hardened against XSS, CSRF, SQL injection, MITM attacks  


## Architecture  
This project is a multi-project, in-process hosted, asynchronous .NET application implementing continuously integrated source control on GitHub. Continuous deployment is not in use, however, project tooling extensions provide little impedance to rapid iteration deployment. All source code is available to read; partial source code is open-sourced and the remaining is kept under private repository control to prevent project copies reaching the open source community.  

Viewing the private repository code is available as a [repository explorer](https://www.roberthowell.dev/sources/source), a read-only development page.  

-----

> ### Tangential Research  
> An investigation took place to find a method of sharing source code and limiting the availability. The findings result in no manageable methods allowing public (any non-organization entity) read-only restriction to sensitive code portions which prevent download, copy, or extraction action. For this reason, sensitive project files are withheld from public repository access. Further findings indicate public-private key cryptographic protections cannot provide a client node secured source code text. The reasons are multiple:
> - client decryption cannot secure a private encryption key
> - developer tools can intercept at the decryption point
> - developer tools are unremovable
> - web browsers do not offer plaintext read only protections
> - browsers protect the user, not the developer
> - encoding text offers only some protections
> - obfuscation can be reverse-engineered
> - memory inspection is always possible
> - TLS network encryption protects transmitted bytes, not the viewed bytes
> - licenses do not prevent copying
> 
> Additionally, it was found this is a fundamental principle limiting modern computing known as the client-side trust problem resulting in the files provided as generated images, significantly preventing copying and downloading.  

-----


## Data Structure  
``` JSON
// Copyright (c) 2026 Robert A. Howell
// author: Robert A. Howell (robhowe-A)
// created_at: 2026-06-10
// description: The database selection from obj id 1
// license: None. This source may not be used without the explit permission from the author.
// native_type: json
// project_name: Page-of-Projects
{
    "Href": "/websites/randomwebbits",
    "Title": "Random Web Bits",
    "ImageUrl": "./images/RWB__2026-02-05.png",
    "InnerText": "Random Web Bits",
    "IsSelected": false,
    "Networking": {
        "IsCloud": null,
        "Security": "TLS",
        "IsProxyBased": true,
        "DnsRecordTypes": [
            "A",
            "AAAA",
            "CNAME"
        ],
        "IpAccessControlled": null,
        "BackendUriEndpoints": []
    },
    "ExternalHref": "https://www.randomwebbits.com/",
    "ImageAltText": "Website tile image.",
    "ExternalTitle": "www.randomwebbits.com",
    "ReferenceHrefs": [
        "https://docs.rhdeveloping.com/content/randomwebbits-readme.html",
        "https://github.com/robhowe-A/RandomWebBits",
        "https://www.randomwebbits.com/src/widgetComponents.tark8aj5j2.js|Code|Widget Components",
        "https://www.randomwebbits.com/src/models/dictionarySearch.js|Code|Dictionary Widget",
        "https://www.randomwebbits.com/src/components/page/apidemo.js|Code|API Demo Component",
        "https://www.randomwebbits.com/pages/hsl",
        "https://www.randomwebbits.com/guides/devtools/sourcestab",
        "https://www.randomwebbits.com/pages/webides",
        "https://github.com/robhowe-A/RandomWebBits/blob/main/README.md|Code|README.md"
    ],
    "SelectionDetails": {
        "Details": "An interactive website providing information and insights about the web and web development.",
        "Database": {
            "Name": null,
            "Engine": null,
            "DataAccess": "Entity Framework, ORM",
            "Migrations": false,
            "HasDatabase": true,
            "ApproachType": "Database First",
            "IsRelationalDb": true
        },
        "Framework": "Node",
        "DetailsList": [
            "Event handling, dynamic element generation, DOM manipulation, document query, loops, asynchronous functions, data structures",
            "Components, models, packages, project extensions, context, etc...",
            "TypeScript frontend",
            "Relational database backend",
            "Node, npm, gulp, packages and script bundles",
            "EcmaScript standards-based code compilation",
            "Data Lake Storage",
            "Azure App Services",
            "Azure Application Insights",
            "Kestrel web server",
            "Security tested using industry tools",
            "Open-source sample on GitHub"
        ],
        "Descriptions": [],
        "Miscellaneous": {
            "Items": null,
            "RequirementsOl": null
        },
        "HostingDetails": {
            "OS": "Linux",
            "IsCloud": true,
            "WebServer": "Kestrel",
            "CloudProvider": null,
            "Containerized": false
        },
        "StartDateMonth": "2022-12",
        "SecondaryLanguage": [
            "JavaScript",
            "NodeJS",
            ".NET 10",
            "ASP.NET",
            "C#"
        ],
        "ProgrammingLanguage": "TypeScript"
    }
}

```

## Data Topography

![Screen Shot 2026-04-28 at 11.59.24-fullpage](./Screen_Shot_2026-04-28_at_11.59.24-fullpage.png)
