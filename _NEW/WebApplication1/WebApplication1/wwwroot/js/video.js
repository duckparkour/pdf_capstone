$(document).ready(function(){

    var trailer = "?enablejsapi=1";
    $(".main").on('click', '#youtube-search-button', function (e) {
        e.preventDefault();
        let source = $('#youtube-search-bar').val();
        if(source.startsWith("https://www.youtube.com/embed/")){
            $('#youtube-video').attr('src',source+trailer);
        }else{
            alert("Please enter a valid embed URL.");
        }
        
    })
});