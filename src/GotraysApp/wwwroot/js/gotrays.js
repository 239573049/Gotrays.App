function scrollTo(id) {
    var dom = document.getElementById(id)
    if (dom) {
        dom.scrollTo(0, dom.scrollHeight);
    }
}

export {
    scrollTo
}