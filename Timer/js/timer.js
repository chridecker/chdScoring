
async function startHubConnection(connection) {
    connection.on("ReceiveFlightData", function (dto) {
        var container = document.querySelector("#timer-container");
        var pauseContainer = container.querySelector(".pause-container");
        var timerContainer = container.querySelector(".timer-container");

        pauseContainer.classList.add("hide");
        timerContainer.classList.add("hide");

        if(dto.pilot == null || dto.pilot == "" ||dto.pilot == undefined){
            pauseContainer.classList.remove("hide");
            
        } else {
            timerContainer.classList.remove("hide");
            var pilotElement = timerContainer.querySelector(".pilot");
            pilotElement.querySelector(".number").innerHTML = dto.pilot.id;
            pilotElement.querySelector(".name").innerHTML = dto.pilot.name;

            var countryImageElement = pilotElement.querySelector(".country .custom-image img");
            countryImageElement.src = dto.pilot.countryImage.src;

            var timeElement = timerContainer.querySelector(".time");

            var stopContainer = timeElement.querySelector(".icon .fa-hand");
            var departureContainer = timeElement.querySelector(".icon .fa-plane-departure");
            var slashContainer = timeElement.querySelector(".icon .fa-plane-slash");
            var pauseContainer = timeElement.querySelector(".icon .fa-play-pause");

            if(dto.leftTime == null || dto.leftTime == "" || dto.leftTime == undefined){
                stopContainer.classList.remove("hide");
                departureContainer.classList.add("hide");
                slashContainer.classList.add("hide");
                pauseContainer.classList.add("hide");
                timeElement.querySelector(".left-time").innerHTML = "00:00";
                return;
            }
            stopContainer.classList.add("hide");
            var leftTime = parseTimeSpan(dto.leftTime);
            var roundTime = parseTimeSpan(dto.round.time);
            if(leftTime <= roundTime){
                pauseContainer.classList.add("hide");
                if(leftTime > 0){
                    departureContainer.classList.remove("hide");
                    slashContainer.classList.add("hide");
                }
                else {
                    departureContainer.classList.add("hide");
                    slashContainer.classList.remove("hide");
                }
            }
            else {
                departureContainer.classList.add("hide");
                slashContainer.classList.add("hide");
                pauseContainer.classList.remove("hide");
            }
            timeElement.querySelector(".left-time").innerHTML = formatMMSS(leftTime);
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
