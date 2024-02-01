function openFileExplorer() 
{
    document.getElementById('fileInput').click();
}

function previewImage() 
{
    var fileInput = document.getElementById('fileInput');
    var uploadedImageContainer = document.getElementById('uploadedImageContainer');
    var uploadedImage = document.getElementById('uploadedImage');

    var file = fileInput.files[0];
    var reader = new FileReader();

    reader.onload = function (e) 
    {
        uploadedImage.src = e.target.result;
        uploadedImageContainer.style.background = 'none';
    };

    if (file) 
    {
        reader.readAsDataURL(file);
    }
}

var user = getUserName();
function saveChanges()
{
    var nickname = $("input[name='name']").val();
    var about = $("input[name='text']").val();
    
    var data = 
        {
        name: nickname,
        about: about
    };
    
    $.ajax({
        url: '/ZusammenDb/UpdateUser', 
        type: 'POST',
        data: {data: data, userName:user},
        success: function (response) {
            open("/Home/UserProfil/");
            console.log(response);  
        }
    });
}