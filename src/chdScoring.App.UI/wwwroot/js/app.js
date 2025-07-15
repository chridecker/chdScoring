window.JsFunctions = {
    addKeyboardListenerEvent: function (handler) {
        let serializeEvent = function (e) {
            if (e) {
                return {
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
            }
        };

        window.document.addEventListener('keydown', keyDown);
        //window.document.addEventListener('keydown', function (e) {
        //    handler.invokeMethodAsync("OnKeyDown", serializeEvent(e));
        //});
    },
    removeKeyboardListenerEvent: function () {
        window.document.removeEventListener('keydown', keyDown);
        //window.document.removeEventListener('keydown', function (e) { });
    }
};

function keyDown(e) {
    alert(e);
    handler.invokeMethodAsync("OnKeyDown", serializeEvent(e));
}