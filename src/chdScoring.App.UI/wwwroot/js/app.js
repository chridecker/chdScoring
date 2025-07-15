

window.JsFunctions = {
    addKeyboardListenerEvent: function (handler) {
        window.document.addEventListener('keydown', (e)=> keyDown(handler,e));
    },
    removeKeyboardListenerEvent: function () {
        window.document.removeEventListener('keydown', (e)=> keyDown(handler,e));
    }
};

function keyDown(handler,e) {
    if (e) {
        var args = {
            key: e.key,
            code: e.keyCode.toString(),
            location: e.location,
            repeat: e.repeat,
            ctrlKey: e.ctrlKey,
            shiftKey: e.shiftKey,
            altKey: e.altKey,
            metaKey: e.metaKey,
            type: e.type
        };
    handler.invokeMethodAsync("KeyDown", args);
    }
    
}