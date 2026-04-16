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
        
                for (const node of nodes) {
                    const text = node.nodeValue;
                    const index = text.toLowerCase().indexOf(lowerQuery);
        
                    if (index >= 0) {
                        const mark = document.createElement("mark");
                        mark.className = "search-highlight";
                        mark.textContent = text.substr(index, query.length);
        
                        const after = document.createTextNode(text.substr(index + query.length));
                        const before = document.createTextNode(text.substr(0, index));
        
                        const parent = node.parentNode;
                        if (!parent) continue;
        
                        parent.replaceChild(after, node);
                        parent.insertBefore(mark, after);
                        parent.insertBefore(before, mark);
        
                        count++;
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
            console.log(message);
             count = window.main.domSearch.highlight(message);
            console.log(count);
            dotNetRef.invokeMethodAsync("ReceiveCount", count);
        })

            return count;
    }
}