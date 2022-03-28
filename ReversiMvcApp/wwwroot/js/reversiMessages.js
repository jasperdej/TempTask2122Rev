"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/reversiHub").build();

connection.on("ReceiveUpdate", function (message) {
    if (message == "Zet gedaan") {
        Game.Reversi.refreshBoard();
    } else if (message == "Game over") {
        Game.Reversi.returnToSpellen();
	}
});

connection.start().then(function () {
    document.getElementById("overlayWait").style.display = "block";
    connection.invoke("SendPresent").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

function sendReversiUpdate(message) {
    connection.invoke("SendUpdate", message).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceivePresent", function () {
    document.getElementById("overlayWait").style.display = "none";
    connection.invoke("SendPresent2").catch(function (err) {
        return console.error(err.toString());
    });
});

connection.on("ReceivePresent2", function () {
    document.getElementById("overlayWait").style.display = "none";
});