$(document).ready(function () {
  // These variables are for the audio recording funtionality and are declared as "globals" in this script to be accessed by
  // different parts of the script
  let currentDocId;
  let mediaRecorder;
  let audioChunks;

  //PDF Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  $('#add-comment').click(function (e) {
     showModal('comment');
  });

  $("#file-button").click(function (e) {
    e.preventDefault();
    $(".main").empty();

    if ($("#toolkit3").is(":hidden")) {
      $("#toolkit3").show();
    }

    $(".main").append(`<iframe src="" id="frame"
            frameborder="1" width="100%" height="100%"></iframe>`);
  });

  //The following four methods handle displaying the modals for opening, saving, and uploading a pdf
  $("#save-pdf-button").click(function (e) {
    showModal("save-pdf");
  });

  $("#open-pdf-button").click(function (e) {
    showModal("open-pdf");
  });
  $("#upload-pdf-button").click(function (e) {
    showModal("upload-pdf");
  });
  
  $('#split-pdf-button').click(function (e) { 
    
    showModal('split-pdf')
  });

  //Video Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  /**
   * This JQuery function handles events that trigger when the video state button is clicked.
   *
   **/
  $("#video-button").click(function (e) {
    $(".main").empty();
    $("#toolkit3").hide("fast");

    /**
     * This function renders the html for when the video state button is clicked
     *
     **/
    $(".main").append(`
            <div class="video-container">
                <iframe id="youtube-video"width="860" height="615" src="https://www.youtube.com/embed/_84yDAKtjIo?enablejsapi=1" frameborder="0" allow="accelerometer;
                    autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe><button class="yt-button" id="url-control-button"><h4>Change the video</h4></button><div class="youtube-url-control" >
                    <input type="text" name="youtube-input" id="youtube-search-bar" placeholder="Enter a youtube embedded url">
            <button class="yt-button" id="youtube-search-button"><h4>Go!</h4></button>
            </div></div>`);

    $("#url-control-button").click(function (e) {
      $(".youtube-url-control").toggle("slow");
    });
  });

  //Audio Functionality/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  /**
   * This click handler creates UI controls for the audio state
   *
   * The controls created are attatched to the "main" element in the DOM. 
   * 
   **/
  $("#audio-button").click(function (e) {
    $(".main").empty();
    $("#toolkit3").hide("fast");

    let audioControlsDiv = $(`<div></div>`);
    let startButton = `<button id="start-recording-button"><img src="https://img.icons8.com/office/24/000000/start.png"/><h4>Start Recording</h4></button>`;
    let stopButton = `<button id="stop-recording-button"><img src="https://img.icons8.com/office/24/000000/stop.png"/><h4>Stop Recording</h4></button>`;
    let openRecordingButton = `<button id="open-recording-button"><img src="https://img.icons8.com/office/24/000000/opened-folder.png"/><h4>Open a Recording</h4></button>`;
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

  /**
   * This is the click handler when start recording is pressed
   *
   * All of the logic behind starting a recording resides inside this function.
   * The display text is changed here as well as the logic for hiding the UI controls.
   *
   **/
  $(".main").on("click", "#start-recording-button", function (e) {
    e.preventDefault();
    $("#recording-status-text")
      .text("Recording In Progress...")
      .addClass("recording-active");
    $("#save-audio-button").hide();

    // the recording is started here. microphone access is gained and events are binded
    navigator.mediaDevices.getUserMedia({ audio: true }).then((stream) => {
      mediaRecorder = new MediaRecorder(stream);
      mediaRecorder.start();

      audioChunks = [];

      mediaRecorder.addEventListener("dataavailable", (event) => {
        audioChunks.push(event.data);
      });

      // create a blob from the audio chunks that were recorded, then create a url
      mediaRecorder.addEventListener("stop", () => {
        const audioBlob = new Blob(audioChunks);
        const audioUrl = URL.createObjectURL(audioBlob);

        //set the source of the audio element to the url created
        let audio = document.querySelector("audio");
        audio.src = audioUrl;
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

  /**
   * Click handler for saving the current audio to the server
   *
   * This method first gets the file name entered into the input field. Then a fetch request is made
   * to the src property of the audio which loads its contents into an array buffer. This array buffer is
   * converted into a base64 string and passed along inside the AJAX call
   *
   **/
  $("#submit-audio-file").click(async function (e) {
    e.preventDefault();
    let fileName = $("#audio-file-name").val();
    let audioUrl = $("audio").attr("src");
    let file = await fetch(audioUrl).then((r) => r.arrayBuffer());
    let fileAsBase64String = btoa(String.fromCharCode(...new Uint8Array(file)));

    /**
     * AJAX call to send the file to the server
     *
     * Post request is made to the OnPostFile method in the page controller. The data being
     * sent is the base64 representation of the file to be uploaded.
     *
     * @httptype POST
     *
     **/
    $.ajax({
      type: "post",
      url: `?handler=File&fileName=${fileName}&typeOfFile=audio`,
      data: fileAsBase64String,
      contentType: false,
      processData: false,
      success: function (response) {
        alert("Success");
      },
      fail: function (response) {
        alert("Failure");
      },
    });
  });

  /**
   * Retrieve the desired file from our database to be loaded into the front end
   *
   * When a file inside open a audio modal is clicked this method is fired. The ID of the file is obtained
   * from the modal and then passed as a parameter inside the AJAX call to get it.
   *
   **/
  $(".audio-file-in-db").on("click", function (e) {
    e.preventDefault();
    let id = $(this)
      .parent()
      .siblings(".audio-sister")
      .find(".audio-file-id")
      .text();

    /**
     * AJAX call to get the file to be displayed.
     *
     * The AJAX call recieves a base64
     * representation of the file when it is successful. Then a blob object is created from the base64 string. From this blob
     * an Object Url is created on the browser to store the file. Finally the URL of the file is set to the audio's source which
     * in turn displays it
     *
     *@httptype GET
     **/
    $.ajax({
      url: "?handler=File&ID=" + id,
      type: "get",
      success: async function (data) {
        let blob = await fetch(`data:audio/mp4;base64,${data}`);
        blob = await blob.blob();
        console.log(blob);

        let urlForAudioBlob = URL.createObjectURL(blob);
        $("audio").attr("src", urlForAudioBlob);
        $("#myModal").toggle();
      },
      error: function () {
        alert("Could not find a file.");
      },
    });
  });

 
});

//Helper functions///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/**
 * This function displays the correct modal based on the parameter passed
 *
 * A switch statement is used to display to modal. Values are represented as a string
 * when passed as a parameter
 *
 * @param action Depending on the value passed, the correct modal will open. Acceptable
 *  values are "save-pdf" "upload-pdf" "open-pdf" "save-audio" "open-audio"
 **/
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
    case "upload-pdf": {
      let modal = document.getElementById("upload-pdf-modal");

      modal.style.display = "block";

      this.onclick = function () {
        modal.style.display = "block";
      };

      $("#upload-pdf-modal-close-button").click(function () {
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
    case "split-pdf": {
      let modal = document.getElementById("split-pdf-modal");

      // When the user clicks on the button, open the modal
      modal.style.display = "block";

      $("#split-pdf-modal-close-button").click(function () {
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

    case "split-pdf": {
      let modal = document.getElementById("split-pdf-modal");

      // When the user clicks on the button, open the modal
      modal.style.display = "block";

      $("#split-pdf-modal-close-button").click(function () {
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

    case "comment": {
      let modal = document.getElementById("comment-modal");

      // When the user clicks on the button, open the modal
      modal.style.display = "block";

      $("#comment-modal-close-button").click(function () {
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
