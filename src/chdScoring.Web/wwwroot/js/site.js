async function startDeviceHubConnection(connection, apiKey) {

    connection.start()
        .then(() => {
            connection.on("NavigateTo", function (dto, redirect) {
                let mode = 'RenderTimer';
                if (redirect == 'controlcenter') {
                    mode = 'live';
                }
                else if (redirect == 'pilots') {
                    mode = 'round';
                }
                const url = new URL(window.location.href);
                url.searchParams.set("mode", mode);
                window.history.pushState({}, "", url);
                window.location.reload();
            });
            connection.on("ToggleTheme", function (dto) {
                const url = new URL(window.location.href);
                let theme = url.searchParams.get("colorScheme");
                if (theme === undefined || theme == null) {
                    theme = 'dark-mode';
                }
                else if (theme == 'dark-mode') {
                    theme = 'light-mode';
                }
                url.searchParams.set("colorScheme", theme);

                window.history.pushState({}, "", url);
                 window.location.reload();
            });
            connection.on("ShutDown", function (dto,restart) {
                 window.location.reload();
            });
            connection.invoke("RegisterDeviceAsync", {
                apiKey: apiKey,
                hostName: 'Web View'
            });
            hideErrorModal();
        })
        .catch(function (err) {
            showErrorModal(err);
            setTimeout(() => startDeviceHubConnection(connection, apiKey), 5000);
            return console.error(err.toString());
        });
}


async function startHubConnection(connection) {
    if (mode == 'Round') {
        connection.on("ReceiveRoundData", function (lst) {
            var x = {
                dtos: lst
            };
            fetch("/Index?handler=RenderRoundResult", {
                method: "Post",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(x)
            })
                .then(res => {
                    if (!res.ok) throw new Error("Fehler beim POST");
                    return res.text(); // weil Partial View HTML ist
                })
                .then(html => {
                    document.getElementById("timer-container").innerHTML = html;
                })
                .catch(err => {
                    console.error("Fehler:", err);
                });
        });
    }
    else {
        connection.on("ReceiveFlightData", function (dto) {
            fetch("/Index?handler=" + render, {
                method: "Post",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(dto)
            })
                .then(res => {
                    if (!res.ok) throw new Error("Fehler beim POST");
                    return res.text(); // weil Partial View HTML ist
                })
                .then(html => {
                    document.getElementById("timer-container").innerHTML = html;
                })
                .catch(err => {
                    console.error("Fehler:", err);
                });
        });
    }
    connection.start()
        .then(() => {
            hideErrorModal();
        })
        .catch(function (err) {
            showErrorModal(err);

            setTimeout(() => startHubConnection(connection), 5000);
            return console.error(err.toString());
        });
}

function showErrorModal(message) {
    var dialog = document.getElementById("error-dialog");
    var dialogContent = document.getElementById("error-dialog-content");
    dialogContent.innerHTML = message;
    dialog.showModal();
}

function hideErrorModal() {
    var dialog = document.getElementById("error-dialog");
    dialog.close();
}