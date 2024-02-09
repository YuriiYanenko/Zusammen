
let message = document.getElementById("message");
let userName = getUserName();

let chatConnection = new signalR.HubConnectionBuilder().withUrl("/Video/Room/chat").build();

document.getElementById("send").disabled = true;
chatConnection.on("RecieveMessage", function(randomName, message){
if (!userName) 
{
    var alphabet = 'abcdefghijklmnopqrstuvwxyz';
    var result = '';
    
    for (var i = 0; i < 5; i++) 
    {
        var randomIndex = Math.floor(Math.random() * alphabet.length);
        var randomLetter = alphabet.charAt(randomIndex);
        result += randomLetter;
    }
        $("#chat-messages").append("<span style='color:black;'>" + randomName + ': ' + message + "</br>"+"</span>");
   
} 
else 
{
        $("#chat-messages").append("<span style='color:black;'>" + userName + ': ' + message + "</br>"+"</span>");
    
}
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
    message.value="";
});
function getRandomLetters() {
    var alphabet = 'abcdefghijklmnopqrstuvwxyz';
    var result = '';


    for (var i = 0; i < 5; i++) {
        var randomIndex = Math.floor(Math.random() * alphabet.length);
        var randomLetter = alphabet.charAt(randomIndex);
        result += randomLetter;
    }

    document.getElementById('output').textContent = result;
}