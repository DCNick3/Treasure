
window.KladWeb = {}

window.KladWeb.ScrollElementIntoView = function(element, alignToTop) {
    element.scrollIntoView(alignToTop);
};

window.KladWeb.WaitForImageLoadAsync = function(img) {
    return new Promise(resolve => {
        img.onload = () => {
            resolve(true);
        }
        if (img.complete)
            resolve(true);
    });
}

window.KladWeb.CopyToClipboard = function(text) {
    var el = document.createElement('textarea');
    el.value = text;
    el.setAttribute('readonly', '');
    el.style.position = 'absolute';
    el.style.left = '-9999px';
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
}
