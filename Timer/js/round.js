
async function startHubConnection(connection) {
    connection.start()
        .then(() => {
            connection.on("ReceiveRoundData", function (lst) {
                const roundInfo = document.querySelector(".round .info");
                roundInfo.innerHTML = "Round " + lst[0].round;
                const results = document.querySelector(".results");
                results.innerHTML = '';
                lst.forEach(res => {
                    results.appendChild(createResult(res))
                });
            });
        })
        .catch(function (err) {
            setTimeout(() => startHubConnection(connection), 5000);
            return console.error(err.toString());
        });
}

function createResult(dto) {
    const row = createElementWithClass("result-row");
    const image = createElementWithClass("custom-image");
    const img = document.createElement("img");
    img.src = dto.pilot.countryImage.src;

    const rank = createElementWithClass("rank",dto.rank);

    const pilot = createElementWithClass("pilot", dto.pilot.name);

    const country = createElementWithClass("country", dto.pilot.countryCode.toUpperCase());

    const score = createElementWithClass("score",dto.score);

    const scoreProm = createElementWithClass("score", dto.scoreProm);
    scoreProm.classList.add("prom");

    image.appendChild(img);
    row.appendChild(image);
    row.appendChild(rank);
    row.appendChild(pilot);
    row.appendChild(country);
    row.appendChild(score);
    row.appendChild(scoreProm);

    return row;
}

function createElementWithClass(className, innerHtml = '') {
    let el = document.createElement("div");
    el.classList.add(className);
    el.innerHTML = innerHtml;
    return el;
}