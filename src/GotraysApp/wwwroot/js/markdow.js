window.MasaBlazor.extendMarkdownIt = function (parser) {
    
    const { md, scope } = parser;
    if (window.markdownitEmoji) {
        md.use(window.markdownitEmoji);
        
    }

    if (scope === "document") {
        md.renderer.rules.code_block = renderCode(md.renderer.rules.code_block, md.options);
        md.renderer.rules.fence = renderCode(md.renderer.rules.fence, md.options);
    }
}
function renderCode(origRule, options) {
    return (...args) => {
        const [tokens, idx] = args;
        const content = tokens[idx].content
            .replaceAll('"', '&quot;')
            .replaceAll("'", "&lt;");
        const origRendered = origRule(...args);
        
        if (content.length === 0)
            return origRendered;

        const id = Math.random().toString(36).substr(3);
        
        const encodedContent = encodeURIComponent(tokens[idx].content);
        
        const render = `
<div style="position: relative">
	${origRendered}
	<button class="markdown-it-code-copy " id="${id}" data-content="${encodedContent}"  onclick="copy('${id}')"
     style="position: absolute; top: 7.5px; right: 6px; cursor: pointer; outline: none;" >
		<span style="font-size: 21px; opacity: 0.4;" class="mdi mdi-content-copy"></span>
	</button>
</div>
`;

        return render;
    };
}

window.copy = (id)=>{
    const button = document.getElementById(id);
    const content = decodeURIComponent(button.getAttribute('data-content'));
    navigator.clipboard.writeText(base64Decode(content));
}
