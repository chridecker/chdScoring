const address = new URL(window.location.href);
const apiKey = address.searchParams.get("apiKey");
const server = address.searchParams.get("server");
const url = "http://" + server + ":8081/chdscoring/";
let name = address.searchParams.get("name");
if (name == null || name == "") {
    name = "Web View";
}

const deviceConnection = new signalR.HubConnectionBuilder()
    .withUrl(url + "device-hub", {
        accessTokenFactory: () => apiKey,
        transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

deviceConnection.onclose(async () => {
    await startDeviceHubConnection(deviceConnection, apiKey, name);
});

const connection = new signalR.HubConnectionBuilder()
    .withUrl(url + "flight-hub", {
        accessTokenFactory: () => apiKey,
        transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

connection.onclose(async () => {
    await startHubConnection(connection);
});

// Start the connection.
startDeviceHubConnection(deviceConnection, apiKey, name);
startHubConnection(connection);