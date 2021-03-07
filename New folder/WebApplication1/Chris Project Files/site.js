$(document).ready(function () {
    let mediaRecorder;
    let audioChunks;

    //Video Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $("#video-icon").click(function (e) {
        $('.main').empty();
        $('.main').append(`<h1 id="youtube-title"><i class="fab fa-youtube"></i>Youtube Playback Function </h1>
            <input type="text" name="youtube-input" id="youtube-search-bar" placeholder="Enter a youtube embedded url">
            <button id="youtube-search-button">Go!</button>
            <div class="video-container">
                <iframe id="#youtube-video"width="860" height="615" src="https://www.youtube.com/embed/Oa9QeMHNOm4" frameborder="0" allow="accelerometer;
                    autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
            </div>`);
    });

    $(".main").on('click', '#youtube-search-button', function (e) {
        e.preventDefault();
        let source = $('#youtube-search-bar').val()
        let x = $('#youtube-video').attr('src')
        console.log(x)

    })

    //Audio Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $('#audio-icon').click(function (e) {
        e.preventDefault();
        $('.main').empty();

        let audioControlsDiv = $(`<div></div>`)
        let startButton = `<button id="start-recording-button">Start Recording</button>`
        let stopButton = `<button id="stop-recording-button">Stop Recording</button>`
        let openRecordingButton = `<button id="open-recording-button">Open a Recording</button>`
        let mainAudioControl = `<audio id="recording-controller" controls><source src="#" type="audio/mp3">Audio not supported</audio>`
        let recordingStatusText = `<h1 id="recording-status-text">Press "Start Recording" To Begin.</h1>`

        $(audioControlsDiv).append(startButton, stopButton, openRecordingButton)
        $('.main').append(audioControlsDiv, recordingStatusText, mainAudioControl);
    });

    //The click handler has to be binded this way since it does not exist until the audio button is selected from the media tab.
    $('.main').on("click", "#start-recording-button", function (e) {
        e.preventDefault()
        $('#recording-status-text').text("Recording In Progress...");

        navigator.mediaDevices.getUserMedia({ audio: true })
            .then(stream => {
                mediaRecorder = new MediaRecorder(stream);
                mediaRecorder.start();

                audioChunks = [];

                mediaRecorder.addEventListener("dataavailable", event => {
                    audioChunks.push(event.data);
                });

                mediaRecorder.addEventListener("stop", () => {
                    const audioBlob = new Blob(audioChunks);
                    const blobToSave = convertBlobToMp4(audioBlob, "Blobbie")
                    const audioUrl = URL.createObjectURL(audioBlob);

                    let audio = document.querySelector('audio');
                    audio.src = audioUrl

                    console.log(blobToSave)
                });
            })
    })

    $('.main').on("click", "#stop-recording-button", function (e) {
        e.preventDefault();
        mediaRecorder.stop();
        $('#recording-status-text').text("Recording Completed.")

    })

    $('.main').on("click", "#open-recording-button", function (e) {
        e.preventDefault();
        console.log("hello")
    })

    //PDF Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $('#file-icon').click(function (e) {
        e.preventDefault();
        $('.main').empty();
        $('.main').append(`<iframe src="https://pdf-lib.js.org/assets/with_update_sections.pdf"
            frameborder="1" width="100%" height="100%"></iframe>`);
    });

    //ETC////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $('#upload-button').click(function (e) {
        let $element = `<button id="modal-btn"> click me, I make a modal</button>
            <div class="modal">
                <div class="modal-content">
                    <span class="close-btn">&times;</span>
                    <p>this is the text inside the modal</p>
                </div>
            </div>`
        $('body').append($element).show();
    })
})

//Helper functions///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function convertBlobToMp4(blob, fileName) {
    blob.lastModifiedDate = new Date();
    blob.name = fileName;
    blob.type = "audio/mp3";

    return blob;
}