window.main = {
    domSearch: {
        searchElems: {
            walker: document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT),
            mark: document.createElement("mark"),
        },
        activePageElems: {
            domSearchOutputCount: document.createElement("p")
        },
        highlight: function (query) {
            document.querySelectorAll("mark.search-highlight").forEach(m => {
                const parent = m.parentNode;
                parent.replaceChild(document.createTextNode(m.textContent), m);
                parent.normalize();
            });

            if (!query) return 0;

            const nodes = [];
            let node;

            //create a new walker
            let walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
            while (node = walker.nextNode()) {
                nodes.push(node);
            }

            let count = 0;
            const lowerQuery = query.toLowerCase();

            //search element markup
            const domSearchOutputElem = document.querySelector("#domSearchOutput");
            let elementsarr = Array.from(domSearchOutputElem.childNodes);
            elementsarr.map(e => {
                console.log(e.textContent);
                e.remove();
                }
            )

            this.activePageElems.domSearchOutputCount.innerText = "";
            domSearchOutputElem.insertAdjacentElement("afterbegin", this.activePageElems.domSearchOutputCount);
            this.activePageElems.domSearchOutputCount.style.display = "inline-block";

            let foundElem = false;

            //iterate node list of elements
            for (const node of nodes) {
                const text = node.nodeValue;
                const domWalkerFindingIndex = text.toLowerCase().indexOf(lowerQuery);

                if (domWalkerFindingIndex >= 0) {
                    let countElement;
                    let domSearchOutputCount;

                    // search has started, begin the walk

                    // create the elements needed of findings container
                    domSearchOutputElem.style.visibility = "visible";
                    countElement = domSearchOutputElem.insertAdjacentElement(
                        "afterbegin",
                        document.createElement("span")
                    );
                    this.activePageElems.domSearchOutputCount.innerText = "Count: ";
                    domSearchOutputCount = document.querySelector("#domSearchOutput p");
                    domSearchOutputCount.insertAdjacentElement("beforeend", countElement);

                    if (foundElem) {
                        domSearchOutputElem.style.visibility = "visible";
                    }

                    this.searchElems.mark.className = "search-highlight";
                    let queryFinding = text.substr(domWalkerFindingIndex, query.length);
                    this.searchElems.mark.textContent = queryFinding;
    
                    const after = document.createTextNode(text.substr(domWalkerFindingIndex + query.length));
                    const before = document.createTextNode(text.substr(0, domWalkerFindingIndex));

                    const parent = node.parentNode;
                    if (!parent) continue;

                    //NEEDED? return node for search value
                    //create node for search value
                    const foundNode = domSearchOutputElem.insertAdjacentElement('beforeend',document.createElement("div"));

                    let originalNode_FoundCharacters = node.parentNode.textContent;
                    let foundNode_FoundCharacters = originalNode_FoundCharacters;
                    
                    
                    let queryResultingEndCharactersFull;

                    // set a char limit
                    const findShowingCharLength = 30;
                    let originalNodeStartingCharacters = originalNode_FoundCharacters.slice(0, findShowingCharLength);

                    let queryResultingEndCharactersStartIndexNum;
                    if (domWalkerFindingIndex < findShowingCharLength) { //query finding is before cutoff
                        foundNode_FoundCharacters = originalNodeStartingCharacters;
                        let trailingCharacters = originalNode_FoundCharacters.slice(30, originalNode_FoundCharacters.length);
                        queryResultingEndCharactersFull = originalNode_FoundCharacters.slice(queryResultingEndCharactersStartIndexNum, queryResultingEndCharactersStartIndexNum + 30);

                        foundNode_FoundCharacters += trailingCharacters;
                        //repeated
                    }
                    else { // query finding is after cutoff
                        foundNode_FoundCharacters = originalNodeStartingCharacters + `...${queryFinding} `; // cut down query result length
                        //NEEDED: check query length to return precision string length
                        
                        queryResultingEndCharactersStartIndexNum = 30 + queryFinding.length;

                        // take out from short query result the middle bits
                        // 36 to the resultingendcharacterstart(which is the queryResultingEndCharactersStartIndexNum?)
                        queryResultingEndCharacters = queryResultingEndCharactersFull.slice(queryResultingEndCharactersStartIndexNum, domWalkerFindingIndex);
                        
                        queryResultingEndCharactersFull = originalNode_FoundCharacters.slice(queryResultingEndCharactersStartIndexNum, queryResultingEndCharactersStartIndexNum + 30);
                        foundNode_FoundCharacters += queryResultingEndCharactersFull;

                        let querysEndIndexCharacterCount = queryResultingEndCharactersStartIndexNum + 30;
                    }
                    let findingStartIndex = walkerFindingIndex + query.length;
                    let captureNextSpaceCharacterIndex = originalNode_FoundCharacters.indexOf(" ", findingStartIndex);
                    let capturePreviousSpaceCharacterIndex = originalNode_FoundCharacters.lastIndexOf(" ", findingStartIndex - 1);
                    let captureWordWithWhitespaceCharacters = originalNode_FoundCharacters.substring(capturePreviousSpaceCharacterIndex, captureNextSpaceCharacterIndex);
                    let queryResultingFirstPartialWordAfterWhitespace = queryResultingEndCharactersFull.indexOf(" ", findingStartIndex - 1);

                        foundNode.textContent = foundNode_FoundCharacters;
                    //click on node

                    //go to element

                    //if visibility hidden, reveal
                    if(node.parentElement.dataset.visibility == 'none') {
                        node.parentElement.dataset.visibility = "initial";
                    }

                    //replace node
                    parent.replaceChild(after, node);
                    parent.insertBefore(this.searchElems.mark, after);
                    parent.insertBefore(before, this.searchElems.mark);

                    foundElem = true;
                    count++;
                    //this.activePageElems.domSearchOutputCount.innerText = count;
                    countElement.textContent = count;
                }
            }

            return count;
        }
    },
    input: (dotNetRef) => {
        const search = document.querySelector("#search");
        let count;
        search.addEventListener('input', async e => {
            let message = e.target.value;

            let words = [
                "\"secure\"",
                "+",
                ",",
                "-->",
                "/api/inventoryitem",
                "/docs.rhdeveloping.com",
                "/docs.rhdeveloping.com",
                "/randomwebbits",
                "/releases",
                "/winhttprequest",
                "/winhttprequest",
                "2024",
                "access",
                "accessible",
                "accomplished",
                "administering",
                "agile",
                "agilestock",
                "analysts",
                "api",
                "app",
                "application",
                "architecture",
                "article",
                "articles",
                "assigned",
                "astronomy",
                "auth",
                "authentication",
                "authorization",
                "azure",
                "backend",
                "banking",
                "barcode",
                "based",
                "bits",
                "blazor",
                "book",
                "book's",
                "books",
                "browser",
                "browsing",
                "bundles",
                "c#",
                "calls",
                "capstone",
                "character",
                "client-side",
                "cloud",
                "collection",
                "common",
                "compiled",
                "complex",
                "components",
                "computer",
                "confirmed",
                "controller",
                "controls",
                "core",
                "course",
                "create",
                "created",
                "creating",
                "creative",
                "cru/ed",
                "css",
                "css/js",
                "data",
                "database",
                "day",
                "days'",
                "delete",
                "delete",
                "demo",
                "demonstrate",
                "demonstrates",
                "demonstrating",
                "demonstration",
                "demos",
                "design",
                "designed",
                "desktop",
                "desktop",
                "desktop-and-web",
                "details",
                "develop",
                "developed",
                "developer",
                "developers",
                "developing",
                "development",
                "development.agilestock",
                "development.agilestockweb",
                "display",
                "distribution",
                "docs",
                "docs.rhdeveloping.com",
                "docs.rhdeveloping.com",
                "docs.rhdeveloping.com",
                "docs.rhdeveloping.com",
                "docs.rhdeveloping.com/content/private-agilestock-readme.html",
                "docs.rhdeveloping.com/content/winhttprequest-readme.html",
                "document",
                "driver",
                "dss",
                "e-commerce",
                "easybank",
                "ecmascript",
                "elements",
                "engineering",
                "ensure",
                "es2024",
                "etc",
                "example",
                "features",
                "figma",
                "flask",
                "flow",
                "focused",
                "following",
                "form",
                "format",
                "framework",
                "frontend",
                "frontend/backend/database",
                "full-data-flow",
                "fully",
                "functionality",
                "functioning",
                "functions.the",
                "gathers",
                "generates",
                "generator",
                "get",
                "github",
                "github.com",
                "github.com",
                "github.com/robhowe-a/randomwebbits",
                "github.com/robhowe-a/randomwebbits",
                "github.com/robhowe-a/winhttprequest",
                "github.com/robhowe-a/winhttprequest",
                "github.com/robhowe-a/winhttprequest/releases",
                "github.com/robhowe-a/winhttprequest/releases",
                "go",
                "google",
                "graphic-user-interface",
                "handling",
                "hardware",
                "hardware",
                "head",
                "howell",
                "html",
                "html/css",
                "html/css/js",
                "http",
                "https://docs.rhdeveloping.com/content/private-agilestock-readme.html",
                "https://docs.rhdeveloping.com/content/winhttprequest-readme.html",
                "https://github.com/robhowe-a/randomwebbits",
                "https://github.com/robhowe-a/winhttprequest",
                "https://github.com/robhowe-a/winhttprequest/releases",
                "i.e",
                "identity",
                "iis",
                "image",
                "indicator",
                "industry",
                "information",
                "inputs",
                "insights",
                "interaction",
                "interactive",
                "interconnect",
                "inventory",
                "isbn",
                "it",
                "javascript",
                "jira",
                "jira's",
                "json",
                "kanban",
                "lake",
                "language",
                "library",
                "linux",
                "live",
                "lookup",
                "major",
                "management",
                "manipulates",
                "markdown",
                "markup",
                "master",
                "member",
                "members",
                "members'",
                "meter",
                "methodology",
                "model",
                "model-view-controller",
                "multi-threading",
                "multiple",
                "mvc",
                "mysql",
                "name",
                "nasa's",
                "news",
                "nextjs",
                "oauth2",
                "object",
                "one",
                "online",
                "ood",
                "oop",
                "open-source",
                "open-source::github",
                "operation",
                "options",
                "options",
                "oriented",
                "page",
                "password",
                "passwords",
                "patch",
                "payment",
                "pci",
                "pi",
                "picture",
                "picture-of-the-day",
                "post",
                "privacy",
                "private",
                "production",
                "products",
                "products'",
                "programmed",
                "programming",
                "projects",
                "provides",
                "providing",
                "public",
                "put",
                "python",
                "quiz",
                "quizzes",
                "random",
                "randomwebbits",
                "raspberry",
                "react",
                "read",
                "refining",
                "releases",
                "request",
                "requirements",
                "responsibility",
                "responsive",
                "rest",
                "rest+cru/ed+api",
                "rest+json",
                "rest,cru/ed,api,json",
                "result",
                "retrieved",
                "retrieves",
                "robert",
                "roberthowell.dev",
                "robhowe-a",
                "sample",
                "scan",
                "scanned",
                "scanner",
                "scanning",
                "science",
                "score",
                "script",
                "scrum",
                "search",
                "searching",
                "secure",
                "security",
                "selection",
                "sends",
                "sensitive",
                "server",
                "services",
                "set",
                "setting",
                "showcase",
                "showcasing",
                "side",
                "simply",
                "single",
                "site",
                "software",
                "solution",
                "sources",
                "spaceflight",
                "speed",
                "sphinx",
                "sprint",
                "sprints",
                "sql",
                "standard",
                "started",
                "status",
                "storage",
                "store",
                "stores",
                "storing",
                "strength",
                "successful",
                "system",
                "tables",
                "tasks",
                "team",
                "teams",
                "techniques",
                "template",
                "test",
                "tested",
                "this",
                "thread",
                "three",
                "throughout",
                "tile",
                "timeline",
                "triad",
                "typescript",
                "typing",
                "ubuntu",
                "universal",
                "update/edit",
                "url-specific",
                "user",
                "user's",
                "users",
                "using",
                "various",
                "varying",
                "via",
                "view",
                "viewing",
                "vue",
                "web",
                "websites",
                "websockets",
                "week",
                "weekly",
                "where",
                "widget",
                "windows",
                "winhttprequest",
                "winhttprequest",
                "wireframe",
                "with",
                "wordpress",
                "workload",
                "xaml/forms/wpf"
            ];

            if (!words.includes(message)) return;
            count = window.main.domSearch.highlight(message);

            dotNetRef.invokeMethodAsync("ReceiveCount", count);
        })

            return count;
    }
}

Blazor.start({
    circuit: {
        configureSignalR: function (builder) {
            builder.configureLogging("Error");
        }
    }
});
