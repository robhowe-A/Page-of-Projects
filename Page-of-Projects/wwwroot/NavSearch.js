window.main = {
    domSearch: {
        highlight: function (query) {
            document.querySelectorAll("mark.search-highlight").forEach(m => {
                const parent = m.parentNode;
                parent.replaceChild(document.createTextNode(m.textContent), m);
                parent.normalize();
            });

            if (!query) return 0;

            const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
            const nodes = [];
            let node;

            while ((node = walker.nextNode())) {
                nodes.push(node);
            }

            let count = 0;
            const lowerQuery = query.toLowerCase();

            //search element markup
            const domSearchOutputElem = document.querySelector("#domSearchOutput");
            //let elementsarr = Array.from(domSearchOutputElem.childNodes);
            // elementsarr.map(e => {
            //     console.log(e.textContent);
            //     //e.remove()
            //     }
            // )
            
            const domSearchOutput = document.querySelector("#domSearch");

            const domSearchOutputCount = document.createElement("span");
            domSearchOutputElem.insertAdjacentElement("afterbegin", domSearchOutputCount);
            domSearchOutputCount.style.display = "inline-block";

            let foundElem = false;

            //iterate node list of elements
            for (const node of nodes) {
                const text = node.nodeValue;
                const index = text.toLowerCase().indexOf(lowerQuery);

                if (index >= 0) {
                    if(foundElem)
                        domSearchOutputElem.style.visibility = "visible";

                    const mark = document.createElement("mark");
                    mark.className = "search-highlight";
                    mark.textContent = text.substr(index, query.length);

                    const after = document.createTextNode(text.substr(index + query.length));
                    const before = document.createTextNode(text.substr(0, index));

                    const parent = node.parentNode;
                    if (!parent) continue;

                    //NEEDED? return node for search value
                    //create node for search value
                    const foundNode = domSearchOutputElem.insertAdjacentElement('beforeend',document.createElement("div"));
                    foundNode.textContent = node.parentNode.textContent;
                    //click on node

                    //go to element

                    //if visibility hidden, reveal
                    if(node.parentElement.dataset.visibility == 'none') {
                        node.parentElement.dataset.visibility = "initial";
                    }

                    //replace node
                    parent.replaceChild(after, node);
                    parent.insertBefore(mark, after);
                    parent.insertBefore(before, mark);

                    foundElem = true;
                    count++;
                    domSearchOutputCount.innerText = count;
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

            //if mesge not in word set, return
            let words = [
                `-->`,
                `,`,
                `"secure"`,
                `/api/inventoryitem`,
                `/randomwebbits`,
                `/releases`,
                `/winhttprequest`,
                `+`,
                `2024`,
                `access`,
                `accessible`,
                `accomplished`,
                `administering`,
                `agile`,
                `agilestock`,
                `analysts`,
                `api`,
                `app`,
                `application`,
                `architecture`,
                `article`,
                `articles`,
                `assigned`,
                `astronomy`,
                `auth`,
                `authentication`,
                `authorization`,
                `azure`,
                `backend`,
                `banking`,
                `barcode`,
                `based`,
                `bits`,
                `blazor`,
                `book`,
                `book's`,
                `books`,
                `browser`,
                `browsing`,
                `bundles`,
                `c#`,
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
                `create`,
                `created`,
                `creating`,
                `creative`,
                `cru/ed`,
                `css`,
                `css/js`,
                `data`,
                `database`,
                `day`,
                `days'`,
                `delete`,
                `delete`,
                `demo`,
                `demonstrate`,
                `demonstrates`,
                `demonstrating`,
                `demonstration`,
                `demos`,
                `design`,
                `designed`,
                `desktop`,
                `desktop-and-web`,
                `details`,
                `develop`,
                `developed`,
                `developers`,
                `developing`,
                `development`,
                `development.agilestock`,
                `development.agilestockweb`,
                `display`,
                `distribution`,
                `docs`,
                `docs.rhdeveloping.com`,
                `docs.rhdeveloping.com`,
                `document`,
                `driver`,
                `dss`,
                `e-commerce`,
                `easybank`,
                `ecmascript`,
                `elements`,
                `engineering`,
                `ensure`,
                `es2024`,
                `etc`,
                `example`,
                `features`,
                `figma`,
                `flask`,
                `flow`,
                `focused`,
                `following`,
                `form`,
                `format`,
                `framework`,
                `frontend`,
                `frontend/backend/database`,
                `full-data-flow`,
                `fully`,
                `functionality`,
                `functioning`,
                `functions.the`,
                `gathers`,
                `generates`,
                `generator`,
                `get`,
                `github`,
                `github.com`,
                `github.com/robhowe-a/randomwebbits`,
                `github.com/robhowe-a/winhttprequest`,
                `github.com/robhowe-a/winhttprequest/releases`,
                `go`,
                `google`,
                `graphic-user-interface`,
                `handling`,
                `hardware`,
                `hardware`,
                `head`,
                `howell`,
                `html`,
                `html/css`,
                `html/css/js`,
                `http`,
                `https://docs.rhdeveloping.com/content/private-agilestock-readme.html`,
                `docs.rhdeveloping.com/content/private-agilestock-readme.html`,
                `/docs.rhdeveloping.com`,
                `docs.rhdeveloping.com`,
                `https://docs.rhdeveloping.com/content/winhttprequest-readme.html`,
                `docs.rhdeveloping.com/content/winhttprequest-readme.html`,
                `/docs.rhdeveloping.com`,
                `docs.rhdeveloping.com`,
                `https://github.com/robhowe-a/randomwebbits`,
                `github.com/robhowe-a/randomwebbits`,
                `github.com`,
                `https://github.com/robhowe-a/winhttprequest`,
                `github.com/robhowe-a/winhttprequest`,
                `/winhttprequest`,
                `winhttprequest`,
                `https://github.com/robhowe-a/winhttprequest/releases`,
                `github.com/robhowe-a/winhttprequest/releases`,
                `i.e`,
                `identity`,
                `iis`,
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
                `isbn`,
                `it`,
                `javascript`,
                `jira`,
                `jira's`,
                `json`,
                `kanban`,
                `lake`,
                `language`,
                `library`,
                `linux`,
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
                `model-view-controller`,
                `multi-threading`,
                `multiple`,
                `mvc`,
                `mysql`,
                `name`,
                `nasa's`,
                `news`,
                `nextjs`,
                `oauth2`,
                `object`,
                `one`,
                `online`,
                `ood`,
                `oop`,
                `open-source`,
                `open-source::github`,
                `operation`,
                `options`,
                `options`,
                `oriented`,
                `page`,
                `password`,
                `passwords`,
                `patch`,
                `payment`,
                `pci`,
                `pi`,
                `picture`,
                `picture-of-the-day`,
                `post`,
                `privacy`,
                `production`,
                `products`,
                `products'`,
                `programmed`,
                `programming`,
                `projects`,
                `provides`,
                `providing`,
                `public`,
                `private`,
                `put`,
                `python`,
                `quiz`,
                `quizzes`,
                `random`,
                `randomwebbits`,
                `raspberry`,
                `react`,
                `read`,
                `refining`,
                `releases`,
                `request`,
                `requirements`,
                `responsibility`,
                `responsive`,
                `rest`,
                `rest,cru/ed,api,json`,
                `rest+cru/ed+api`,
                `rest+json`,
                `result`,
                `retrieved`,
                `retrieves`,
                `robert`,
                `roberthowell.dev`,
                `robhowe-a`,
                `sample`,
                `scan`,
                `scanned`,
                `scanner`,
                `scanning`,
                `science`,
                `score`,
                `script`,
                `scrum`,
                `search`,
                `searching`,
                `secure`,
                `security`,
                `selection`,
                `sends`,
                `sensitive`,
                `server`,
                `services`,
                `set`,
                `setting`,
                `showcase`,
                `showcasing`,
                `side`,
                `simply`,
                `single`,
                `site`,
                `software`,
                `solution`,
                `sources`,
                `spaceflight`,
                `speed`,
                `sphinx`,
                `sprint`,
                `sprints`,
                `sql`,
                `standard`,
                `started`,
                `status`,
                `storage`,
                `store`,
                `stores`,
                `storing`,
                `strength`,
                `successful`,
                `system`,
                `desktop`,
                `tables`,
                `tasks`,
                `team`,
                `teams`,
                `techniques`,
                `template`,
                `test`,
                `tested`,
                `this`,
                `thread`,
                `three`,
                `throughout`,
                `tile`,
                `timeline`,
                `triad`,
                `typescript`,
                `typing`,
                `ubuntu`,
                `universal`,
                `update/edit`,
                `url-specific`,
                `user`,
                `user's`,
                `users`,
                `using`,
                `various`,
                `varying`,
                `via`,
                `view`,
                `viewing`,
                `vue`,
                `web`,
                `websites`,
                `websockets`,
                `week`,
                `weekly`,
                `where`,
                `widget`,
                `windows`,
                `winhttprequest`,
                `wireframe`,
                `with`,
                `wordpress`,
                `workload`,
                `xaml/forms/wpf`
            ];

            if (!words.includes(message)) return;
            count = window.main.domSearch.highlight(message);

            dotNetRef.invokeMethodAsync("ReceiveCount", count);
        })

            return count;
    }
}
