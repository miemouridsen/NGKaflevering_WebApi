var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

document.getElementById("subscribe").addEventListener("click", Subscribe);

function Subscribe() {
    document.getElementById("hasSubscribed").innerHTML = "You have already subscribed";
    connection.start().then(function () {
        console.log("connection succeded");
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceiveNotification", function (message) {
    document.getElementById("datentime").innerHTML = "Date and time: " + message.dateNTime;
    document.getElementById("temp").innerHTML = "Temperature: " + message.temperature;
    document.getElementById("airmos").innerHTML = "Air Moisture: " + message.airMoisture;
    document.getElementById("airpres").innerHTML = "Air Pressure: " + message.airPressure;
    document.getElementById("location").innerHTML = "Location: " + message.location.name +
        " (" + message.location.latitude + ", " + message.location.longitude + ")";
    console.log(message);
});
