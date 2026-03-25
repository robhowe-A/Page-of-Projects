using System.Text.Json;

namespace ProjectsPage.Components;

public class SubselectionMiscellaneous
{
    public List<string>? Items { get; set; }
    public List<string>? RequirementsOl { get; set; }
}

public class SelectionDetails
{
    public string? Details { get; set; }
    public string? ProgrammingLanguage { get; set; }
    public string? Framework { get; set; }
    public List<string>? SecondaryLanguage { get; set; }
    public List<string>? DetailsList { get; set; }
    public List<string>? Descriptions { get; set; }
    public SubselectionMiscellaneous? Miscellaneous { get; set; }
}

public class FrameSelectionOption
{
    public required string Href { get; set; }
    public required string Title { get; set; }
    public required string InnerText { get; set; }
    public required string ExternalHref { get; set; }
    public required string ExternalTitle { get; set; }
    public required string ImageUrl { get; set; }
    public required string ImageAltText { get; set; }
    public required SelectionDetails SelectionDetails { get; set; }
    public bool IsSelected { get; set; } = false;
};

public partial class FrameSelection
{
    
    
    private const string WebsitesOptions = """
                                           [
                                               {
                                                   "href": "/websites/randomwebbits",
                                                   "title": "Random Web Bits",
                                                   "innerText": "Random Web Bits",
                                                   "externalHref": "https://www.randomwebbits.com/",
                                                   "externalTitle": "www.randomwebbits.com",
                                                   "ImageUrl": "./images/RWB__2026-02-05.png",
                                                   "ImageAltText": "Website tile image.",
                                                   "SelectionDetails": {
                                                     "Details": "An interactive website providing information and insights about the web and web development.",
                                                     "ProgrammingLanguage": "TypeScript",
                                                     "Framework": "Blazor",
                                                     "SecondaryLanguage": ["NodeJS", "JavaScript", ".NET 9"],
                                                     "DetailsList": [
                                                       "HTML/CSS/JS",
                                                       "Script bundles",
                                                       "Open-source",
                                                       "JavaScript: Modules, bundles, packages, etc.",
                                                       "TypeScript: compiled to ES2024 (ECMAScript 2024) standard",
                                                       "Data Lake Storage",
                                                       "Azure App Services",
                                                       "Azure Application Insights",
                                                       "Security tested"
                                                     ],
                                                     "Descriptions": [],
                                                     "Miscellaneous": {}
                                                   }
                                               },
                                               {
                                                   "href": "/websites/quizzes-app",
                                                   "title": "Quizzes App",
                                                   "innerText": "Quizzes App",
                                                   "externalHref": "https://quizzes-demo.rhdeveloping.com/",
                                                   "externalTitle": "quizzes-demo.rhdeveloping.com",
                                                   "ImageUrl": "./images/QuizzesApp__2026-02-05.png",
                                                   "ImageAltText": "Website tile image.",
                                                   "SelectionDetails": {
                                                     "Details": "A web application demonstration where users take an online quiz. A quiz (model), created in a JSON format on the backend, is retrieved by the quiz page (view) via controller (controller). The server manipulates the object data and inputs live, using websockets, providing the user's quiz functionality.",
                                                     "ProgrammingLanguage": "C#",
                                                     "Framework": "Blazor",
                                                     "SecondaryLanguage": [".NET 10", "SQL"],
                                                     "DetailsList": [
                                                       "MVC: Model-View-Controller",
                                                       "REST+JSON",
                                                       "OAuth2 (Authentication, Authorization), Core Identity, Google Auth",
                                                       "Azure Storage",
                                                       "Azure App Services",
                                                       "Azure SQL",
                                                       "Azure Application Insights",
                                                       "Security tested"
                                                     ],
                                                     "Descriptions": [],
                                                     "Miscellaneous": {}
                                                   }
                                               },
                                               {
                                                   "href": "/websites/spaceflight-news-app",
                                                   "title": "SpaceFlight News App",
                                                   "innerText": "SpaceFlight News App",
                                                   "externalHref": "https://rh-snapi-site.rhdeveloping.com/",
                                                   "externalTitle": "rh-snapi-site.rhdeveloping.com",
                                                   "ImageUrl": "./images/SpaceFlight_News_App__2026-02-05.png",
                                                   "ImageAltText": "Website tile image.",
                                                   "SelectionDetails": {
                                                     "Details": "This web application demonstrates a backend API REST interconnect of multiple sources: spaceflight news articles and NASA's Astronomy Picture Of the Day. A service thread gathers and stores the data in the database. The web application is programmed to allow 30 days' article browsing and the same-day picture-of-the-day.",
                                                     "ProgrammingLanguage": "C#",
                                                     "Framework": "Blazor",
                                                     "SecondaryLanguage": [".NET 10", "SQL"],
                                                     "DetailsList": [
                                                       "IIS, Windows, Ubuntu Linux",
                                                       "OOD software engineering, web triad design (frontend/backend/database), multi-threading",
                                                       "CRU/ED: Create, Read, Update/Edit, Delete",
                                                       "Security tested"
                                                     ],
                                                     "Descriptions": [],
                                                     "Miscellaneous": {}
                                                   }
                                               },
                                               {
                                                   "href": "/websites/agilestock-web",
                                                   "title": "AgileStock Web",
                                                   "innerText": "AgileStock Web",
                                                   "externalHref": "https://roberthagilestockweb.com/",
                                                   "externalTitle": "roberthagilestockweb.com",
                                                   "ImageUrl": "./images/AgileStockWeb__2026-02-05.png",
                                                   "ImageAltText": "Website tile image.",
                                                   "SelectionDetails": {
                                                     "Details": "This project was for computer science major capstone course. We, a team of 5, developed a desktop-and-web accessible software solution for book inventory management. Our application met the following requirements set:",
                                                     "ProgrammingLanguage": "Python",
                                                     "Framework": "Flask",
                                                     "SecondaryLanguage": ["SQL"],
                                                     "DetailsList": [
                                                       "REST+CRU/ED+API+JSON -->Developers, see /api/inventoryitem GET",
                                                       "Object oriented programming (OOP)",
                                                       "Agile development via Jira + kanban + sprint teams",
                                                       "Azure MySQL database",
                                                       "Azure App Services",
                                                       "Security tested"
                                                     ],
                                                     "Descriptions": [
                                                       "Throughout the project (a three month timeline), we used Jira's software to develop in agile scrum methodology. I was the scrum master. I assigned our team members' workload each week. In weekly sprints, we accomplished varying tasks, from creating components, developing functionality, administering the production server to refining functions.\nThe end result was successful requirements met; we developed a full-data-flow inventory management system that started with barcode scanning (hardware using a raspberry pi) to searching the barcode using an online data library to storing the book's data in a cloud database to viewing the data in a website. Each team member was core to a flow operation in the development.\nAgileStock: The scanner system/desktop application was Noah's responsibility (he's one of the 4 team members), as he focused on the hardware, driver development.\nAgileStockWeb: I took on the data + web responsibility as I focused more on the web side." 
                                                     ],
                                                     "Miscellaneous": {
                                                         "RequirementsOL": [
                                                             "Must be able to scan a book's barcode for ISBN lookup",
                                                             "Must search for universal available data about using the scanned ISBN",
                                                             "Must store the book data for inventory management",
                                                             "Must allow inventory access by web browser"
                                                         ]
                                                     }
                                                   }
                                               },
                                               {
                                                   "href": "/websites/httprequest-app",
                                                   "title": "Http Request App",
                                                   "innerText": "Http Request App",
                                                   "externalHref": "https://www.httprequest.app/",
                                                   "externalTitle": "www.httprequest.app",
                                                   "ImageUrl": "./images/HTTP_Request__2026-02-05.png",
                                                   "ImageAltText": "Website tile image.",
                                                   "SelectionDetails": {
                                                     "Details": "Useful to developers and security analysts, this is a Windows application sends http request calls (i.e. GET, POST, PUT, etc.) and retrieves URL-specific page data. The WordPress website, here, is a showcase for the application.",
                                                     "ProgrammingLanguage": "C#",
                                                     "Framework": "",
                                                     "SecondaryLanguage": ["Forms", "WPF"],
                                                     "DetailsList": [
                                                       "XAML/Forms/WPF",
                                                       "Desktop development, graphic-user-interface design, interactive controls, interactive design",
                                                       "HTTP",
                                                       "Open-source GitHub sample available",
                                                       "GITHUB: https://github.com/robhowe-A/WinHttpRequest."
                                                     ],
                                                     "Descriptions": [],
                                                     "Miscellaneous": {}
                                                   }
                                               },
                                               {
                                                   "href": "/websites/project-docs",
                                                   "title": "Project Docs",
                                                   "innerText": "Project Docs",
                                                   "externalHref": "https://docs.rhdeveloping.com/",
                                                   "externalTitle": "docs.rhdeveloping.com",
                                                   "ImageUrl": "./images/Projects-Docs__2026-02-05.png",
                                                   "ImageAltText": "Website tile image.",
                                                   "SelectionDetails": {
                                                     "Details": "This site is simply a collection of document markdown put together from my public/private projects.",
                                                     "ProgrammingLanguage": "Markdown",
                                                     "Framework": "Sphinx",
                                                     "SecondaryLanguage": ["Python"],
                                                     "DetailsList": [],
                                                     "Descriptions": [],
                                                     "Miscellaneous": {}
                                                   }
                                               }
                                           ]
                                           """;

    private const string DemosOptions = """
                                           [
                                              {
                                                  "href": "/demos/paymentform",
                                                  "title": "Payment Form",
                                                  "innerText": "Payment Form",
                                                  "externalHref": "https://encryptedpaymentinformation-demo.rhdeveloping.com/form",
                                                  "externalTitle": "encryptedpaymentinformation-demo.rhdeveloping.com/form",
                                                  "ImageUrl": "./images/Form__2026-02-05.png",
                                                  "ImageAltText": "Demo tile image.",
                                                  "SelectionDetails": {
                                                    "Details": "This example is a secure payment form development. It is designed to demonstrate secure handling of sensitive payment information and ensure user privacy in an e-commerce setting.",
                                                    "ProgrammingLanguage": "JavaScript",
                                                    "Framework": "Vue",
                                                    "SecondaryLanguage": ["C#"],
                                                    "DetailsList": [
                                                        "PCI DSS",
                                                        "Security Tested"
                                                    ],
                                                    "Miscellaneous": {}
                                                  }
                                              },
                                              {
                                                  "href": "/demos/passwordgenerator",
                                                  "title": "Password Generator",
                                                  "innerText": "Password Generator",
                                                  "externalHref": "https://passwordgenerator-demo.rhdeveloping.com/",
                                                  "externalTitle": "passwordgenerator-demo.rhdeveloping.com",
                                                  "ImageUrl": "./images/Password_Generator__2026-02-05.png",
                                                  "ImageAltText": "Demo tile image.",
                                                  "SelectionDetails": {
                                                    "Details": "This is a fully functioning application widget. It generates secure passwords based on user selection from the character options. It provides a secure meter score of the inputs. Secure passwords are confirmed by a strength indicator of \"SECURE\".",
                                                    "ProgrammingLanguage": "JavaScript",
                                                    "Framework": "Vue",
                                                    "SecondaryLanguage": [],
                                                    "DetailsList": [
                                                        "HTML/CSS"
                                                    ],
                                                    "Miscellaneous": {}
                                                  }
                                              },
                                              {
                                                  "href": "/demos/typingspeedtest",
                                                  "title": "Typing Speed Test",
                                                  "innerText": "Typing Speed Test",
                                                  "externalHref": "https://typingspeedtest-demo.rhdeveloping.com/",
                                                  "externalTitle": "typingspeedtest-demo.rhdeveloping.com",
                                                  "ImageUrl": "./images/Typing_Speed_Test__2026-02-05.png",
                                                  "ImageAltText": "Demo tile image.",
                                                  "SelectionDetails": {
                                                    "Details": "A website demonstration for one to take a typing speed test. This demonstrates complex client-side programming tasks in a server distribution architecture.",
                                                    "ProgrammingLanguage": "JavaScript",
                                                    "Framework": "Vue",
                                                    "SecondaryLanguage": [],
                                                    "DetailsList": [
                                                        "HTML/CSS"
                                                    ],
                                                    "Miscellaneous": {}
                                                  }
                                              },
                                              {
                                                  "href": "/demos/products",
                                                  "title": "Products",
                                                  "innerText": "Products",
                                                  "externalHref": "https://products.rhdeveloping.com/",
                                                  "externalTitle": "products.rhdeveloping.com",
                                                  "ImageUrl": "./images/Products__2026-02-05.png",
                                                  "ImageAltText": "Demo tile image.",
                                                  "SelectionDetails": {
                                                    "Details": "A website demonstrating products' display and details. This demonstrates cloud programming and features inventory architecture.",
                                                    "ProgrammingLanguage": "JavaScript",
                                                    "Framework": "React",
                                                    "SecondaryLanguage": [".NET 9"],
                                                    "DetailsList": [
                                                        "REST+CRU/ED+API",
                                                        "HTML/CSS/JS",
                                                        "Azure Tables",
                                                        "Azure Storage",
                                                        "Azure App Services"
                                                    ],
                                                    "Miscellaneous": {}
                                                  }
                                              },
                                              {
                                                  "href": "/demos/easybank",
                                                  "title": "Easybank",
                                                  "innerText": "Easybank",
                                                  "externalHref": "https://easybank.rhdeveloping.com/",
                                                  "externalTitle": "easybank.rhdeveloping.com",
                                                  "ImageUrl": "./images/Easybank __2026-02-05.png",
                                                  "ImageAltText": "Demo tile image.",
                                                  "SelectionDetails": {
                                                    "Details": "This is a banking website page and demonstrates markup techniques common in the developing industry.",
                                                    "ProgrammingLanguage": "HTML",
                                                    "Framework": "NextJS",
                                                    "SecondaryLanguage": [],
                                                    "DetailsList": [
                                                     "CSS",
                                                     "Wireframe",
                                                     "Responsive design",
                                                     "Figma template"
                                                    ],
                                                    "Miscellaneous": {}
                                                  }
                                              },
                                              {
                                                   "href": "/demos/creativesinglepage",
                                                   "title": "Creative Single Page",
                                                   "innerText": "Creative Single Page",
                                                   "externalHref": "https://creative-demo.rhdeveloping.com/",
                                                   "externalTitle": "creative-demo.rhdeveloping.com",
                                                   "ImageUrl": "./images/Creative-Single-Page-Site__2026-02-05.png",
                                                   "ImageAltText": "Demo tile image.",
                                                   "SelectionDetails": {
                                                     "Details": "This demonstration is a creative website showcasing various design elements and user interaction.",
                                                     "ProgrammingLanguage": "HTML",
                                                     "Framework": "",
                                                     "SecondaryLanguage": [],
                                                     "DetailsList": [
                                                        "CSS/JS",
                                                        "Wireframe",
                                                        "Responsive design",
                                                        "Figma template"
                                                     ],
                                                     "Miscellaneous": {}
                                                   }
                                              }
                                           ]
                                        """;

    public static List<FrameSelectionOption> WebsitesOptionsData()
    {
        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(WebsitesOptions,
                   options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("WebsitesOptions is null");
    }

    public static List<FrameSelectionOption> DemosOptionsData()
    {
        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(DemosOptions,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("DemosOptions is null");
    }
};
