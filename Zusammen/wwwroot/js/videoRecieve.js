"use strict";
document.getElementById("hidded").hidden=true;
var connection = new signalR.HubConnectionBuilder().withUrl("/Video/Room/video").build({ skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets});

document.getElementById("start").disabled = true;

connection.on("RecieveVideo", function (videoPath){
    let film = document.getElementById("film-video");
  
    film.setAttribute("src", videoPath);
});
connection.start().then(function (){
    document.getElementById("start").disabled = false;  
}).catch(function (err) {
    return console.error(err.toString());
});
    
document.getElementById("start").addEventListener("click", function (event){
    let path = document.getElementById("hidded").value;
    connection.invoke("SendVideo", path).catch(function (err){
        
       return console.error(err.toString()); 
    });
    event.preventDefault();
});