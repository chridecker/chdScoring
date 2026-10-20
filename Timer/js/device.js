
async function startDeviceHubConnection(connection, apiKey, name) {
    connection.start()
        .then(() => {
            connection.on("NavigateTo", function (dto, redirect) {
                let goTo = window.location.href;
                if (redirect == 'controlcenter') {
                    goTo = window.location.href.replace("index", "live").replace("round", "live");
                }
                else if (redirect == 'pilots') {
                    goTo = window.location.href.replace("index", "round").replace("live", "round");
                }
                else {
                    goTo = window.location.href.replace("live", "index").replace("round", "index");
                }
                window.location.href = goTo;
            });
            connection.on("SetZoom", function (dto, zoom) {
                document.body.style.zoom = zoom + "%";
            });
            connection.on("ToggleTheme", function (dto) {
                document.querySelector("html").classList.toggle("light-mode");
            });
            connection.on("ShutDown", function (dto, restart) {
                window.location.reload();
            });
            connection.invoke("RegisterDeviceAsync",
                {
                    apiKey: apiKey,
                    hostName: name
                });
        })
        .catch(function (err) {
            setTimeout(() => startDeviceHubConnection(connection, apiKey, name), 5000);
            return console.error(err.toString());
        });
}