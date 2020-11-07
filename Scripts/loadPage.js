
var page = location.href;
if (page == "https://localhost:44374/") 
{
    $("#layout").hide(); 
    setTimeout(function () {
        document.getElementById("loader_style").disabled = true; 
        $("#load_span").hide();
        $("#layout").show();
    }, 1000);
}