const pathElement = document.getElementById("path");
const roomElement = document.getElementById("roomId");
pathElement.hidden = true;
roomElement.hidden = true;

var connection = new signalR.HubConnectionBuilder().withUrl("/Video/Room/video").build();

document.getElementById("start").disabled = true;

connection.on("ReceiveVideo", function (videoPath) {
    let film = document.getElementById("film-video");
    film.setAttribute("src", videoPath);
});

connection.start().then(function () {
    console.log("Connection started successfully.");
    document.getElementById("start").disabled = false;
    connection.invoke("JoinRoom", parseInt(roomElement.value)).catch(function (err) {
        console.error("Error invoking JoinRoom: " + err.toString());
    });
}).catch(function (err) {
    console.error("Error starting connection: " + err);
});

document.getElementById("start").addEventListener("click", function (event) {
    let path = pathElement.value;
    let id= parseInt(roomElement.value);
    connection.invoke("SendVideo", id, path).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Add an event listener to leave the room when the page is closed or navigated away
window.addEventListener("beforeunload", function () {
    connection.invoke("LeaveRoom", roomElement.value).catch(function (err) {
        return console.error(err.toString());
    });
});
