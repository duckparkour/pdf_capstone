// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$(document).ready(function () {

    $("#video-icon").click(function (e) {
        $('.main').empty();
        $('.main').append(` <h1 id="youtube-title"><i class="fab fa-youtube"></i>Youtube Playback Function </h1>
            <input type="text" name="youtube-input" id="youtube-search-bar" placeholder="Enter a youtube">
            <button id="youtube-search-button">Search!</button>
            <div class="video-container">
                <iframe width="860" height="615" src="https://www.youtube.com/embed/Oa9QeMHNOm4" frameborder="0" allow="accelerometer; 
                    autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
            </div>`);
    });

    $('#file-icon').click(function (e) {
        e.preventDefault();
        console.log('hello');
        $('.main').empty();
        $('.main').append(`<iframe src="https://pdf-lib.js.org/assets/with_update_sections.pdf" 
            frameborder="1" width="100%" height="100%"></iframe>`);
    });
});
