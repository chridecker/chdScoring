// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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
            connection.invoke("RegisterAsControlCenter");
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