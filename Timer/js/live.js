
async function startHubConnection(connection) {
    connection.on("ReceiveFlightData", function (dto) {
        var container = document.querySelector("#timer-container");
        var pauseContainer = container.querySelector(".pause-container");
        var controlCenter = container.querySelector(".control-center");

        pauseContainer.classList.add("hide");
        controlCenter.classList.add("hide");

        if(dto.pilot == null || dto.pilot == "" ||dto.pilot == undefined){
            pauseContainer.classList.remove("hide");
            
        } else {
            controlCenter.classList.remove("hide");
            var pilotElement = controlCenter.querySelector(".pilot-data");
            pilotElement.querySelector(".name").innerHTML =   dto.pilot.id + " " + dto.pilot.name;
            
            var countryImageElement = pilotElement.querySelector(".country .custom-image img");
            countryImageElement.src = dto.pilot.countryImage.src;
            
            var timeElement = controlCenter.querySelector(".left-time");
            timeElement.innerHTML = formatMMSS(parseTimeSpan(dto.leftTime));

            var maneouvresElement = controlCenter.querySelector(".maneouvres");
            maneouvresElement.innerHTML = "";
            Object.values(dto.maneouvreLst)[0].forEach(maneuver => {
                var maneuverElement = document.createElement("div");
                maneuverElement.classList.add("maneouvre");
                var figurIdElement = document.createElement("div");
                figurIdElement.classList.add("figur-id");
                figurIdElement.innerHTML = maneuver.id.toString().padStart(2, "0");

                var figurElement = document.createElement("div");
                figurElement.classList.add("figur");
                figurElement.innerHTML = maneuver.name + " (" + maneuver.value + ")";
                maneuverElement.appendChild(figurElement);
                maneuverElement.appendChild(figurIdElement);
                maneuverElement.appendChild(figurElement);
                maneouvresElement.appendChild(maneuverElement);
            

                var judgesScoresElement = document.createElement("div");
                judgesScoresElement.classList.add("judge-scores");
                judgesScoresElement.dataset.judgesCount = dto.judges.length;

                dto.judges.forEach(judge => {
                    var judgeScoreElement = document.createElement("div");
                    judgeScoreElement.classList.add("judge-score");
                    var scoreElement = document.createElement("div");
                    scoreElement.classList.add("score-raw");

                    var scoreValue = Object.values(dto.maneouvreLst)[judge.id-1][maneuver.id-1].score;
                    if(scoreValue != null && scoreValue != undefined){
                        scoreElement.innerHTML = scoreValue;
                    }
                    judgeScoreElement.appendChild(scoreElement);

                    judgesScoresElement.appendChild(judgeScoreElement);
                });
                maneuverElement.appendChild(judgesScoresElement);
            },0);
                

               

               

        }
    });
    
    connection.start()
        .then(() => {
            //connection.invoke("RegisterAsControlCenter");
            //hideErrorModal();
        })
        .catch(function (err) {
            setTimeout(() => startHubConnection(connection), 5000);
            return console.error(err.toString());
        });
    }
function parseTimeSpan(timeSpan) {
    var pre = 1;
    if (!timeSpan) {
        return 0;
    }
    if(timeSpan.startsWith("-")){
        return 0;
    }
    let [time, fraction = "0"] = timeSpan.split(".");
    const [hours, minutes, seconds] = time.split(":").map(Number);

    return (hours * 3600000 +
        minutes * 60000 +
        seconds * 1000
    );
}
function formatMMSS(milliseconds) {
    const totalSeconds = Math.floor(milliseconds / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;

    return `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`;
}
