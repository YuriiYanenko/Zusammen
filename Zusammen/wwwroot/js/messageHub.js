
let message = document.getElementById("message");
let userName = getUserName();

let chatConnection = new signalR.HubConnectionBuilder().withUrl("/Video/Room/chat").build();

document.getElementById("send").disabled = true;

chatConnection.on("RecieveMessage", function(userName, message){
    $("#chat-messages").append("<span style='color:black;'>" + userName + ': ' + message + "</br>"+"</span>");    
});

chatConnection.start().then(function () {
    document.getElementById("send").disabled = false;
    chatConnection.invoke("JoinRoom", modelData.id).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
}); 

$("#send").click(function(){
    console.log(message.value);
    chatConnection.invoke("SendMessage", parseInt(modelData.id), userName, message.value);
});