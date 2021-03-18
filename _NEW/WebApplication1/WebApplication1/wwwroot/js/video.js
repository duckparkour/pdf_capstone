$(document).ready(function(){

    var trailer = "?enablejsapi=1";
    // This code loads the IFrame Player API code asynchronously.
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
    //This function creates an <iframe> (and YouTube player)after the API code downloads.
    var player;
    function onYouTubeIframeAPIReady() {
        player = new YT.Player('#youtube-video', {
            events: {
                'onReady': onPlayerReady,
                'onStateChange': onPlayerStateChange
            }
        });
    }
    //Launch video as it's ready
    function onPlayerReady(event) {
        event.target.playVideo();
    }
    $(".main").on('click', '#youtube-search-button', function (e) {
        e.preventDefault();
        let source = $('#youtube-search-bar').val();
        //player.loadVideoById(source);
        $('#youtube-video').attr('src',source+trailer);
    })
});