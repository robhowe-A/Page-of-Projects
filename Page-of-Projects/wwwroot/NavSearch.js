window.main = {
    domSearch: {
        searchElems: {
            walker: document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT)//,
            //mark: document.createElement("mark"),
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

                let queryFinding = text.slice(domWalkerFindingIndex, domWalkerFindingIndex + query.length); //TODO:NOW: has inconsistent query findings

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


                    const mark = document.createElement("mark"); //from 1.x.x version, before

                    mark.className = "search-highlight";
                    //                               from: domWalkerFindingIndex
                    mark.textContent = text.substr(domWalkerFindingIndex, query.length); //from 1.x.x version, before, with changes to index substr param
    
                    const after = document.createTextNode(text.substring(domWalkerFindingIndex + query.length));
                    const before = document.createTextNode(text.substring(0, domWalkerFindingIndex));

                    const parent = node.parentNode;
                    if (!parent) continue;

                    //NEEDED? return node for search value
                    //create node for search value
                    const foundNode = domSearchOutputElem.insertAdjacentElement('beforeend',document.createElement("div"));

                    let originalNode_FoundCharacters = node.parentNode.textContent;
                    let foundNode_CharactersFindings = originalNode_FoundCharacters;

                    // set a char limit
                    const findShowingCharacterLength = 20;
                    const findShowingMaximumCharacterLength = 60;
                    const getCharactersPreviousSpaceCharacterIndex = (characters, wordIndex) => {
                        return characters.lastIndexOf(" ", wordIndex - 1);
                    }
                    const getCharactersNextSpaceCharacterIndex = (characters, wordIndex) => {
                        return characters.indexOf(" ", wordIndex);
                    }
                    
                    let findShowingCharacterCutoffWordNextSpaceIndex = getCharactersNextSpaceCharacterIndex(originalNode_FoundCharacters, findShowingCharacterLength);
                    let originalNode_StartingCharacters = originalNode_FoundCharacters.slice(0, findShowingCharacterCutoffWordNextSpaceIndex);
                    
                    let queryResultingEndCharactersStartIndexNum;
                    let queryResultingEndCharacters;
                    let queryResultingEndCharactersFull;
                    
                    
                    if (domWalkerFindingIndex < findShowingCharacterLength) { //query finding is before cutoff
                        foundNode_CharactersFindings = originalNode_StartingCharacters;
                        //NEEDED?: query slice at character cutoff length
                        let trailingCharacters = originalNode_FoundCharacters.slice(findShowingCharacterCutoffWordNextSpaceIndex, originalNode_FoundCharacters.length);

                        foundNode_CharactersFindings += trailingCharacters;
                    }
                    else { // query finding is after cutoff
                        foundNode_CharactersFindings = originalNode_StartingCharacters + `... `; // cut down query result length

                        //NEEDED?: check query length to return precision string length
                        queryResultingEndCharactersStartIndexNum = findShowingCharacterLength + queryFinding.length;

                        // new variable that describes the difference from the full lengths' findings from the queryResultingEndCharactersFulll
                        queryResultingEndCharactersFull = originalNode_FoundCharacters.substring(domWalkerFindingIndex, originalNode_FoundCharacters.length);
                        foundNode_CharactersFindings += queryResultingEndCharactersFull;

                        //let querysEndIndexCharacterCount = queryResultingEndCharactersStartIndexNum + findShowingCharacterLength; //TODO - for now, unused
                    }
                    // clip the end from the findings' characters to a maximum number
                    if (foundNode_CharactersFindings.length > findShowingMaximumCharacterLength) {
                        let foundNode_CharactersFindingsEndWordIndex = getCharactersNextSpaceCharacterIndex(foundNode_CharactersFindings, findShowingMaximumCharacterLength)
                        if (foundNode_CharactersFindingsEndWordIndex >= 0) {
                            foundNode_CharactersFindings = foundNode_CharactersFindings.substring(0, foundNode_CharactersFindingsEndWordIndex);
                            foundNode_CharactersFindings += "...";
                        }
                    }
                    
                    foundNode.textContent = foundNode_CharactersFindings;

                    if (node) { //TODO: trim last hr element
                        const domSearchOutputElemRulingBreaker = foundNode.insertAdjacentElement(
                            "afterend",
                            document.createElement("hr")
                        );
                    }

                    //click on node

                    //go to element

                    //if visibility hidden, reveal
                    if(node.parentElement.dataset.visibility == 'none') {
                        node.parentElement.dataset.visibility = "initial";
                    }

                    //replace node
                    console.debug("inserted");
                    parent.replaceChild(after, node);
                    //get rid of mark
                    //parent.insertBefore(this.searchElems.mark, after);
                    parent.insertBefore(mark, after); //from 1.x.x version, before

                    //get rid of mark
                    //parent.insertBefore(before, this.searchElems.mark);
                    parent.insertBefore(before, mark); //from 1.x.x version, before

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
                `-->`,
                `,`,
                `"SECURE"`,
                `/api/inventoryitem`,
                `/RandomWebBits`,
                `/releases`,
                `/WinHttpRequest`,
                `+`,
                `2024`,
                `access`,
                `accessible`,
                `accomplished`,
                `administering`,
                `agile`,
                `AgileStock`,
                `analysts`,
                `API`,
                `App`,
                `application`,
                `architecture`,
                `article`,
                `articles`,
                `assigned`,
                `Astronomy`,
                `Auth`,
                `Authentication`,
                `Authorization`,
                `Azure`,
                `backend`,
                `banking`,
                `barcode`,
                `based`,
                `Bits`,
                `Blazor`,
                `book`,
                `book's`,
                `books`,
                `browser`,
                `browsing`,
                `bundles`,
                `C#`,
                `calls`,
                `capstone`,
                `character`,
                `client-side`,
                `cloud`,
                `collection`,
                `common`,
                `compiled`,
                `complex`,
                `components`,
                `computer`,
                `confirmed`,
                `controller`,
                `controls`,
                `core`,
                `course`,
                `Create`,
                `created`,
                `creating`,
                `creative`,
                `CRU/ED`,
                `CSS`,
                `CSS/JS`,
                `data`,
                `database`,
                `Day`,
                `days'`,
                `Delete`,
                `DELETE`,
                `Demo`,
                `demonstrate`,
                `demonstrates`,
                `demonstrating`,
                `demonstration`,
                `Demos`,
                `design`,
                `designed`,
                `Desktop`,
                `desktop-and-web`,
                `details`,
                `develop`,
                `developed`,
                `developer`,
                `developers`,
                `developing`,
                `development`,
                `development.AgileStock`,
                `development.AgileStockWeb`,
                `display`,
                `distribution`,
                `Docs`,
                `docs.rhdeveloping.com`,
                `docs.rhdeveloping.com`,
                `document`,
                `driver`,
                `DSS`,
                `e-commerce`,
                `Easybank`,
                `ECMAScript`,
                `elements`,
                `engineering`,
                `ensure`,
                `ES2024`,
                `etc`,
                `example`,
                `features`,
                `Figma`,
                `Flask`,
                `flow`,
                `focused`,
                `following`,
                `form`,
                `format`,
                `Framework`,
                `frontend`,
                `frontend/backend/database`,
                `full-data-flow`,
                `fully`,
                `functionality`,
                `functioning`,
                `functions.The`,
                `gathers`,
                `generates`,
                `Generator`,
                `GET`,
                `GitHub`,
                `github.com`,
                `github.com/robhowe-A/RandomWebBits`,
                `github.com/robhowe-A/WinHttpRequest`,
                `github.com/robhowe-A/WinHttpRequest/releases`,
                `Go`,
                `Google`,
                `graphic-user-interface`,
                `handling`,
                `hardware`,
                `hardware`,
                `HEAD`,
                `Howell`,
                `HTML`,
                `HTML/CSS`,
                `HTML/CSS/JS`,
                `http`,
                `https://docs.rhdeveloping.com/content/private-agilestock-readme.html`,
                `docs.rhdeveloping.com/content/private-agilestock-readme.html`,
                `/docs.rhdeveloping.com`,
                `docs.rhdeveloping.com`,
                `https://docs.rhdeveloping.com/content/winhttprequest-readme.html`,
                `docs.rhdeveloping.com/content/winhttprequest-readme.html`,
                `/docs.rhdeveloping.com`,
                `docs.rhdeveloping.com`,
                `https://github.com/robhowe-A/RandomWebBits`,
                `github.com/robhowe-A/RandomWebBits`,
                `github.com`,
                `https://github.com/robhowe-A/WinHttpRequest`,
                `github.com/robhowe-A/WinHttpRequest`,
                `/WinHttpRequest`,
                `WinHttpRequest`,
                `https://github.com/robhowe-A/WinHttpRequest/releases`,
                `github.com/robhowe-A/WinHttpRequest/releases`,
                `i.e`,
                `Identity`,
                `IIS`,
                `image`,
                `indicator`,
                `industry`,
                `information`,
                `inputs`,
                `insights`,
                `interaction`,
                `interactive`,
                `interconnect`,
                `inventory`,
                `ISBN`,
                `It`,
                `JavaScript`,
                `Jira`,
                `Jira's`,
                `JSON`,
                `kanban`,
                `Lake`,
                `Language`,
                `library`,
                `Linux`,
                `live`,
                `lookup`,
                `major`,
                `management`,
                `manipulates`,
                `markdown`,
                `markup`,
                `master`,
                `member`,
                `members`,
                `members'`,
                `meter`,
                `methodology`,
                `model`,
                `Model-View-Controller`,
                `multi-threading`,
                `multiple`,
                `MVC`,
                `MySQL`,
                `Name`,
                `NASA's`,
                `news`,
                `NextJS`,
                `OAuth2`,
                `object`,
                `one`,
                `online`,
                `OOD`,
                `OOP`,
                `Open-source`,
                `Open-source::GitHub`,
                `operation`,
                `options`,
                `OPTIONS`,
                `oriented`,
                `page`,
                `Password`,
                `passwords`,
                `PATCH`,
                `payment`,
                `PCI`,
                `pi`,
                `Picture`,
                `picture-of-the-day`,
                `POST`,
                `privacy`,
                `production`,
                `Products`,
                `products'`,
                `programmed`,
                `programming`,
                `projects`,
                `provides`,
                `providing`,
                `public`,
                `private`,
                `put`,
                `Python`,
                `quiz`,
                `Quizzes`,
                `query`,
                `queries`,
                `Random`,
                `RandomWebBits`,
                `raspberry`,
                `React`,
                `Read`,
                `refining`,
                `releases`,
                `request`,
                `requirements`,
                `responsibility`,
                `Responsive`,
                `REST`,
                `REST,CRU/ED,API,JSON`,
                `REST+CRU/ED+API`,
                `REST+JSON`,
                `result`,
                `retrieved`,
                `retrieves`,
                `Robert`,
                `roberthowell.dev`,
                `robhowe-A`,
                `sample`,
                `scan`,
                `scanned`,
                `scanner`,
                `scanning`,
                `science`,
                `score`,
                `Script`,
                `scrum`,
                `search`,
                `searching`,
                `secure`,
                `security`,
                `selection`,
                `sends`,
                `sensitive`,
                `server`,
                `Services`,
                `set`,
                `setting`,
                `showcase`,
                `showcasing`,
                `side`,
                `simply`,
                `Single`,
                `site`,
                `software`,
                `solution`,
                `sources`,
                `spaceflight`,
                `speed`,
                `Sphinx`,
                `sprint`,
                `sprints`,
                `SQL`,
                `standard`,
                `started`,
                `Status`,
                `Storage`,
                `store`,
                `stores`,
                `storing`,
                `strength`,
                `successful`,
                `system`,
                `desktop`,
                `Tables`,
                `tasks`,
                `team`,
                `teams`,
                `techniques`,
                `template`,
                `Test`,
                `tested`,
                `this`,
                `thread`,
                `three`,
                `Throughout`,
                `tile`,
                `timeline`,
                `triad`,
                `TypeScript`,
                `typing`,
                `Ubuntu`,
                `universal`,
                `Update/Edit`,
                `URL-specific`,
                `user`,
                `user's`,
                `users`,
                `using`,
                `various`,
                `varying`,
                `via`,
                `view`,
                `viewing`,
                `Vue`,
                `web`,
                `Websites`,
                `websockets`,
                `week`,
                `weekly`,
                `where`,
                `widget`,
                `Windows`,
                `WinHttpRequest`,
                `Wireframe`,
                `with`,
                `WordPress`,
                `workload`,
                `XAML/Forms/WPF`
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
