
var fontAwesomeFiles = {
    0: 'css/fontawesome/solid.svg',
    1: 'css/fontawesome/regular.svg',
    2: 'css/fontawesome/light.svg',
    3: 'css/fontawesome/thin.svg',
    4: 'css/fontawesome/sharp-solid.svg',
    5: 'css/fontawesome/sharp-regular.svg',
    6: 'css/fontawesome/sharp-light.svg',
    7: 'css/fontawesome/sharp-thin.svg',
    8: 'css/fontawesome/duotone.svg',
    9: 'css/fontawesome/sharp-duotone-solid.svg',
    10: 'css/fontawesome/brands.svg',
};

var xmlFiles = {};

loadXMLDocuments();

function loadXMLDocuments() {
    for (var style in fontAwesomeFiles) {
        try {
            loadXMLDocument(style);
        }
        catch (err) {
            console.error('Failed to load style ' + style + ': ' + err.message);
        }
    }
}

function loadXMLDocument(style) {
    var file = fontAwesomeFiles[style];

    if (typeof file == "undefined") { throw new Error('Icon Typ ' + style + ' nicht definiert'); }

    const xhr = new XMLHttpRequest();

    xhr.open('GET', file, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            xmlFiles[style] = xhr.responseXML;
        }
    };

    xhr.send();
}

function loadSpecificIcon(iconName, node, style) {
    var xmlDocument = xmlFiles[style];

    if (!xmlDocument) {
        setTimeout(function () { loadSpecificIcon(iconName, node, style); }, 100);
        return;
    }

    var content = xmlDocument.getElementById(iconName);
    if (typeof content === 'undefined' || content === null) {
        content = xmlFiles[0].getElementById(iconName);
        node.setAttribute('fontawesome-style-not-found', '');
    }

    var path = content.innerHTML;

    var viewBox = content.getAttribute('viewBox');

    node.setAttribute('viewBox', viewBox);
    node.setAttribute('fontawesome-icon', iconName);
    node.setAttribute('fontawesome-style', style);
    node.firstElementChild.outerHTML = path;
}

var targetNode = document.body;
var config = { childList: true, subtree: true };

var callback = function (mutationsList, observer) {
    for (var mutation of mutationsList) {
        if (mutation.type !== 'childList') { return; }

        mutation.addedNodes.forEach(function (addedNode) {
            if (addedNode.nodeName.toLowerCase() === 'svg' && addedNode.firstElementChild.nodeName.toLowerCase() === 'use') {

                var icon = addedNode.firstElementChild.getAttribute('href');
                if (!icon.startsWith('#')) { return; }

                var style = addedNode.firstElementChild.getAttribute('fontawesome-style');

                loadSpecificIcon(icon.replace('#', ''), addedNode, style);
            }
        });
    }
};

var observer = new MutationObserver(callback);
observer.observe(targetNode, config);