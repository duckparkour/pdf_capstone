$(document).ready(function(){

    var trailer = "?enablejsapi=1";
    $(".main").on('click', '#youtube-search-button', function (e) {
        e.preventDefault();
        let source = $('#youtube-search-bar').val();
        //player.loadVideoById(source);
        $('#youtube-video').attr('src',source+trailer);
    })
});