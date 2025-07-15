

window.jsKeyHandler = null;

window.JsFunctions = {
    addKeyboardListenerEvent: function (handler) {
        window.jsKeyHandler = handler;
        window.document.addEventListener('keydown', this.handleKeyInput);
    },
    removeKeyboardListenerEvent: function () {
        window.document.removeEventListener('keydown', this.handleKeyInput);
        window.jsKeyHandler = null;
    },
    handleKeyInput: function (e) {
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
            window.jsKeyHandler.invokeMethodAsync("KeyDown", args);
        }
    },
};