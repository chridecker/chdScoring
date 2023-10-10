﻿window.JsFunctions = {
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

        // window.document.addEventListener('onkeydown', function (e) { // Original error
        window.document.addEventListener('keydown', function (e) {
            handler.invokeMethodAsync("OnKeyDown",serializeEvent(e));
            //DotNet.invokeMethodAsync('chdScoring.Client', 'JsKeyDown', serializeEvent(e), handler)
        });
    },
    removeKeyboardListenerEvent: function (foo) {
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

        // window.document.addEventListener('onkeydown', function (e) { // Original error
        window.document.removeEventListener('keydown', function (e) {});
    }
};