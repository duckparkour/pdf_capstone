$(document).ready(function () {
  let mediaRecorder;
  let audioChunks;

  //PDF Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  $("#file-button").click(function (e) {
    e.preventDefault();
    $(".main").empty();
    if ($("#toolkit3").is(":hidden")) {
      $("#toolkit3").show();
    }
    $(".main").append(`<iframe src="" id="frame"
            frameborder="1" width="100%" height="100%"></iframe>`);
  });

  $("#save-pdf-button").click(function (e) {
    showModal("save-pdf");
  });

  $("#open-pdf-button").click(function (e) {
    showModal("open-pdf");
  });

  //Video Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  $("#video-button").click(function (e) {
    $(".main").empty();
    $("#toolkit3").hide();

    $(
      ".main"
    ).append(`<h1 id="youtube-title"><i class="fab fa-youtube"></i>Youtube Playback Function </h1>
            <input type="text" name="youtube-input" id="youtube-search-bar" placeholder="Enter a youtube embedded url">
            <button id="youtube-search-button">Go!</button>
            <div class="video-container">
                <iframe id="youtube-video"width="860" height="615" src="https://www.youtube.com/embed/Oa9QeMHNOm4?enablejsapi=1" frameborder="0" allow="accelerometer;
                    autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
            </div>`);
  });

  $(".main").on("click", "#youtube-search-button", function (e) {
    e.preventDefault();
    let source = $("#youtube-search-bar").val();
    let x = $("#youtube-video").attr("src");
    console.log(x);
  });

  //Audio Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  $("#audio-button").click(function (e) {
    $(".main").empty();
    $("#toolbar3").hide();

    let audioControlsDiv = $(`<div></div>`);
    let startButton = `<button id="start-recording-button"><i id='start-recording-icon' class="fas fa-circle"></i><h4>Start Recording</h4></button>`;
    let stopButton = `<button id="stop-recording-button"><i id='stop-recording-icon' class="fas fa-square"></i><h4>Stop Recording</h4></button>`;
    let openRecordingButton = `<button id="open-recording-button"><i id='open-recording-icon' class="fas fa-file-audio"></i><h4>Open a Recording</h4></button>`;
    let mainAudioControl = `<audio id="recording-controller" controls><source src="#" type="audio/mp3">Audio not supported</audio>`;
    let saveAudioButton = `<button id="save-audio-button"><i class="fas fa-save" id="save-recording-icon"></i><h4>Save</h4></button>`;
    let recordingStatusText = `<h1 id="recording-status-text">Press "Start Recording" To Begin.</h1>`;

    $(audioControlsDiv).append(startButton, stopButton, openRecordingButton);
    $(audioControlsDiv).addClass("recording-buttons-container");

    $(".main").append(
      audioControlsDiv,
      recordingStatusText,
      mainAudioControl,
      saveAudioButton
    );
    $("#save-audio-button").hide();
  });

  //The click handler has to be binded this way since it does not exist until the audio button is selected from the media tab.
  $(".main").on("click", "#start-recording-button", function (e) {
    e.preventDefault();
    $("#recording-status-text")
      .text("Recording In Progress...")
      .addClass("recording-active");
    $("#save-audio-button").hide();

    navigator.mediaDevices.getUserMedia({ audio: true }).then((stream) => {
      mediaRecorder = new MediaRecorder(stream);
      mediaRecorder.start();

      audioChunks = [];

      mediaRecorder.addEventListener("dataavailable", (event) => {
        audioChunks.push(event.data);
      });

      mediaRecorder.addEventListener("stop", () => {
        const audioBlob = new Blob(audioChunks);
        const blobToSave = convertBlobToMp4(audioBlob, "Blobbie");
        const audioUrl = URL.createObjectURL(audioBlob);

        let audio = document.querySelector("audio");
        audio.src = audioUrl;

        console.log(blobToSave);
      });
    });
  });

  $(".main").on("click", "#stop-recording-button", function (e) {
    e.preventDefault();
    mediaRecorder.stop();
    $("#recording-status-text")
      .text("Recording Completed.")
      .removeClass("recording-active");
    $("#save-audio-button").show();
  });

  $(".main").on("click", "#save-audio-button", function (e) {
    showModal("save-audio");
  });

  $(".main").on("click", "#open-recording-button", function (e) {
    showModal("open-audio");
  });
});

//Helper functions///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function convertBlobToMp4(blob, fileName) {
  blob.lastModifiedDate = new Date();
  blob.name = fileName;
  blob.type = "audio/mp3";

  return blob;
}

function showModal(action) {
  switch (action) {
    case "save-pdf": {
      let modal = document.getElementById("save-pdf-modal");

      modal.style.display = "block";

      this.onclick = function () {
        modal.style.display = "block";
      };

      $("#save-pdf-modal-close-button").click(function () {
        modal.style.display = "none";
      });

      // When the user clicks anywhere outside of the modal, close it
      window.onclick = function (event) {
        if (event.target == modal) {
          modal.style.display = "none";
        }
      };
      break;
    }

    case "open-pdf": {
      let modal = document.getElementById("open-pdf-modal");

      // When the user clicks on the button, open the modal
      modal.style.display = "block";

      $("#open-pdf-modal-close-button").click(function () {
        modal.style.display = "none";
      });

      // When the user clicks anywhere outside of the modal, close it
      window.onclick = function (event) {
        if (event.target == modal) {
          modal.style.display = "none";
        }
      };
      break;
    }

    case "save-audio": {
      let modal = document.getElementById("save-modal");

      modal.style.display = "block";

      $("#save-audio-modal-close-button").click(function () {
        modal.style.display = "none";
      });

      // When the user clicks anywhere outside of the modal, close it
      window.onclick = function (event) {
        if (event.target == modal) {
          modal.style.display = "none";
        }
      };
      break;
    }

    case "open-audio": {
      let modal = document.getElementById("myModal");

      modal.style.display = "block";

      $("#open-audio-modal-close-button").click(function () {
        modal.style.display = "none";
      });

      // When the user clicks anywhere outside of the modal, close it
      window.onclick = function (event) {
        if (event.target == modal) {
          modal.style.display = "none";
        }
      };
      break;
    }

    default:
      alert("Could not open a modal.");
      break;
  }
}
